using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Json;
using LitJson;
using UnityEngine;

class UserInfoManager : Singleton<UserInfoManager>
{
    public UserInfo Info { get; set; }
    
    // private List<UserCard> _UserCards { get; set; }
    // private List<UserDeck> _UserDecks { get; set; }
    // private List<UserCampaign> _UserCampaigns { get; set; }

    // public IReadOnlyList<UserCard> UserCards => _UserCards.AsReadOnly();
    public Dictionary<int, UserChamp> userChampionMap = new Dictionary<int, UserChamp>();
    public IReadOnlyList<UserChamp> UserChamps => userChampionMap.Values.ToList().AsReadOnly();
    // public IReadOnlyList<UserDeck> UserDecks => _UserDecks.AsReadOnly();
    // public IReadOnlyList<UserCampaign> UserCampaigns => _UserCampaigns.AsReadOnly();
    
    // public Dictionary<int, UserCard> userCardMap = new Dictionary<int, UserCard>();
    // public Dictionary<int, UserDeck> userDeckMap = new Dictionary<int, UserDeck>();
    // public Dictionary<int, UserCampaign> userCampaignMap = new Dictionary<int, UserCampaign>();
    
    public int clearStep = 0;
    public long serverTick = 0;
    public long diffTick = 0;

    
    
    public async Task<UserInfo> GetUserInfo()
    {
        try
        {
            string json = await WebCall.Instance.GetRequestAsync($"/rest/user/{FirebaseAuth.Instance.User.UserId}");
            Info = JsonUtility.FromJson<UserInfo>(json);
            Debug.Log("GetUserInfo : " + json);
            Debug.Log("stamina : " + Info.stamina);
            Debug.Log("staminaLastTick : " + Info.staminaLastTick);
            
            return Info;
        }
        catch (Exception e)
        {
            Debug.LogError($"Error. {e.Message}");
            return null;
        }
    }

    public async Task<bool> SetName(string name)
    {
        try
        {
            Dictionary<string, string> body = new Dictionary<string, string>();
            body.Add("name", name);
            
            string json = await WebCall.Instance.PostRequestAsync($"/action/set-name", JsonMapper.ToJson(body));
            var userInfo = JsonUtility.FromJson<UserInfo>(json);
            if (userInfo.name != null)
            {
                Info.name = userInfo.name;
            }
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Error. {e.Message}");
            return false;
        }
    }
    
    public async Task<bool> GetUserChamps()
    {
        try
        {
            string json = await WebCall.Instance.GetRequestAsync($"/rest/user/{FirebaseAuth.Instance.User.UserId}/champs");
            JsonData jsonData = JsonMapper.ToObject(json);
            Debug.Log("users : " + json);

            for (var i = 0; i < jsonData["champs"].Count; i++)
            {
                UserChamp userChamp = UserChamp.FromJson(jsonData["champs"][i]);
                userChampionMap.Add(userChamp.champId, userChamp);
            }

            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Error. {e.Message}");
            return false;
        }
    }
    
    public async Task<bool> GetServerTick()
    {
        try
        {
            string json = await WebCall.Instance.GetRequestAsync($"/rest/etc/server-tick");
            JsonData jsonData = JsonMapper.ToObject(json);

            serverTick = (long) jsonData["currentTick"];
            diffTick = DateTimeOffset.Now.ToUnixTimeMilliseconds() - serverTick;
            
            Debug.Log("serverTikck : " + serverTick);
            
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Error. {e.Message}");
            return false;
        }
    }
    
    // public async Task<List<UserCard>> GetUserCards()
    // {
    //     try
    //     {
    //         string json = await WebCall.Instance.GetRequestAsync($"/rest/user/{FirebaseAuth.Instance.User.UserId}/cards");
    //         JsonData jsonData = JsonMapper.ToObject(json);
    //         Debug.Log("User Cards : "+json);
    //         _UserCards = new List<UserCard>();
    //         for (var i = 0; i < jsonData["cards"].Count; i++)
    //         {
    //             UserCard userCard = UserCard.FromJson(jsonData["cards"][i]);
    //             _UserCards.Add(userCard);
    //             userCardMap.Add(userCard.cardId, userCard);
    //         }
    //         return _UserCards;
    //     }
    //     catch (Exception e)
    //     {
    //         Debug.LogError($"Error. {e.Message}");
    //         return null;
    //     }
    // }
    
