
using System;
using JetBrains.Annotations;
using Script.Champions.Jax;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class MobJaxAINoneState : MobAINoneState
{
    private JaxBehaviour _jaxBehaviour;

    public MobJaxAINoneState(MobStatusController mobStatusController
    ) : base(mobStatusController)
    {
        _jaxBehaviour = mobStatusController.championBehavior as JaxBehaviour;
    }
    
    public override void UpdateState()
    {
        if (_jaxBehaviour.championStatusController == null)
            return;
        
        if (_jaxBehaviour.isDie == true)
            return;
        
        if (_jaxBehaviour.isMoveEnable && (
            _jaxBehaviour.championStatusController.championState == ChampionState.IDLE ||
            _jaxBehaviour.championStatusController.championState == ChampionState.RUNNING ))
        {
            if (_jaxBehaviour.attack1Range.GetComponent<AttackRangeScript>().target != null)
            {
                if (_jaxBehaviour.skill2CoolTime <= 0.0f) {
                    mobStatusController.AddEvent(new Skill2Event());
                } else if (_jaxBehaviour.skill3CoolTime <= 0.0f) {
                    mobStatusController.AddEvent(new Skill3Event());
                } else if (_jaxBehaviour.skill1CoolTime <= 0.0f && _jaxBehaviour.isInQRange(mobStatusController.target.position))
                {
                    _jaxBehaviour.qTargetVec = mobStatusController.target.position;
                    mobStatusController.AddEvent(new Skill1Event());
                } else {
                    if(_jaxBehaviour.normalAttackCoolTime <= 0.0f)
                        mobStatusController.AddEvent(new NormalAttackEvent());
                    else if(_jaxBehaviour.normalAttackCoolTime >= 0.2f)
                        mobStatusController.updateNewState(mobStatusController.mobAIRunAwayState);
                }
            }
            else
            {
                if (_jaxBehaviour.skill2CoolTime <= 0.0f) {
                    mobStatusController.AddEvent(new JaxSkill2Event());
                } else if (_jaxBehaviour.skill3CoolTime <= 0.0f) {
                    mobStatusController.AddEvent(new JaxSkill3Event());
                } else if (_jaxBehaviour.skill1CoolTime <= 0.0f && _jaxBehaviour.isInQRange(mobStatusController.target.position))
                {
                    _jaxBehaviour.qTargetVec = mobStatusController.target.position;
                    mobStatusController.AddEvent(new JaxSkill1Event());
                } 
            }
        }
        base.UpdateState();
    }
}

public class MobJaxAIChaseState : MobAIChaseState
{
    private JaxBehaviour _jaxBehaviour;
    public MobJaxAIChaseState(MobStatusController mobStatusController
    ) : base(mobStatusController)
    {
        _jaxBehaviour = mobStatusController.championBehavior as JaxBehaviour;
    }
    
    public override void UpdateState()
    {
        //Debug.Log("chasing...");
        if (_jaxBehaviour.isDie == true)
            return;    
        
        if (_jaxBehaviour.skill2CoolTime <= 0.0f) {
            mobStatusController.AddEvent(new Skill2Event());
        }else if (_jaxBehaviour.skill3CoolTime <= 0.0f) {
            mobStatusController.AddEvent(new Skill3Event());
        }else if (_jaxBehaviour.skill1CoolTime <= 0.0f && _jaxBehaviour.isInQRange(mobStatusController.target.position))
        {
            _jaxBehaviour.qTargetVec = mobStatusController.target.position;
            mobStatusController.AddEvent(new Skill1Event());
        } else if (_jaxBehaviour.attack1Range.GetComponent<AttackRangeScript>().target != null
              &&  _jaxBehaviour.normalAttackCoolTime <= 0.0f)
        {
            mobStatusController.AddEvent(new NormalAttackEvent());
            mobStatusController.updateNewState(mobStatusController.mobAINoneState);
        } else {
            base.UpdateState();
        }
    }
}

public class MobJaxAIRunAwayState : MobAIRunAwayState
{
    public MobJaxAIRunAwayState(MobStatusController mobStatusController
    ) : base(mobStatusController)
    {
        
    }
}

public class MobJaxAIDistanceState : MobAIDistanceState
{
    private JaxBehaviour _jaxBehaviour;
    public MobJaxAIDistanceState(MobStatusController mobStatusController
    ) : base(mobStatusController)
    {
        _jaxBehaviour = mobStatusController.championBehavior as JaxBehaviour;
    }
    
    public override void UpdateState()
    {

        if (_jaxBehaviour.isDie == true)
            return;
        
        if (_jaxBehaviour.skill2CoolTime <= 0.0f) {
            mobStatusController.AddEvent(new Skill2Event());
        } else if (_jaxBehaviour.skill3CoolTime <= 0.0f) {
            mobStatusController.AddEvent(new Skill3Event());
        } else if (_jaxBehaviour.skill1CoolTime <= 0.0f && _jaxBehaviour.isInQRange(mobStatusController.target.position))
        {
            _jaxBehaviour.qTargetVec = mobStatusController.target.position;
            mobStatusController.AddEvent(new Skill1Event());
        } else if (_jaxBehaviour.attack1Range.GetComponent<AttackRangeScript>().target != null
              &&  _jaxBehaviour.normalAttackCoolTime <= 0.0f)
        {
            mobStatusController.AddEvent(new NormalAttackEvent());
            mobStatusController.updateNewState(mobStatusController.mobAINoneState);
        } else {
            base.UpdateState();
        }
    }
}