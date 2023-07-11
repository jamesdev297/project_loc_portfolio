using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace Script
{
    
public class MatchSystemScript : MonoBehaviour
{
    private PhotonView photonView;
    public GameObject versusText;
    public GameObject leftPlayerNickname;
    public GameObject rightPlayerNickname;
    public Text matchText;
    public GameObject matchPanel;
    public Text detailText;
    
    public GameObject leftPlayerPanel;
    public GameObject rightPlayerPanel;

    private Launcher _launcher;
    public AudioSource matchSound;
    private IEnumerator updateTextEnumerator;
    private IEnumerator updateDetailEnumerator;


    // Start is called before the first frame update
    void Start()
    {
        _launcher = GameObject.Find("Launcher").GetComponent<Launcher>();
        Debug.Log("MatchSystemScript Start()...");
        photonView = PhotonView.Get(this);

        Debug.Log($"SCREEN SIZE {Screen.height}");
        updateTextEnumerator = UpdateMatchText();
        StartCoroutine(updateTextEnumerator);

        updateDetailEnumerator = UpdateDetailText();
        StartCoroutine(updateDetailEnumerator);
    }
    
    IEnumerator UpdateDetailText()
    {
        detailText.text = "";
        yield return new WaitForSeconds(3.0f);

        for (int i = 0; i < 5; i++)
        {
            detailText.text = $"탐색 범위 확대중... ({i+1}/5)";
            yield return new WaitForSeconds(3.5f);
        }
    }
    
    IEnumerator UpdateMatchText()
    {
        int dotCount = 0;
        while (true)
        {
            if (!matchPanel.activeSelf)
                break;

            dotCount = (++dotCount) % 3;
            
            string dot = "";
            for (int i = 0; i <= dotCount; i++)
            {
                dot += ".";
            }
            matchText.text = "매치중" + dot;
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void SendToAnother()
    {
        string championModelDtoJson = JsonUtility.ToJson(ChampionModelDto.newChamiponModel(GameManager.instance.playerModel.selectChampionModel));
        string playerModelDtoJson = JsonUtility.ToJson(new PlayerModelDto(GameManager.instance.playerModel));
        photonView.RPC("RPCMatch", RpcTarget.Others, championModelDtoJson, playerModelDtoJson);
        // photonView.RPC("RPCChatMessage", RpcTarget.All, "selectedChampionJSON");
    }

    public void ShowVersus()
    {
        photonView.RPC("RPCShowVersus", RpcTarget.All);
    }

    public void ShowVersusBot(int botChampionId)
    {
        StopCoroutine(updateTextEnumerator);
        StopCoroutine(updateDetailEnumerator);
        matchPanel.SetActive(false);
        versusText.SetActive(true);
        leftPlayerPanel.SetActive(true);
        rightPlayerPanel.SetActive(true);

        GameObject leftPlayer = null;
        GameObject rightPlayer = null;
        GameObject leftPlayerOffset = GameObject.Find("LeftPlayerOffset");
        GameObject rightPlayerOffset = GameObject.Find("RightPlayerOffset");
        
        int championId = GameManager.instance.playerModel.selectChampionId;
        GameObject o = GameManager.instance.championStandPrefabList[championId];
        GameObject e = GameManager.instance.championStandPrefabList[botChampionId];

        leftPlayer = o;
        rightPlayer = e;
        
        leftPlayerNickname.GetComponent<Text>().text = UserInfoManager.Instance.Info.name;
        rightPlayerNickname.GetComponent<Text>().text = GameManager.instance.enemyPlayerModel.nickname;
       
        GameObject leftPrefab = Instantiate(leftPlayer, leftPlayerOffset.transform, true);
        GameObject rightPrefab = Instantiate(rightPlayer, rightPlayerOffset.transform, true);

        float yLeftLocalPosition = -200;
        float yRightLocalPosition = -200;
        
        leftPrefab.transform.localPosition = new Vector3(
            0.0f,
            yLeftLocalPosition,
            leftPrefab.transform.localPosition.z
        );
        rightPrefab.transform.localPosition = new Vector3(
            0.0f,
            yRightLocalPosition,
            rightPrefab.transform.localPosition.z
        );
    }

    public void GoToNextLevel()
    {
        matchSound.Play();
        photonView.RPC("RPCGoNextLevel", RpcTarget.All);
    }

    public void RequestSendToMe()
    {
        photonView.RPC("RPCRequestSendToMe", RpcTarget.Others);
    }

    [PunRPC]
    public void RPCRequestSendToMe()
    {
        SendToAnother();
    }
    
    [PunRPC]
    public void RPCGoNextLevel()
    {
        StartCoroutine(DelayAndAction(3.0f, () =>
        {
            Destroy(GameObject.Find("Launcher"));
            PhotonNetwork.LoadLevel("Multiplay");
        }));
    }
    
    [PunRPC]
    public void RPCShowVersus()
    {
        StartCoroutine(DelayAndAction(1.0f, () =>
        {
            StopCoroutine(updateDetailEnumerator);
            StopCoroutine(updateTextEnumerator);
            matchPanel.SetActive(false);

            versusText.SetActive(true);
            leftPlayerPanel.SetActive(true);
            rightPlayerPanel.SetActive(true);

            GameObject leftPlayer = null;
            GameObject rightPlayer = null;
            GameObject leftPlayerOffset = GameObject.Find("LeftPlayerOffset");
            GameObject rightPlayerOffset = GameObject.Find("RightPlayerOffset");
        
            
            int championId = GameManager.instance.playerModel.selectChampionId;
            GameObject o = GameManager.instance.championStandPrefabList[championId];
            
            int enemyChampionId = GameManager.instance.enemyPlayerModel.selectChampionId;
            GameObject e = GameManager.instance.championStandPrefabList[enemyChampionId];

            if (GameManager.instance.isLeftPlayer)
            {
                leftPlayer = o;
                rightPlayer = e;

                Debug.Log($"LEFT PLAYER NICKANME : {GameManager.instance.playerModel.nickname}");
                leftPlayerNickname.GetComponent<Text>().text = GameManager.instance.playerModel.nickname;
                rightPlayerNickname.GetComponent<Text>().text = GameManager.instance.enemyPlayerModel.nickname;
            }
            else
            {
                leftPlayer = e;
                rightPlayer = o;
                
                leftPlayerNickname.GetComponent<Text>().text = GameManager.instance.enemyPlayerModel.nickname;
                rightPlayerNickname.GetComponent<Text>().text = GameManager.instance.playerModel.nickname;
            }
            
            
            GameObject leftPrefab = Instantiate(leftPlayer, leftPlayerOffset.transform, true);
            GameObject rightPrefab = Instantiate(rightPlayer, rightPlayerOffset.transform, true);

            float yLeftLocalPosition = -200;
            float yRightLocalPosition = -200;
            
            leftPrefab.transform.localPosition = new Vector3(
                0.0f,
                yLeftLocalPosition,
                leftPrefab.transform.localPosition.z
            );
            rightPrefab.transform.localPosition = new Vector3(
                0.0f,
                yRightLocalPosition,
                rightPrefab.transform.localPosition.z
            );
        }));
    }

    IEnumerator DelayAndAction(float delayedSec, Action action)
    {
        yield return new WaitForSeconds(delayedSec);
        action();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [PunRPC]
    void RPCMatch(string championModelDtoJson, string playerModelDtoJson)
    {
        ChampionModelDto championModelDto = JsonUtility.FromJson<ChampionModelDto>(championModelDtoJson);
        PlayerModelDto playerModelDto = JsonUtility.FromJson<PlayerModelDto>(playerModelDtoJson);
        Debug.Log("playerModelDto : " + playerModelDto.nickname);
        
        ChampionModel championModel = ChampionFactory.Create(championModelDto.championType);
        championModel.attackDamage = championModelDto.attackDamage;
        championModel.attackSpeed = championModelDto.attackSpeed;
        championModel.movementSpeed = championModelDto.movementSpeed;
        championModel.armor = championModelDto.armor;
        championModel.health = championModelDto.health;
        championModel.currentHealth = championModel.health;
        PlayerModel playerModel = new PlayerModel(playerModelDto);

        Debug.Log("championModel MATT : " + JsonUtility.ToJson(championModel));

        GameManager.instance.enemyPlayerModel = playerModel;
        GameManager.instance.EnemyChampionModel = championModel;
    }

}
}
