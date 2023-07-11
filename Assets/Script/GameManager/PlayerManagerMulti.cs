using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using LitJson;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using Photon.Pun;
using Script.Champions.Thresh;
using Script.Model.Game;
using Unity.Jobs;
using UnityEngine.UI;

namespace Script
{
    public class PlayerManagerMulti : MonoBehaviourPunCallbacks, IPunObservable
    {
        public static Color enemySkinColor = new Color(0.6f, 1.0f, 0.5f, 1.0f);

        #region Singleton
        private static PlayerManagerMulti instance;

        public static PlayerManagerMulti Instance
        {
            get
            {
                if (null == instance)
                {
                    return null;
                }

                return instance;
            }
        }
        #endregion

        [SerializeField] public Vector2 joystickThreshold = new Vector2(0.2f, 0.2f);
        public Transform groundLT;
        public Transform groundRB;
        public MyFloatingJoystick floatingJoystick;

        public Transform[] playerStartPoint;

        public PlayerModel playerModel;
        public static GameObject LocalPlayerInstance { get; set; }
        public GameObject black;

        public GameObject me;
        public GameObject enemy;

        public GameObject playerOffset;
        public GameObject mobOffset;

        public GameObject attackButton;
        public GameObject skill1Button;
        public GameObject skill2Button;
        public GameObject skill3Button;
        public GameObject joystickObject;

        private List<GameObject> buttonsList = new List<GameObject>();
        public GameObject gameSetText;
        
        private int playCount = 60;
        
        public Text countDownText;
        
        private PhotonView photonView;

        public GameObject gameSetDim;
        public Canvas canvas;

        public AudioSource gameSetSound;
        public AudioSource hitSound;
        public bool gameSet = false;

        int index = 0;

        private void Awake()
        {
            instance = this;
        }

        private void ShowButtons()
        {
            foreach (var o in buttonsList)
            {
                o.SetActive(true);
            }
        }
        
        private void HideButtons()
        {
            foreach (var o in buttonsList)
            {
                o.SetActive(false);
            }

            me.GetComponent<PlayerMoveController>().speedMagnitude = Vector2.zero;
            if (GameManager.instance.isBot)
            {
                enemy.GetComponent<MobStatusController>().updateNewState(new MobAIGameEndState(enemy.GetComponent<MobStatusController>()));
            }
        }

        void Start()
        {
            gameSet = false;
            foreach (var player in PhotonNetwork.PlayerList)
            {
                if (GameManager.instance.playerModel.uid != player.UserId)
                {
                    GameManager.instance.enemyPlayerModel.uid = player.UserId;
                }
            }
            
            buttonsList.Add(attackButton);
            buttonsList.Add(skill1Button);
            buttonsList.Add(skill2Button);
            buttonsList.Add(skill3Button);
            buttonsList.Add(joystickObject);

            photonView = PhotonView.Get(this);
            if (GameManager.instance == null)
            {
                playerModel = new PlayerModel("test");
            }
            else
            {
                playerModel = GameManager.instance.playerModel;
            }

            attackButton.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(playerModel.selectChampionModel.attackIconPath);
            skill1Button.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(playerModel.selectChampionModel.skill1IconPath);
            skill2Button.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(playerModel.selectChampionModel.skill2IconPath);
            skill3Button.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(playerModel.selectChampionModel.skill3IconPath);
            
            // _launcher = GameObject.Find("Launcher").GetComponent<Launcher>();
            //floatingJoystick = GameObject.Find("Joystick").GetComponent<FloatingJoystick>();
            groundLT = GameObject.Find("GroundLT").transform;
            groundRB = GameObject.Find("GroundRB").transform;
            
            Debug.Log("PlayerManagerMulti Start()..");
            Debug.Log($"Enemy maxHealth. {GameManager.instance.EnemyChampionModel.health}");


            if (GameManager.instance.isBot)
            {
                SpawnPlayerBot();
            }
            else
            {
                if(GameManager.instance.isLeftPlayer)
                    SpawnPlayer();
                else
                    StartCoroutine(delayedAndSpawnPlayer()); 
            }
            
            StartCoroutine(delayedAndRemoveBlack());
            GameManager.instance.lastRoomName = PhotonNetwork.CurrentRoom.Name;
        }

        void InitRangeCircle()
        {
            
        }

        IEnumerator delayedAndSpawnPlayer()
        {
            yield return new WaitForSeconds(0.2f);
            SpawnPlayer();
        }
        
