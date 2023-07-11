using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlurScript : MonoBehaviour, IPointerDownHandler
{
    public CampaignStepScene campaignStepScene;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        campaignStepScene.OnTapBlur();
    }

}
