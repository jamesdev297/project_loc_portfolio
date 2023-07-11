using System;
using UnityEngine;
using UnityEngine.UI;

public class DefaultOneBtnPopup :MonoBehaviour
{
    public GameObject defaultOneBtnPopup;
    public Text txtDefaultContent;
    private Action onDefaultClickOkay;
    
    private Animator defaultOneBtnAnim;
    
    public static DefaultOneBtnPopup instance { get; private set; }
    
    
    private void Awake()
    {
        instance = this;
        defaultOneBtnAnim = defaultOneBtnPopup.GetComponent<Animator>();
    }
    
    private void Update()
    {
        if (defaultOneBtnAnim.gameObject.activeSelf && defaultOneBtnAnim.GetCurrentAnimatorStateInfo(0).IsName("close") && defaultOneBtnAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            defaultOneBtnPopup.SetActive(false);
        }
    }
    
    public void OpenDefaultOneBtnPopUp(
        string content,
        Action onClickOkay)
    {
        txtDefaultContent.text = content;
        onDefaultClickOkay = onClickOkay;
        defaultOneBtnPopup.SetActive(true);
    }
    
    public void OnClickOkay()
    {
        onDefaultClickOkay();
        ClosePopup();
    }
    
    public void ClosePopup()
    {
        defaultOneBtnAnim.SetTrigger("close");
    }
    
}