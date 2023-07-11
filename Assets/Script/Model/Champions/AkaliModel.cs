using System;

[Serializable]
public class AkaliModel : ChampionModel
{
    public AkaliModel()
    {
        normalAttackDamageModel =  new DamageModel
        {
            powerWeight = attackDamage,
            knuckBackDelayTime = Constants.defaultKnuckBackDelayTime
        };
        
    }
    
    
}