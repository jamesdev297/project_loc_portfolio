using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Script;
public class PhotonParent : MonoBehaviour
{
    private void Awake()
    {
        if (PhotonNetwork.IsConnected)
        {
            if(gameObject.name != "Player")
            {
                gameObject.transform.SetParent(GameObject.Find("PlayerOffset(Clone)").transform);
            }
            else
            {
                Debug.Log("Not found parent.");
            }
        }
    }
}
