
using System;
using UnityEngine;

[Serializable]
public class LucianModel : ChampionModel
{
    public LucianModel()
    {
        attackIconPath = "Image/UI/ingame/lucian/lucian_icon";
        skill1IconPath = "Image/UI/ingame/lucian/lucian_icon-3";
        skill2IconPath = "Image/UI/ingame/lucian/lucian_icon-2";
        skill3IconPath = "Image/UI/ingame/lucian/lucian_icon-4";

    }

    public override void setAttackDamage()
    {
        Debug.Log("Lucian setAttackDamage");
        normalAttackDamageModel =  new DamageModel
        {
            powerWeight = attackDamage,
            knuckBackDelayTime = Constants.defaultKnuckBackDelayTime
        };
    }

    public override void setSkills()
    {
        Debug.Log("Lucian setSkills");
        skill1DamageModel =  new DamageModel
        {
            powerWeight = skills[0].factors[0],
        };

        skill2DamageModel = new DamageModel{
            powerWeight = skills[1].factors[0],
            knuckBackDelayTime = skills[1].factors[1],
        };
        
        attack3DamageModel =  new DamageModel
        {
            powerWeight = skills[2].factors[0],
        };
    }
}
