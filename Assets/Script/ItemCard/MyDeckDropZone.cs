using System;
using System.Collections.Generic;
using Json;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MyDeckDropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{

    public GameObject itemCard;

    public void OnDrop(PointerEventData eventData)
    {
        // Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        // TabPopup tabPopup = this.GetComponentInParent<Canvas>().GetComponent<TabPopup>();
        //
        // int cardId = CardModel.getIdByPrefabName(d.name);
        // // int itemCardCount = UserInfoManager.Instance.userCardMap[cardId].count;
        //
        // int deckItemCardCount = 0;
        //
        // int count = 0;
        //
        // foreach (var tabPopupDeckItemCardId in tabPopup.deckItemCardIds)
        // {
        //     if (tabPopupDeckItemCardId == cardId)
        //         deckItemCardCount += 1;
        //
        //     if (tabPopupDeckItemCardId != -1)
        //         count += 1;
        // }
        //
        // if (d != null && count < 5 && deckItemCardCount < itemCardCount
        //     && d.parentRectTrans.parent.parent != transform.parent && transform.childCount == 0)
        // {
        //     GameObject newItemCardObj = Instantiate(itemCard, transform);
        //     string enName = CardModel.ConvertIdToEnName(cardId).ToString();
        //     newItemCardObj.name = enName;
        //     newItemCardObj.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/UI/CardItem/" + enName);
        //     newItemCardObj.transform.SetParent(transform);
        //     newItemCardObj.transform.localPosition = new Vector3(0, 0, 0);
        //     tabPopup.deckItemCardIds[int.Parse(transform.name)] = cardId;
        //     
        //     tabPopup.OnDeckAbility();
        // }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("OnPointerEnter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("OnPointerExit");
    }
}
