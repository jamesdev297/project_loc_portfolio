using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampaignScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.transition.SetTrigger("End");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