    // public async Task<List<UserDeck>> GetUserDecks()
    // {
    //     try
    //     {
    //         string json = await WebCall.Instance.GetRequestAsync($"/rest/user/{FirebaseAuth.Instance.User.UserId}/decks");
    //         
    //         Debug.Log("user deck : "+json);
    //         JsonData jsonData = JsonMapper.ToObject(json);
    //         _UserDecks = new List<UserDeck>();
    //         
    //         for (var i = 0; i < jsonData["decks"].Count; i++)
    //         {
    //             var userDeck = UserDeck.FromJson(jsonData["decks"][i]);
    //             _UserDecks.Add(userDeck);
    //             userDeckMap.Add(userDeck.id, userDeck);
    //         }
    //         return _UserDecks;
    //     }
    //     catch (Exception e)
    //     {
    //         Debug.LogError($"Error. {e.Message}");
    //         return null;
    //     }
    // }
    
    // public async Task<List<UserCampaign>> GetUserCampaigns()
    // {
    //     try
    //     {
    //         string json = await WebCall.Instance.GetRequestAsync($"/rest/user/{FirebaseAuth.Instance.User.UserId}/campaigns");
    //         JsonData jsonData = JsonMapper.ToObject(json);
    //         _UserCampaigns = new List<UserCampaign>();
    //         for (var i = 0; i < jsonData["campaigns"].Count; i++)
    //         {
    //             var userCampaign = UserCampaign.FromJson(jsonData["campaigns"][i]);
    //             _UserCampaigns.Add(userCampaign);
    //             userCampaignMap.Add(userCampaign.campaignId, userCampaign);
    //             if (userCampaign.isClear)
    //             {
    //                 clearStep = Mathf.Max(GameInfoManager.Instance.CampaignMap[userCampaign.campaignId].step,
    //                     clearStep);
    //             }
    //         }
    //         return _UserCampaigns;
    //     }
    //     catch (Exception e)
    //     {
    //         Debug.LogError($"Error. {e.Message}");
    //         return null;
    //     }
    // }

    // public UserDeck GetSelectDeck()
    // {
    //     if (GameManager.instance.playerModel.selectDeckId < 0)
    //         return null;
    //     UserDeck userDeck;
    //     userDeckMap.TryGetValue(GameManager.instance.playerModel.selectDeckId, out userDeck);
    //     return userDeck;
    // }

    // public int GetNextDeckId()
    // {
    //     // int[] arr = new int[20];
    //     // for (int i = 0; i < 20; i++)
    //     // {
    //     //     arr[i] = 0;
    //     // }
    //     // foreach (var userDeck in UserDecks)
    //     // {
    //     //     arr[userDeck.id] = 1;
    //     // }
    //     // for (int i = 0; i < 20; i++)
    //     // {
    //     //     if (arr[i] == 0)
    //     //     {
    //     //         return i;
    //     //     }
    //     // }
    //
    //     return UserDecks.Count;
    // }

    // public void AddCardInBag(int cardId)
    // {
    //     UserCard userCard;
    //     userCardMap.TryGetValue(cardId, out userCard);
    //     if (userCard != null)
    //     {
    //         userCard.count += 1;
    //     }
    //     else
    //     {
    //         userCard = new UserCard();
    //         userCard.count = 1;
    //         userCard.cardId = cardId;
    //         _UserCards.Add(userCard);
    //     }
    // }
    
    // public async Task<bool> RemoveCardInBag(int cardId)
    // {
    //     try
    //     {
    //         
    //         var result = await SellCard(cardId);
    //
    //         return result;
    //     }
    //     catch (Exception e)
    //     {
    //         Debug.LogError($"Error. {e.Message}");
    //         return false;
    //     }
    // }

    // public void AddCardInDeck(int deckId, int cardId)
    // {
    //     UserDeck userDeck;
    //     userDeckMap.TryGetValue(deckId, out userDeck);
    //     if (userDeck == null)
    //         return ;
    //     // var IEuserCard = userDeck.cards.Where((card => card.cardId == cardId));
    //     // if (IEuserCard != null && IEuserCard.Count() > 0)
    //     // {
    //     //     var userCard = IEuserCard.First();
    //     //     userCard.count += 1;
    //     // }
    //     // else
    //     // {
    //     //     var userCard = new UserCard();
    //     //     userCard.cardId = cardId;
    //     //     userCard.count = 1;
    //     //     userDeck.cards.Add(userCard);
    //     // }
    // }

