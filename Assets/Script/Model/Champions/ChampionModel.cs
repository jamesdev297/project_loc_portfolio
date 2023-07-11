
using System;
using System.Collections.Generic;
using Json;
using LitJson;
using UnityEngine;

[Serializable]
public class ChampionModelDto
{
    public string championType;
    public int attackDamage;
    public int armor;
    public int health;
    public int movementSpeed;
    public int attackSpeed;

    public static ChampionModelDto newChamiponModel(ChampionModel championModel)
    {
        ChampionModelDto championModelDto = new ChampionModelDto();
        championModelDto.championType = championModel.name;
        championModelDto.attackDamage = championModel.attackDamage;
        championModelDto.armor = championModel.armor;
        championModelDto.health = championModel.health;
        championModelDto.movementSpeed = championModel.movementSpeed;
        championModelDto.attackSpeed = championModel.attackSpeed;
        return championModelDto;
    }
}

[Serializable]
public class ChampionModel
{
    public int id;
    public string name;
    public int attackDamage;
    public int armor;
    public int health;
    public float currentHealth;
    public int movementSpeed;
    public int attackSpeed;
    public string desc;
    
    public List<SkillModel1> skills = new List<SkillModel1>();
    public List<PriceModel> priceTable = new List<PriceModel>();

    public DamageModel normalAttackDamageModel;
    public DamageModel attack3DamageModel;
    public DamageModel skill1DamageModel;
    public DamageModel skill2DamageModel;

    public string attackIconPath;
    public string skill1IconPath;
    public string skill2IconPath;
    public string skill3IconPath;

    public virtual void setSkills()
    {
        Debug.Log("In the BaseClass setSkills");
    }

    public virtual void setAttackDamage()
    {
        Debug.Log("In the BaseClass setAttackDamage"); 
    }

    public static ChampionModel Init(int id, string name, int attackDamage, int armor, int health, int attackSpeed, int movementSpeed, string desc)
    {
        ChampionModel championModel = ChampionFactory.Create(id);
        championModel.id = id;
        championModel.name = name;
        championModel.attackDamage = attackDamage;
        championModel.armor = armor;
        championModel.health = health;
        championModel.currentHealth = health;
        championModel.attackSpeed = attackSpeed;
        championModel.movementSpeed = movementSpeed;
        championModel.desc = desc;
        return championModel;
    }
    
    public override string ToString()
    {
        return $"name:{name}\nid{id}\nattackDamage:{attackDamage}\n" +
               $"armor:{armor}\nhealth:{health}\n" +
               $"movementSpeed:{movementSpeed}\nattackSpeed:{attackSpeed}\n" +
               $"desc:{desc}\n";
    }

    public static int getIdByPrefabName(string prefabName)
    {
        switch (prefabName)
        {
            case Constants.Jax:
                return Constants.JaxId;
            case Constants.Thresh:
                return Constants.ThreshId;
            case Constants.Lucian:
                return Constants.LucianId;
            case Constants.Orn:
                return Constants.OrnId;
            case Constants.Akali:
                return Constants.AkaliId;
            default:
                return Constants.JaxId;
        }
    }
    
    public static string GetEnglishNameById(int championId)
    {
        switch (championId)
        {
            case Constants.JaxId:
                return Constants.Jax;
            case Constants.ThreshId:
                return Constants.Thresh;
            case Constants.LucianId:
                return Constants.Lucian;
            case Constants.OrnId:
                return Constants.Orn;
            case Constants.AkaliId:
                return Constants.Akali;
            default:
                return Constants.Jax;
        }
    }
    
    public static ChampionModel FromJson(JsonData map)
    {
        int id = (int) map["id"];
        ChampionModel championModel = ChampionFactory.Create(id);
        championModel.id = id;
        championModel.name = (string) map["name"];
        championModel.armor = (int) map["armor"];
        championModel.health = (int) map["health"];
        championModel.currentHealth = championModel.health;
        championModel.attackDamage = (int) map["attackDamage"];
        championModel.attackSpeed = (int) map["attackSpeed"];
        championModel.movementSpeed = (int) map["movementSpeed"];
        championModel.desc = (string) map["desc"];
        
        if (map["priceTable"] != null)
        {
            for (int i = 0; i < map["priceTable"].Count; i++)
            {
                championModel.priceTable.Add(PriceModel.FromJson(map["priceTable"][i]));
            }
        }
        if (map["skills"] != null)
        {
            for (int i = 0; i < map["skills"].Count; i++)
            {
                championModel.skills.Add(SkillModel1.FromJson(map["skills"][i]));
            }
        }

        championModel.setSkills();
        championModel.setAttackDamage();
        
        return championModel;
    }
}