        IEnumerator delayedAndRemoveBlack()
        {
            yield return new WaitForSeconds(0.7f);
            if (!GameManager.instance.isBot)
            {
                for (int i = 0; i < 20; i++)
                {
                    if (me == null || enemy == null)
                    {
                        yield return new WaitForSeconds(0.5f);
                    }
                    else
                    {
                        InitPlayers();
                    }
                }
            }
            black.SetActive(false);
            ShowButtons();
            InitRangeCircle();
        }

        public async Task<bool> GameOut(string _winner)
        {
            try
            {
                string winner = null;

                if (_winner == null)
                {
                    int enemyHealth = Mathf.RoundToInt(GameManager.instance.EnemyChampionModel.currentHealth);
                    int myHealth = Mathf.RoundToInt(GameManager.instance.playerModel.selectChampionModel.currentHealth);
                    
                    if (enemyHealth == myHealth)
                    {
                        winner = "";
                    }
                    else
                    {
                        winner = enemyHealth < myHealth
                            ? GameManager.instance.playerModel.uid
                            : (GameManager.instance.isBot ? "bot" : GameManager.instance.enemyPlayerModel.uid);
                    }
                }
                else
                {
                    winner = _winner;
                }
                
                
                Debug.Log($"WINNER {winner}");
                
                Dictionary<string, string> body = new Dictionary<string, string>();
                body.Add("AppId", "123");
                body.Add("GameId", PhotonNetwork.CurrentRoom.Name);
                body.Add("winnerUid", winner);

                System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
                double startTime = (System.DateTime.UtcNow - epochStart).TotalMilliseconds;
                
                string json = await WebCall.Instance.PostRequestAsync($"/action/game-out", JsonMapper.ToJson(body));
                JsonData jsonData = JsonMapper.ToObject(json);
                Debug.Log("GameOut json : " + json);

                
                var gameOutModel = GameOutModel.FromJson(jsonData);
                GameManager.instance.lastGameModel = gameOutModel;
                
                
                System.DateTime epochEnd = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
                double endTime = (System.DateTime.UtcNow - epochEnd).TotalMilliseconds;

                
                Debug.Log($"GameOut : {(int)(endTime - startTime)}");

                await Task.Delay(Mathf.Max(3000 - (int)(endTime - startTime), 0));

                MultiFinish();
                return true;;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error. {e.Message}");
                MultiFinish();
                return false;
            }
        }

       
        IEnumerator delayedAndGoMultiFinish()
        {
            Boolean ret = false;
            for (int i = 0; i < 10; i++)
            {
                yield return new WaitForSeconds(1.0f);
                // Task task = GetGameResult(ret);
                // yield return new WaitUntil(() => task.IsCompleted);
                
                Debug.Log("GAME RESULT : " + ret);
                if (ret)
                    break;
            }
        }

        void MultiFinish()
        {
            if (PhotonNetwork.CurrentRoom.Players.Count == 1)
            {
                Debug.Log("MultiFinish Count 1");
                // PhotonNetwork.Disconnect();
                PhotonNetwork.LeaveRoom();
                // PhotonNetwork.Destroy(photonView);

            }
            else
            {
                Debug.Log("MultiFinish Leave Room");

                photonView.RPC("RPCLeaveRoom", RpcTarget.Others);
            }
            // PhotonNetwork.Disconnect();
            // PhotonNetwork.Destroy(photonView);
            // SceneManager.LoadScene("MultiFinish");
        }

        [PunRPC]
        public void RPCLeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
            // PhotonNetwork.Destroy(photonView);

        }
        
        
        void CountDown()
        {
            photonView.RPC("RPCCountDown", RpcTarget.All);
        }

        [PunRPC]
        public void RPCCountDown()
        {
            if (playCount > 0)
            {
                playCount -= 1;
                countDownText.text = playCount.ToString();
                if (playCount == 0)
                {
                    if (!gameSet)
                    {
                        gameSetSound.Play();
                        gameSetText.SetActive(true);
                        HideButtons();
                        
                        if (PhotonNetwork.IsMasterClient)
                        {
                            GameOut(null);
                        }
                        gameSet = true;
                    }
                    // else
                    // {
                    //     StartCoroutine(delayedAndGoMultiFinish());
                    // }
                }
            }
        }

        [PunRPC]
        public void RPCOnePlayerDied()
        {
            StartCoroutine(StartDie());
        }

