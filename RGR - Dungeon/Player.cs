using System;
using System.Collections.Generic;

namespace RGR___Dungeon
{
    sealed class Player : Character
    {
        #region fields
        public int score;
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
            attacks.Add(new Attack(25, 85, false, "удар по торсу"));
            attacks.Add(new Attack(30, 55, false, "удар по голове"));
            attacks.Add(new Attack(16, 95, false, "удар по рукам"));
            attacks.Add(new Attack(15, 100, false, "удар по ногам"));
            attacks.Add(new Attack(0, 100, true, "лечение"));
            level = 1;
            maxhealth = 100;
            Health = 100;
            healthPotions = 2;
            score = 0;
            expirience = 0;
            strength = 0;
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
        public void CheckLevelUp()
        {
            while(expirience >= expToNextLevel)
                try
                {
                    Console.WriteLine("Новый уровень! \n"
                                      + "Выберите характеристику для улучшения: 1 - Здоровье, 2 - Сила");
                    int i = int.Parse(Console.ReadLine());
                    expirience -= expToNextLevel;
                    level += 1;
                    expToNextLevel += 5 + (int)Math.Pow(2, level);
                    switch(i)
                    {
                        case 1: maxhealth += 10;
                            Health += 10;
                            break;
                        case 2: strength += 1; 
                            foreach(Attack attack in attacks)
                    {
                        if(!attack.special)
                        attack.damage += strength * 2;
                    }break;
                        default: maxhealth += 10;
                            Health += 10;
                            break;
                    }
                    
                }
                catch(Exception)
                {
                    CheckLevelUp();
                }
        }
        public int GetLevel()
        {
            return level;
        }
        protected override void TakeDamage(int dmg, Attack attack) => Health -= dmg;
        public void TakePotions(int value) => this.healthPotions += value;
        private void UsePotion()
        {
            if (healthPotions > 0)
            {
                this.healthPotions -= 1;
                Heal(40);
            }
            else Console.WriteLine("У Вас нет зелий");
        }
        #endregion
    }
}
