using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using LitJson;
using Script.Model.Game;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LogoSceneScript : MonoBehaviour
{
    private Animator _animator;

    public GameObject errPanel;

    public Text contentText;

    private Action pressAction;
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void StartCheckVersion()
    {
        StartCoroutine(checkVersionEnumerator());
    }

    public async Task<bool> CheckVersion()
    {
        try
        {
            string json = await WebCall.Instance.GetRequestAsync($"/server-info");
            JsonData jsonData = JsonMapper.ToObject(json);
            Debug.Log($"CheckVersion {json} {Application.version}");

            bool active = (bool) jsonData["active"];
            if (!active)
            {
              errPanel.SetActive(true);
              contentText.text = (string) jsonData["msg"];
            } else if (Application.version.CompareTo(((string) jsonData["clientVersion"])) < 0)
            {
              errPanel.SetActive(true);  
              contentText.text = "The game’s version is outdated.\nPlease use it after updating.";
              pressAction = () =>
              {
                  if (Application.platform == RuntimePlatform.Android)
                  {
                      Application.OpenURL("market://details?id=com.pugstudio.battleOfLegends");
                  }else if (Application.platform == RuntimePlatform.IPhonePlayer)
                  {
                      // Application.OpenURL("itms-apps://itunes.apple.com/app/id1602261003");
                  }
                  else
                  {
                      Application.Quit();
                      Debug.Log("NOPE");
                  }
              };
              
            } else
            {
                _animator.SetBool("done", true);
            }
            return false;
        }
        catch (Exception e)
        {
            Debug.LogError($"Error. {e.Message}");
        }
        return false;
    }

    
    IEnumerator checkVersionEnumerator()
    {
        Task task = CheckVersion();
        yield return new WaitUntil(() => task.IsCompleted);
    }
    
    public void GoNextPage()
    {
        SceneManager.LoadScene("IntroScene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PressOkButton()
    {
        if (pressAction != null)
            pressAction();
        else
        {
            Application.Quit();
        }
    }
}
