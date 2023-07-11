using System;
using System.Collections.Generic;
using Json;
using LitJson;
using UnityEngine;

[Serializable]
public class CardModel
{
    
    protected List<AbilityModel> abilityModels = new List<AbilityModel>();

    public int id;
    protected string name;
    protected string desc;
    
    public static int getIdByPrefabName(string name)
    {
        switch (name)
        {
            case "LongSword":
                return 0;
            case "Dagger":
                return 1;
            case "ClothArmor":
                return 2;
            case "Thronmail":
                return 3;
            case "Stonemail":
                return 4;
            case "VampiricScepter":
                return 5;
        }

        return -1;
    }

    public override string ToString()
    {
        return $"name:{name}\nid{id}\ndesc:{desc}\nability:{abilityModels}";
    }

    public static ItemCardFactory.ItemCardType ConvertIdToEnName(int id)
    {
        switch (id)
        {
            case 0:
                return ItemCardFactory.ItemCardType.LongSword;
            case 1:
                return ItemCardFactory.ItemCardType.Dagger;
            case 2:
                return ItemCardFactory.ItemCardType.ClothArmor;
            case 3:
                return ItemCardFactory.ItemCardType.Thronmail;
            case 4:
                return ItemCardFactory.ItemCardType.Stonemail;
            case 5:
                return ItemCardFactory.ItemCardType.VampiricScepter;
            default:
                return ItemCardFactory.ItemCardType.VampiricScepter;
            
        }
    }
    
    // protected void initAbility()
    // {
    //     foreach (var abilityModel in abilityModels)
    //     {
    //         switch (abilityModel.GETName())
    //         {
    //             case Constants.AttackDamage:
    //                 defaultAttackDamage += abilityModel.GETAbility();
    //                 break;
    //             case Constants.Armor:
    //                 defaultArmor += abilityModel.GETAbility();
    //                 break;
    //             case Constants.Health:
    //                 defaultHealth += abilityModel.GETAbility();
    //                 break;
    //             case Constants.MovementSpeed:
    //                 defaultMovementSpeed += abilityModel.GETAbility();
    //                 break;
    //             case Constants.AttackSpeed:
    //                 defaultAttackSpeed += abilityModel.GETAbility();
    //                 break;
    //
    //         }
    //     }
    // }

    public static CardModel FromJson(JsonData map)
    {
        int id = (int) map["id"];
        CardModel cardModel = ItemCardFactory.Create(id);
        cardModel.id = id;
        cardModel.name = (string) map["name"];
        
        if(map.ContainsKey("attackDamage"))
            cardModel.abilityModels.Add(new AttackDamage((int) map["attackDamage"]));
        if(map.ContainsKey("armor"))
            cardModel.abilityModels.Add(new Armor((int) map["armor"]));
        if(map.ContainsKey("health"))
            cardModel.abilityModels.Add(new Health((int) map["health"]));
        if(map.ContainsKey("movementSpeed"))
            cardModel.abilityModels.Add(new MovementSpeed((int) map["movementSpeed"]));
        if(map.ContainsKey("attackSpeed"))
            cardModel.abilityModels.Add(new AttackSpeed((int) map["attackSpeed"]));
        if(map.ContainsKey("lifesteal"))
            cardModel.abilityModels.Add(new LifeSteal((int) map["lifesteal"]));
        if(map.ContainsKey("cooldown"))
            cardModel.abilityModels.Add(new CoolDown((int) map["cooldown"]));
        if(map.ContainsKey("cooldownReduction"))
            cardModel.abilityModels.Add(new CoolDownReduction((int) map["cooldownReduction"]));
        if(map.ContainsKey("reactiveDamage"))
            cardModel.abilityModels.Add(new ReactiveDamage((int) map["reactiveDamage"]));
        
        cardModel.desc = (string) map["desc"];
        return cardModel;
    }
    
    public string GETName()
    {
        if (name == null)
            return "";
        return name;
    }

    public List<AbilityModel> GETAbilities()
    {

        return abilityModels;
    }
}