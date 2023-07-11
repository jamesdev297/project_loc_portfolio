
using Json;
using LitJson;
using Reward;

public class CampaignModel
{
    public int stage;
    public int step;
    public int stamina;
    public string name;
    public int id;
    public string desc;
    public int difficulty;
    public RewardModel reward;
    public string getStageStepString()
    {
        return stage + "-" + step;
    }
    public CampaignModel()
    {
        
    }
    
    public static CampaignModel FromJson(JsonData map)
    {
        CampaignModel campaignModel = new CampaignModel();
        campaignModel.stage = (int) map["stage"];
        campaignModel.step = (int) map["step"];
        campaignModel.stamina = (int) map["stamina"];
        campaignModel.id = (int) map["id"];
        campaignModel.desc = (string) map["desc"];
        campaignModel.difficulty = (int) map["difficulty"];
        campaignModel.reward = RewardModel.FromJson(map["reward"]);
        return campaignModel;
    }
}
