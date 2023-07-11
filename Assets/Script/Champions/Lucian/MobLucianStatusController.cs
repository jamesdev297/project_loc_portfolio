using Script.Champions.Lucian;
using UnityEngine;

public class MobLucianStatusController : MobStatusController
{
    [SerializeField] private float _speed = 2.0f;
    
    private void Awake()
    {
        moveController = GetComponent<MobMoveController>();
    }
    
    protected override void stateInit()
    {
        mobAINoneState = new MobLucianAINoneState(this);
        mobAIChaseState = new MobLucianAIChaseState(this);
        mobAIRunAwayState = new MobLucianAIRunAwayState(this);
        mobAIDistance = new MobLucianAIDistanceState(this);
    }
    
    public override void AddEvent(ChampionStatusEvent championStatusEvent)
    {
        if (championStatusEvent is LucianNormalAttackEvent)
        {
            Attack1();
        }
        else if (championStatusEvent is LucianSkill1Event)
        {
            Skill1();
        }
        else if (championStatusEvent is LucianSkill2Event)
        {
            Skill2();
        }else if (championStatusEvent is LucianSkill3Event)
        {
            Skill3();
        }
        
        base.AddEvent(championStatusEvent);
    }
    
}