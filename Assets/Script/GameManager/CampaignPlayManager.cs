using System;
using System.Collections;
using System.Collections.Generic;
using Json;
using LitJson;
using Script.Champions.Thresh;
using UnityEngine;
using UnityEngine.UI;

namespace Script
{
    public class CampaignPlayManager : MonoBehaviour
    {
        private static CampaignPlayManager instance;

        public static CampaignPlayManager Instance
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

        [SerializeField] public Vector2 joystickThreshold = new Vector2(0.2f, 0.2f);
        public Transform groundLT;
        public Transform groundRB;
        public MyFloatingJoystick floatingJoystick;

        public GameObject playerOffset;
        public GameObject mobOffset;
        public Transform playerStartPoint;
        public Transform mobStartPoint;
        public CameraFollow cameraFollow;

        public PlayerModel playerModel;

        public GameObject dimObject;

        private void Awake()
        {
            instance = this;
        }

        void Start()
        {
            if (GameManager.instance == null)
            {
                playerModel = new PlayerModel("test");
            }
            else
            {
                playerModel = GameManager.instance.playerModel;
            }
            
            floatingJoystick = GameObject.Find("Joystick").GetComponent<MyFloatingJoystick>();
            groundLT = GameObject.Find("GroundLT").transform;
            groundRB = GameObject.Find("GroundRB").transform;


            GameObject playerOffset = initPlayer();
            initMob(playerOffset);
            
            // InvokeRepeating("CheckMobHp", 2.0f, 1.0f);
            //StartCoroutine(CreateDim());
        }

        void CheckMobHp()
        {
            if (GameManager.instance.EnemyChampionModel.health <= 0)
            {
                ClearCampaign();
            }
        }

        IEnumerator CreateDim()
        {
            Debug.Log("Create Dim!!!");
            
            yield return new WaitForSeconds(2.0f);
            GameObject temp = Instantiate(dimObject, Vector3.zero, Quaternion.identity);
            temp.transform.SetParent(GameObject.Find("Canvas").transform);
            temp.transform.localPosition = Vector3.zero;
        }

        GameObject initPlayer()
        {
            //playerModel.selectedChampion = ChampionFactory.Create(ChampionFactory.ChampionType.Jax);
            GameObject playerOffsetSpawned = Instantiate(playerOffset, Vector3.zero, Quaternion.identity);
            Instantiate(GameManager.instance.allChampionPrefabMap[ChampionFactory.ChampionType.Jax.ToString()], Vector3.zero, Quaternion.identity, playerOffsetSpawned.transform);
            playerOffsetSpawned.transform.position = playerStartPoint.position;
            playerOffsetSpawned.GetComponent<ChampionStatusController>().championModel = GameManager.instance.playerModel.selectChampionModel;
            playerOffsetSpawned.GetComponent<ChampionStatusController>().healthText = GameObject.Find("MyHp").GetComponent<Text>();

            // set target
            cameraFollow.target = playerOffsetSpawned.transform;
            return playerOffsetSpawned;
        }

        void initMob(GameObject playerOffset)
        {
            ChampionFactory.ChampionType championType = ChampionFactory.ChampionType.Orn;
            
            
            GameObject mobOffsetSpawned = Instantiate(mobOffset, Vector3.zero, Quaternion.identity);
            GameObject temp = Instantiate(GameManager.instance.allChampionPrefabMap[championType.ToString()], Vector3.zero, Quaternion.identity, mobOffsetSpawned.transform);

            mobOffsetSpawned.AddComponent<MobOrnStatusController>();
            mobOffsetSpawned.GetComponent<MobStatusController>().championModel = ChampionFactory.Create(championType);
            mobOffsetSpawned.GetComponent<ChampionStatusController>().healthText = GameObject.Find("MobHp").GetComponent<Text>();

            mobOffsetSpawned.transform.position = mobStartPoint.position;
            mobOffsetSpawned.GetComponent<MobStatusController>().target = playerOffset.transform;
        }
        
        public async void ClearCampaign()
        {
            ExitCampaignResponse exitCampaignResponse;
            try
            {
                string json = await WebCall.Instance.GetRequestAsync($"/action/exit-campaign");
                exitCampaignResponse = JsonUtility.FromJson<ExitCampaignResponse>(json);
                Debug.Log("ClearCampaign : " + exitCampaignResponse);
                StartCoroutine(CreateDim());
            }
            catch (Exception e)
            {
                Debug.LogError($"Error. {e.Message}");
            }
        }

    }

    
}