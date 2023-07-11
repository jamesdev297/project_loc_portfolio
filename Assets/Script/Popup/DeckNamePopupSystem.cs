using System;
using UnityEngine;
using UnityEngine.UI;

public class DeckNamePopupSystem: MonoBehaviour
{
    public GameObject deckNamePopup;
    private Animator deckNamePopupAnim;
    private Action onDeckNameClickOkay;
    
    
    public static DeckNamePopupSystem instance { get; private set; }
    private void Awake()
    {
        instance = this;
        deckNamePopupAnim = deckNamePopup.GetComponent<Animator>();
    }

    private void Update()
    {
        if (deckNamePopupAnim.gameObject.activeSelf && deckNamePopupAnim.GetCurrentAnimatorStateInfo(0).IsName("close") && deckNamePopupAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            deckNamePopup.SetActive(false);
        }
    }
    
    public void OpenDeckNamePopUp(
        Action onClickOkay)
    {
        deckNamePopup.SetActive(true);
        onDeckNameClickOkay = onClickOkay;
    }
    
    public void OnClickOkay()
    {
        onDeckNameClickOkay();
        ClosePopup();
    }
    
    public void ClosePopup()
    {
        deckNamePopupAnim.SetTrigger("close");
    }
}