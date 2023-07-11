
using System.Collections.Generic;
using Json;
using LitJson;

namespace Reward
{
    public class RewardModel
    {
        public List<Reward.Card> cards;
        public int gold;
        public int exp;
        
        public static RewardModel FromJson(JsonData map)
        {
            RewardModel rewardModel = new RewardModel();
            rewardModel.cards = new List<Card>();
            for (var i = 0; i < map["cards"].Count; i++)
            {
                rewardModel.cards.Add(Card.FromJson(map["cards"][i]));
            }
            
            rewardModel.gold = (int) map["gold"];
            rewardModel.exp = (int) map["exp"];
            return rewardModel;
        }
    }

    public class Card
    {
        public int cardId;
        
        public static Card FromJson(JsonData map)
        {
            Card card = new Card();
            card.cardId = (int) map["cardId"];
            return card;
        }
    }
}

