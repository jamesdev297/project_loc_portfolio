using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RewardCardScript : MonoBehaviour, IPointerDownHandler
{
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
        //Debug.Log("Tap this" + GameObject.FindWithTag("Dim").GetComponent<DimScript>());
        //GameObject.FindWithTag("Dim").GetComponent<DimScript>().SelectRewardCard(gameObject);
        GameManager.instance.dimObject.GetComponent<DimScript>().SelectRewardCard(gameObject);
    }
}