    // public void AddDeck(int deckId, string deckName)
    // {
    //     if (userDeckMap.ContainsKey(deckId))
    //         return;
    //
    //     var userDeck = new UserDeck();
    //     userDeck.name = deckName;
    //     userDeck.id = deckId;
    //     _UserDecks.Add(userDeck);
    //     userDeckMap.Add(deckId, userDeck);
    // }

    // public void RemoveDeck(int deckId)
    // {
    //     UserDeck userDeck;
    //     userDeckMap.TryGetValue(deckId, out userDeck);
    //     if (userDeck == null)
    //         return;
    //     _UserDecks.Remove(userDeck);
    //     userDeckMap.Remove(userDeck.id);
    // }

    // public void RemoveCardInDeck(int deckId, int cardIndex)
    // {
    //     if(userDeckMap.ContainsKey(deckId))
    //         userDeckMap[deckId].cards.RemoveAt(cardIndex);
    // }

    // deprecated API
    // public async Task<bool> AddUserChamp(int champId)
    // {
    //     try
    //     {
    //         Dictionary<string, int> body = new Dictionary<string, int>();
    //         body.Add("champId", champId);
    //         
    //         string json = await WebCall.Instance.PostRequestAsync($"/action/purchase-champ", JsonMapper.ToJson(body));
    //         JsonData jsonData = JsonMapper.ToObject(json);
    //
    //         UserInfo userInfo = UserInfo.FromJson(jsonData["userInfo"]);
    //         UserChamp userChamp = UserChamp.FromJson(jsonData["userChampInfo"]);
    //
    //         _UserChamps.Add(userChamp);
    //         userChampionMap.Add(userChamp.champId, userChamp);
    //         
    //         return true;
    //     }
    //     catch (Exception e)
    //     {
    //         Debug.LogError($"Error. {e.Message}");
    //         return false;
    //     }
    // }

    public async Task<bool> PurchaseChamp(int champId, int day)
    {
        try
        {
            Dictionary<string, int> body = new Dictionary<string, int>();
            body.Add("champId", champId);
            body.Add("day", day);
            
            string json = await WebCall.Instance.PostRequestAsync($"/action/purchase-champ", JsonMapper.ToJson(body));
            JsonData jsonData = JsonMapper.ToObject(json);

            UserInfo userInfo = UserInfo.FromJson(jsonData["userInfo"]);
            Info.gold = userInfo.gold;
            UserChamp userChamp = UserChamp.FromJson(jsonData["userChampInfo"]);
            if (userChampionMap.ContainsKey(champId) == true)
            {
                userChampionMap[champId].expiryTick = userChamp.expiryTick;
            }
            else
            {
                userChampionMap.Add(userChamp.champId, userChamp);
            }

            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Error. {e.Message}");
            return false;
        }
    }
    
    // public bool IsClearCampaign(int campaignId)
    // {
    //     return userCampaignMap.ContainsKey(campaignId) && userCampaignMap[campaignId].isClear;
    // }

    public void UpdateGold(int goldOffset)
    {
        Info.gold += goldOffset;
        
        ShowGold();
        
        //TODO send server

    }

    public void ShowGold()
    {
        var environmentGameObject = GameObject.Find("Enviroment");
        if (environmentGameObject != null)
        {
            MainScene mainScene = environmentGameObject.GetComponent<MainScene>();
            if (mainScene != null)
            {
                mainScene.gold.text = Utils.GetCommaNum(Info.gold);
            }
        }
    }
    
    public async Task<bool> RewardedStamina(string key)
    {
        try
        {
            Dictionary<string, string> body = new Dictionary<string, string>();
            body.Add("key", key);
            
            string json = await WebCall.Instance.PostRequestAsync($"/action/ad-reward-stamina", JsonMapper.ToJson(body));
            JsonData jsonData = JsonMapper.ToObject(json);
            
            UserInfo userInfo = UserInfo.FromJson(jsonData);
            Info.stamina = userInfo.stamina;
            Info.staminaLastTick = userInfo.staminaLastTick;

            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Error. {e.Message}");
            return false;
        }
    }
    
    public async Task<bool> RewardedMoreGold(string key, string gameId)
    {
        try
        {
            Dictionary<string, string> body = new Dictionary<string, string>();
            body.Add("key", key);
            body.Add("gameId", gameId);
            
            string json = await WebCall.Instance.PostRequestAsync($"/action/ad-reward-more-gold", JsonMapper.ToJson(body));
            JsonData jsonData = JsonMapper.ToObject(json);
            
            UserInfo userInfo = UserInfo.FromJson(jsonData);
            Info.gold = userInfo.gold;

            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Error. {e.Message}");
            return false;
        }
    }
    
