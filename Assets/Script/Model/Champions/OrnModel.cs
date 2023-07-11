using System;

[Serializable]
public class OrnModel : ChampionModel
{
    public OrnModel()
    {

        normalAttackDamageModel =  new DamageModel
        {
            powerWeight = attackDamage,
            knuckBackDelayTime = Constants.defaultKnuckBackDelayTime
        };
        skill1DamageModel =  new DamageModel
        {
            knuckBackDelayTime = Constants.defaultKnuckBackDelayTime
        };
        skill2DamageModel =  new DamageModel
        {
            knuckBackDelayTime = Constants.defaultKnuckBackDelayTime
        };
        
    }
}