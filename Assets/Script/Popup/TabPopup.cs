using System;
using System.Collections.Generic;
using System.Linq;
using Json;
using Script.Popup;
using UnityEngine;
using UnityEngine.UI;

public class TabPopup : MonoBehaviour
{
    
    public GameObject tabPopUp;
    private Action onTabClickOkay;
    private Action OnDestroyChampionPrefabFromMainScene;
    private Action OnInstantiateChampionPrefabFromMainScene;
    
    // tab
    public GameObject championTab;
    // public GameObject itemCardTab;
    public GameObject championTabPanel;
    // public GameObject itemCardTabPanel;
    
    private int tabIndex;
    private int championIndex;

    // ability
    public GameObject itemAbilityPanel;
    public GameObject attackDamage;
    public GameObject armor;
    public GameObject health;
    public GameObject attackSpeed;
    public GameObject movementSpeed;

    public GameObject selectItemCard;
    
    //TabPopup animation
    private Animator tabAnim;

    //DeckDropDown
    // public Dropdown championTabDropdown;
    // public Dropdown cardTabDropdown;
    // public GameObject deckCount;
    // public InputField addDeckNameInputField;
    
    //Champion
    List<GameObject> prefabInstanceClones = new List<GameObject>();
    public GameObject championChange;
    public GameObject championName;

    // deckItemCard
    // public GameObject itemCard;
    // public List<int> deckItemCardIds = new List<int>();

    // deck Ability
    // List<GameObject> prefabInstanceDeckAbility = new List<GameObject>();
    // public GameObject attackDamagePrefab;
    // public GameObject armorPrefab;
    // public GameObject healthPrefab;
    // public GameObject attackSpeedPrefab;
    // public GameObject movementSpeedPrefab;
    
    // inventoryItemCard
    // public List<GameObject> inventoryItemCards = new List<GameObject>();
    // public GameObject inventoryContent;
    // public GameObject itemCardCount;
    
    // deck
    // public GameObject myDeck;
    // public GameObject deckAbilityPanel;

    public static TabPopup Instance;
    
