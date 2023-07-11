using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnAttackButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private float defaultChildLocalPosY;
    // Start is called before the first frame update
    void Start()
    {
        defaultChildLocalPosY = transform.GetChild(0).localPosition.y;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.GetChild(0).localPosition = new Vector3(
            transform.GetChild(0).localPosition.x,
            defaultChildLocalPosY - 2.0f,
            transform.GetChild(0).localPosition.z
        );
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        transform.GetChild(0).localPosition = new Vector3(
            transform.GetChild(0).localPosition.x,
            defaultChildLocalPosY,
            transform.GetChild(0).localPosition.z
        );
    }
}
