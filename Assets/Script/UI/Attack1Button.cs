using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class Attack1Button : MonoBehaviour, IPointerDownHandler
{
    public PlayerStatusController playerStatusController;

    private void Start()
    {
        
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Press A");
        playerStatusController.attack();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("Press A");
            playerStatusController.attack();
        }
    }
}
