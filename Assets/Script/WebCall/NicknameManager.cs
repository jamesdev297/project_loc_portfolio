using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class NicknameManager : MonoBehaviour
{
    #region singleton
    private static NicknameManager instance;
    public static NicknameManager Instance
    {
        get
        {
            return instance;
        }
    }
    #endregion

    [SerializeField]
    Text userInfoText;

    [SerializeField]
    GameObject setNameWindow;

    [SerializeField]
    InputField nameInputField;

    ConcurrentQueue<ResponsePack> responseQueue = new ConcurrentQueue<ResponsePack>();

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (responseQueue.TryDequeue(out ResponsePack responsePack))
        {
            switch (responsePack.Key)
            {
                case "SetName":
                    if (responsePack.ResponseCode != 200)
                    {
                        // Error
                    }
                    else
                    {
                        Debug.Log($"Success Set Nickname. {responsePack.Body}");

                        try
                        {
                            var userInfo = JsonUtility.FromJson<Json.UserInfo>(responsePack.Body);

                            setNameWindow.SetActive(false);

                            userInfoText.text = userInfo.ToString();
                        }
                        catch (Exception e)
                        {
                            Debug.LogError($"Error. {e.Message}");
                        }
                    }
                    break;
            }
        }
    }

    public void ShowSetNameWindow()
    {
        setNameWindow.SetActive(true);
    }

    public void SetName(string name)
    {
        var instance = WebCall.Instance;

        var url = $"/action/set-name";

        // local
        instance.PostRequest(url, $"{{ \"uid\":\"{FirebaseAuth.Instance.User.UserId}\", \"name\":\"{name}\" }}", responseQueue, "SetName");
        
        // live
        //instance.PostRequest(url, $"{{ \"name\":\"{name}\"  }}", responseQueue, "SetName");
    }

    public void OnClickSetName()
    {
        SetName(nameInputField.text);
    }
}
