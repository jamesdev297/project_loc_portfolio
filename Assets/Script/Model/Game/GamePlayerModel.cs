using System;
using LitJson;
using UnityEngine;

namespace Script.Model.Game
{
    public class GamePlayerModel
    {
        private string uid;
        private string name;
        private double elo;
        private double gainingElo;
        private int gainingGold;


        public int GetGainingGold()
        {
            return gainingGold;
        }

        public double GetGainingElo()
        {
            return gainingElo;
        }

        public double GetElo()
        {
            return elo;
        }
        
        public string GetUid()
        {
            return uid;
        }
        
        public static GamePlayerModel FromJson(JsonData map)
        {
            GamePlayerModel gamePlayerModel = new GamePlayerModel();
            Debug.Log($"GamePlayerModel {map["gainingElo"]}");
            
            gamePlayerModel.uid = (string) map["uid"];
            gamePlayerModel.name = (string) map["name"];
            JsonData elo = map["elo"];
            if (elo.IsInt)
            {
                int value = (int) elo;
                gamePlayerModel.elo = value * 1.0;
            }else if (elo.IsDouble)
            {
                gamePlayerModel.elo = (double) elo;
            }
            
            JsonData gainingElo = map["gainingElo"];
            if (gainingElo.IsInt)
            {
                int value = (int) gainingElo;
                gamePlayerModel.gainingElo = value * 1.0;
            }else if (gainingElo.IsDouble)
            {
                gamePlayerModel.gainingElo = (double) gainingElo;
            }
            gamePlayerModel.gainingGold = (int) map["gainingGold"];
            return gamePlayerModel;
        }
    }
}