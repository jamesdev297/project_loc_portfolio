
using System;
using Script;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class MobAIState
{
    protected MobStatusController mobStatusController;
    public MobAIState(
        MobStatusController mobStatusController)
    {
        this.mobStatusController = mobStatusController;
    }

    public virtual void EnterState()
    {
    }

    public virtual void UpdateState()
    {
    }

    public virtual void ExitState()
    {
    }
    
    public Type? RuntimeType { get; }
}

public class MobAIGameEndState : MobAIState
{
//    mobAIFsm.StopAllCoroutine();  

    public MobAIGameEndState(MobStatusController mobStatusController
    ) : base(mobStatusController)
    {
    }

    public override void EnterState()
    {
        mobStatusController.AddEvent(new IdleEvent());
        mobStatusController.StopAllCoroutine();
    }

    public override void UpdateState()
    {
        mobStatusController.animator.SetFloat(Constants.moveMagnitude, 0.0f);
    }

    public override void ExitState()
    {
        
    }
}

public class MobAINoneState : MobAIState
{
//    mobAIFsm.StopAllCoroutine();  

    public MobAINoneState(MobStatusController mobStatusController
    ) : base(mobStatusController)
    {
    }

    public override void EnterState()
    {
        mobStatusController.AddEvent(new IdleEvent());

        mobStatusController.StopAllCoroutine();

        float delayedSec = Random.Range(0.2f, 1.2f);
        
        mobStatusController.StartDelayedAndAction(delayedSec, () =>
        {
            int caseRandom = Random.Range(0, 11);
            Debug.Log("rand:" + caseRandom);
            
            if (caseRandom > 7)
            {
                mobStatusController.updateNewState(mobStatusController.mobAIDistance);
            }
            else if(caseRandom > 2)
            {
                mobStatusController.updateNewState(mobStatusController.mobAIChaseState);
            }
            else
            {
                mobStatusController.updateNewState(mobStatusController.mobAINoneState);
            }

        });
    }

    public override void UpdateState()
    {
        mobStatusController.animator.SetFloat(Constants.moveMagnitude, 0.0f);
    }

    public override void ExitState()
    {
        
    }
}

public class MobAIChaseState : MobAIState
{
    public MobAIChaseState(MobStatusController mobStatusController
    ) : base(mobStatusController)
    {
    }

    public override void EnterState()
    {
        Debug.Log("Enter chase!");
        mobStatusController.StopAllCoroutine();
        mobStatusController.StartRepeatUpdatePath(mobStatusController.target.position);
        mobStatusController.AddEvent(new RunningEvent());
    }

    public override void UpdateState()
    {
       // Debug.Log("running chase!");
        mobStatusController.mobMoveController.GoToWaypoint();
    }

    public override void ExitState()
    {
    }
}

public class MobAIRunAwayState : MobAIState
{
    public MobAIRunAwayState(MobStatusController mobStatusController
    ) : base(mobStatusController)
    {
    }

    public override void EnterState()
    {
        
    }

    public override void UpdateState()
    {
        
    }

    public override void ExitState()
    {
        
    }
}

public class MobAIDistanceState : MobAIState
{
    public MobAIDistanceState(MobStatusController mobStatusController
    ) : base(mobStatusController)
    {
    }

    public override void EnterState()
    {
        Debug.Log("Enter distance!");
        mobStatusController.StopAllCoroutine();
        Vector3 distancePoint = mobStatusController.getDistancePoint();
        mobStatusController.StartRepeatUpdatePath(distancePoint);
        mobStatusController.AddEvent(new RunningEvent());
    }

    public override void UpdateState()
    {
       // Debug.Log("running chase!");
        mobStatusController.mobMoveController.GoToWaypoint();
    }

    public override void ExitState()
    {
        
    }
}