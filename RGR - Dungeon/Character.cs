using System;
using System.Collections.Generic;

namespace RGR___Dungeon
{
    [Serializable]
    abstract class Character : Entity
    {
        [Serializable]
        protected class Attack
        {

            #region fields
            Random rnd = new Random();
            private int damage;
            private string name;
            private AttackType type;
            private int baseDamage;
            private int baseChance;
            private int successChance;
            #endregion
            public Attack(int dmg, int chance, string Name, AttackType Type = AttackType.physical)
            {
                damage = dmg; SuccessChance = chance; name = Name;
                type = Type; baseDamage = dmg; baseChance = SuccessChance;
            }
            public Attack(int dmg, int chance, string Name)
            {
                damage = dmg; SuccessChance = chance; name = Name;
                type = AttackType.physical; baseDamage = dmg; baseChance = SuccessChance;
            }
            #region propeties
            public int BaseDamage => baseDamage;
            public string Name => name;
            public int Damage { set => damage = value; get => damage; }
            public int SuccessChance { get => successChance; set => successChance = value; }
            internal AttackType Type { set => type = value; get => type; }
            public int BaseChance => baseChance;
            #endregion

            public bool AttackEvent(Character attacked, Attack attack, Character attacker)
            {
                bool success;
                int dmg = Damage;
                int i = rnd.Next(1, 100);
                if (i <= SuccessChance)
                {
                    Console.WriteLine(string.Format("{0} успешно использовал приём {1}", attacker.name, attack.Name));
                    success = true;
                    if (attack.type != AttackType.special)
                    {
                        if (attacked.weakSpots.Contains(attack.Name))
                        {
                            dmg = (int)(dmg * 1.5);
                            Console.WriteLine("Вы попали в уязвимое место!");
                        }
                        if (attack.Type == attacked.weaknessType)
                        {
                            dmg = (int)(dmg * 1.5);
                            Console.WriteLine($"{attacked.name} уязвим к данному типу урона.");
                        }
                        else if (attack.Type == attacked.resistance)
                        {
                            dmg = (int)(dmg / 1.5);
                            Console.WriteLine($"{attacked.name} устойчив к данному типу урона.");
                        }
                        if (rnd.Next(1, 101) < 10)
                        {
                            dmg *= 2;
                            Console.WriteLine("Крит!");

                        }
                        Console.WriteLine($"Урон: {dmg}");
                        attacked.TakeDamage(dmg, attack.type);
                    }
                }
                else
                {
                    Console.WriteLine(string.Format("У {1} не вышло использовать {0}", attack.Name, attacker.name));
                    success = false;
                }
                return success;
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
        public AttackType WeaknessType => weaknessType;
        public AttackType Resistance => resistance;
        #endregion

        #region Methods
        public void Heal(int value) => Health += value;
        public abstract void TakeDamage(int dmg, AttackType attackType);
        public abstract void InflictAttack(Character attacked);
        #endregion
    }
}

