using System;
using UnityEditor.UIElements;
using UnityEngine;
using Random = System.Random;

public class MobJaxStatusController : MobStatusController
{
    private Random random = new Random();
    
    // Start is called before the first frame update
    private void Awake()
    {
        moveController = GetComponent<MobMoveController>();
    }
    
    protected override void stateInit()
    {
        mobAINoneState = new MobJaxAINoneState(this);
        mobAIChaseState = new MobJaxAIChaseState(this);
        mobAIRunAwayState = new MobJaxAIRunAwayState(this);
        mobAIDistance = new MobJaxAIDistanceState(this);
    }
    
    protected override void NormalAttackTrigger()
    {
        if (random.NextDouble() > 0.7f)
        {
            championBehavior.normalAttackCoolTime = 1.0f + (float) random.NextDouble();
        }
            
        if (championBehavior.normalAttackCnt++ >= 3)
        {
            championBehavior.normalAttackCnt = 0;
            animator.SetTrigger("attack3");
        }
        else
        {
            animator.SetTrigger("attack1");
        }
    } 
}
