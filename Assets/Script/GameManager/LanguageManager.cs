using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable] //인스펙터 테이블에 노출시키기 위함.
public class Lang
{
    public string lang,langLocalize; // lang에는 언어를 영어로 ex) Korean, langLocalize  언어의 현지화 이름을 담아줄 것이다. ex)한국어
    public List<string> value = new List<string>(); // 언어의 Text value들을 담아준다.
}

public class LanguageManager : MonoBehaviour 
{

    //싱글톤 선언
    public static LanguageManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }

        else Destroy(this);

        InitLang();
    }

    const string langURL = "https://docs.google.com/spreadsheets/d/1d6Ml2PspOrKmDmymyTS0_fRSTIIQVQZ9s--4E0ksd5Y/export?format=tsv"; //언어 데이터가 담겨진 구글 스프레드 시트의 링크
    public event System.Action LocalizeChanged = () => {};  
    public event System.Action LocalizeSettingChanged = () => {};

    //언어가 바뀌는 것을 알려주기 위해 Action 선언.

    public int curLangIndex;    // 현재 언어의 인덱스
    public List<Lang> Langs;    // 언어 데이터 클래스의 리스트


    // InitLang 함수에서는 저장해놓은 언어 인덱스값이 있다면 가져오고 , 없다면 기본언어(영어)의 인덱스 값을 가져온다.
    void InitLang()
    {
        int langIndex = PlayerPrefs.GetInt("LangIndex", -1);
        int systemIndex = Langs.FindIndex(x => x.lang.ToLower() == Application.systemLanguage.ToString().ToLower());
        if (systemIndex == -1) systemIndex = 0;
        int index = langIndex == -1 ? systemIndex : langIndex;

        SetLangIndex(index); // 값을 가져온 뒤 SetLangIndex의 매개변수로 넣어준다 
    }


    public void SetLangIndex(int index)
    {
        curLangIndex = index;   //initlang에서 구한 언어의 인덱스 값을 curLangIndex에 넣어줌 
        PlayerPrefs.SetInt("LangIndex", curLangIndex);  //저장
        LocalizeChanged();  //텍스트들 현재 언어로 변경
        LocalizeSettingChanged();   //드랍다운의 value변경
    }


    [ContextMenu("언어 가져오기")]    //ContextMenu로 게임중이 아닐 때에도 실행 가능 
    void GetLang()
    {
        StartCoroutine(GetLangCo());
    }

    IEnumerator GetLangCo()
    {
        UnityWebRequest www = UnityWebRequest.Get(langURL); //스프레드 시트의 url을 가져오고
        yield return www.SendWebRequest();  //가져올 때 까지 대기 
        SetLangList(www.downloadHandler.text);  //스프레드 시트의 데이터 값을 SetLangList에 넣어준다.
    }

    void SetLangList(string tsv)    
    {
        // 이차원 배열 생성

        string[] row = tsv.Split('\n'); //스페이스를 기준을 행 분류 
        int rowSize = row.Length;
        int columnSize = row[0].Split('\t').Length; //탭을 기준으로 열 분류
        string[,] Sentence = new string[rowSize, columnSize];   // 이차원 배열 선언


        // 이차원 배열에 데이터 넣어주기
        for (int i = 0; i < rowSize; i++)
        {
            string[] column = row[i].Split('\t');
            for (int j = 0; j < columnSize; j++)
                Sentence[i, j] = column[j];
        }

        Langs = new List<Lang>();   //새로운 Langs 객체 생성하고,

        for (int i = 0; i < columnSize; i++)
        {
            Lang lang = new Lang(); //하나의 언어를 의미하는 객체 생성
            lang.lang = Sentence[0, i]; //언어의 영어 이름 
            lang.langLocalize = Sentence[1, i]; //언어의 현지화 이름

            for (int j = 2; j < rowSize; j++) lang.value.Add(Sentence[j, i]);   //텍스트 value들을 객체에 넣어준다.
            Langs.Add(lang);    //언어들이 담겨있는 리스트에 추가
        }
    }
}