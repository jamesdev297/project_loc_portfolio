using System;
using System.Collections.Generic;
using LitJson;

[Serializable]
public class SkillModel1
{
        public int id;
        public List<float> factors;
        public int cooldown;
        public static SkillModel1 FromJson(JsonData map)
        {
                SkillModel1 skillModel = new SkillModel1();
                skillModel.id = (int) map["id"];
                skillModel.cooldown = (int) map["cooldown"];
                skillModel.factors = new List<float>();
                for (int i = 0; i < map["factors"].Count; i++)
                {
                        if (map["factors"][i].IsInt)
                        {
                                skillModel.factors.Add(((int) map["factors"][i]) * 1.0f);
                        }
                        else
                        {
                                skillModel.factors.Add(((float)((double) map["factors"][i])));
                        }
                }
                return skillModel;
        }
}