
using System;
using UnityEngine;

[Serializable]
public class JaxModel : ChampionModel
{
    public JaxModel()
    {
        attackIconPath = "Image/UI/ingame/jax/jax_attack";
        skill1IconPath = "Image/UI/ingame/jax/jax_q";
        skill2IconPath = "Image/UI/ingame/jax/jax_e";
        skill3IconPath = "Image/UI/ingame/jax/jax_roll";

    }

    public override void setAttackDamage()
    {
        Debug.Log("Jax setAttackDamage");
        normalAttackDamageModel =  new DamageModel
        {
            powerWeight = attackDamage,
            knuckBackDelayTime = Constants.defaultKnuckBackDelayTime,
        };
    }
    public override void setSkills()
    {
        Debug.Log("Jax setSkills");
        skill2DamageModel =  new DamageModel
        {
            powerWeight = skills[1].factors[0],
            knuckBackDelayTime = skills[1].factors[1],
            stunDelayTime = skills[1].factors[2],
        };
        
        attack3DamageModel =  new DamageModel
        {
            powerWeight = skills[2].factors[0],
            knuckBackDelayTime = skills[2].factors[1],
        };
    }
}

