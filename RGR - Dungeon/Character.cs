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
            public int successChance;
            public int damage;
            public bool special;
            public string name;

            public Attack(int dmg, int chance, bool spec, string Name)
            {
                damage = dmg; successChance = chance; special = spec; this.name = Name;
            }
            public void AttackEvent(Character attacked, Attack attack, Character attacker)
            {
                Random rnd = new Random();
                int i = rnd.Next(1, 100);
                if (i <= successChance)
                {
                    Console.WriteLine(string.Format("{0} успешно использовал приём {1}, урон: {2} ", attacker.name, attack.name, attack.damage));
                    attacked.TakeDamage(this.damage, attack);
                }
                else Console.WriteLine(string.Format("У {1} не вышло использовать {0}", attack.name, attacker.name));
            }
        }

        #region fields
        protected List<Attack> attacks = new List<Attack>();
        protected int maxhealth;
        private int health;
        public string name;
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

        #region Properties
        #endregion

        #region Methods
        public void Heal(int value) => Health += value;
        protected abstract void TakeDamage(int dmg, Attack attack);
        public abstract void InflictAttack(Character attacked);
        #endregion
    }
}

