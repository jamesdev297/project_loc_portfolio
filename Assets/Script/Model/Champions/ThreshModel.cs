

using System;

[Serializable]
public class ThreshModel : ChampionModel
{
    public ThreshModel()
    {
        attackIconPath = "Image/UI/ingame/thresh/thresh_attack";
        skill1IconPath = "Image/UI/ingame/thresh/thresh_skill2";
        skill2IconPath = "Image/UI/ingame/thresh/thresh_skill1";
        skill3IconPath = "Image/UI/ingame/thresh/thresh_skill3";
    }
    
    public override void setAttackDamage()
    {
        normalAttackDamageModel =  new DamageModel
        {
            powerWeight = attackDamage,
            knuckBackDelayTime = Constants.defaultKnuckBackDelayTime,
        };
    }
    public override void setSkills()
    {
        skill2DamageModel =  new DamageModel
        {
            powerWeight = skills[1].factors[0],
            knuckBackDelayTime = skills[1].factors[1],
            stunDelayTime = skills[1].factors[2],
        };
        
        attack3DamageModel =  new ThreshAttack3
        {
            powerWeight = skills[2].factors[0],
            knuckBackDelayTime = skills[2].factors[1],
        };
    }
}
