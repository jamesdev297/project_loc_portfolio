using System.Collections;
using System.Collections.Generic;
using Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainSelectChampionScene : MonoBehaviour
{

    private List<int> championIdList = new List<int>();
    public GameObject championContents;
    void Start()
    {
        GameManager.instance.transition.SetTrigger("End");
        
        
        
        InitChampion();
    }

    private void InitChampion()
    {
        championIdList.Clear();
        
        int index = 0;
        Debug.Log("UserChampionCount : " +UserInfoManager.Instance.userChampionMap.Count);
        Debug.Log("UserChampionCount : " +UserInfoManager.Instance.UserChamps.Count);
        foreach (var champ in GameInfoManager.Instance.Champs)
        {
            championIdList.Add(champ.id);
            
            championContents.transform.GetChild(index).GetChild(0).GetComponent<Image>().color = Color.white;
            championContents.transform.GetChild(index).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/Champion/ThumbNail/" + ChampionModel.GetEnglishNameById(champ.id));
            championContents.transform.GetChild(index).GetChild(1).GetComponent<Text>().text = champ.name;
            championContents.transform.GetChild(index).GetChild(2).GetComponent<Image>().gameObject.SetActive(false);
            championContents.transform.GetChild(index).GetComponent<Outline>().effectColor = Color.white;
            if (!UserInfoManager.Instance.userChampionMap.ContainsKey(champ.id))
            {
                championContents.transform.GetChild(index).GetChild(2).GetComponent<Image>().gameObject.SetActive(true);
            }
            
            if (GameManager.instance.playerModel.selectChampionId == champ.id)
            {
                championContents.transform.GetChild(index).GetComponent<Outline>().effectColor = Color.red;
            }
            index += 1;
        }
    }

    public void GoMainScene()
    {
        StartCoroutine(LoadScene("MainScene"));
    }

    // public void OnClickChampionBtn(int index)
    // {
    //     
    //     if (index > championIdList.Count)
    //         return;
    //     
    //     int championId = championIdList[index];
    //
    //     ChampionModel championModel = GameInfoManager.Instance.ChampMap[championId];
    //     
    //     if (!UserInfoManager.Instance.userChampionMap.ContainsKey(championId))
    //     {
    //         DefaultPopup.instance.OpenDefaultPopUp(
    //             championModel.name + "을 사시겠습니까?\n" + Utils.GetCommaNum(championModel.price) + "골드 입니다.",
    //             () =>
    //             {
    //                 // 예
    //                 BuyChampion(championModel);
    //             },
    //             () =>
    //             {
    //                 // 아니오
    //             });
    //     }
    //     else
    //     {
    //         GameManager.instance.playerModel.selectChampionId = championId;
    //         InitChampion();
    //     }
    // }

    // private async void BuyChampion(ChampionModel championModel)
    // {
    //     if(UserInfoManager.Instance.Info.gold < championModel.price)
    //         DefaultOneBtnPopup.instance.OpenDefaultOneBtnPopUp(
    //             "골드가 부족합니다.",
    //             () =>
    //             {
    //                 // 예
    //             });
    //     else
    //     {
    //         UserInfoManager.Instance.Info.gold -= championModel.price;
    //         // await UserInfoManager.Instance.AddUserChamp(championModel.id, );
    //         //GameManager.instance.playerModel.setSelectChampId(championModel.id);
    //         //SaveSystem.SavePlayerData(GameManager.instance.playerModel);
    //         InitChampion();
    //     }
    // }
    
    IEnumerator LoadScene(string sceneName)
    {
        GameManager.instance.transition.SetTrigger("Start");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(sceneName);
    }
}
