using System;
using System.Collections.Generic;
using Json;
using Script.Model;
using UnityEngine;

[Serializable]
public class PlayerModelDto
{
    public string uid;
    public string nickname;
    // public UserDeck selectDeck;
    public int selectedChampionId;
    public PlayerModelDto(PlayerModel playerModel)
    {
        uid = playerModel.uid;
        nickname = playerModel.nickname;
        // selectDeck = UserInfoManager.Instance.userDeckMap[playerModel.selectDeckId];
        selectedChampionId = playerModel.selectChampionId;
    }
}

[Serializable]
public class PlayerModel
{
    public string uid;
    // public string dataPath;
    public string nickname;
    public int selectChampionId;
    // public int selectDeckId;
    // public UserDeck selectDeckModel;
    public ChampionModel selectChampionModel;
    public PlayerModel(string uid)
    {
        this.uid = uid;
    }

    public PlayerModel(String nickname, int selectChampionId)
    {
        this.nickname = nickname;
        this.selectChampionId = selectChampionId;
    }

    public PlayerModel(PlayerModelDto playerModelDto)
    {
        nickname = playerModelDto.nickname;
        selectChampionId = playerModelDto.selectedChampionId;
        // selectDeckModel = playerModelDto.selectDeck;
    }

    public void setSelectChampId(int id)
    {
        selectChampionId = id;
        selectChampionModel = GameInfoManager.Instance.NewChampionModel(id);
        
        SaveSystem.SavePlayerData(this);
    }
    
    // public void setSelectDeckId(int id)
    // {
    //     selectDeckId = id;
    //     
    //     SaveSystem.SavePlayerData(this);
    // }
    
    public void InitPlayerModel(string nickname, int selectChampionId, int selectDeckId)
    {
        this.nickname = nickname;
        this.selectChampionId = selectChampionId;
        // this.selectDeckId = selectDeckId;
    }
    
    public void InitChampionAbility()
    {

        selectChampionModel = GameInfoManager.Instance.NewChampionModel(selectChampionId);
        
        int attackDamageTotal = selectChampionModel.attackSpeed;
        int armorTotal = selectChampionModel.armor;
        int maxHealthTotal = selectChampionModel.health;
        int attackSpeedTotal = selectChampionModel.attackSpeed;
        int movementSpeedTotal = selectChampionModel.movementSpeed;

        /*UserDeck userDeck;
        UserInfoManager.Instance.userDeckMap.TryGetValue(selectDeckId, out userDeck);
        
        if (userDeck == null)
        {
            Debug.Log("user deck null : " + selectDeckId);
            return;
        }*/
        
        // 덱 능력 추가
        /*foreach (var cardId in userDeck.cards)
        {
            if(cardId == -1)
                continue;
            
            var cardModel = GameInfoManager.Instance.CardMap[cardId];
            
            foreach (var abilityModel in cardModel.GETAbilities())
            {
                
                Debug.Log("ability : " + abilityModel.GETName());
                
                if (abilityModel.GETName().Equals("AttackDamage"))
                {
                    attackDamageTotal += abilityModel.GETAbility();
                } else if (abilityModel.GETName().Equals("Armor"))
                {
                    armorTotal += abilityModel.GETAbility();
                } else if (abilityModel.GETName().Equals("Health"))
                {
                    maxHealthTotal += abilityModel.GETAbility();
                } else if (abilityModel.GETName().Equals("AttackSpeed"))
                {
                    attackSpeedTotal += abilityModel.GETAbility();
                } else if (abilityModel.GETName().Equals("MovementSpeed"))
                {
                    movementSpeedTotal += abilityModel.GETAbility();
                }
            }
        }*/
        
        selectChampionModel.attackDamage = attackDamageTotal;
        selectChampionModel.armor = armorTotal;
        selectChampionModel.health = maxHealthTotal;
        selectChampionModel.currentHealth = maxHealthTotal;
        selectChampionModel.attackSpeed = attackSpeedTotal;
        selectChampionModel.movementSpeed = movementSpeedTotal;

        //Debug.Log($"champion ability {JsonUtility.ToJson(selectChampionModel)}");
    }
    
    public void SavePlayer()
    {
        SaveSystem.SavePlayerData(this);
    }
    
    public PlayerData LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer(uid);
        if (data != null)
        {
            nickname = data.nickname;
            selectChampionId = data.selectChampionId;
            // selectDeckId = data.selectDeckId;
        }
        return data;
    }
    
}