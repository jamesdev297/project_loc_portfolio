
using UnityEngine;

public class DamageModel
{
    public GameObject owner;
    public int addedFixedPower = 0;
    public float powerWeight = 1.0f;
    public float stunDelayTime = 0;
    public float knuckBackDelayTime = 0;
    public int KnuckBackPower = 0;
    
    public DamageModel()
    {
        
    }
}

public class ThreshAttack3 : DamageModel
{
    
}