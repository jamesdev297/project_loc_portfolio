using System;
using System.Collections;
using System.Collections.Generic;
using Script.Model;
using Script.Model.Game;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public List<GameObject> championPrefabList = new List<GameObject>();
    public List<GameObject> championStandPrefabList = new List<GameObject>();
    public Dictionary<string, GameObject> allChampionPrefabMap = new Dictionary<string, GameObject>();

    public static GameManager instance;
    public PlayerModel playerModel;
    public Animator transition;
    public GameObject sceneLoader;
    public GameObject banner;


    public int botChampionId;
    public PlayerModel enemyPlayerModel;
    public ChampionModel EnemyChampionModel;

    public List<int> maxProficiencyByLevelList = new List<int>();
    public GameObject dimObject;
    
    public string persistentDataPath;
    public bool isLeftPlayer = false;
    public bool isBot = false;
    
    public List<string> fakeNames;

    public GameOutModel lastGameModel;
    public string lastRoomName;

    void Awake()
    {
        instance = this;
    }
    
    void Start() {
        DontDestroyOnLoad(sceneLoader);
        DontDestroyOnLoad(banner);
        InitChampion();
        transition.SetTrigger("Init");
        Debug.Log("GameManager Start()...");
    }

    private void Update()
    {
       
    }

    void InitChampion()
    {
        /*allChampionMap.Add("Jax", ChampionFactory.Create(ChampionFactory.ChampionType.Jax));
        allChampionMap.Add("Lucian", ChampionFactory.Create(ChampionFactory.ChampionType.Lucian));
        allChampionMap.Add("Thresh", ChampionFactory.Create(ChampionFactory.ChampionType.Thresh));
        allChampionMap.Add("Orn", ChampionFactory.Create(ChampionFactory.ChampionType.Orn));*/
        
        foreach (var champion in championPrefabList)
        {
            allChampionPrefabMap.Add(champion.name, champion);
        }
        
        maxProficiencyByLevelList.Add(200);
        maxProficiencyByLevelList.Add(200);
        maxProficiencyByLevelList.Add(200);
        maxProficiencyByLevelList.Add(160);
        maxProficiencyByLevelList.Add(260);
        maxProficiencyByLevelList.Add(360);
        maxProficiencyByLevelList.Add(560);
        
    }

    // public void InitPlayerModel()
    // {
    //     Debug.Log("InitPlayerModel!!");
    //     Dictionary<string, UserChampModel> proficiencyMap = new Dictionary<string, UserChampModel>();
    //     Dictionary<string, DeckModel> deckMap = new Dictionary<string, DeckModel>();
    //     List<CardModel> cardList = new List<CardModel>();
    //     cardList.Add(new LongSwordModel());
    //     cardList.Add(new DaggerModel());
    //     cardList.Add(new RubyCrystalModel());
    //     cardList.Add(new ClothArmorModel());
    //     cardList.Add(new VampiricScepterModel());
    //     cardList.Add(new RejuvenationBeadModel());
    //     cardList.Add(new VampiricScepterModel());
    //     cardList.Add(new VampiricScepterModel());
    //     
    //     List<CardModel> deckCardList = new List<CardModel>();
    //     deckCardList.Add(new RejuvenationBeadModel());
    //     deckCardList.Add(new VampiricScepterModel());
    //     
    //     DeckModel deckModel = new DeckModel("deck", deckCardList);
    //     DeckModel deckModel1 = new DeckModel("deck1", new List<CardModel>());
    //     DeckModel deckModel2 = new DeckModel("deck2", new List<CardModel>());
    //     
    //     deckMap.Add("deck", deckModel);
    //     deckMap.Add("deck1", deckModel1);
    //     deckMap.Add("deck2", deckModel2);
    //     
    //     Dictionary<string, bool> championMap = new Dictionary<string, bool>();
    //     foreach (var championModel in GameInfoManager.Instance.CampaignMap.Values)
    //     {
    //         proficiencyMap.Add(championModel.name, new UserChampModel());
    //         if(championModel.name == "Jax")
    //             championMap.Add(championModel.name, true);
    //         else
    //             championMap.Add(championModel.name, false);
    //     }
    //
    //     List<StageModel> stageList = new List<StageModel>();
    //     StageModel stage1 = new StageModel(1, 0, "Thresh");
    //     StageModel stage2 = new StageModel(2, 0, "Lucian");
    //     StageModel stage3 = new StageModel(3, 0, "Jax");
    //     StageModel stage4 = new StageModel(4, 0, "Orn");
    //     
    //     stageList.Add(stage1);
    //     stageList.Add(stage2);
    //     stageList.Add(stage3);
    //     stageList.Add(stage4);
    //     
    //     
    //     // playerModel.InitPlayerModel(playerName.text, "Jax", "deck", deckMap, championMap, cardList, 30000, 3);
    //     playerModel.InitPlayerModel("test", 0, 0);
    //     playerModel.InitChampionAbility();
    //     
    //     SaveSystem.SavePlayerData(playerModel);
    // }
    
    public void GoMainScene() {
        StartCoroutine(LoadScene("MainScene"));
    }
    public IEnumerator LoadScene(string sceneName)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(sceneName);
    }
    
  
    public static IEnumerator FadeOut(AudioSource source, float fadingTime, Func<float, float, float, float> Interpolate)
    {
        float startVolume = source.volume;
        float frameCount = fadingTime / Time.deltaTime;
        float framesPassed = 0;

        while (framesPassed <= frameCount)
        {
            var t = framesPassed++ / frameCount;
            source.volume = Interpolate(startVolume, 0, t);
            yield return null;
        }

        source.volume = 0;
        source.Pause();
    }
    public static IEnumerator FadeIn(AudioSource source, float resultVolume, float fadingTime, Func<float, float, float, float> Interpolate)
    {
        source.Play();
        source.volume = 0;

        float frameCount = fadingTime / Time.deltaTime;
        float framesPassed = 0;

        while (framesPassed <= frameCount)
        {
            var t = framesPassed++ / frameCount;
            source.volume = Interpolate(0, resultVolume, t);
            yield return null;
        }

        source.volume = resultVolume;
    }
}
