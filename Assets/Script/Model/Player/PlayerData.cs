using System;
using System.Collections.Generic;
using Script.Model;


[Serializable]
public class PlayerData
{
    public string nickname;
    public int selectChampionId;
    public int selectDeckId;
    
    public PlayerData(PlayerModel player)
    {
        nickname = player.nickname;
        selectChampionId = player.selectChampionId;
        // selectDeckId = player.selectDeckId;
    }
    
    public PlayerData()
    {
       
    }
}