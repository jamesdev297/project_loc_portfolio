public abstract class SkillModel
{
    private string skillName;
    private string skillImagePath;

    protected DamageModel _damageModel;

    public DamageModel GetDamageModel()
    {
        return _damageModel;
    }

    public SkillModel(string skillName, string skillImagePath)
    {
        this.skillName = skillName;
        this.skillImagePath = skillImagePath;
    }
    
}