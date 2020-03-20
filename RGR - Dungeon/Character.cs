using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGR___Dungeon
{
    abstract class Character : Entity
    {
        protected class Attack
        {
            #region fields
            private int successChance;
            private int damage;
            private bool special;
            private string name;
            private AttackType type;
            private int baseDamage;
            private int useChanceMin;
            private int useChanceMax;
            #endregion
            public Attack(int dmg, int chance, bool spec, string Name, AttackType Type = AttackType.physical)
            {
                damage = dmg; successChance = chance; special = spec; name = Name; type = Type; baseDamage = dmg;
            }
            public Attack(int dmg, int chance, bool spec, string Name)
            {
                damage = dmg; successChance = chance; special = spec; name = Name; type = AttackType.physical; baseDamage = dmg;
            }
            public Attack(int dmg, int chance, bool spec, string Name, int usechancemin, int usechancemax, AttackType Type = AttackType.physical)
            {
                damage = dmg; successChance = chance; special = spec; name = Name; type = Type; baseDamage = dmg; useChanceMin = usechancemin; useChanceMax = usechancemax;
            }
            #region propeties
            public int BaseDamage => baseDamage;
            public string Name => name;
            public bool Special => special;
            public int Damage { set => damage = value; get => damage; }
            public int SuccessChance => successChance;
            internal AttackType Type { set => type = value; get => type; }
            public int UseChanceMin => useChanceMin;
            public int UseChanceMax => useChanceMax;
            #endregion

            public void AttackEvent(Character attacked, Attack attack, Character attacker)
            {
                Random rnd = new Random();
                int dmg = Damage;
                int i = rnd.Next(1, 100);
                if (i <= SuccessChance)
                {
                    Console.WriteLine(string.Format("{0} успешно использовал приём {1}", attacker.name, attack.Name));
                    if (attacked.weakSpots.Contains(attack.Name))
                    {
                        dmg *= (int)1.5;
                        Console.WriteLine("Вы попали в уязвимое место!");
                    }
                    if (attack.Type == attacked.weaknessType)
                    {
                        dmg *= (int)1.5;
                        Console.WriteLine("Враг уязвим к данному типу урона.");
                    }
                    else if (attack.Type == attacked.resistance)
                    {
                        dmg /= (int) 1.5;
                        Console.WriteLine("Враг устойчив к данному типу урона.");
                    }
                    if(rnd.Next(1, 101) < 10)
                    {
                        dmg *= 2;
                        Console.WriteLine("Крит!");

                    }
                    Console.WriteLine($"Урон: {dmg}");
                    attacked.TakeDamage(dmg, attack);
                }
                else Console.WriteLine(string.Format("У {1} не вышло использовать {0}", attack.Name, attacker.name));
            }
        }

        #region fields
        public string name;
        protected List<Attack> attacks = new List<Attack>();
        protected int maxhealth;
        protected AttackType weaknessType;
        protected AttackType resistance;
        private int health;
        protected List<string> weakSpots = new List<string>();
        #endregion

        #region Properties
        public int Health
        {
            get => health;
            set
            {
                if (value <= maxhealth)
                {
                    health = value;
                }
                else
                {
                    health = maxhealth;
                }
            }
        }
        #endregion

        #region Methods
        public void Heal(int value) => Health += value;
        protected abstract void TakeDamage(int dmg, Attack attack);
        public abstract void InflictAttack(Character attacked);
        #endregion
    }
}

