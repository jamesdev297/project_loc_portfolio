using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ExitGames.Client.Photon.StructWrapping;
using LitJson;
using Photon.Pun;
using Script.Model.Game;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using WebSocketSharp;

public class GameSetDimScript : MonoBehaviour
{
    private GameOutModel _gameOutModel;
    public GameObject gameSetDim;

    private Text myPlayerResult;
    private Text enemyPlayerResult;

    private Text myPlayerNickname;
    private Text enemyPlayerNickname;

    public AudioSource victorySound;
    public AudioSource loseSound;

    public AudioSource goldSound;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"GameSetDimScript {GameManager.instance.lastGameModel == null}");
        if (GameManager.instance.lastGameModel == null)
        {
            StartCoroutine(GetGameResultPeriodic());
        }
        else
        {
            _gameOutModel = GameManager.instance.lastGameModel;
            NextStep();
        }
    }

    public string GetLastGameId()
    {
        return _gameOutModel.GetGameId();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    string GetWinText()
    {
        return "승리";
    }

    string GetLoseText()
    {
        return "패배";
    }
    
    string GetDrawText()
    {
        return "무승부";
    }

    void NextStep()
    {
        GameObject.Find("LoadingText").SetActive(false);
        gameSetDim.SetActive(true);
        
        myPlayerResult = GameObject.Find("MyResultText").GetComponent<Text>();
        enemyPlayerResult = GameObject.Find("EnemyResultText").GetComponent<Text>();
        myPlayerNickname = GameObject.Find("MyPlayerNickname").GetComponent<Text>();
        enemyPlayerNickname = GameObject.Find("EnemyPlayerNickname").GetComponent<Text>();

        myPlayerNickname.text = GameManager.instance.playerModel.nickname;
        enemyPlayerNickname.text = GameManager.instance.enemyPlayerModel.nickname;

        if (_gameOutModel.GetWinnerUid().IsNullOrEmpty())
        {
            myPlayerResult.text = GetDrawText();
            enemyPlayerResult.text = GetDrawText();
        } else if (_gameOutModel.GetWinnerUid().Equals(FirebaseAuth.Instance.User.UserId))
        {
            victorySound.Play();
            myPlayerResult.text = GetWinText();
            enemyPlayerResult.text = GetLoseText();
        } else
        {
            loseSound.Play();
            myPlayerResult.text = GetLoseText();
            enemyPlayerResult.text = GetWinText();
        }

        Text gainGoldText = GameObject.Find("GainGoldText").GetComponent<Text>();
        Text gainEloText = GameObject.Find("ScoreText").GetComponent<Text>();

        GamePlayerModel gamePlayerModel =
            _gameOutModel.GetPlayerA().GetUid().Equals(GameManager.instance.playerModel.uid)
                ? _gameOutModel.GetPlayerA()
                : _gameOutModel.GetPlayerB();
        int gainingGold = gamePlayerModel.GetGainingGold();
        gainGoldText.text = (gainingGold > 0 ? "+": "") + gainingGold;
        int gainingElo = Mathf.RoundToInt((float) gamePlayerModel.GetGainingElo());
        gainEloText.text = (gainingElo > 0 ? "+" : "") + gainingElo;
        
        Debug.Log($"NextStep {UserInfoManager.Instance.Info.elo} {gamePlayerModel.GetGainingElo()}");
        UserInfoManager.Instance.Info.elo += Mathf.RoundToInt((float) gamePlayerModel.GetGainingElo());
        UserInfoManager.Instance.Info.gold += gamePlayerModel.GetGainingGold();


        int selectChampionId = GameManager.instance.playerModel.selectChampionId;
        GameObject myChamp = Instantiate(GameManager.instance.championStandPrefabList[selectChampionId],
            GameObject.Find("MyPlayerOffset").transform, true);
        if (selectChampionId != 1)
        {
            float size = 7.1f;
            myChamp.transform.localScale = new Vector3(size, size, size);
            myChamp.transform.localPosition = new Vector3(0.0f, -150.0f, 0.0f);
        }
        else
        {
            float size = 556.0f;
            myChamp.transform.localScale = new Vector3(size, size, size);
            myChamp.transform.localPosition = new Vector3(0.0f, -180.0f, 0.0f);
        }

        GameObject enemyChamp = Instantiate(GameManager.instance.championStandPrefabList[GameManager.instance.enemyPlayerModel.selectChampionId],
            GameObject.Find("EnemyPlayerOffset").transform, true);
        enemyChamp.transform.localPosition = new Vector3(0.0f, -100.0f, 0.0f);
            
    }

    public void UpdateEarnGoldMore()
    {
        goldSound.Play();
        Text gainGoldText = GameObject.Find("GainGoldText").GetComponent<Text>();

        GamePlayerModel gamePlayerModel =
            _gameOutModel.GetPlayerA().GetUid().Equals(GameManager.instance.playerModel.uid)
                ? _gameOutModel.GetPlayerA()
                : _gameOutModel.GetPlayerB();
        int gainingGold = gamePlayerModel.GetGainingGold() * 3;
        gainGoldText.text = (gainingGold > 0 ? "+": "") + gainingGold;
    }

    IEnumerator GetGameResultPeriodic()
    {
        Task task = GetGameResult();
        yield return new WaitUntil(() => task.IsCompleted);
        NextStep();
    }
    
    public async Task<bool> GetGameResult()
    {
        for (int i = 0; i < 10; i++)
        {
            try
            {
                string json = await WebCall.Instance.GetRequestAsync($"/rest/multi/games/{GameManager.instance.lastRoomName}");
                JsonData jsonData = JsonMapper.ToObject(json);
                Debug.Log($"GetGameResult {json}");
                GameOutModel gameOutModel = GameOutModel.FromJson(jsonData);
                DateTime gameoutTime = DateTime.Parse(gameOutModel.gameoutDttm);
                DateTime utcTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                

                 if (gameOutModel.GetStatus().Equals("closed"))
                {
                    _gameOutModel = gameOutModel;
                    return true;
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error. {e.Message}");
            }
            await Task.Delay(1500);
        }

        return false;
    }

    public void OnPressOKButton()
    {
        GameManager.instance.isBot = false;
        GameManager.instance.lastRoomName = null;
        GameManager.instance.lastGameModel = null;
        SceneManager.LoadScene("MainScene");
    }
}