    public async Task<bool> MultiPlay()
    {
        try
        {
            Dictionary<string, string> body = new Dictionary<string, string>();

            string json = await WebCall.Instance.PostRequestAsync($"/action/play-multi", JsonMapper.ToJson(body));
            JsonData jsonData = JsonMapper.ToObject(json);
            
            Debug.Log("MultiPlay : " + json);

            UserInfo userInfo = UserInfo.FromJson(jsonData);
            Info.stamina = userInfo.stamina;
            Info.staminaLastTick = userInfo.staminaLastTick;

            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Error. {e.Message}");
            return false;
        }
    }
    
    // public void SortUserCardsByName()
    // {
    //     _UserCards = _UserCards.OrderBy(x => GameInfoManager.Instance.CardMap[x.cardId].GETName()).ToList();
    // }
    
    // public async Task<bool> AddDeck()
    // {
    //     try
    //     {
    //         string json = await WebCall.Instance.PostRequestAsync($"/action/purchase-deckpage", "");
    //         Debug.Log("add deck : " + json);
    //         JsonData jsonData = JsonMapper.ToObject(json);
    //         var userInfo = UserInfo.FromJson(jsonData["userInfo"]);
    //         var userDeck = UserDeck.FromJson(jsonData["deckInfo"]);
    //         
    //         Info.deckPage = userInfo.deckPage;
    //         userDeckMap.Add(userDeck.id, userDeck);
    //         _UserDecks.Add(userDeck);
    //         
    //         return true;
    //     }
    //     catch (Exception e)
    //     {
    //         Debug.LogError($"Error. {e.Message}");
    //         return false;
    //     }
    // }
    
    // public async Task<bool> SellCard(int cardId)
    // {
    //     try
    //     {
    //
    //         Dictionary<string, int> body = new Dictionary<string, int>();
    //         body.Add("cardId", cardId);
    //         body.Add("count", 1);
    //
    //         string json = await WebCall.Instance.PostRequestAsync($"/action/sell-card", JsonMapper.ToJson(body));
    //         
    //         Debug.Log("Sell Card : " + json);
    //         var userCard = JsonUtility.FromJson<UserCard>(json);
    //         if (userCard.count == 0)
    //         {
    //             userCardMap.Remove(cardId);
    //
    //             int index = 0;
    //             for (var i = 0; i < _UserCards.Count; i++)
    //             {
    //                 if (_UserCards[i].count == 0)
    //                 {
    //                     index = i;
    //                     break;
    //                 }
    //             }
    //             _UserCards.RemoveAt(index);
    //         }
    //         else
    //         {
    //             userCardMap[cardId].count = userCard.count;
    //             _UserCards.Where((card => card.cardId == cardId)).First().count = userCard.count;
    //         }
    //         
    //         return true;
    //     }
    //     catch (Exception e)
    //     {
    //         Debug.LogError($"Error. {e.Message}");
    //         return false;
    //     }
    // }
    
    // public async Task<bool> SetDeck(string deckName, int deckId, List<int> deckItemCards)
    // {
    //     try
    //     {
    //        
    //         Dictionary<string, dynamic> body = new Dictionary<string, dynamic>();
    //         body.Add("name", deckName);
    //         body.Add("cards", deckItemCards);
    //         
    //         string json = await WebCall.Instance.PostRequestAsync($"/rest/user/{FirebaseAuth.Instance.User.UserId}/decks/{deckId}", JsonMapper.ToJson(body));
    //         Debug.Log("set deck : " + json);
    //         
    //         var userDeck = JsonUtility.FromJson<UserDeck>(json);
    //
    //         if (userDeckMap.ContainsKey(userDeck.id))
    //         {
    //             userDeckMap[userDeck.id] = userDeck;
    //             int index = 0;
    //             for (int i = 0; i < _UserCards.Count; i++)
    //             {
    //                 if (userDeck.id == _UserDecks[i].id)
    //                 {
    //                     index = i;
    //                     break;
    //                 }
    //             }
    //             _UserDecks[index] = userDeck;
    //         }
    //
    //         return true;
    //     }
    //     catch (Exception e)
    //     {
    //         Debug.LogError($"Error. {e.Message}");
    //         return false;
    //     }
    // }
}
