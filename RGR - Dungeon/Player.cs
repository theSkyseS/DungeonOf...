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
        private int armor;
        private int strength;
        private int expToNextLevel;
        private int level;
        private Weapon currentWeapon;
        #endregion

        #region Properties
        public int Level => level;

        public int Armor
        {
            get => armor; 
            set
            {
                if (value > 0)
                    armor = value;
                else
                {
                    Health += (armor + value);
                    armor = 0;
                }
            }
        }
        #endregion

        public Player()
        {
            name = "Игрок";
            attacks.Add(new Attack(25, 85, false, "удар по торсу", AttackType.physical));
            attacks.Add(new Attack(30, 55, false, "удар по голове", AttackType.physical));
            attacks.Add(new Attack(16, 95, false, "удар по рукам", AttackType.physical));
            attacks.Add(new Attack(15, 100, false, "удар по ногам", AttackType.physical));
            attacks.Add(new Attack(0, 100, true, "лечение"));
            level = 1;
            maxhealth = 100;
            Health = 100;
            healthPotions = 2;
            score = 0;
            expirience = 0;
            strength = 0;
            Armor = 0;
            expToNextLevel = 10;
            weaknessType = AttackType.nothing;
        }

        #region methods
        protected override void TakeDamage(int dmg, Attack attack)
        {
            if(Armor > 0)
            {
                Armor -= dmg; 
            }
            else
                Health -= dmg;
        }

        private void UsePotion()
        {
            if (healthPotions > 0)
            {
                this.healthPotions -= 1;
                Heal(10 * level);
            }
            else Console.WriteLine("У Вас нет зелий");
        }
        private void RegenerateAttacks()
        {
            foreach (Attack attack in attacks)
            {
                if (!attack.Special)
                {
                    attack.Damage = attack.BaseDamage + currentWeapon.Damage + strength * 2;
                    attack.Type = currentWeapon.AttackType;
                }
            }
        }
        public override void InflictAttack(Character attacked)
        {
            Console.WriteLine("Введите номер атаки: 1-торс, 2-голова, 3-руки, 4-ноги, 5-использовать зелье");
            try
            {
                int i = int.Parse(Console.ReadLine());
                Attack UsedAttack = attacks[i - 1];
                if (UsedAttack.Special) UsePotion();
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
                        case 1:
                            maxhealth += 10;
                            Health += 10;
                            break;
                        case 2: strength += 1;
                            RegenerateAttacks();
                            break;
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
        public void TakeNewWeapon(Weapon weapon)
        {
            try
            {
                Console.WriteLine(string.Format("Вы нашли {0}. Урон: {1}, Прочность: {2}, Тип атаки: {3}",
                                                weapon.WeaponName,
                                                weapon.Damage,
                                                weapon.Durability,
                                                weapon.AttackType));
                if (currentWeapon == null)
                {
                    Console.WriteLine();
                    Console.WriteLine("У Вас нет оружия");
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine(string.Format("Ваше текущее оружие: {0}. Урон: {1}, Прочность: {2}, Тип атаки {3}",
                                                currentWeapon.WeaponName,
                                                currentWeapon.Damage,
                                                currentWeapon.Durability,
                                                currentWeapon.AttackType));
                }
                Console.WriteLine();
                Console.WriteLine("Введите: 1 - взять новое оружие, 2 - оставить текущее оружие");
                int choice = int.Parse(Console.ReadLine());
                if(choice == 1)
                {
                    Console.Clear();
                    currentWeapon = weapon;
                    Console.WriteLine(String.Format("Вы взяли {0}", weapon.WeaponName));
                    RegenerateAttacks();
                }
                else if (choice == 2) Console.WriteLine(String.Format("Вы оставили {0}", currentWeapon.WeaponName));
            }
            catch (Exception)
            {
                TakeNewWeapon(weapon);
            }
        }
        public void TakePotions(int value) => this.healthPotions += value;
        public void DamageWeapon()
        {
            if (currentWeapon != null)
            {
                currentWeapon.Durability -= 1;
                if (currentWeapon.Durability <= 0)
                {
                    Console.WriteLine("Ваше оружие сломалось, Вам стоит поискать себе другое");
                    currentWeapon = null;
                    RegenerateAttacks();
                }
            }
        }
        #endregion
    }
}
