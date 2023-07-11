using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LitJson;
using UnityEngine;


[Serializable]
public class Serialization<TKey, TValue> : ISerializationCallbackReceiver
{
    [SerializeField]
    List<TKey> keys;
    [SerializeField]
    List<TValue> values;

    Dictionary<TKey, TValue> target;
    public Dictionary<TKey, TValue> ToDictionary() { return target; }

    public Serialization(Dictionary<TKey, TValue> target)
    {
        this.target = target;
    }

    public void OnBeforeSerialize()
    {
        keys = new List<TKey>(target.Keys);
        values = new List<TValue>(target.Values);
    }

    public void OnAfterDeserialize()
    {
        var count = Math.Min(keys.Count, values.Count);
        target = new Dictionary<TKey, TValue>(count);
        for (var i = 0; i < count; ++i)
        {
            target.Add(keys[i], values[i]);
        }
    }
}

class GameInfoManager : Singleton<GameInfoManager>
{
    public bool firstLoginUser = false;

    public Dictionary<int, ChampionModel> ChampMap = new Dictionary<int, ChampionModel>();
    public IReadOnlyList<ChampionModel> Champs => ChampMap.Values.ToList().AsReadOnly();
    
    // public List<CardModel> Cards { get; set; }
    // public List<CampaignModel> Campaigns { get; set; }
    
    // public Dictionary<int, CardModel> CardMap = new Dictionary<int, CardModel>();
    // public Dictionary<int, CampaignModel> CampaignMap = new Dictionary<int, CampaignModel>();
    // public Dictionary<string, List<CampaignModel>> StageStepMap = new Dictionary<string, List<CampaignModel>>();

    public ChampionModel NewChampionModel(int champId)
    {
        ChampionModel old = ChampMap[champId];
        ChampionModel championModel = ChampionModel.Init(
            champId,
            old.name,
            old.attackDamage,
            old.armor,
            old.health,
            old.attackSpeed,
            old.movementSpeed,
            old.desc
            );
        
        championModel.skill1DamageModel = old.skill1DamageModel;
        championModel.attack3DamageModel = old.attack3DamageModel;
        championModel.skill2DamageModel = old.skill2DamageModel;
        championModel.skill1IconPath = old.skill1IconPath;
        championModel.skill2IconPath = old.skill2IconPath;
        championModel.skill3IconPath = old.skill3IconPath;
        championModel.normalAttackDamageModel = old.normalAttackDamageModel;
        
        championModel.skills = old.skills;
        championModel.priceTable = old.priceTable;
        return championModel;
    }
    public async Task<bool> GetChamps()
    {
        try
        {
            string json = await WebCall.Instance.GetRequestAsync($"/rest/game-data/champs");
            JsonData jsonData = JsonMapper.ToObject(json);
            Debug.Log("champs : " + json);
            for (var i = 0; i < jsonData["champs"].Count; i++)
            {
                var result = ChampionModel.FromJson(jsonData["champs"][i]);
                ChampMap.Add(result.id, result);
            }

            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Error. {e.Message}");
            return false;
        }
        
    }
    
    // public async Task<List<CardModel>> GetCards()
    // {
    //     try
    //     {
    //         string json = await WebCall.Instance.GetRequestAsync($"/rest/game-data/cards");
    //         Debug.Log("cards : " + json);
    //         JsonData jsonData = JsonMapper.ToObject(json);
    //         Cards = new List<CardModel>();
    //         for (var i = 0; i < jsonData["cards"].Count; i++)
    //         {
    //             var result = CardModel.FromJson(jsonData["cards"][i]);
    //             Cards.Add(result);
    //             CardMap.Add(result.id, result);
    //
    //         }
    //         return Cards;
    //     }
    //     catch (Exception e)
    //     {
    //         Debug.LogError($"Error. {e.Message}");
    //         return null;
    //     }
    // }
    
    // public async Task<List<CampaignModel>> GetCampaigns()
    // {
    //     try
    //     {
    //         string json = await WebCall.Instance.GetRequestAsync($"/rest/game-data/campaigns");
    //         JsonData jsonData = JsonMapper.ToObject(json);
    //         Campaigns = new List<CampaignModel>();
    //         Debug.Log("campaigns " + json);
    //         Debug.Log("campaigns " + jsonData["campaigns"].Count);
    //         for (var i = 0; i < jsonData["campaigns"].Count; i++)
    //         {
    //             var result = CampaignModel.FromJson(jsonData["campaigns"][i]);
    //             Campaigns.Add(result);
    //             CampaignMap.Add(result.id, result);
    //             string key = result.stage + ":" + result.step;
    //             Debug.Log("key " + key  + " - model:" + JsonUtility.ToJson(result));
    //             if (!StageStepMap.ContainsKey(key))
    //             {
    //                 StageStepMap[key] = new List<CampaignModel>();
    //             }
    //             StageStepMap[key].Add(result);
    //         }
    //         return Campaigns;
    //     }
    //     catch (Exception e)
    //     {
    //         Debug.LogError($"Error. {e.Message}");
    //         return null;
    //     }
    // }
}
