using Script.Champions.Orn;
using UnityEngine;

public class MobOrnStatusController : MobStatusController
{
    [SerializeField] private float _speed = 2.0f;

    private ChampionModel _championModel;

    private void Awake()
    {
        _championModel = new OrnModel();
        moveController = GetComponent<MobMoveController>();
    }
    
    protected override void stateInit()
    {
        mobAINoneState = new MobOrnAINoneState(this);
        mobAIChaseState = new MobOrnAIChaseState(this);
        mobAIRunAwayState = new MobOrnAIRunAwayState(this);
        mobAIDistance = new MobOrnAIDistanceState(this);
    }
    
    public override void AddEvent(ChampionStatusEvent championStatusEvent)
    {
        if (championStatusEvent is OrnNormalAttackEvent)
        {
            championBehavior.normalAttackCnt++;
            animator.SetTrigger("attack1");
        }
        else if (championStatusEvent is OrnSkill1Event)
        {
            championBehavior.skill1CoolTime = championModel.skills[0].cooldown;
            animator.SetTrigger("skill1");
        }
        else if (championStatusEvent is OrnSkill2Event)
        {
            championBehavior.skill2CoolTime = championModel.skills[1].cooldown;
            animator.SetTrigger("skill2");
        }
        // else if (championStatusEvent is OrnSkill3Event)
        // {
        //     championBehavior.skill3CoolTime = championModel.skill3CoolTime;
        //     animator.SetTrigger("skill3");
        // }
        else
        {
            championBehavior.normalAttackCnt = 0;
        }
        base.AddEvent(championStatusEvent);
    }
    
}