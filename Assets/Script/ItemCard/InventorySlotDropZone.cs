using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotDropZone: MonoBehaviour, IDropHandler
{
    
    public GameObject myDeck;
    
    public void OnDrop(PointerEventData eventData)
    {
        // Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        // TabPopup tabPopup = this.GetComponentInParent<Canvas>().GetComponent<TabPopup>();
        //
        // if (d != null && d.parentRectTrans.parent != transform.parent)
        // {
        //
        //     tabPopup.deckItemCardIds[int.Parse(d.transform.parent.name)] = -1;
        //
        //     // 덱 카드 제거
        //     if(myDeck.transform.GetChild(int.Parse(d.transform.parent.name) + 2).childCount > 0)
        //         Destroy(myDeck.transform.GetChild(int.Parse(d.transform.parent.name) + 2).GetChild(0).gameObject);
        //
        //     tabPopup.OnDeckAbility();
        // }
    }
    
}