        IEnumerator StartDie()
        {
            yield return new WaitForSeconds(2.5f);

            if (!gameSet)
            {
                gameSetSound.Play();
                gameSetText.SetActive(true);
                HideButtons();
                
                if (PhotonNetwork.IsMasterClient)
                {
                    GameOut(null);
                }
                gameSet = true;
            }
        }

        void SpawnPlayerBot()
        {
            GameObject playerOffsetSpawned = Instantiate(playerOffset, Vector3.zero, Quaternion.identity);
            int championId = GameManager.instance.playerModel.selectChampionId;
            GameObject o = GameManager.instance.championPrefabList[championId];
            GameObject player = Instantiate(o, Vector3.zero, Quaternion.identity);
            me = playerOffsetSpawned;
            
            player.transform.SetParent(playerOffsetSpawned.transform);
            playerOffsetSpawned.transform.position = playerStartPoint[0].position;
            InvokeRepeating("CountDown", 0.0f, 1.0f);
        
            ChampionStatusController myStatusController = playerOffsetSpawned.GetComponent<ChampionStatusController>();
            myStatusController.championModel = GameManager.instance.playerModel.selectChampionModel;
            myStatusController.healthText = GameObject.Find("LeftHp").GetComponent<Text>();
            myStatusController.healthBar = GameObject.Find("LeftHpBar").GetComponent<Image>();
            myStatusController.healthBarBack = GameObject.Find("LeftHpBarBack").GetComponent<Image>();
            myStatusController.championBehavior = playerOffsetSpawned.GetComponentInChildren<ChampionBehavior>();
            myStatusController.championBehavior.championStatusController = myStatusController;
            myStatusController.championBehavior.isMine = true;
            myStatusController.initHealth();

            int mobChampionId = GameManager.instance.botChampionId;
            
            GameObject mobPrefab = GameManager.instance.championPrefabList[mobChampionId];
            GameObject mobOffsetSpawned = Instantiate(mobOffset, Vector3.zero, Quaternion.identity);
            GameObject mob = Instantiate(mobPrefab, Vector3.zero, Quaternion.identity);
            mob.GetComponentInChildren<SpriteRenderer>().color = enemySkinColor;
            mob.transform.SetParent(mobOffsetSpawned.transform);
            mobOffsetSpawned.transform.position = playerStartPoint[1].position;
            enemy = mobOffsetSpawned;

            MobStatusController botStatusController = null;
            
            if (mobChampionId == 0)
            {
                mobOffsetSpawned.AddComponent<MobJaxStatusController>();
                botStatusController = mobOffsetSpawned.GetComponent<MobJaxStatusController>();
            } else if(mobChampionId == 1) {
                mobOffsetSpawned.AddComponent<MobThreshStatusController>();
                botStatusController = mobOffsetSpawned.GetComponent<MobThreshStatusController>();
            } else if(mobChampionId == 2) {
                mobOffsetSpawned.AddComponent<MobLucianStatusController>();
                botStatusController = mobOffsetSpawned.GetComponent<MobLucianStatusController>();
            }

            botStatusController.championBehavior = mobOffsetSpawned.GetComponentInChildren<ChampionBehavior>();
            botStatusController.championBehavior.championStatusController = botStatusController;
            botStatusController.championBehavior.defaultColor = enemySkinColor;
            
            botStatusController.championModel = GameManager.instance.EnemyChampionModel;
            botStatusController.target = playerOffsetSpawned.transform;
            botStatusController.healthText = GameObject.Find("RightHp").GetComponent<Text>();
            botStatusController.healthBar = GameObject.Find("RightHpBar").GetComponent<Image>();
            botStatusController.healthBarBack = GameObject.Find("RightHpBarBack").GetComponent<Image>();
            botStatusController.healthBar.color = new Color(254, 0, 255);
            botStatusController.initHealth();

            myStatusController.target = mobOffsetSpawned.transform;

        }
        
        GameObject SpawnPlayer()
        {
            int selectChampionId = GameManager.instance.playerModel.selectChampionId;
            
            GameObject playerOffsetSpawned = PhotonNetwork.Instantiate("PlayerOffset", Vector3.zero, Quaternion.identity);
            GameObject player = PhotonNetwork.Instantiate(GameManager.instance.championPrefabList[selectChampionId].name, Vector3.zero, Quaternion.identity);

            playerOffsetSpawned.name = "Offset";
            player.name = "Player";
            player.transform.SetParent(playerOffsetSpawned.transform);

            if (GameManager.instance.isLeftPlayer)
            {
                playerOffsetSpawned.transform.position = playerStartPoint[0].position;
                InvokeRepeating("CountDown", 0.0f, 1.0f);
            }
            else
            {
                playerOffsetSpawned.transform.position = playerStartPoint[1].position;
            }
            
            return playerOffsetSpawned;
        }

