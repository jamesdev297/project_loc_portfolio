using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Script.Champions.Thresh
{
    public class MobThreshAINoneState : MobAINoneState
    {
        private ThreshBehaviour _threshBehaviour;

        public MobThreshAINoneState(MobStatusController mobStatusController
        ) : base(mobStatusController)
        {
            _threshBehaviour = mobStatusController.championBehavior as ThreshBehaviour;
        }

        public override void UpdateState()
        {
            if (_threshBehaviour.championStatusController == null)
                return;
        
            if (_threshBehaviour.isDie == true)
                return;
            
            
            if (_threshBehaviour.isMoveEnable && (
                _threshBehaviour.championStatusController.championState == ChampionState.IDLE ||
                _threshBehaviour.championStatusController.championState == ChampionState.RUNNING ))
            {
                if (_threshBehaviour.attack1Range.GetComponent<AttackRangeScript>().target != null)
                {
                    if (_threshBehaviour.normalAttackCnt >= 0)
                    {
                        if (Random.Range(0, 10) > 5)
                        {
                            mobStatusController.StopAllCoroutine();
                            mobStatusController.updateNewState(mobStatusController.mobAIDistance);
                        }
                        else
                        {
                            if (_threshBehaviour.skill2CoolTime <= 0.0f)
                            {
                                mobStatusController.AddEvent(new Skill2Event());
                            }
                            else
                            {
                                mobStatusController.AddEvent(new NormalAttackEvent());
                            }
                        }
                    }
                    else
                    {
                        if (_threshBehaviour.skill2CoolTime <= 0.0f)
                        {
                            mobStatusController.AddEvent(new Skill2Event());
                        }
                        else
                        {
                            mobStatusController.AddEvent(new NormalAttackEvent());
                        }
                    }
                }
            }
            base.UpdateState();
        }
    }

    public class MobThreshAIChaseState : MobAIChaseState
    {
        private ThreshBehaviour _threshBehaviour;
        public MobThreshAIChaseState(MobStatusController mobStatusController
        ) : base(mobStatusController)
        {
            _threshBehaviour = mobStatusController.championBehavior as ThreshBehaviour;
        }

        public override void UpdateState()
        {
            if (_threshBehaviour.isDie == true)
                return;    
            
            //Debug.Log("chasing...");
            
            if (_threshBehaviour.attack1Range.GetComponent<AttackRangeScript>().target && mobStatusController.championBehavior.skill2CoolTime <= 0.0f)
            {
                mobStatusController.AddEvent(new Skill2Event());
                mobStatusController.updateNewState(mobStatusController.mobAINoneState);
            }else if (_threshBehaviour.skill1Range.GetComponent<AttackRangeScript>().target && mobStatusController.championBehavior.skill1CoolTime <= 0.0f)
            {
                mobStatusController.AddEvent(new Skill1Event());
                mobStatusController.updateNewState(mobStatusController.mobAINoneState);
            } else if (_threshBehaviour.attack1Range.GetComponent<AttackRangeScript>().target != null)
            {
                mobStatusController.AddEvent(new NormalAttackEvent());
                mobStatusController.updateNewState(mobStatusController.mobAINoneState);
            } else if (mobStatusController.championBehavior.skill3CoolTime <= 0.0f)
            {
                mobStatusController.AddEvent(new Skill3Event());
            } else
            {
                base.UpdateState();
            }
        }
    }

    public class MobThreshAIRunAwayState : MobAIRunAwayState
    {
        public MobThreshAIRunAwayState(MobStatusController mobStatusController
        ) : base(mobStatusController)
        {
        
        }
    }

    public class MobThreshAIDistanceState : MobAIDistanceState
    {
        private ThreshBehaviour _threshBehaviour;
        public MobThreshAIDistanceState(MobStatusController mobStatusController
        ) : base(mobStatusController)
        {
            _threshBehaviour = mobStatusController.championBehavior as ThreshBehaviour;
        }

        public override void UpdateState()
        {
            if (_threshBehaviour.isDie == true)
                return;   
            
            if (_threshBehaviour.attack1Range.GetComponent<AttackRangeScript>().target && mobStatusController.championBehavior.skill2CoolTime <= 0.0f)
            {
                mobStatusController.AddEvent(new Skill2Event());
                mobStatusController.updateNewState(mobStatusController.mobAINoneState);
            }else if (_threshBehaviour.skill1Range.GetComponent<AttackRangeScript>().target && mobStatusController.championBehavior.skill1CoolTime <= 0.0f)
            {
                mobStatusController.AddEvent(new Skill1Event());
                mobStatusController.updateNewState(mobStatusController.mobAINoneState);
            } else if (_threshBehaviour.attack1Range.GetComponent<AttackRangeScript>().target != null)
            {
                mobStatusController.AddEvent(new NormalAttackEvent());
                mobStatusController.updateNewState(mobStatusController.mobAINoneState);
            }else if (mobStatusController.championBehavior.skill3CoolTime <= 0.0f)
            {
                mobStatusController.AddEvent(new Skill3Event());
            } else
            {
                base.UpdateState();
            }
        }
    }
}