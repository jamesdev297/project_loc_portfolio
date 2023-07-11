using System;
using UnityEngine;
using UnityEngine.UI;

public class PopupSystem : MonoBehaviour
{
    public GameObject defaultPopup;
    public GameObject nameSettingPopup;
    public GameObject championDeckPopUp;
    public Text txtDefaultTitle, txtDefaultContent;
    public Text txtNameSettingTitle;
    public Text txtChampionDeckTitle;
    
    private Action onDefaultClickOkay, onDefaultClickCancel;
    private Action onNameSettingClickOkay;
    private Action onChampionDeckClickOkay;


    private Animator defaultAnim;
    private Animator nameSettingAnim;
    private Animator championDeckAnim;
    public static PopupSystem instance { get; private set; }

    private void Awake()
    {
        instance = this;
        nameSettingAnim = nameSettingPopup.GetComponent<Animator>();
        championDeckAnim = championDeckPopUp.GetComponent<Animator>();
        
    }

    private void Update()
    {
        if (defaultAnim.gameObject.activeSelf && defaultAnim.GetCurrentAnimatorStateInfo(0).IsName("close") && defaultAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            defaultPopup.SetActive(false);
        }
        
        if (nameSettingAnim.gameObject.activeSelf && nameSettingAnim.GetCurrentAnimatorStateInfo(0).IsName("close") && nameSettingAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            nameSettingPopup.SetActive(false);
        }
        
        if (championDeckAnim.gameObject.activeSelf && championDeckAnim.GetCurrentAnimatorStateInfo(0).IsName("close") && championDeckAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            championDeckPopUp.SetActive(false);
        }
    }

    public void OpenNicknamePopUp(
        string title,
        Action onClickOkay)
    {
        txtNameSettingTitle.text = title;
        onNameSettingClickOkay = onClickOkay;
        nameSettingPopup.SetActive(true);
    }
    
    public void OpenDefaultPopUp(
        string title,
        string content,
        Action onClickOkay,
        Action onClickCancel)
    {
        txtDefaultTitle.text = title;
        txtDefaultContent.text = content;
        onDefaultClickOkay = onClickOkay;
        onDefaultClickCancel = onClickCancel;
        defaultPopup.SetActive(true);
    }
    
    public void OpenChampionDeckPopUp(
        string title,
        Action onClickOkay)
    {
        txtChampionDeckTitle.text = title;
        onChampionDeckClickOkay = onClickOkay;
        championDeckPopUp.SetActive(true);
    }

    public void OnClickOkay()
    {
        if (onDefaultClickOkay != null)
        {
            onDefaultClickOkay();
            defaultAnim.SetTrigger("close");
            onDefaultClickOkay = null;
        }
        
        if (onNameSettingClickOkay != null)
        {
            nameSettingAnim.SetTrigger("close");
            onNameSettingClickOkay();
            onNameSettingClickOkay = null;
        }
        
        if (onChampionDeckClickOkay != null)
        {
            onChampionDeckClickOkay();
            championDeckAnim.SetTrigger("close");
            onChampionDeckClickOkay = null;
        }
    }

    public void OnClickCancel()
    {
        if (onDefaultClickCancel != null)
        {
            onDefaultClickCancel();
            
            defaultAnim.SetTrigger("close");
        }
    }
    
    public void ClosePopup()
    {
        defaultAnim.SetTrigger("close");
        nameSettingAnim.SetTrigger("close");
        championDeckAnim.SetTrigger("close");
    }
}
