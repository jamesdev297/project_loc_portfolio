using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DefaultPopup: MonoBehaviour
{
    public GameObject defaultPopup;
    public Text txtDefaultTitle, txtDefaultContent;
    private Action onDefaultClickOkay, onDefaultClickCancel;
    
    private Animator defaultAnim;
    public GameObject dim;

    public static DefaultPopup instance { get; private set; }
    
    private void Awake()
    {
        instance = this;
        defaultAnim = defaultPopup.GetComponent<Animator>();
    }
    
    private void Update()
    {
        if (defaultAnim.gameObject.activeSelf && defaultAnim.GetCurrentAnimatorStateInfo(0).IsName("close") && defaultAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            defaultPopup.SetActive(false);
        }
    }
    
    public void OpenDefaultPopUp(
        string content,
        Action onClickOkay,
        Action onClickCancel)
    {
        txtDefaultContent.text = content;
        onDefaultClickOkay = onClickOkay;
        onDefaultClickCancel = onClickCancel;
        defaultPopup.SetActive(true);
        if(dim != null)
            dim.SetActive(true);
    }
    
    public void OnClickOkay()
    {
        onDefaultClickOkay();
        ClosePopup();
    }

    public void OnClickCancel()
    {
        onDefaultClickCancel();
        ClosePopup();
    }
    
    public void ClosePopup()
    {
        defaultAnim.SetTrigger("close");
        if(dim != null)
            dim.SetActive(false);
        StartCoroutine(DelayedInactivePopUp());
    }

    IEnumerator DelayedInactivePopUp()
    {
        yield return new WaitForSeconds(0.5f);
        defaultPopup.SetActive(false);
    }
}