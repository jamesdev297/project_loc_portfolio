using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Script;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainScene : MonoBehaviour
{
    public AudioSource mainBgm;
    public Text playerName;
    public Text playerElo;
    public Text gold;
    public Text stamina;
    public float transitionTime = 1.5f;
    public Dropdown dropdown;
    public ToggleGroup ToggleGroup;
    private int championIndex;
    public Text staminaTimerText;
    public List<Text> championExpireTimerText;

    public GameObject selectChampion;
    public GameObject championName;

    List<GameObject> prefabInstanceClones = new List<GameObject>();

    public Launcher launcher;
    private void Start()
    {
        // setStamina();

        GameManager.instance.transition.SetTrigger("End");
        //LoadingSceneManager.instance.InitPlayerModel();
        
        
        // playerName.text = GameManager.instance.playerModel.nickname;
        playerName.text = UserInfoManager.Instance.Info.name;
        playerElo.text = Mathf.RoundToInt((float) UserInfoManager.Instance.Info.elo).ToString();
        gold.text = Utils.GetCommaNum(UserInfoManager.Instance.Info.gold);
        int selectChampionId = GameManager.instance.playerModel.selectChampionId;

        // FIXME 선택된 챔피언 아이디 -1 경우 찾아야됨.
        if (selectChampionId == -1)
        {
            GameManager.instance.playerModel.selectChampionId = 0;
            selectChampionId = 0;
        }

        OnDestroyChampionPrefab();
        
        for (int i = 0; i < GameManager.instance.championPrefabList.Count; i++)
        {
            GameObject o = GameManager.instance.championPrefabList[i];
            if (selectChampionId == ChampionModel.getIdByPrefabName(o.name))
            {
                championIndex = i; 
                OnInstantiateChampionPrefab();
                break;
            }
        }
        StartCoroutine(GameManager.FadeIn(mainBgm, 0.56f,2.8f,  Mathf.SmoothStep));
        
        // SetDropdownOptions();
        
        InvokeRepeating("championTimer", 0.0f, 1.0f);
    }

    private async void  setStamina()
    {
        
        await UserInfoManager.Instance.GetUserInfo();

        staminaTimerText.text = "00 : 00";
        stamina.text = Utils.GetCommaNum(UserInfoManager.Instance.Info.stamina);
        
        InvokeRepeating("staminaTimer", 0.0f, 1.0f);
    }

    private void OnDisable()
    {
        if (GameManager.instance.banner == null)
            return;
    }

    private void staminaTimer()
    {
        if(UserInfoManager.Instance.Info.stamina < UserInfoManager.Instance.Info.maxStamina)
            updateStamina();
        else
        {
            staminaTimerText.text = "";
        }
    }

    private void championTimer()
    {
        // 챔피언 남은 기간 체크
        for (int i = 1; i < 3; i++)
        {

            int champId = i;
            if (UserInfoManager.Instance.userChampionMap.ContainsKey(champId))
            {

                long expiryMilliseconds = UserInfoManager.Instance.userChampionMap[champId].expiryTick - DateTimeOffset.Now.ToUnixTimeMilliseconds();

                // Debug.Log($"ALLL {champId} {expiryMilliseconds}");
                if (expiryMilliseconds < 0)
                {
                    UserInfoManager.Instance.userChampionMap.Remove(champId);

                    GameManager.instance.playerModel.selectChampionId = 0;
                    TabPopup.Instance.SelectChampion();
                }
                else
                {
                    if (champId == 1 || champId == 2)
                    {
                        TimeSpan timeSpan = TimeSpan.FromMilliseconds(expiryMilliseconds);
                        if (timeSpan.Days != 0)
                        {
                            championExpireTimerText[i - 1].text = timeSpan.Days + "일 남음";
                        }
                        else if (timeSpan.Hours != 0)
                        {
                            championExpireTimerText[i - 1].text = timeSpan.Hours + "시간 남음";
                        }
                        else if (timeSpan.Minutes != 0)
                        {
                            championExpireTimerText[i - 1].text = timeSpan.Minutes + "분 남음";
                        }
                        else
                        {
                            championExpireTimerText[i - 1].text = timeSpan.Seconds + "초 남음";
                        }
                    }
                }
            }
            else
            {
                championExpireTimerText[i - 1].text = "미보유";
            }
        }
    }
    
    private void updateStamina()
    {
        
        long localTick = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        
        long seconds = ((UserInfoManager.Instance.Info.staminaLastTick + (Constants.staminaMinute * 60 * 1000)) - (localTick + UserInfoManager.Instance.diffTick)) / 1000;
        staminaTimerText.text = (seconds / 60 ).ToString("00")  + ":" + (seconds%60).ToString("00");
        
        // 15분 주기
        if ((localTick + UserInfoManager.Instance.diffTick) >= (UserInfoManager.Instance.Info.staminaLastTick + (Constants.staminaMinute * 60 * 1000)))
        {
            Debug.Log("update stamina !!!!!!");
            UserInfoManager.Instance.Info.stamina += 1;
            stamina.text = Utils.GetCommaNum(UserInfoManager.Instance.Info.stamina);
            UserInfoManager.Instance.Info.staminaLastTick = localTick;
        }
        
    }

    // private void SetDropdownOptions() // Dropdown 목록 생성
    // {
    //     dropdown.options.Clear();
    //     int i = 0;
    //     foreach (var entry in UserInfoManager.Instance.userDeckMap)
    //     {
    //         Dropdown.OptionData option = new Dropdown.OptionData();
    //         option.text = entry.Value.name;
    //         dropdown.options.Add(option);
    //
    //         if (entry.Key == GameManager.instance.playerModel.selectDeckId)
    //         {
    //             dropdown.value = GameManager.instance.playerModel.selectDeckId;
    //         }
    //         i++;
    //     }
    // }
 
    // public void SelectButton()
    // {
    //     GameManager.instance.playerModel.setSelectDeckId(dropdown.value);
    // }

    private void OnInstantiateChampionPrefab()
    {
        int championId = GameManager.instance.playerModel.selectChampionId;
        

        GameObject o = GameManager.instance.championStandPrefabList[championId];
        prefabInstanceClones.Add(Instantiate(o));
        var first = prefabInstanceClones.FirstOrDefault();

        first.transform.parent = selectChampion.transform;
        first.transform.localPosition = new Vector3(0, 0, 0);
        
        if (championId == 0)
        {
            first.transform.localScale = new Vector3(6f, 6f, 6f);
        }else if (championId == 1)
        {
            first.transform.localScale = new Vector3(480f, 480f, 480f);
        }else if (championId == 2)
        {
            first.transform.localScale = new Vector3(5f, 5f, 5f);
        }
        
        championName.GetComponent<Text>().text = GameInfoManager.Instance.ChampMap[championId].name;

        // 선택한 챔피언 저장
        GameManager.instance.playerModel.selectChampionId = ChampionModel.getIdByPrefabName(o.name.Substring(0, o.name.IndexOf('(')));
        
        SaveSystem.SavePlayerData(GameManager.instance.playerModel);
    }

    private void OnDestroyChampionPrefab()
    {
        if (prefabInstanceClones.Count > 0)
        {
            var first = prefabInstanceClones.FirstOrDefault();
            prefabInstanceClones.Remove(first);
            Destroy(first);
        }
    }
    
    public void GoTest()
    {
        
        SceneManager.LoadScene("Campaign");
       //StartCoroutine(LoadScene("Campaign"));
    }
    
    public void GoSinglePlayStageScene()
    {
       SceneManager.LoadScene("Campaign");
        // SceneManager.LoadScene("CampaignStepScene");
        //StartCoroutine(LoadScene("SinglePlayStageScene"));
       // StartCoroutine(LoadScene("Campaign"));
    }
    
    
    public void GoMultiPlayScene()
    {
        // if (UserInfoManager.Instance.Info.stamina > 0)
        // {
            DefaultPopup.instance.OpenDefaultPopUp(
            "PvP에 참여하면 1행동력을 소모합니다. 참여하시겠습니까?",
        () =>
                {
                    // 예
                    launcher.Connect();
                },
       () =>
                {
                    // 아니오
                    
                });
        /*}
        else
        {
            DefaultPopup.instance.OpenDefaultPopUp(
                "행동력이 부족합니다. 광고를 시청하면 1행동력을 드립니다. 시청하시겠습니까?",
                () =>
                {
                    // 예
                },
                () =>
                {
                    // 아니오
                });
        }*/
    }

    public void OnChampionTabPopupBtn()
    {
        TabPopup.Instance.OpenTabPopUp(0,
    // SetDropdownOptions, 
            OnDestroyChampionPrefab, 
            OnInstantiateChampionPrefab);
    }
    
  //   public void OnDeckTabPopupBtn()
  //   {
  //       TabPopup.Instance.OpenTabPopUp(1, 
  // SetDropdownOptions,
  //           OnDestroyChampionPrefab,
  //           OnInstantiateChampionPrefab);
  //   }
    
    IEnumerator LoadScene(string sceneName)
    {
        GameManager.instance.transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneName);
    }
    
    public void OpenBuy()
    {
        BuyUseChampionPopup.instance.OpenBuyUseChampionPopUp(
            async () =>
            {
                bool isSuccess = true;
                Toggle toggle = ToggleGroup.ActiveToggles().FirstOrDefault();

                int index = 0;
                int selectChampionId = BuyUseChampionPopup.instance.selectedChampionId;
                
                if (toggle.name == "Option 1")
                {
                    index = 0;
                } else if (toggle.name == "Option 2")
                {
                    index = 1;
                } else if (toggle.name == "Option 3")
                {
                    index = 2;
                }

                PriceModel priceModel = GameInfoManager.Instance.ChampMap[selectChampionId].priceTable[index];
                
                if (UserInfoManager.Instance.Info.gold < priceModel.price)
                {
                    DefaultOneBtnPopup.instance.OpenDefaultOneBtnPopUp(
                        "골드가 부족합니다.",
                        () =>
                        {
                            Debug.Log("구매 실패 : 골드 부족");
                        }
                    );
                    return;
                }

                bool result = await UserInfoManager.Instance.PurchaseChamp(selectChampionId, priceModel.day);
                
                // 서버 통신 성공
                if (result == true)
                {
                    UserInfoManager.Instance.ShowGold();
                    TabPopup.Instance.InitSelectChampion();
                }
            },
            () =>
            {
                Debug.Log("닫기");
            });
    }
}
