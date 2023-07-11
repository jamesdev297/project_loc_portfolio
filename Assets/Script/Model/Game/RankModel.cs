namespace Script.Model.Game
{
    using LitJson;

    public class RankModel
    {
        public string uid;
        public string name;
        public double elo;
        
        public static RankModel FromJson(JsonData map)
        {
            RankModel rankModel = new RankModel();
            rankModel.uid = (string) map["uid"];
            rankModel.name = (string) map["name"];
            JsonData elo = map["elo"];
            if (elo.IsInt)
            {
                int value = (int) elo;
                rankModel.elo = value * 1.0;
            }else if (elo.IsDouble)
            {
                rankModel.elo = (double) elo;
            }
            return rankModel;
        }
    }
}