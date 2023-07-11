using UnityEngine;

namespace Script.Champions.Orn
{
    public class MobOrnAINoneState : MobAINoneState
    {
        private OrnBehavior _ornBehavior;

        public MobOrnAINoneState(MobStatusController mobStatusController
        ) : base(mobStatusController)
        {
            _ornBehavior = mobStatusController.championBehavior as OrnBehavior;
        }

        public override void UpdateState()
        {

            if (_ornBehavior.isMoveEnable)
            {
                if (_ornBehavior.attack1Range.GetComponent<AttackRangeScript>().target != null)
                {
                    mobStatusController.AddEvent(new OrnNormalAttackEvent());
                }
                else if (_ornBehavior.skill2Range.GetComponent<AttackRangeScript>().target != null)
                {
                    if (mobStatusController.championBehavior.skill1CoolTime <= 0.0f)
                    {
                        mobStatusController.AddEvent(new OrnSkill1Event());
                    }
                    else if (mobStatusController.championBehavior.skill2CoolTime <= 0.0f)
                    {
                        mobStatusController.AddEvent(new OrnSkill2Event());
                    }
                }
            }
            base.UpdateState();
        }
        
    }

    public class MobOrnAIChaseState : MobAIChaseState
    {
        private OrnBehavior _ornBehaviour;
        
        public MobOrnAIChaseState(MobStatusController mobStatusController
        ) : base(mobStatusController)
        {
            _ornBehaviour = mobStatusController.championBehavior as OrnBehavior;
        }

        public override void UpdateState()
        {
          
            if (_ornBehaviour.attack1Range.GetComponent<AttackRangeScript>().target != null)
            {
                if (_ornBehaviour.attack1Range.GetComponent<AttackRangeScript>().target != null)
                {
                    mobStatusController.AddEvent(new OrnNormalAttackEvent());
                    mobStatusController.updateNewState(mobStatusController.mobAINoneState);
                }
                else if (_ornBehaviour.skill2Range.GetComponent<AttackRangeScript>().target != null)
                {
                    if (mobStatusController.championBehavior.skill1CoolTime <= 0.0f)
                    {
                        mobStatusController.AddEvent(new OrnSkill1Event());
                        mobStatusController.updateNewState(mobStatusController.mobAINoneState);
                    }
                    else if (mobStatusController.championBehavior.skill2CoolTime <= 0.0f)
                    {
                        mobStatusController.AddEvent(new OrnSkill2Event());
                        mobStatusController.updateNewState(mobStatusController.mobAINoneState);
                    }
                }
            }
            else
            {
                base.UpdateState();
            }
        }
    }

    public class MobOrnAIRunAwayState : MobAIRunAwayState
    {
        public MobOrnAIRunAwayState(MobStatusController mobStatusController
        ) : base(mobStatusController)
        {
        }
    }

    public class MobOrnAIDistanceState : MobAIDistanceState
    {
        private OrnBehavior _ornBehaviour;
        private float normalAttackValidCoolTime;
        private bool normalAttackValid;
        
        public MobOrnAIDistanceState(MobStatusController mobStatusController
        ) : base(mobStatusController)
        {
            _ornBehaviour = mobStatusController.championBehavior as OrnBehavior;
        }
        
        public override void EnterState()
        {
            normalAttackValidCoolTime = 1.0f;
            normalAttackValid = false;
            _ornBehaviour.normalAttackCnt = 0;
            base.EnterState();
        }
        
        public override void UpdateState()
        {
            if (normalAttackValidCoolTime > 0.0f)
            {
                normalAttackValidCoolTime -= Time.deltaTime;
            }
            else
            {
                normalAttackValidCoolTime = Random.Range(0.5f, 1.0f);
                if (Random.Range(0, 10) > 5)
                {
                    normalAttackValid = true;
                }
                else
                {
                    normalAttackValid = false;
                }
            }
            
            if (_ornBehaviour.attack1Range.GetComponent<AttackRangeScript>().target != null)
            {
                if (_ornBehaviour.attack1Range.GetComponent<AttackRangeScript>().target != null)
                {
                    mobStatusController.AddEvent(new OrnNormalAttackEvent());
                    mobStatusController.updateNewState(mobStatusController.mobAINoneState);
                }
                else if (_ornBehaviour.skill2Range.GetComponent<AttackRangeScript>().target != null)
                {
                    if (mobStatusController.championBehavior.skill1CoolTime <= 0.0f)
                    {
                        mobStatusController.AddEvent(new OrnSkill1Event());
                        mobStatusController.updateNewState(mobStatusController.mobAINoneState);
                    }
                    else if (mobStatusController.championBehavior.skill2CoolTime <= 0.0f)
                    {
                        mobStatusController.AddEvent(new OrnSkill2Event());
                        mobStatusController.updateNewState(mobStatusController.mobAINoneState);
                    }
                }
            }
            else
            {
                base.UpdateState();
            }
        }
    }
}