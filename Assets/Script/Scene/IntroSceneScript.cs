using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using WebSocketSharp;

public class IntroSceneScript : MonoBehaviour
{
    public AudioSource introBgm;
    public float transitionTime = 1.5f;
    private bool isInit = false;
    private byte color = 255;
    private PlayerModel playerModel;

    public GameObject guestLoginButton;
    public GameObject dimObjec;
    public GameObject failedPopUp;

    public GameObject loginProgress;
    
    public InputField playerName;

    public GameObject bg;
    public GameObject nicknamePanel;

    ConcurrentQueue<string> commandQueue = new ConcurrentQueue<string>();

    // Start is called before    the first frame update
    void Start()
    {
        //StartCoroutine(LoadAsyncIntroSceneCoroutine());
        TextAsset fakeNamesTextAsset = Resources.Load("fakename") as TextAsset;
        GameManager.instance.fakeNames = new List<string>(JsonUtility.FromJson<FakeNameArray>(fakeNamesTextAsset.ToString()).names);
        
        GameManager.instance.persistentDataPath = Application.persistentDataPath;
        Debug.Log("data Path: " + GameManager.instance.persistentDataPath);
    }
    
    IEnumerator LoadAsyncMainSceneCoroutine()
    {
        StartCoroutine(GameManager.FadeOut(introBgm, 1.3f, Mathf.SmoothStep));
        GameManager.instance.transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene("MainScene");
    }

    public void GuestLogin()
    {
        guestLoginButton.SetActive(false);
        FirebaseAuth.Instance.LoginAnynim();
        loginProgress.SetActive(true);
        loginProgress.GetComponentInChildren<Text>().text = "로그인 시도중...";
    }

    public async void EnterNickName()
    {
        Debug.Log("Player name : " + playerName.text);
        if ("".Equals(playerName.text) || Regex.IsMatch(playerName.text, Constants.nicknamePattern) == false)
        {
            return;
        }
        
        nicknamePanel.SetActive(false);
        loginProgress.SetActive(true);
        var success = await UserInfoManager.Instance.SetName(playerName.text);
        Debug.Log("set nickname result : " + success);

        if (success)
        {
            GameInfoManager.Instance.firstLoginUser = true;
            playerModel.nickname = playerName.text;
            await UserInfoManager.Instance.GetUserInfo();
            await GetGameUserInfo();
            playerModel.InitChampionAbility();
        }
        else
        {
            loginProgress.GetComponentInChildren<Text>().text = "닉네임 등록 실패";
        }
    }

    public void FailedLoginOnPressOK()
    {
        dimObjec.SetActive(false);
        failedPopUp.SetActive(false);
    }

    private async Task GetGameUserInfo()
    {
        try
        {
            await GameInfoManager.Instance.GetChamps();
            // List<CardModel> cards = await GameInfoManager.Instance.GetCards();
            // List<CampaignModel> campaigns = await GameInfoManager.Instance.GetCampaigns();

            // 유저정보 가져오기
            await UserInfoManager.Instance.GetUserChamps();
            
            // 서버 tick 가져오기
            await UserInfoManager.Instance.GetServerTick();
            
            // 스테미너 회복 테스트용으로 추가
            // await UserInfoManager.Instance.MultiPlay();
            
            // List<UserCard> userCards = await UserInfoManager.Instance.GetUserCards();
            // List<UserDeck> userDeck = await UserInfoManager.Instance.GetUserDecks();
            // List<UserCampaign> userCampaigns = await UserInfoManager.Instance.GetUserCampaigns();
                            
            commandQueue.Enqueue("Main");
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isInit)
        {
            nicknamePanel.SetActive(true);
            isInit = false;
            
            /*color -= 5;
            Color tempColor = new Color32(color, color, color, 255);
            // bg.GetComponent<Image>().color = tempColor;

            if (color < 45)
            {
                nicknamePanel.SetActive(true);
                isInit = false;
            }*/
        }
        
        if (FirebaseAuth.Instance.LoginResultQueue.TryDequeue(out var loginResult))
        {
            Task.Run(async () =>
            {
                if (loginResult.Success)
                {
                    Debug.Log("login success " + FirebaseAuth.Instance.User.UserId);
                    
                    PlayerModel playerModel = new PlayerModel(FirebaseAuth.Instance.User.UserId);
                    PlayerData playerData = playerModel.LoadPlayer();
                    if (playerData == null)
                    {
                        // 계정 생성을 위한 딜레이
                        await Task.Delay(1 * 1000);
                        try
                        {
                            playerModel.SavePlayer();
                        }
                        catch (Exception e)
                        {
                            Debug.LogError(e);
                        }
                    }

                    playerModel.uid = FirebaseAuth.Instance.User.UserId;
                    var userInfo = await UserInfoManager.Instance.GetUserInfo();

                    // Debug.Log(userInfo);
                  
                    if (userInfo.name.IsNullOrEmpty())
                    {
                        this.playerModel = playerModel;
                        GameManager.instance.playerModel = playerModel;
                        commandQueue.Enqueue("SetNickname");
                    }
                    else
                    {
                        // 게임정보 가져오기
                       await GetGameUserInfo();
                       playerModel.nickname = userInfo.name;
                       GameManager.instance.playerModel = playerModel;
                       playerModel.InitChampionAbility();
                    }
                }
                else
                {
                    commandQueue.Enqueue("ShowLoginFailedWindow");
                }
            });
        }
        
        if (commandQueue.TryDequeue(out var command))
        {
            switch (command)
            {
                case "SetNickname":
                    loginProgress.SetActive(false);
                    isInit = true;
                    break;
                case "Main":
                    Debug.Log("GO MAIN");
                    StartCoroutine(LoadAsyncMainSceneCoroutine());
                //    userInfoText.text = UserInfoManager.Instance.Info.ToString();
                    break;
                case "ShowLoginFailedWindow":
                    guestLoginButton.SetActive(true);
                    dimObjec.SetActive(true);
                    failedPopUp.SetActive(true);
                    break;
                default:
                    Debug.LogError($"Unknown Command. {command}");
                    break;
            }
        }
    }


    public void GoLoginScene()
    {
        if (Debug.isDebugBuild == true)
        {
            SceneManager.LoadScene("Login");
        }
    }
    
    [System.Serializable]
    public class FakeNameArray
    {
        public string[] names;
    }
    
}
