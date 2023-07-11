using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BuyUseChampionPopup: MonoBehaviour
{
    public GameObject defaultPopup;
    public GameObject championName;
    public GameObject championImage;
    public int selectedChampionId = 1;
    private Action onDefaultClickOkay, onDefaultClickCancel;

    private Animator defaultAnim;
    public GameObject dim;

    public Text option1Label;
    public Text option2Label;
    public Text option3Label;

    public Text option1CostText;
    public Text option2CostText;
    public Text option3CostText;

    public static BuyUseChampionPopup instance { get; private set; }
    
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
    
    public void OpenBuyUseChampionPopUp(
        Action onClickOkay,
        Action onClickCancel)
    {
        onDefaultClickOkay = onClickOkay;
        onDefaultClickCancel = onClickCancel;
        selectedChampionId = 2;
        championName.GetComponent<Text>().text = GameInfoManager.Instance.ChampMap[selectedChampionId].name;
        championImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/Champion/ThumbNail/" + ChampionModel.GetEnglishNameById(selectedChampionId));

        UpdatePriceTable();

        defaultPopup.SetActive(true);
        if(dim != null)
            dim.SetActive(true);
    }

    private void UpdatePriceTable()
    {
        option1Label.text =
            GameInfoManager.Instance.ChampMap[selectedChampionId].priceTable[0].day + "일 ";
        option1CostText.text =
            Utils.GetCommaNum(GameInfoManager.Instance.ChampMap[selectedChampionId].priceTable[0].price);
        
        option2Label.text =
            GameInfoManager.Instance.ChampMap[selectedChampionId].priceTable[1].day + "일 ";
        option2CostText.text =
            Utils.GetCommaNum(GameInfoManager.Instance.ChampMap[selectedChampionId].priceTable[1].price);

        option3Label.text =
            GameInfoManager.Instance.ChampMap[selectedChampionId].priceTable[2].day + "일 ";
        option3CostText.text =
            Utils.GetCommaNum(GameInfoManager.Instance.ChampMap[selectedChampionId].priceTable[2].price);
    }

    public void LeftBtn()
    {
        if (selectedChampionId == 1)
            selectedChampionId = 2;
        else
            selectedChampionId = 1;
        
        championName.GetComponent<Text>().text = GameInfoManager.Instance.ChampMap[selectedChampionId].name;
        championImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/Champion/ThumbNail/" + ChampionModel.GetEnglishNameById(selectedChampionId));
        
        UpdatePriceTable();
    }
    
    public void RightBtn()
    {
        if (selectedChampionId == 1)
            selectedChampionId = 2;
        else
            selectedChampionId = 1;
        
        championName.GetComponent<Text>().text = GameInfoManager.Instance.ChampMap[selectedChampionId].name;
        championImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/Champion/ThumbNail/" + ChampionModel.GetEnglishNameById(selectedChampionId));
        
        UpdatePriceTable();
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