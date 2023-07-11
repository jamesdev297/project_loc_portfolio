

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Draggable: MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IDragHandler
{
    private RectTransform rectTrans;
    public RectTransform parentRectTrans;
    private Canvas rootCanvas;
    private Vector2 offset;
    private Vector3 parentOffset;

    public GameObject attackDamagePrefab;
    public GameObject armorPrefab;
    public GameObject healthPrefab;
    public GameObject attackSpeedPrefab;
    public GameObject movementSpeedPrefab;
    private GameObject itemAbilityPanel;
    List<GameObject> prefabInstanceClones = new List<GameObject>();

    
    void Awake()
    {
        rectTrans = this.GetComponent<RectTransform>();
        parentRectTrans = rectTrans.parent as RectTransform;
        this.rootCanvas = this.GetComponentInParent<Canvas>();
        itemAbilityPanel = this.rootCanvas.transform.GetChild(0).GetChild(3).GetChild(0).GetChild(0).GetChild(0).gameObject;
    }

    private void HideItemCardAbility()
    {
        int count = itemAbilityPanel.transform.GetChild(2).childCount;

        if (count > 0)
        {
            for (int i = 0; i < count; i++)
            {
                GameObject o = itemAbilityPanel.transform.GetChild(2).GetChild(0).gameObject;
                Destroy(o);
            }
        }
        
        while (prefabInstanceClones.Count > 0)
        {
            var first = prefabInstanceClones.FirstOrDefault();
            prefabInstanceClones.Remove(first);
            Destroy(first);
        }
    }
    
    private void ShowItemCardAbility()
    {
        // HideItemCardAbility();
        //
        // itemAbilityPanel.SetActive(true);
        //
        // int itemId = CardModel.getIdByPrefabName(transform.name);
        //
        // Image cardImage = itemAbilityPanel.transform.GetChild(0).GetComponent<Image>();
        // Text cardName = itemAbilityPanel.transform.GetChild(1).GetComponent<Text>();
        // GameObject parent = itemAbilityPanel.transform.GetChild(2).gameObject;
        //
        // cardImage.sprite = Resources.Load<Sprite>("Image/UI/CardItem/"+transform.name);
        // cardImage.color = Color.white;
        // cardName.text = GameInfoManager.Instance.CardMap[itemId].GETName();
        //
        // CardModel cardModel = GameInfoManager.Instance.CardMap[itemId];
        // GameObject newObj;
        //
        // int index = 0;
        // foreach (var abilityModel in cardModel.GETAbilities())
        // {
        //     if("AttackDamage".Equals(abilityModel.GETName()))
        //         newObj = Instantiate(attackDamagePrefab, parent.transform);
        //     else if("Armor".Equals(abilityModel.GETName()))
        //         newObj = Instantiate(armorPrefab, parent.transform);
        //     else if("Health".Equals(abilityModel.GETName()))
        //         newObj = Instantiate(healthPrefab, parent.transform);
        //     else if("AttackSpeed".Equals(abilityModel.GETName()))
        //         newObj = Instantiate(attackSpeedPrefab, parent.transform);
        //     else if("MovementSpeed".Equals(abilityModel.GETName()))
        //         newObj = Instantiate(movementSpeedPrefab, parent.transform);
        //     else if("LifeSteal".Equals(abilityModel.GETName()))
        //         newObj = Instantiate(movementSpeedPrefab, parent.transform);
        //     else if("CoolDown".Equals(abilityModel.GETName()))
        //         newObj = Instantiate(movementSpeedPrefab, parent.transform);
        //     else if("CoolDownReduction".Equals(abilityModel.GETName()))
        //         newObj = Instantiate(movementSpeedPrefab, parent.transform);
        //     else if("ReactiveDamage".Equals(abilityModel.GETName()))
        //         newObj = Instantiate(movementSpeedPrefab, parent.transform);
        //     else
        //         newObj = Instantiate(movementSpeedPrefab, parent.transform);
        //     
        //     newObj.transform.GetChild(1).GetComponent<Text>().text = "+" + abilityModel.GETAbility();
        //
        //     (newObj.transform as RectTransform).anchorMin = new Vector2(0, 0.5f);
        //     (newObj.transform as RectTransform).anchorMax = new Vector2(0, 0.5f);
        //     (newObj.transform as RectTransform).pivot = new Vector2(0, 0.5f);
        //     prefabInstanceClones.Add(newObj);
        //     // position.x = rectTransform.rect.width * index;
        //     // newObj.transform.localPosition = position;
        //     // index += 1;
        // }
    }

    private void SelectItemCard()
    {
        this.rootCanvas = this.GetComponentInParent<Canvas>();
        TabPopup tabPopup = rootCanvas.GetComponent<TabPopup>();
        tabPopup.selectItemCard = transform.gameObject;
    }
    public void OnPointerDown(PointerEventData eventData)
    {

        ShowItemCardAbility();

        SelectItemCard();
        
        //this.transform.SetAsLastSibling();
        this.transform.SetParent(rootCanvas.transform);
        parentOffset = this.transform.position;
        //eventData.position 포인터 스크린 포지션
        //다운위치 Offset 을 기억하자
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            this.parentRectTrans,
            eventData.position,
            (this.rootCanvas.renderMode == RenderMode.ScreenSpaceOverlay) ? null : this.rootCanvas.worldCamera,
            out offset))
        {
            //현재 클한지점의 로컬 위치에서 나의 현제 로컬위치까지의 차이량을 기억한다.
            this.offset.x = this.offset.x - this.transform.localPosition.x;
            this.offset.y = this.offset.y - this.transform.localPosition.y;
 
        }
        
        this.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        //this.GetComponent<CanvasGroup>().blocksRaycasts = false;
        //결과값 ( this.parentRectTransform 의 로컬로 나온다 )
        Vector2 outLocalPos = Vector2.zero;
 
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            this.parentRectTrans,
            eventData.position,
            (this.rootCanvas.renderMode == RenderMode.ScreenSpaceOverlay) ? null : this.rootCanvas.worldCamera,
            out outLocalPos))
        {
            this.transform.localPosition = outLocalPos - offset;
        }
 
    }
    
    public void returnToParent()
    {
        //원래 붙어있던 부모로 돌아가삼
        this.transform.SetParent(this.parentRectTrans);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        
        Debug.Log("================OnPointerUp=====================");
        Debug.Log("1. " + parentRectTrans);
        Debug.Log("2. " + transform.name);

        this.transform.SetParent(this.parentRectTrans);
        this.transform.localPosition = new Vector3(0, 0, 0);
        
        this.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}
