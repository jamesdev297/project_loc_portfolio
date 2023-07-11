using System;
using LitJson;
using UnityEngine;

namespace Script.Model.Game
{
    public class GameOutModel
    {
        private string status;
        private string gameId;
        private string winnerUid;
        public string gameoutDttm;
        private GamePlayerModel playerA;
        private GamePlayerModel playerB;

        public string GetStatus()
        {
            return status;
        }
        
        public string GetGameId()
        {
            return gameId;
        }
        
        public string GetWinnerUid()
        {
            return winnerUid;
        }

        public GamePlayerModel GetPlayerA()
        {
            return playerA;
        }
        
        public GamePlayerModel GetPlayerB()
        {
            return playerB;
        }
        
        public static GameOutModel FromJson(JsonData map)
        {
            GameOutModel gameOutModel = new GameOutModel();
            gameOutModel.status = (string) map["status"];
            gameOutModel.gameId = (string) map["gameId"];
            gameOutModel.winnerUid = (string) map["winnerUid"];
            gameOutModel.gameoutDttm = (string) map["gameoutDttm"];
            if (map["playerA"] != null)
            {
                gameOutModel.playerA = GamePlayerModel.FromJson(map["playerA"]);
            }
            if (map["playerB"] != null)
            {
                gameOutModel.playerB = GamePlayerModel.FromJson(map["playerB"]);
            }
            return gameOutModel;
        }
    }
}