using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using System.Threading.Tasks;
using System.Collections.Concurrent;

public class FirebaseLogin : MonoBehaviour
{
    [SerializeField]
    GameObject loginFailedWindow;

    [SerializeField]
    Text userInfoText;

    string clientId = "690125153937-uh7mddadmhiuuedougi9rmhhgpl45e4q.apps.googleusercontent.com";
    string clientSecret = "WekTH5jBNl9z7SkxYT1ButcS";

    ConcurrentQueue<string> commandQueue = new ConcurrentQueue<string>();

    public void LoginAnonym()
    {
        FirebaseAuth.Instance.LoginAnynim();
    }

    public void LoginGoogle()
    {
        //var googleSignInScript = AndroidGoogleSignIn.Init(this.gameObject);
        //googleSignInScript.SignIn(clientId, GoogleSuccessCallback, GoogleErrorCallback);
    }

    private void GoogleErrorCallback(string obj)
    {
        Debug.LogError("GoogleErrorCallback. " + obj);
    }

    //private void GoogleSuccessCallback(AndroidGoogleSignInAccount obj)
    //{
    //    Debug.Log($"GoogleSuccessCallback. {obj.DisplayName} / {obj.Email} / {obj.Token}");
    //
    //    FirebaseAuth.Instance.LoginGoogle(obj.Token);
    //}

    public void Update()
    {
        if (FirebaseAuth.Instance.LoginResultQueue.TryDequeue(out var loginResult))
        {
            Task.Run(async () =>
            {
                if (loginResult.Success)
                {
                    var userInfo = await UserInfoManager.Instance.GetUserInfo();
                    Debug.Log(userInfo);

                    if (userInfo.name == "")
                    {
                        commandQueue.Enqueue("SetNickname");
                    }
                    else
                    {
                        commandQueue.Enqueue("PrintUserInfo");
                    }
                }
                else
                {
                    commandQueue.Enqueue("ShowLoginFailedWindow");
                }
            });
        }

        if (commandQueue.TryDequeue(out var command))
        {
            switch (command)
            {
                case "SetNickname":
                    NicknameManager.Instance.ShowSetNameWindow();
                    break;
                case "PrintUserInfo":
                    userInfoText.text = UserInfoManager.Instance.Info.ToString();
                    break;
                case "ShowLoginFailedWindow":
                    loginFailedWindow.SetActive(true);
                    break;
                default:
                    Debug.LogError($"Unknown Command. {command}");
                    break;
            }
        }
    }
}
