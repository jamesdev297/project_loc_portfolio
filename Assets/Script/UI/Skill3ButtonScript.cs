using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Skill3ButtonScript : MonoBehaviour, IPointerDownHandler
{
    public PlayerStatusController playerStatusController;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Press Skill3");
        playerStatusController.skill3();
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Press R");
            playerStatusController.skill3();
        }
    }
}