        private void InitPlayers()
        {
            // Debug.Log("InitPlayers !!!!!!!");
            if (me == null || enemy == null)
            {
                Debug.Log($"InitPlayers error me: {me} enemy: {enemy}");
                return;
            }

            ChampionStatusController leftPlayerController;
            ChampionStatusController rightPlayerController;
            
            me.GetComponent<ChampionStatusController>().championBehavior.championStatusController = me.GetComponent<ChampionStatusController>();
            me.GetComponent<ChampionStatusController>().championBehavior.isMine = true;

            // enemy.GetComponentInChildren<SpriteRenderer>().color = new Color(0.6f, 1.0f, 0.5f, 1.0f);

            // enemy.GetComponent<PlayerMoveController>().footRing.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.4468f, 0.4196f, 0.58f);

            if (GameManager.instance.isLeftPlayer)
            {
                leftPlayerController = me.GetComponent<ChampionStatusController>();
                rightPlayerController = enemy.GetComponent<ChampionStatusController>();
            }
            else
            {
                leftPlayerController = enemy.GetComponent<ChampionStatusController>();
                rightPlayerController = me.GetComponent<ChampionStatusController>();
            }
            
            
            me.GetComponent<ChampionStatusController>().target = enemy.transform;
            enemy.GetComponent<ChampionStatusController>().target = me.transform;
            enemy.GetComponentInChildren<SpriteRenderer>().color = enemySkinColor;

            leftPlayerController.healthText = GameObject.Find("LeftHp").GetComponent<Text>();
            leftPlayerController.healthBar = GameObject.Find("LeftHpBar").GetComponent<Image>();
            leftPlayerController.healthBarBack = GameObject.Find("LeftHpBarBack").GetComponent<Image>();
            leftPlayerController.RegisterChampionModel();

            rightPlayerController.healthText = GameObject.Find("RightHp").GetComponent<Text>();
            rightPlayerController.healthBar = GameObject.Find("RightHpBar").GetComponent<Image>();
            rightPlayerController.healthBarBack = GameObject.Find("RightHpBarBack").GetComponent<Image>();
            rightPlayerController.RegisterChampionModel();
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        ////////////////////////////////////////
        //      Photon Callback
        ////////////////////////////////////////

        public override void OnLeftRoom()
        {
            Debug.LogFormat("OnLeftRoom()"); // seen when other disconnects
            SceneManager.LoadScene("MultiFinish");
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.LogFormat("OnPlayerEnteredRoom() {0}", newPlayer.NickName); // not seen if you're the player connecting

            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
            }
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Debug.LogFormat("OnPlayerLeftRoom() {0}", otherPlayer.NickName); // seen when other disconnects

            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
                
                if (GameManager.instance.lastGameModel == null)
                {
                   
                    if (!gameSet)
                    {
                        gameSetText.SetActive(true);
                        HideButtons();
                        gameSetSound.Play();
                        
                        GameOut(GameManager.instance.playerModel.uid);
                        gameSet = true;
                    }
                }
                else
                {
                    MultiFinish();
                }
            }
        }
        
        #region IPunObservable implementation
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            /*// if (GameManager.instance.playerModel.selectChampionModel == null)
                // return;
            Debug.Log($"OnPhotonSerializeView() : {info.Sender}");

            if (stream.IsWriting)
            {
                Debug.Log($"send hp : {GameManager.instance.playerModel.selectChampionModel.currentHealth}");
                stream.SendNext(GameManager.instance.playerModel.selectChampionModel.currentHealth);
            }
            else
            {
                if (stream.Count <= 0)
                    return;

                var obj = stream.ReceiveNext();
                if (obj != null)
                {
                    float value = (float) obj;
                    Debug.Log($"enemy hp : {value}");

                    if (GameManager.instance.EnemyChampionModel.currentHealth != value)
                    {
                        GameManager.instance.EnemyChampionModel.currentHealth = value;
                        if(enemy != null)
                            enemy.GetComponent<ChampionStatusController>().UpdateHealthText();
                    }
                }
            }*/
        }
        #endregion

    }
}