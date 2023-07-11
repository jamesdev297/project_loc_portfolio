using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExitGames.Client.Photon.StructWrapping;
using LitJson;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Random = UnityEngine.Random;

namespace Script
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        byte maxPlayerPerRoom = 2;
        string gameVersion = "1";

        private IEnumerator delayedMatchBot;
        bool isConnecting;
        

  

        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            DontDestroyOnLoad(this);
        }

        public async Task<String> GetMatchToken()
        {
            try
            {
                string json = await WebCall.Instance.GetRequestAsync($"/action/photon-auth-token");
                JsonData jsonData = JsonMapper.ToObject(json);
                Debug.Log("GetMatchToken : " + json);
            
                return (String) jsonData["token"];;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error. {e.Message}");
                return null;
            }
        }
        
        public async Task<bool> ChangeSingleMode()
        {
            try
            {
                StartCoroutine(MatchBot());
                /*Dictionary<string, string> body = new Dictionary<string, string>();
                body.Add("AppId", "123");
                body.Add("GameId", PhotonNetwork.CurrentRoom.Name);

                string json =
                    await WebCall.Instance.PostRequestAsync("/action/change-single-mode", JsonMapper.ToJson(body));
                Debug.Log("ChangeSingleMode : " + json);

                JsonData jsonData = JsonMapper.ToObject(json);
                // Debug.Log("ChangeSingleMode GameId: " + PhotonNetwork.CurrentRoom.Name);

                if (((string) jsonData["gameMode"]).Equals("single"))
                {
                    StartCoroutine(MatchBot());
                }*/
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error. {e.Message}");
                return false;
            }
        }
        
        public async void Connect()
        {
            PhotonNetwork.LoadLevel("MatchMaking");

            String token = await GetMatchToken();
            GameManager.instance.isBot = false;

            Debug.Log($"TOKEN : {token}");
            isConnecting = true;
            
            AuthenticationValues authValues = new AuthenticationValues();
            authValues.AuthType = CustomAuthenticationType.Custom;
            authValues.AddAuthParameter("token", token);
            PhotonNetwork.AuthValues = authValues;
            

            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                PhotonNetwork.GameVersion = gameVersion;
                PhotonNetwork.ConnectUsingSettings();
            }
        }


        ////////////////////////////////////
        //      Photon Callback
        ////////////////////////////////////
        public override void OnConnectedToMaster()
        {
            Debug.Log("OnConnectedToMaster() was called by PUN" + isConnecting);

            if (isConnecting)
            {
                PhotonNetwork.JoinRandomRoom();
            }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.LogWarningFormat("OnDisconnected() was called by PUN with reason {0}", cause);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayerPerRoom,PublishUserId = true });
        }

        IEnumerator DelayedAndMatchBot()
        {
            float term = 1.0f;
            term = Random.Range(3.0f, 4.0f);

            
            /*if (Debug.isDebugBuild == true)
            {
                term = Random.Range(7.0f, 15.0f);
                // term = 1.0f;
            }
            else
            {
                term = Random.Range(7.0f, 15.0f);
            }
            */

            yield return new WaitForSeconds(term);
            PhotonNetwork.CurrentRoom.IsOpen = false;
            yield return ChangeSingleMode();
        }

        private int randomBotChampId()
        {
            List<int> list = new List<int>(){0, 1, 2};
            //TODO : 챔피언 구현 다되면 GameInfoManager에서 챔피언 개수로 변경
            return list.ElementAt(Random.Range(0, list.Count));

            /*
            int num = Random.Range(0, UserInfoManager.Instance.UserChamps.Count);
        
            Debug.Log("bot num : " + num);
        
            return UserInfoManager.Instance.UserChamps[num].champId;*/
        }
        
        IEnumerator MatchBot()
        {

            GameManager.instance.botChampionId = randomBotChampId();
            
            GameManager.instance.isBot = true;
            GameManager.instance.EnemyChampionModel = GameInfoManager.Instance.NewChampionModel(GameManager.instance.botChampionId);
            string nickname = GameManager.instance.fakeNames.ElementAt(
                Random.Range(0, GameManager.instance.fakeNames.Count));
            if (nickname.Length > 6)
            {
                nickname = nickname.Substring(0, 6);
            }
            GameManager.instance.enemyPlayerModel = new PlayerModel(nickname, GameManager.instance.botChampionId);
            yield return new WaitForSeconds(1.0f);
            GameObject.Find("MatchSystem").GetComponent<MatchSystemScript>().ShowVersusBot(GameManager.instance.botChampionId);
            GameObject.Find("MatchSystem").GetComponent<MatchSystemScript>().GoToNextLevel();
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("OnJoinedRoom() called by PUN. Now this client is in a room.");

            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                GameManager.instance.isLeftPlayer = true;
                Debug.Log("We load the 'Multiplay'");
                delayedMatchBot = DelayedAndMatchBot();
                StartCoroutine(delayedMatchBot);
            }
            else
            {
                if(delayedMatchBot != null)
                    StopCoroutine(delayedMatchBot);
                
                if(PhotonNetwork.IsMasterClient)
                    PhotonNetwork.CurrentRoom.IsOpen = false;
                
                GameManager.instance.isLeftPlayer = false;
                GameObject.Find("MatchSystem").GetComponent<MatchSystemScript>().ShowVersus();
                GameObject.Find("MatchSystem").GetComponent<MatchSystemScript>().GoToNextLevel();
                GameObject.Find("MatchSystem").GetComponent<MatchSystemScript>().RequestSendToMe();
            }
            GameObject.Find("MatchSystem").GetComponent<MatchSystemScript>().SendToAnother();
        }
    }
}