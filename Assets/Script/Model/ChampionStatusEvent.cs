public class ChampionStatusEvent
{
        
}

public class DamagedEvent : ChampionStatusEvent
{
    public DamageModel damageModel;

    public DamagedEvent(DamageModel damageModel)
    {
        this.damageModel = damageModel;
    }
}

public class StunEvent : ChampionStatusEvent
{
    public StunEvent()
    {
    }
}

public class AttackEvent : ChampionStatusEvent
{
    public AttackEvent()
    {
    }
}

public class SkillEvent : ChampionStatusEvent
{
    public SkillEvent()
    {
    }
}

public class Skill1Event : SkillEvent
{
    public Skill1Event()
    {
    }
}

public class Skill1OnEvent : SkillEvent
{
    public Skill1OnEvent()
    {
    }
}

public class Skill1OffEvent : SkillEvent
{
    public Skill1OffEvent()
    {
    }
}

public class Skill2Event : SkillEvent
{
    public Skill2Event()
    {
    }
}

public class Skill3Event : SkillEvent
{
    public Skill3Event()
    {
    }
}

public class NormalAttackEvent : AttackEvent
{
    public NormalAttackEvent()
    {
        
    }
}

public class Attack3Event : AttackEvent
{
    public Attack3Event()
    {
        
    }
}


public class RunningEvent : ChampionStatusEvent
{
    public RunningEvent()
    {
        
    }
}

public class IdleEvent : ChampionStatusEvent
{
    public IdleEvent()
    {
        
    }
}