using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Json
{
    public class Champ
    {
        public int damage;
        public int hp;
        public int movementSpeed;
        public List<Skill> skills;
        public int armor;
        public int price;
        public int attackSpeed;
        public string name;
        public int id;
        public string desc;
    }

    public class Champs
    {
        public Champ[] champs;
    }

    public class Card
    {
        public float stat1;
        public float stat2;
        public float stat3;
        public float stat4;
        public string name;
        public int id;
        public string desc;
    }

    public class Cards
    {
        public List<Card> cards;
    }

    public class Campaign
    {
        public int stamina;
        public int stage;
        public string name;
        public int step;
        public int id;
        public string desc;
    }

    public class Campaigns
    {
        public List<Campaign> campaigns;
    }

    public class Skill
    {
        public List<int> factors;
        public int id;
    }
}