    //select champion
    public GameObject championContents;

    
    void Start()
    {
        tabAnim = tabPopUp.GetComponent<Animator>();
    }
    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        // if (tabAnim.gameObject.activeSelf && tabAnim.GetCurrentAnimatorStateInfo(0).IsName("close") && tabAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        // {
        //     tabPopUp.SetActive(false);
        // }
    }
    
    public void OpenTabPopUp(
        int index,
        // Action onClickOkay,
        Action onDestroyChampionPrefabFromMainScene,
        Action onInstantiateChampionPrefabFromMainScene)
    {
        
        // onTabClickOkay = onClickOkay;
        OnDestroyChampionPrefabFromMainScene = onDestroyChampionPrefabFromMainScene;
        OnInstantiateChampionPrefabFromMainScene = onInstantiateChampionPrefabFromMainScene;
        tabIndex = index;
        tabPopUp.SetActive(true);
        SelectChampion();
        OnChangeTab(tabIndex);
    }
    
    public void InitSelectChampion()
    {
        
        int index = 0;
        Debug.Log("UserChampionCount : " +UserInfoManager.Instance.userChampionMap.Count);
        foreach (var champ in GameInfoManager.Instance.Champs)
        {
            Debug.Log("champ : " + champ.name);

            championContents.transform.GetChild(index).GetChild(0).GetComponent<Image>().color = Color.white;
            championContents.transform.GetChild(index).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/Champion/SelectImage/" + ChampionModel.GetEnglishNameById(champ.id));
            championContents.transform.GetChild(index).GetChild(2).GetComponent<Text>().text = champ.name;
            championContents.transform.GetChild(index).GetChild(3).gameObject.SetActive(false);

            int championId = champ.id;
            
            if (champ.name != "잭스")
            {
                if (UserInfoManager.Instance.userChampionMap.ContainsKey(championId) == false)
                {
                    if(championId == 1 || championId == 2)
                        championContents.transform.GetChild(index).GetChild(4).GetComponent<Text>().text = "미보유";
                }
            }

            championContents.transform.GetChild(index).GetComponent<Outline>().effectColor = Color.white;
            if (!UserInfoManager.Instance.userChampionMap.ContainsKey(champ.id))
            {
                championContents.transform.GetChild(index).GetChild(3).gameObject.SetActive(true);
            }
            
            if (GameManager.instance.playerModel.selectChampionId == champ.id)
            {
                championContents.transform.GetChild(index).GetComponent<Outline>().effectColor = Color.red;
            }
            index += 1;
        }
    }

    private void OnChangeTab(int index)
    {

        tabIndex = index;
        
        InitChampionPrefab();
        InitChampionAbility();
        // SetDropdownOptions();
        // InitDeckCardPrefab();
        // InitInventoryCardPrefab();
        
        switch (index)
        {
            case 0:
                // 챔피언 탭
                championTab.transform.GetChild(0).gameObject.SetActive(true);
                championTab.transform.GetChild(1).gameObject.SetActive(false);
                championTabPanel.SetActive(true);
                
                // 아이템 탭
                // itemCardTab.transform.GetChild(0).gameObject.SetActive(false);
                // itemCardTab.transform.GetChild(1).gameObject.SetActive(true);
                // itemCardTabPanel.SetActive(false);
                break;
            case 1:
                // 챔피언 탭
                championTab.transform.GetChild(0).gameObject.SetActive(false);
                championTab.transform.GetChild(1).gameObject.SetActive(true);
                championTabPanel.SetActive(false);
                
                // 아이템 탭
                // itemCardTab.transform.GetChild(0).gameObject.SetActive(true);
                // itemCardTab.transform.GetChild(1).gameObject.SetActive(false);
                // itemCardTabPanel.SetActive(true);
                break;
        }
    }

    public void TabBtn(int index)
    {
        OnChangeTab(index);
    }
    
    private void OnDestroyChampionPrefab()
    {
        if (prefabInstanceClones.Count > 0)
        {
            var first = prefabInstanceClones.FirstOrDefault();
            prefabInstanceClones.Remove(first);
            Destroy(first);
        }
    }
    
    private void OnInstantiateChampionPrefab()
    {
        GameObject o = GameManager.instance.championStandPrefabList[championIndex];
        prefabInstanceClones.Add(Instantiate(o));
        var first = prefabInstanceClones.FirstOrDefault();

        first.transform.parent = championChange.transform;
        first.GetComponent<SpriteRenderer>().sortingOrder = 6;

        first.transform.localPosition = new Vector3(0, 0, 0);
        
        
        if (championIndex == 0)
        {
            first.transform.localScale = new Vector3(6f, 6f, 0f);
        }
        else if (championIndex == 1)
        {
            first.transform.localScale = new Vector3(436.8167f, 436.8167f, 0f);
        }
        else if (championIndex == 2)
        {
            first.transform.localScale = new Vector3(4.613887f, 4.613887f, 0f);
        }
        else if (championIndex == 3)
        {
            first.transform.localScale = new Vector3(324f, 324f, 0f);
        }
        else if (championIndex == 4)
        {
            first.transform.localScale = new Vector3(54f, 54f, 0f);
        }
        
        
        championName.GetComponent<Text>().text = GameInfoManager.Instance.ChampMap[championIndex].name;
        
        // 선택한 챔피언 저장
        GameManager.instance.playerModel.setSelectChampId(championIndex);
    }

    public void OnClickGoReview()
    {
        GetComponent<ReviewAlertPopUp>().show();
    }
    
    public void OnClickChampionBtn(int championId)
    {
        
        if (championId > GameInfoManager.Instance.ChampMap.Count)
            return;
        
        Debug.Log($"OnClickChampionBtn {championId} {UserInfoManager.Instance.userChampionMap.ContainsKey(championId)}");
        if (championId != 0 && (UserInfoManager.Instance.userChampionMap.ContainsKey(championId) == false || UserInfoManager.Instance.userChampionMap[championId].expiryTick == 0))
        //if (championId != 0 && (UserInfoManager.Instance.userChampionMap.ContainsKey(championId) == false || UserInfoManager.Instance.userChampionMap[championId].expireDays == 0))
        {
            DefaultOneBtnPopup.instance.OpenDefaultOneBtnPopUp(
                "이용권을 먼저 구매하세요",
                () =>
                {
                    Debug.Log("확인");
                });
        } else
        {
            GameManager.instance.playerModel.selectChampionId = championId;
            SelectChampion();
        }
        
        // if (championId > GameInfoManager.Instance.ChampMap.Count)
        //     return;
        //
        // ChampionModel championModel = GameInfoManager.Instance.ChampMap[championId];
        //
        // if (!UserInfoManager.Instance.userChampionMap.ContainsKey(championId))
        // {
        //     DefaultPopup.instance.OpenDefaultPopUp(
        //         championModel.name + "을 사시겠습니까?\n" + Utils.GetCommaNum(championModel.price) + "골드 입니다.",
        //         () =>
        //         {
        //             // 예
        //             BuyChampion(championModel);
        //         },
        //         () =>
        //         {
        //             // 아니오
        //         });
        // }
        // else
        // {
        //     GameManager.instance.playerModel.selectChampionId = championId;
        //     SelectChampion();
        // }
    }

    public void SelectChampion()
    {
        InitSelectChampion();
        InitChampionPrefab();
        InitChampionAbility();
        if(OnDestroyChampionPrefabFromMainScene != null)
            OnDestroyChampionPrefabFromMainScene();
        if(OnInstantiateChampionPrefabFromMainScene != null)
            OnInstantiateChampionPrefabFromMainScene();
    }
    
    // private async void BuyChampion(ChampionModel championModel)
    // {
    //     if(UserInfoManager.Instance.Info.gold < championModel.price)
    //         DefaultOneBtnPopup.instance.OpenDefaultOneBtnPopUp(
    //             "골드가 부족합니다.",
    //             () =>
    //             {
    //                 // 예
    //             });
    //     else
    //     {
    //         Debug.Log("champion id : " + championModel.id);
    //         UserInfoManager.Instance.Info.gold -= championModel.price;
    //         UserInfoManager.Instance.ShowGold();
    //         await UserInfoManager.Instance.AddUserChamp(championModel.id);
    //         GameManager.instance.playerModel.setSelectChampId(championModel.id);
    //         SelectChampion();
    //     }
    // }
    
    private void InitChampionPrefab()
    {
        OnDestroyChampionPrefab();
        int selectChampionId = GameManager.instance.playerModel.selectChampionId;

        Debug.Log("InitChampionPrefab selectChampionId : " + selectChampionId);
        
        for (int i = 0; i < GameManager.instance.championPrefabList.Count; i++)
        {
            GameObject o = GameManager.instance.championPrefabList[i];
            if (selectChampionId == ChampionModel.getIdByPrefabName(o.name))
            {
                championIndex = i; 
                OnInstantiateChampionPrefab();
                break;
            }
        }
    }
    
    // private void SetDropdownOptions() // Dropdown 목록 생성
    // {
    //     championTabDropdown.options.Clear();
    //     cardTabDropdown.options.Clear();
    //     
    //     for (int i = 0; i < UserInfoManager.Instance.UserDecks.Count; i++)
    //     {
    //
    //         Dropdown.OptionData option = new Dropdown.OptionData();
    //         option.text = UserInfoManager.Instance.userDeckMap[i].name;
    //         championTabDropdown.options.Add(option);
    //         cardTabDropdown.options.Add(option);
    //     }
    //     championTabDropdown.value = GameManager.instance.playerModel.selectDeckId;
    //     cardTabDropdown.value = GameManager.instance.playerModel.selectDeckId;
    //
    //     // 덱 추가
    //     Dropdown.OptionData addOption1 = new Dropdown.OptionData();
    //     addOption1.text = "+ 추가";
    //     cardTabDropdown.options.Add(addOption1);
    //     deckCount.GetComponent<Text>().text = (GameManager.instance.playerModel.selectDeckId+1) + " / " + UserInfoManager.Instance.Info.deckPage;
    // }


    // public void ChampionTabDeckButton()
    // {
    //     if (championTabDropdown.value == GameManager.instance.playerModel.selectDeckId)
    //         return;
    //     
    //     itemAbilityPanel.SetActive(false);
    //     GameManager.instance.playerModel.setSelectDeckId(championTabDropdown.value);
    //     
    //     OnDeckAbility();
    //     InitChampionAbility();
    // }

    // public void OnDeckAbility()
    // {
    //     HideDeckAbility();
    //     
    //     
    //     double totalAttackDamage = 0;
    //     double totalArmor = 0;
    //     double totalHealth = 0;
    //     double totalAttackSpeed = 0;
    //     double totalMovementSpeed = 0;
    //
    //     foreach (var cardId in deckItemCardIds)
    //     {
    //         if(cardId == -1)
    //             continue;
    //
    //         var cardModel = GameInfoManager.Instance.CardMap[cardId];
    //
    //         foreach (var abilityModel in cardModel.GETAbilities())
    //         {
    //             if (abilityModel.GETName().Equals("AttackDamage"))
    //             {
    //                 totalAttackDamage += abilityModel.GETAbility();
    //             } else if (abilityModel.GETName().Equals("Armor"))
    //             {
    //                 totalArmor += abilityModel.GETAbility();
    //             } else if (abilityModel.GETName().Equals("Health"))
    //             {
    //                 totalHealth += abilityModel.GETAbility();
    //             } else if (abilityModel.GETName().Equals("AttackSpeed"))
    //             {
    //                 totalAttackSpeed += abilityModel.GETAbility();
    //             } else if (abilityModel.GETName().Equals("MovementSpeed"))
    //             {
    //                 totalMovementSpeed += abilityModel.GETAbility();
    //             }
    //         }
    //     }
    //     
    //     GameObject parent = deckAbilityPanel;
    //
    //     int i = 0;
    //     if (totalAttackDamage > 0)
    //     {
    //         GameObject newObj;
    //         newObj = Instantiate(attackDamagePrefab, parent.transform);
    //         (newObj.transform as RectTransform).anchorMin = new Vector2(0, 0.5f);
    //         (newObj.transform as RectTransform).anchorMax = new Vector2(0, 0.5f);
    //         (newObj.transform as RectTransform).pivot = new Vector2(0, 0.5f);
    //         newObj.transform.GetChild(1).GetComponent<Text>().text = "+" + totalAttackDamage;
    //         prefabInstanceDeckAbility.Add(newObj);
    //         Debug.Log(newObj.transform.localPosition);
    //         i += 1;
    //     }
    //
    //     if (totalArmor > 0)
    //     {
    //         GameObject newObj;
    //         newObj = Instantiate(armorPrefab, parent.transform);
    //         (newObj.transform as RectTransform).anchorMin = new Vector2(0, 0.5f);
    //         (newObj.transform as RectTransform).anchorMax = new Vector2(0, 0.5f);
    //         (newObj.transform as RectTransform).pivot = new Vector2(0, 0.5f);
    //         newObj.transform.GetChild(1).GetComponent<Text>().text = "+" + totalArmor;
    //         prefabInstanceDeckAbility.Add(newObj);
    //
    //         newObj.transform.localPosition = new Vector3(newObj.transform.localPosition.x +(newObj.transform as RectTransform).rect.width * i, 0);
    //         i += 1;
    //     }
    //
    //     if (totalHealth > 0)
    //     {
    //         GameObject newObj;
    //         newObj = Instantiate(healthPrefab, parent.transform);
    //         (newObj.transform as RectTransform).anchorMin = new Vector2(0, 0.5f);
    //         (newObj.transform as RectTransform).anchorMax = new Vector2(0, 0.5f);
    //         (newObj.transform as RectTransform).pivot = new Vector2(0, 0.5f);
    //         newObj.transform.GetChild(1).GetComponent<Text>().text = "+" + totalHealth;
    //         prefabInstanceDeckAbility.Add(newObj);
    //         newObj.transform.localPosition = new Vector3(newObj.transform.localPosition.x + (newObj.transform as RectTransform).rect.width * i, 0);
    //         i += 1;
    //     }
    //
    //     if (totalAttackSpeed > 0)
    //     {
    //         GameObject newObj;
    //         newObj = Instantiate(attackSpeedPrefab, parent.transform);
    //         (newObj.transform as RectTransform).anchorMin = new Vector2(0, 0.5f);
    //         (newObj.transform as RectTransform).anchorMax = new Vector2(0, 0.5f);
    //         (newObj.transform as RectTransform).pivot = new Vector2(0, 0.5f);
    //         newObj.transform.GetChild(1).GetComponent<Text>().text = "+" + totalAttackSpeed;
    //         prefabInstanceDeckAbility.Add(newObj);
    //         newObj.transform.localPosition = new Vector3(newObj.transform.localPosition.x + (newObj.transform as RectTransform).rect.width * i, 0);
    //         i += 1;
    //     }
    //
    //     if (totalMovementSpeed > 0)
    //     {
    //         GameObject newObj;
    //         newObj = Instantiate(movementSpeedPrefab, parent.transform);
    //         (newObj.transform as RectTransform).anchorMin = new Vector2(0, 0.5f);
    //         (newObj.transform as RectTransform).anchorMax = new Vector2(0, 0.5f);
    //         (newObj.transform as RectTransform).pivot = new Vector2(0, 0.5f);
    //         newObj.transform.GetChild(1).GetComponent<Text>().text = "+" + totalMovementSpeed;
    //         prefabInstanceDeckAbility.Add(newObj);
    //         newObj.transform.localPosition = new Vector3(newObj.transform.localPosition.x + (newObj.transform as RectTransform).rect.width * i, 0);
    //     }
    // }
    
    // private void HideDeckAbility()
    // {
    //     int count = deckAbilityPanel.transform.childCount;
    //
    //     if (count > 0)
    //     {
    //         for (int i = 0; i < count; i++)
    //         {
    //             GameObject o = deckAbilityPanel.transform.GetChild(0).gameObject;
    //             Destroy(o);
    //         }
    //     }
    //     
    //     while (prefabInstanceDeckAbility.Count > 0)
    //     {
    //         var first = prefabInstanceDeckAbility.FirstOrDefault();
    //         prefabInstanceDeckAbility.Remove(first);
    //         Destroy(first);
    //     }
    // }

    // private void OnChangedDeckDropdown(int value)
    // {
    //     Debug.Log("dropdown value : " + value);
    //     GameManager.instance.playerModel.setSelectDeckId(value);
    //     
    //     cardTabDropdown.value = GameManager.instance.playerModel.selectDeckId;
    //     
    //     InitDeckCardPrefab();
    //     deckCount.GetComponent<Text>().text = (GameManager.instance.playerModel.selectDeckId + 1) + " / " + UserInfoManager.Instance.Info.deckPage;
    // }
    
    // public void ItemCardTabDeckButton()
    // {
    //
    //     Debug.Log("cardTapDropdown : " + cardTabDropdown.value);
    //     
    //     if (cardTabDropdown.value == GameManager.instance.playerModel.selectDeckId)
    //         return;
    //         
    //     itemAbilityPanel.SetActive(false);
    //     
    //     if (cardTabDropdown.value == cardTabDropdown.options.Count - 1)
    //     {
    //         cardTabDropdown.value = GameManager.instance.playerModel.selectDeckId;
    //
    //         if (UserInfoManager.Instance.Info.gold < 50)
    //         {
    //             DefaultOneBtnPopup.instance.OpenDefaultOneBtnPopUp(
    //             "골드가 부족합니다.",
    //             () =>
    //             {
    //                 // 예
    //             });
    //             return;
    //         }
    //
    //         DefaultPopup.instance.OpenDefaultPopUp(
    //         "50 골드인데 추가 하실래요?",
    //         () =>
    //         {
    //             AddDeck();
    //         },
    //         () =>
    //         {
    //             // 아니오
    //         });
    //     }
    //     else
    //     {
    //         OnChangedDeckDropdown(cardTabDropdown.value);
    //         InitChampionAbility();
    //     }
    // }

    // private async void AddDeck()
    // {
    //     if (await UserInfoManager.Instance.AddDeck() == false)
    //         return;
    //     
    //     SetDropdownOptions();
    //     InitDeckCardPrefab();
    //     
    //     //50 골드 차감
    //     UserInfoManager.Instance.UpdateGold(-50);
    //     OnDeckAbility();
    //
    //
    // }

    // private void InitDeckCardPrefab()
    // {
    //     RemoveDeckCardPrefab();
    //     
    //     
    //     UserDeck uerDeck = UserInfoManager.Instance.GetSelectDeck();
    //     
    //     if (uerDeck == null)
    //         return;
    //     
    //     GameObject parent = myDeck;
    //     for (int index = 0; index < uerDeck.cards.Count; index++)
    //     {
    //
    //         int cardId = uerDeck.cards[index];
    //         if (cardId == -1)
    //         {
    //             deckItemCardIds.Add(-1);
    //             continue;
    //         }
    //         
    //         GameObject newObj = Instantiate(itemCard, parent.transform.GetChild(index + 2));
    //         string enName = CardModel.ConvertIdToEnName(cardId).ToString();
    //         newObj.name = enName;
    //         newObj.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/UI/CardItem/" + enName);
    //         newObj.transform.parent = parent.transform.GetChild(index + 2);
    //         deckItemCardIds.Add(cardId);
    //
    //     }
    //
    //     OnDeckAbility();
    // }
    
    // private void RemoveDeckCardPrefab()
    // {
    //     GameObject parent = myDeck;
    //
    //     deckItemCardIds.Clear();
    //     
    //     for (int i = 2; i < 7; i++)
    //     {
    //         if(parent.transform.GetChild(i).childCount > 0)
    //             Destroy(parent.transform.GetChild(i).GetChild(0).gameObject);
    //     }
    // }
    
    // private void InitInventoryCardPrefab()
    // {
    //     RemoveInventory();
    //     
    //     int index = 0;
    //     GameObject newItemCardObj;
    //     GameObject newItemCardCountObj;
    //     IReadOnlyList<UserCard> userCardList = UserInfoManager.Instance.UserCards;
    //     GameObject parent = inventoryContent;
    //     
    //     foreach (var userCard in userCardList)
    //     {
    //         
    //         if(userCard.count == 0)
    //             continue;
    //         
    //         newItemCardObj = Instantiate(itemCard, parent.transform.GetChild(index));
    //         String enName = CardModel.ConvertIdToEnName(userCard.cardId).ToString();
    //         newItemCardObj.name = enName;
    //         newItemCardObj.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/UI/CardItem/" + enName);
    //         newItemCardObj.transform.parent = parent.transform.GetChild(index);
    //         inventoryItemCards.Add(newItemCardObj);
    //         
    //         newItemCardCountObj = Instantiate(itemCardCount, parent.transform.GetChild(index));
    //         newItemCardCountObj.GetComponent<Text>().text = userCard.count.ToString();
    //         newItemCardCountObj.transform.parent = parent.transform.GetChild(index);
    //
    //         index += 1;
    //     }
    // }

    // public void SortCardBtn()
    // {
    //     RemoveInventory();
    //
    //     UserInfoManager.Instance.SortUserCardsByName();
    //     GameObject parent = inventoryContent;
    //
    //     int index = 0;
    //     GameObject newObj;
    //
    //     var myCardList = UserInfoManager.Instance.UserCards;
    //     foreach (var userCard in myCardList)
    //     {
    //         var cardModel = GameInfoManager.Instance.CardMap[userCard.cardId];
    //         newObj = Instantiate(itemCard, parent.transform.GetChild(index));
    //         newObj.name = cardModel.GETName();
    //         newObj.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/ItemCard/"+cardModel.GETName());
    //         newObj.transform.GetChild(1).GetComponent<Text>().text = cardModel.GETName();
    //         newObj.transform.parent = parent.transform.GetChild(index);
    //         (newObj.transform as RectTransform).anchoredPosition = new Vector2(0, 0);
    //         inventoryItemCards.Add(newObj);
    //
    //         index += 1;
    //     }
    //     
    // }
    
    // private void RemoveInventory()
    // {
    //     
    //     GameObject parent = inventoryContent;
    //     
    //
    //     for (int i = 0; i < 20; i++)
    //     {
    //         if (parent.transform.GetChild(i).childCount > 0)
    //         {
    //             Destroy(parent.transform.GetChild(i).GetChild(0).gameObject);
    //             Destroy(parent.transform.GetChild(i).GetChild(1).gameObject);
    //         }
    //     }
    //
    //     inventoryItemCards.Clear();
    // }

    // public void DeleteItemCardBtn()
    // {
    //     if (selectItemCard == null || itemAbilityPanel.activeSelf == false)
    //         return;
    //
    //     int cardId = CardModel.getIdByPrefabName(selectItemCard.name);
    //     int cardCount = UserInfoManager.Instance.userCardMap[cardId].count;
    //
    //     bool isSell = true;
    //     foreach (var userDeck in UserInfoManager.Instance.UserDecks)
    //     {
    //         int deckCardCount = userDeck.cards.Count(x => x == cardId);
    //         if (deckCardCount >= cardCount)
    //         {
    //             isSell = false;
    //             break;
    //         }
    //     }
    //
    //     if (isSell == false)
    //     {
    //         DefaultOneBtnPopup.instance.OpenDefaultOneBtnPopUp(
    //             "해당 카드가\n덱에서 사용중입니다.",
    //             () =>
    //             {
    //                 // 예
    //             });
    //         return;
    //     }
    //     
    //     DefaultPopup.instance.OpenDefaultPopUp(
    //         "카드를 삭제하면 골드를 얻습니다.",
    //         () =>
    //         {
    //             // 예
    //             OnDeleteItemCard();
    //         },
    //         () =>
    //         {
    //             // 아니오
    //         });
    // }

    // private async void OnDeleteItemCard()
    // {
    //     if (selectItemCard == null)
    //         return;
    //
    //     int cardId = CardModel.getIdByPrefabName(selectItemCard.name);
    //     int cardCount = UserInfoManager.Instance.userCardMap[cardId].count;
    //
    //     if (cardCount < 1)
    //         return;
    //     
    //     //카드 삭제
    //     bool result = await UserInfoManager.Instance.RemoveCardInBag(cardId);
    //
    //     if (result == false)
    //         return;
    //     
    //     InitInventoryCardPrefab();
    //     itemAbilityPanel.SetActive(false);
    //     
    //     //100 골드 획득
    //     UserInfoManager.Instance.UpdateGold(100);
    //     SaveSystem.SavePlayerData(GameManager.instance.playerModel);
    // }
    
    private void InitChampionAbility()
    {
        GameManager.instance.playerModel.InitChampionAbility();
        
        ChampionModel champion = GameManager.instance.playerModel.selectChampionModel;
        
        // 공격력
        attackDamage.GetComponent<Text>().text = "+" + champion.attackDamage;

        // 방어력
        armor.GetComponent<Text>().text = "+" + champion.armor;
        
        // 체력
        health.GetComponent<Text>().text = "+" + champion.health;
       
        // 이동속도
        attackSpeed.GetComponent<Text>().text = "+"+ champion.attackSpeed;
        
        // 공격속도
        movementSpeed.GetComponent<Text>().text = "+" + champion.movementSpeed;
    }

    // public async void SetDeckBtn()
    // {
    //     int selectDeckId =GameManager.instance.playerModel.selectDeckId;
    //     UserDeck userDeck = UserInfoManager.Instance.userDeckMap[selectDeckId];
    //
    //     bool result = await UserInfoManager.Instance.SetDeck(userDeck.name, userDeck.id, deckItemCardIds.ToList());
    //
    //     if (result == false)
    //         return;
    // }
    
    public void OnClickTabOkay()
    {
        // onTabClickOkay();
        CloseTabPopup();
    }
    
    public void CloseTabPopup()
    {
        OnDestroyChampionPrefab();
        tabAnim.SetTrigger("close");
        Invoke("DisableTab", 0.6f);
    }

    void DisableTab()
    {
        if(tabAnim.gameObject.activeSelf)
            tabPopUp.SetActive(false);
    }
}
