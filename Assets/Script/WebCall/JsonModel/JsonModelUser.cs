using LitJson;

namespace Json
{
    public class UserInfo
    {
        public int stamina;
        public long staminaLastTick;
        public double elo;
        public bool admin;
        public int maxStamina;
        public string uid;
        public string name;
        public int gold;
        public int deckPage;

        public override string ToString()
        {
            return $"name:{name}\nuid:{uid}\ngold:{gold}\nstamina:{stamina}\nmaxStamina:{maxStamina}\n" +
                   $"elo:{elo}\nadmin:{admin}\nstaminaLastTick:{staminaLastTick}\ndeckPage:{deckPage}";
        }
        
        public static UserInfo FromJson(JsonData map)
        {
            UserInfo userInfo = new UserInfo();
            userInfo.stamina = (int) map["stamina"];
            userInfo.staminaLastTick = (long) map["staminaLastTick"];
            
            JsonData elo = map["elo"];
            if (elo.IsInt)
            {
                int value = (int) elo;
                userInfo.elo = value * 1.0;
            }else if (elo.IsDouble)
            {
                userInfo.elo = (double) elo;
            }
            
            userInfo.admin = (bool) map["admin"];
            userInfo.maxStamina = (int) map["maxStamina"];
            userInfo.uid = (string) map["uid"];
            userInfo.name = (string) map["name"];
            userInfo.gold = (int) map["gold"];
            userInfo.deckPage = (int) map["deckPage"];
            return userInfo;
        }
        
    }


    // [Serializable]
    // public class UserCard
    // {
    //     public int cardId;
    //     public int count;
    //     public static UserCard FromJson(JsonData map)
    //     {
    //         UserCard userCard = new UserCard();
    //         userCard.cardId = (int) map["cardId"];
    //         userCard.count = (int) map["count"];
    //         return userCard;
    //     }
    //     
    //     public override string ToString()
    //     {
    //         return $"cardId:{cardId}\ncount:{count}\n";
    //     }
    //     
    // }

    public class UserChamp
    {
        public int champId;
        public int level;
        public int exp;
        public int nextExp;
        public long expiryTick;
        
        public static UserChamp FromJson(JsonData map)
        {
            UserChamp userChamp = new UserChamp();
            if (!map.ContainsKey("champId"))
                userChamp.champId = 0;
            else
                userChamp.champId = (int) map["champId"];
            userChamp.level = (int) map["level"];
            userChamp.exp = (int) map["exp"];
            userChamp.nextExp = (int) map["nextExp"];
            userChamp.expiryTick = (long) map["expiryTick"];
            
            return userChamp;
        }
        
        public override string ToString()
        {
            return $"champId:{champId}\nlevel:{level}\nexp:{exp}\nnextExp:{nextExp}\n";
        }
    }

    // [Serializable]
    // public class UserDeck
    // {
    //     public int id;
    //     public string name;
    //     public List<int> cards;
    //     
    //     public static UserDeck FromJson(JsonData map)
    //     {
    //         UserDeck userDeck = new UserDeck();
    //         userDeck.id = (int) map["id"];
    //         userDeck.name = (string) map["name"];
    //         userDeck.cards = new List<int>();
    //         
    //         for (int i = 0; i < map["cards"].Count; i++)
    //         {
    //             userDeck.cards.Add(int.Parse(map["cards"][i].ToString()));
    //         }
    //         
    //         return userDeck;
    //     }
    //
    //     public override string ToString()
    //     {
    //         return $"id:{id}\nname:{name}\ncards:{cards}\n";
    //     }
    // }
    


    // public class UserCampaign
    // {
    //     public int campaignId;
    //     public bool isClear;
    //     public long clearTime;
    //     
    //     public static UserCampaign FromJson(JsonData map)
    //     {
    //         UserCampaign userCampaign = new UserCampaign();
    //         userCampaign.campaignId = (int) map["campaignId"];
    //         userCampaign.isClear = (bool) map["isClear"];
    //         userCampaign.clearTime = (long) map["clearTime"];
    //         return userCampaign;
    //     }
    // }
}