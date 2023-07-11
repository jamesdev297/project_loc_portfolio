
using System;

[Serializable]
public abstract class AbilityModel
{
   protected string name;
   protected float ability;

   public string GETName()
   {
      return name;
   }

   public float GETAbility()
   {
      return ability;
   }

   public string GETDescription()
   {
      return name + " : " + ability;
   }

   public override string ToString()
   {
      return $"name: {name}\nability: {ability}";
   }
}

[Serializable]
public class AttackDamage : AbilityModel
{
   public AttackDamage(float a)
   {
      name = Constants.AttackDamage;
      ability = a;
   }
}

[Serializable]
public class Armor : AbilityModel
{
   public Armor(float a)
   {
      name = Constants.Armor;
      ability = a;
   }
}

[Serializable]
public class Health : AbilityModel
{
   public Health(float a)
   {
      name = Constants.Health;
      ability = a;
   }
}

[Serializable]
public class MovementSpeed : AbilityModel
{
   public MovementSpeed(float a)
   {
      name = Constants.MovementSpeed;
      ability = a;
   }
}


[Serializable]
public class AttackSpeed : AbilityModel
{
   public AttackSpeed(float a)
   {
      name = Constants.AttackSpeed;
      ability = a;
   }
}

[Serializable]
public class LifeSteal : AbilityModel
{
   public LifeSteal(float a)
   {
      name = Constants.LifeSteal;
      ability = a;
   }
}


[Serializable]
public class CoolDown : AbilityModel
{
   public CoolDown(float a)
   {
      name = Constants.CoolDown;
      ability = a;
   }
}


[Serializable]
public class CoolDownReduction : AbilityModel
{
   public CoolDownReduction(float a)
   {
      name = Constants.CoolDownReduction;
      ability = a;
   }
}


[Serializable]
public class ReactiveDamage : AbilityModel
{
   public ReactiveDamage(float a)
   {
      name = Constants.ReactiveDamage;
      ability = a;
   }
}
