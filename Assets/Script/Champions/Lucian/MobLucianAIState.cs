using UnityEngine;

namespace Script.Champions.Lucian
{
    public class MobLucianAINoneState : MobAINoneState
    {
        private LucianBehavior _lucianBehavior;

        public MobLucianAINoneState(MobStatusController mobStatusController
        ) : base(mobStatusController)
        {
            _lucianBehavior = mobStatusController.championBehavior as LucianBehavior;
        }

        public override void UpdateState()
        {
            if (_lucianBehavior.championStatusController == null)
                return;
            
            if (_lucianBehavior.isDie == true)
                return;
            
            if (_lucianBehavior.proceedingSkill3 == true)
            {
                mobStatusController.updateNewState(mobStatusController.mobAIDistance);
                return;
            }

            if (_lucianBehavior.isMoveEnable && (
                _lucianBehavior.championStatusController.championState == ChampionState.IDLE ||
                _lucianBehavior.championStatusController.championState == ChampionState.RUNNING ))
            {
                if (_lucianBehavior.distance1Range.GetComponent<AttackRangeScript>().target != null)
                {
                    if (_lucianBehavior.skill2CoolTime <= 0.0f)
                    {
                        mobStatusController.AddEvent(new LucianSkill2Event());
                    }
                }else if (_lucianBehavior.attack1Range.GetComponent<AttackRangeScript>().target != null)
                {
                    if (_lucianBehavior.skill3CoolTime <= 0.0f)
                    {
                        mobStatusController.AddEvent(new LucianSkill3Event());
                        mobStatusController.updateNewState(mobStatusController.mobAIDistance);
                    }else
                    {
                        mobStatusController.AddEvent(new LucianNormalAttackEvent());    
                    }
                }
            }
            base.UpdateState();
        }
        
    }

    public class MobLucianAIChaseState : MobAIChaseState
    {
        private LucianBehavior _lucianBehaviour;
        
        public MobLucianAIChaseState(MobStatusController mobStatusController
        ) : base(mobStatusController)
        {
            _lucianBehaviour = mobStatusController.championBehavior as LucianBehavior;
        }
        
        public override void EnterState()
        {
            mobStatusController.StopAllCoroutine();

            bool isLeft = (Random.Range(0, 10) > 5) ? true : false;
            float offsetX = isLeft ? 5.0f : -5.0f;
            Vector3 targetPos = new Vector3(mobStatusController.target.position.x + offsetX, mobStatusController.target.position.y
                , mobStatusController.target.position.z);
            
            mobStatusController.StartRepeatUpdatePath(targetPos);
            mobStatusController.AddEvent(new RunningEvent());
        }

        public override void UpdateState()
        {
            
            if (_lucianBehaviour.isDie == true)
                return;
            
            if (_lucianBehaviour.proceedingSkill3 == true)
            {
                base.UpdateState();
                return;
            }
            
            if (_lucianBehaviour.isMoveEnable == false)
            {
                mobStatusController.updateNewState(mobStatusController.mobAINoneState);
                return;
            }

            if (_lucianBehaviour.distance1Range.GetComponent<AttackRangeScript>().target != null)
            {
                if (_lucianBehaviour.skill2CoolTime <= 0.0f)
                {
                    mobStatusController.AddEvent(new LucianSkill2Event());
                }
            } else if (_lucianBehaviour.attack1Range.GetComponent<AttackRangeScript>().target != null)
            {
                if (_lucianBehaviour.skill3CoolTime <= 0.0f)
                {
                    mobStatusController.AddEvent(new LucianSkill3Event());
                    mobStatusController.updateNewState(mobStatusController.mobAIDistance);
                }else
                {
                    mobStatusController.AddEvent(new LucianNormalAttackEvent());    
                }
            }
            else
            {
                if (_lucianBehaviour.skill1CoolTime <= 0.0f)
                {
                    mobStatusController.AddEvent(new LucianSkill1Event());
                    mobStatusController.updateNewState(mobStatusController.mobAIDistance);
                }
            }
            base.UpdateState();

        }
    }

    public class MobLucianAIRunAwayState : MobAIRunAwayState
    {
        public MobLucianAIRunAwayState(MobStatusController mobStatusController
        ) : base(mobStatusController)
        {
        }
    }

    public class MobLucianAIDistanceState : MobAIDistanceState
    {
        private LucianBehavior _lucianBehaviour;
        
        public MobLucianAIDistanceState(MobStatusController mobStatusController
        ) : base(mobStatusController)
        {
            _lucianBehaviour = mobStatusController.championBehavior as LucianBehavior;
        } 
        
        public override void UpdateState()
        {
            Debug.Log("UPDate Lucain");
            
            if (_lucianBehaviour.isDie == true)
                return;
            
            if (_lucianBehaviour.proceedingSkill3 == true)
            {
                mobStatusController.updateNewState(mobStatusController.mobAIChaseState);
                return;
            }
            
            if (_lucianBehaviour.isMoveEnable == false)
            {
                mobStatusController.updateNewState(mobStatusController.mobAINoneState);
                return;
            }

            
            if (_lucianBehaviour.distance1Range.GetComponent<AttackRangeScript>().target != null)
            {
                if (_lucianBehaviour.skill2CoolTime <= 0.0f)
                {
                    mobStatusController.AddEvent(new LucianSkill2Event());
                }
            } else if (_lucianBehaviour.attack1Range.GetComponent<AttackRangeScript>().target != null)
            {
                if (_lucianBehaviour.skill3CoolTime <= 0.0f)
                {
                    mobStatusController.AddEvent(new LucianSkill3Event());
                    // mobStatusController.updateNewState(mobStatusController.mobAIDistance);
                }else
                {
                    mobStatusController.AddEvent(new LucianNormalAttackEvent());    
                }
            }
            else
            {
                if (_lucianBehaviour.skill1CoolTime <= 0.0f)
                {
                    mobStatusController.AddEvent(new LucianSkill1Event());
                    // mobStatusController.updateNewState(mobStatusController.mobAIDistance);
                }
            }
            base.UpdateState();
        }
    }
}