using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Globalization;
using System.Threading.Tasks;
using ExitGames.Client.Photon.StructWrapping;
using LitJson;
using Script.Model.Game;
using UnityEngine.UI;

public class LeaderBoardManager : MonoBehaviour
{
    public GameObject rankListPanel;
    private Animator tabAnim;
    public Transform content;
    public GameObject rankItem;

    public GameObject loadingText;
    public GameObject header;
    // public GameObject myRank;
    private List<GameObject> items = new List<GameObject>();
    List<RankModel> rankModels = new List<RankModel>();
    void Awake()
    {
    }

    private void Start()
    {
        
    }

    void addItem(int num, string nickname, int elo)
    {
        GameObject temp = Instantiate(rankItem, content.position, Quaternion.identity);
        temp.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        temp.transform.SetParent(content, false);
        temp.transform.GetChild(0).GetComponent<Text>().text = num.ToString();
        temp.transform.GetChild(1).GetComponent<Text>().text = nickname;
        temp.transform.GetChild(2).GetComponent<Text>().text = elo.ToString();

        if (num == 1)
        {
            temp.transform.GetChild(3).GetComponent<Image>().color = Color.yellow;

        }else if (num == 2)
        {
            temp.transform.GetChild(3).GetComponent<Image>().color = Color.grey;

        }else
        {
            temp.transform.GetChild(3).gameObject.SetActive(false);
        }
        items.Add(temp);
    }

    public async Task<bool> GetRanks()
    {
        try
        {
            string json = await WebCall.Instance.GetRequestAsync($"/rest/multi/leaderboard");
            JsonData jsonData = JsonMapper.ToObject(json);
            Debug.Log($"GetRanks {json}");
            
            if (jsonData["users"] != null)
            {
                for (int i = 0; i < jsonData["users"].Count; i++)
                {
                    rankModels.Add(RankModel.FromJson(jsonData["users"][i]));
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error. {e.Message}");
        }
        return false;
    }

    
    IEnumerator LoadTopRank()
    {
        Task task = GetRanks();
        yield return new WaitUntil(() => task.IsCompleted);
        loadingText.SetActive(false);
        header.SetActive(true);
        ShowRanks();
    }

    void ShowRanks()
    {
        for (int i=0;i<rankModels.Count;i++)
        {
            var rankModel = rankModels[i];
            addItem(i + 1, rankModel.name, (int) Mathf.Round((float) rankModel.elo));
        }
    }
    
    public void hide()
    {
        foreach (var item in items)
        {
            Destroy(item);
        }
        items.Clear();
        rankModels.Clear();

        header.SetActive(false);
        tabAnim.SetTrigger("close");
        Invoke("DisableTab", 0.6f);
    }
    
    void DisableTab()
    {
        if(tabAnim.gameObject.activeSelf)
            rankListPanel.SetActive(false);
    }
    public void show()
    {
        rankListPanel.SetActive(true);
        tabAnim = rankListPanel.GetComponent<Animator>();
        StartCoroutine(LoadTopRank());
        loadingText.SetActive(true);
    }
    


}
