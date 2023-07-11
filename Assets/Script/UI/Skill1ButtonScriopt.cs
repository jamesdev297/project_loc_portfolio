using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Skill1ButtonScriopt : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    public PlayerStatusController playerStatusController;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Press Skill1 On");
        playerStatusController.skill1On();
    }
    
    public virtual void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("PressUp Skill1");
        playerStatusController.skill1();
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("PressOff Skill1");
        playerStatusController.skill1Off();
    }
    

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Press Q");
            playerStatusController.skill1On();
        }
        
        if (Input.GetKeyUp(KeyCode.Q))
        {
            Debug.Log("Press Q");
            playerStatusController.skill1();
        }
    }

}
