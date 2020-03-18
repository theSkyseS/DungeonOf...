using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections.Generic;

namespace RGR___Dungeon
{
    sealed class Player : Character
    {
        #region fields
        public int score;
        public int maxhealth;
        public int healthPotions;
        public int expirience;
        public int strength;
        private int expToNextLevel;
        private int level;
        #endregion

        #region Properties

        #endregion

        #region methods
        public Player()
        {
            name = "Игрок";
            attacks.Add(new Attack(15, 85, false, "удар по торсу"));
            attacks.Add(new Attack(20, 55, false, "удар по голове"));
            attacks.Add(new Attack(6, 95, false, "удар по рукам"));
            attacks.Add(new Attack(5, 100, false, "удар по ногам"));
            attacks.Add(new Attack(0, 100, true, "лечение"));
            level = 1;
            health = 100;
            maxhealth = 100;
            healthPotions = 2;
            score = 0;
            expirience = 0;
            strength = 1;
            expToNextLevel = 10;
        }
        public override void InflictAttack(Character attacked)
        {
            Console.WriteLine("Введите номер атаки: 1-торс, 2-голова, 3-руки, 4-ноги, 5-использовать зелье");
            try
            {
                int i = int.Parse(Console.ReadLine());
                Attack UsedAttack = attacks[i - 1];
                if (UsedAttack.special) UsePotion();
                UsedAttack.AttackEvent(attacked, UsedAttack, this);
            }
            catch (Exception)
            {
                Console.WriteLine("Введите корректное значение");
                InflictAttack(attacked);
            }
            
        }
        public void levelUp()
        {
            try
            {
                Console.WriteLine("Выберите характеристику для улучшения: 1 - Здоровье, 2 - Сила");
                int i = int.Parse(Console.ReadLine());
                expToNextLevel += 5 + (int)Math.Pow(2, level);
                expirience = 0;
                switch(i)
                {
                    case 1: maxhealth += 10;
                        health += 10;
                        break;
                    case 2: strength += 1; break;
                    default: maxhealth += 10;
                        health += 10;
                        break;
                }
                foreach(Attack attack in attacks)
                {
                    attack.damage += strength * 2;
                }
            }
            catch(Exception)
            {
                levelUp();
            }
        }
        protected override void TakeDamage(int dmg, Attack attack) => health -= dmg;
        public void TakePotions(int value) => this.healthPotions += value;
        private void UsePotion()
        {
            this.healthPotions -= 1;
            if (maxhealth - health >= 20)
                Heal(20);
            else
                Heal(maxhealth - health);
        }
        #endregion

    }
}
