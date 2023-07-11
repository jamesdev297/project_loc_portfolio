using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Skill2ButtonScript : MonoBehaviour, IPointerDownHandler
{
    public PlayerStatusController playerStatusController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Press Skill2");
        playerStatusController.skill2();
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Press E");
            playerStatusController.skill2();
        }
    }
}
