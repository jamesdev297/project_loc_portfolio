using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CampaignStepScene : MonoBehaviour
{
    public int selectedStep;
    public List<string> levelOfDifficultyList = new List<string>();
    List<GameObject> prefabInstanceClones = new List<GameObject>();
    public Text levelOfDifficultText;
    public int currentLevelOfDifficultyIndex = 0;

    public GameObject stepPopUp;
    public GameObject blur;

    public GameObject difficultLeftButton;
    public GameObject difficultRightButton;

    public GameObject reward1;
    public GameObject reward2;
    public GameObject reward3;

    private Dictionary<int, List<string>> rewardByDifficultMap = new Dictionary<int, List<string>>();
    
    private void Start()
    {
        GameManager.instance.transition.SetTrigger("End");
        
        // GameObject stage = GameObject.Find("Step" + selectedStep);
        // stage.GetComponent<Outline>().enabled = true;
    }

    private string GetDifficultyName(int difficulty)
    {
        switch (difficulty)
        {
            case 0:
                return "쉬움";
                break;
            case 1:
                return "보통";
                break;
            case 2:
                return "어려움";
                break;
        }

        return "";
    }

    public void initStepPopUp(int step)
    {
        // levelOfDifficultyList.Clear();
        // rewardByDifficultMap.Clear();
        //
        // List<CampaignModel> campaignModels = GameInfoManager.Instance.StageStepMap["1:" + step];
        // foreach (var campaignModel in campaignModels)
        // {
        //     levelOfDifficultyList.Add(GetDifficultyName(campaignModel.difficulty));
        //     foreach (var rewardCard in campaignModel.reward.cards)
        //     {
        //         if (!GameInfoManager.Instance.CardMap.ContainsKey(rewardCard.cardId))
        //             return;
        //         
        //         CardModel cardModel = GameInfoManager.Instance.CardMap[rewardCard.cardId];
        //         if (!rewardByDifficultMap.ContainsKey(campaignModel.difficulty))
        //         {
        //             rewardByDifficultMap.Add(campaignModel.difficulty, new List<string>());
        //         }
        //         rewardByDifficultMap[campaignModel.difficulty].Add(CardModel.ConvertIdToEnName(cardModel.id).ToString());
        //     }
        // }
        //
        // UpdateDifficult();
    }

    public void OnSelectStepBtn(int stepNumber)
    {
        initStepPopUp(stepNumber);
        
        Debug.Log("clear stage : " + UserInfoManager.Instance.clearStep);
        if (UserInfoManager.Instance.clearStep + 1 < stepNumber)
            return;
        
        /*GameObject beforeStage = GameObject.Find("Step" + selectedStep);
        beforeStage.GetComponent<Outline>().enabled = false;
        GameObject afterStage = GameObject.Find("Step" + stepNumber);
        afterStage.GetComponent<Outline>().enabled = true;
        */

        selectedStep = stepNumber;

        currentLevelOfDifficultyIndex = 0;
        
        UpdateDifficult();

        stepPopUp.SetActive(true);
        blur.SetActive(true);
    }
    
    public void GoSinglePlayBtn()
    {
        DefaultPopup.instance.OpenDefaultPopUp(
            "전투를 할려면 광고를 시청해야합니다.\n시청하시겠습니까?",
            () =>
            {
                SceneManager.LoadScene("Campaign");             
            },
            () =>
            {
                // 아니오
            });
    }

    private void UpdateDifficult()
    {
        levelOfDifficultText.text = levelOfDifficultyList[currentLevelOfDifficultyIndex];

        List<GameObject> images = new List<GameObject>();
        images.Add(reward1);
        images.Add(reward2);
        images.Add(reward3);
        
        foreach (var image in images)
        {
            image.SetActive(false);
        }
        
        for (var i = 0; i < rewardByDifficultMap[currentLevelOfDifficultyIndex].Count; i++)
        {
            var cardName = rewardByDifficultMap[currentLevelOfDifficultyIndex][i];
            images[i].SetActive(true);
            images[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/UI/CardItem/" + cardName);
        }

        if(currentLevelOfDifficultyIndex == 0)
            difficultLeftButton.SetActive(false);
        else
            difficultLeftButton.SetActive(true);
        
        if(currentLevelOfDifficultyIndex == levelOfDifficultyList.Count-1)
            difficultRightButton.SetActive(false);
        else
            difficultRightButton.SetActive(true);
    }
    
    public void OnRightBtn()
    {
        Debug.Log("right btn");

        if (currentLevelOfDifficultyIndex < levelOfDifficultyList.Count - 1)
            currentLevelOfDifficultyIndex += 1;
        
        UpdateDifficult();
    }
    
    public void OnLeftBtn()
    {
        Debug.Log("left btn");
        
        if (currentLevelOfDifficultyIndex > 0)
            currentLevelOfDifficultyIndex -= 1;
        
        UpdateDifficult();
    }
    
    public void GoMainScene()
    {
        GameManager.instance.GoMainScene();
    }
    
    IEnumerator DelayedInactivePopUp()
    {
        yield return new WaitForSeconds(0.5f);
        stepPopUp.SetActive(false);
    }
    
    public void OnTapBlur()
    {
        stepPopUp.GetComponent<Animator>().SetTrigger("close");
        blur.SetActive(false);
        StartCoroutine(DelayedInactivePopUp());
    }
    
}