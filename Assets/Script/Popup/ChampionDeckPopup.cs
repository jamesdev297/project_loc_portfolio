using System;
using System.Collections.Generic;
using System.Linq;
using Json;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class ChampionDeckPopup : MonoBehaviour
{
    // deckPopup
    // public GameObject championDeckPopUp;
    // public Text txtChampionDeckTitle;
    // private Action onChampionDeckClickOkay;
    
    //deckPopup animation
    // private Animator championDeckAnim;

    // ability
    public GameObject attackDamage;
    public GameObject armor;
    public GameObject health;
    public GameObject attackSpeed;
    public GameObject movementSpeed;
    
    public GameObject selectedChampion;
    public Dropdown dropdown;
    private SpriteRenderer sprite;
    List<Object> prefabInstanceClones = new List<Object>();
    
    // DeckCard
    // public GameObject itemCard;
    // private List<GameObject> deckItemCards = new List<GameObject>();
    
    public static ChampionDeckPopup instance { get; private set; }
    
    private void Awake()
    {
        instance = this;
        // championDeckAnim = championDeckPopUp.GetComponent<Animator>();

        //UpdateDeckCards();
    }
    
    private void Update()
    {
        // if (championDeckAnim.gameObject.activeSelf && championDeckAnim.GetCurrentAnimatorStateInfo(0).IsName("close") && championDeckAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        // {
        //     championDeckPopUp.SetActive(false);
        // }
    }
    
    // public void OpenChampionDeckPopUp(
    //     string title,
    //     Action onClickOkay)
    // {
    //
    //     txtChampionDeckTitle.text = title;
    //     onChampionDeckClickOkay = onClickOkay;
    //     championDeckPopUp.SetActive(true);
    //
    //     
    //     // foreach (var deckMapKey in GameManager.instance.playerModel.deckMap.Keys)
    //     // {
    //     //     Debug.Log(GameManager.instance.playerModel.deckMap[deckMapKey].GetCardList().Count + " : " + deckMapKey);
    //     // }
    //     // InitDeckList();
    //     AbilityChampion();
    //     SelectChampion();
    // }
    
    public void OnClickOkay()
    {
        // onChampionDeckClickOkay();
        ClosePopup();
    }
    
    public void ClosePopup()
    {
        if (prefabInstanceClones.Count > 0)
        {
            var first = prefabInstanceClones.FirstOrDefault();
            prefabInstanceClones.Remove(first);
            Destroy(first);
        }
        // championDeckAnim.SetTrigger("close");
    }

    // private void InitDeckList()
    // {
    //     IReadOnlyList<UserDeck> myDeckList = UserInfoManager.Instance.UserDecks;
    //     dropdown.options.Clear();
    //     foreach (UserDeck userDeck in myDeckList)
    //     {
    //         dropdown.options.Add(new Dropdown.OptionData(userDeck.name));
    //     }
    //     Dropdown.OptionData addDeckOption =new Dropdown.OptionData("추가");
    //     dropdown.options.Add(addDeckOption);
    // }

    // public void SelectButton()
    // {
    //     Debug.Log("Dropdown Value: "+ dropdown.value +", List Selected: " + (dropdown.value + 1));
    //     //GameManager.instance.playerModel.selectDeckId = UserInfoManager.Instance.deckNameIdMap[dropdown.options[dropdown.value].text];
    //     Debug.Log(GameManager.instance.playerModel.selectDeckId + "=========111");
    //     UpdateDeckCards();
    //     AbilityChampion();
    //     
    // }

    // private void UpdateDeckCards()
    // {
    //     if (deckItemCards.Count > 0)
    //     {
    //         int count = deckItemCards.Count;
    //
    //         for (int i = 0; i < count; i++)
    //         {
    //             Destroy(deckItemCards[i]); 
    //         }
    //         deckItemCards.Clear();
    //     }
    //     
    //     GameObject newObj;
    //
    //     int selectDeckId = GameManager.instance.playerModel.selectDeckId;
    //
    //     if (UserInfoManager.Instance.GetSelectDeck() == null)
    //         return;
    //
    //     UserDeck selectDeck = UserInfoManager.Instance.GetSelectDeck();
    //     
    //     // 카드 개수
    //     transform.GetChild(1).GetChild(3).GetChild(0).GetChild(1).GetComponent<Text>().text = "( " + selectDeck.cards.Count + " / 5 )";
    //     
    //     // foreach (var card in selectDeck.cards)
    //     // {
    //     //     var cardModel = GameInfoManager.Instance.CardMap[card.cardId];
    //     //     deckItemCards.Add(Instantiate(itemCard, transform));
    //     //     newObj = deckItemCards.LastOrDefault();
    //     //     newObj.name = cardModel.GETName();
    //     //     newObj.transform.position = Vector3.zero;
    //     //     newObj.GetComponentInChildren<Image>().color = Color.blue;
    //     //     newObj.GetComponentsInChildren<Image>()[2].gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/ItemCard/"+cardModel.GETName());
    //     //     newObj.GetComponentInChildren<Text>().text = cardModel.GETName();
    //     //     newObj.transform.parent = transform.GetChild(1).GetChild(3).GetChild(1).GetChild(0);
    //     // }
    // }

    private void SelectChampion()
    {
        if (prefabInstanceClones.Count > 0)
        {
            var first = prefabInstanceClones.FirstOrDefault();
            prefabInstanceClones.Remove(first);
            Destroy(first);
        }
        
        foreach (var championPreFab in GameManager.instance.championPrefabList)
        {
            if (ChampionModel.getIdByPrefabName(championPreFab.name) == GameManager.instance.playerModel.selectChampionId)
            {
                GameObject newObj = Instantiate(championPreFab, selectedChampion.transform.position, Quaternion.identity);
                
                newObj.transform.localPosition = new Vector3(selectedChampion.transform.position.x,selectedChampion.transform.position.y,0);
                sprite = newObj.GetComponent<SpriteRenderer>();
 
                if (sprite)
                {
                    sprite.sortingOrder = 10;
                }
                prefabInstanceClones.Add(newObj);
                break;
            }
        }
    }
    
    private void AbilityChampion()
    {
        // if (UserInfoManager.Instance.GetSelectDeck() == null)
        //     return ;
        
        int selectChampionId = GameManager.instance.playerModel.selectChampionId;
        var championModel = ChampionFactory.Create(selectChampionId);
    
        double attackDamageTotal = championModel.attackSpeed;
        double armorTotal = championModel.armor;
        double healthTotal = championModel.health;
        double movementSpeedTotal = championModel.movementSpeed;
        double attackSpeedTotal = championModel.attackSpeed;
       
        // 덱 능력 추가
        // foreach (var card in UserInfoManager.Instance.GetSelectDeck().cards)
        // {
        //     var cardModel = GameInfoManager.Instance.CardMap[card.cardId];
        //     attackDamageTotal += cardModel.GETDefaultAttackDamage();
        //     armorTotal += cardModel.GETDefaultArmor();
        //     healthTotal += cardModel.GETDefaultHealth();
        //     movementSpeedTotal += cardModel.GETDefaultMovementSpeed();
        //     attackSpeedTotal += cardModel.GETDefaultAttackSpeed();
        // }
        
        // 공격력
        attackDamage.GetComponent<Text>().text = attackDamageTotal.ToString();

        // 방어력
        armor.GetComponent<Text>().text = armorTotal.ToString();
        
        // 체력
        health.GetComponent<Text>().text = healthTotal.ToString();
       
        // 이동속도
        attackSpeed.GetComponent<Text>().text = movementSpeedTotal.ToString();
        
        // 공격속도
        movementSpeed.GetComponent<Text>().text = attackSpeedTotal.ToString();

    }
}