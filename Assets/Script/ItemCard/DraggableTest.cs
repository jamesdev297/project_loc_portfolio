using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableTest: MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    
    private RectTransform rectTrans;
    private RectTransform parentRectTrans;
    private Canvas rootCanvas;
    private Vector2 offset;
    
    void Awake()
    {
        rectTrans = this.GetComponent<RectTransform>();
        parentRectTrans = this.rectTrans.parent as RectTransform;
        this.rootCanvas = this.GetComponentInParent<Canvas>();
    }
 
 
    public void OnPointerDown(PointerEventData eventData)
    {
        
        //this.transform.SetAsLastSibling();
        this.transform.SetParent(rootCanvas.transform);
        
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
    }
 
 
    public void OnPointerUp(PointerEventData eventData)
    {
//        this.GetComponent<CanvasGroup>().blocksRaycasts = true;
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

}