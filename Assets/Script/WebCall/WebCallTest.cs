using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebCallTest : MonoBehaviour
{
    [SerializeField]
    Text infoText;

    [SerializeField]
    Text logText;

    ConcurrentQueue<ResponsePack> responseQueue = new ConcurrentQueue<ResponsePack>();

    public void Exitcampaign()
    {
        string uid = FirebaseAuth.Instance.User.UserId;
        int campaignId = 0;
        int champId = 0;
        bool win = true;

        string jsonBody = $"{{ \"uid\":\"{uid}\", \"campaignId\":{campaignId}, \"champId\":{champId}, \"win\":true }}";

        WebCall.Instance.PostRequest("/action/exit-campaign", jsonBody, responseQueue, "Exitcampaign");
    }

    public void PurchaseChamp()
    {
        string uid = FirebaseAuth.Instance.User.UserId;
        int champId = 0;

        string jsonBody = $"{{ \"uid\":\"{uid}\", \"champId\":{champId} }}";

        WebCall.Instance.PostRequest("/action/purchase-champ", jsonBody, responseQueue, "PurchaseChamp");
    }

    public void ADRewardStamina()
    {
        string uid = FirebaseAuth.Instance.User.UserId;
        string key = "adkey";

        string jsonBody = $"{{ \"uid\":\"{uid}\", \"key\":\"{key}\" }}";

        WebCall.Instance.PostRequest("/action/ad-reward-stamina", jsonBody, responseQueue, "ADRewardStamina");
    }

    public void PlayMulti()
    {
        string uid = FirebaseAuth.Instance.User.UserId;

        string jsonBody = $"{{ \"uid\":\"{uid}\" }}";

        WebCall.Instance.PostRequest("/action/play-multi", jsonBody, responseQueue, "PlayMulti");
    }

    public void AddGold()
    {
        string uid = FirebaseAuth.Instance.User.UserId;
        int gold = 1000;

        string jsonBody = $"{{ \"uid\":\"{uid}\", \"gold\":{gold} }}";

        WebCall.Instance.PostRequest("/admin/add-gold", jsonBody, responseQueue, "AddGold");
    }

    public void GetUserInfo()
    {
        string uid = FirebaseAuth.Instance.User.UserId;

        WebCall.Instance.GetRequest($"/rest/user/{uid}", responseQueue, "GetUserInfo");
    }

    public void Update()
    {
        if (responseQueue.TryDequeue(out var responsePack))
        {
            Debug.Log($"Key:{responsePack.Key}, ResponseCode:{responsePack.ResponseCode}, Body:{responsePack.Body}");

            logText.text = $"Key:{responsePack.Key}\nResponseCode:{responsePack.ResponseCode}\nBody:{responsePack.Body}";

            if (responsePack.Key == "GetUserInfo")
            {
                var userInfo = JsonUtility.FromJson<Json.UserInfo>(responsePack.Body);
                
                infoText.text = userInfo.ToString();
            }
        }
    }
    public void ResetAccount()
    {
        
        WebCall.Instance.PostRequest("/admin/reset", "", responseQueue, "ResetAccount");

    }
}
