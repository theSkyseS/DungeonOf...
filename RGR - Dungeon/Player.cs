using System;

namespace RGR___Dungeon
{
    [Serializable]
    sealed class Player : Character
    {
        #region fields
        public static int difficulty = 1;
        public int score;
        public int healthPotions;
        public int expirience;
        private int strength;
        private int expToNextLevel;
        private int level;
        private Weapon currentWeapon;
        public Armor currentArmor;
        #endregion

        #region Properties
        public int Level => level;
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
            expToNextLevel = 10;
            currentWeapon = new NoWeapon();
            currentArmor = new NoArmor();
            resistance = AttackType.nothing;
            weaknessType = AttackType.nothing;
        }

        #region methods
        #region attack
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
        private void RegenerateAttacks()
        {
            foreach (Attack attack in attacks)
            {
                if (!attack.Special)
                {
                    attack.Damage = attack.BaseDamage + currentWeapon.Damage + strength * 6;
                    attack.Type = currentWeapon.AttackType;
                }
            }
        }
        #endregion
        #region weapon
        public void TakeNewWeapon(Weapon weapon)
        {
            try
            {
                Console.Clear();
                Console.WriteLine(string.Format("Вы нашли {0}. Урон: {1}, Прочность: {2}, Тип атаки: {3}",
                                                weapon.WeaponName,
                                                weapon.Damage,
                                                weapon.Durability,
                                                weapon.AttackType));
                if (currentWeapon.GetType() == typeof(NoWeapon))
                {
                    Console.WriteLine("\nУ Вас нет оружия");
                }
                else
                {
                    Console.WriteLine(string.Format("\nВаше текущее оружие: {0}. Урон: {1}, Прочность: {2}, Тип атаки {3}",
                                                currentWeapon.WeaponName,
                                                currentWeapon.Damage,
                                                currentWeapon.Durability,
                                                currentWeapon.AttackType));
                }
                Console.WriteLine("\nВведите: 1 - взять новое оружие, 2 - оставить текущее оружие");
                int choice = int.Parse(Console.ReadLine());
                if (choice == 1)
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
        public void DamageWeapon()
        {
            if (currentWeapon.GetType() != typeof(NoWeapon))
            {
                currentWeapon.Durability -= 1;
                if (currentWeapon.Durability <= 0)
                {
                    Console.WriteLine("Ваше оружие сломалось, Вам стоит поискать себе другое");
                    currentWeapon = new NoWeapon();
                    RegenerateAttacks();
                }
            }
        }
        #endregion
        #region armor
        public void TakeNewArmor(Armor armor)
        {
            try
            {
                Console.Clear();
                Console.WriteLine(string.Format("Вы нашли {0}. Защита: {1}, Устойчивость: {2}, Уязвимость: {3}",
                                                armor.ArmorName,
                                                armor.durability,
                                                armor.Resistance,
                                                armor.Weakness));
                if (currentArmor.GetType() == typeof(NoArmor))
                {
                    Console.WriteLine("\nУ Вас нет брони");
                }
                else
                {
                    Console.WriteLine(string.Format("\nВаша броня:{0}. Защита: {1}, Устойчивость: {2}, Уязвимость: {3}",
                                                currentArmor.ArmorName,
                                                currentArmor.durability,
                                                currentArmor.Resistance,
                                                currentArmor.Weakness));
                }
                Console.WriteLine("\nВведите: 1 - взять новую броню, 2 - оставить текущую броню");
                int choice = int.Parse(Console.ReadLine());
                if (choice == 1)
                {
                    Console.Clear();
                    currentArmor = armor;
                    resistance = armor.Resistance;
                    weaknessType = armor.Weakness;
                    Console.WriteLine($"Вы взяли {armor.ArmorName}");
                }
                else if (choice == 2) Console.WriteLine($"Вы оставили {currentArmor.ArmorName}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "\n" + e.StackTrace + "\n" + e.Source + "\n" + e.InnerException);
                TakeNewArmor(armor);
            }
        }
        public void DamageArmor(int damage)
        {
            if (currentArmor.GetType() != typeof(NoArmor))
            {
                currentArmor.durability -= damage;
                if (currentArmor.durability <= 0)
                {
                    Console.WriteLine("Ваша броня сломалась, Вам стоит поискать себе другую");
                    currentArmor = new NoArmor();
                    resistance = AttackType.nothing;
                    weaknessType = AttackType.nothing;
                }
            }
        }
        #endregion
        public void CheckLevelUp()
        {
            while (expirience >= expToNextLevel)
                try
                {
                    Console.WriteLine("Новый уровень! \n"
                                      + "Выберите характеристику для улучшения: 1 - Здоровье, 2 - Сила");
                    int i = int.Parse(Console.ReadLine());
                    expirience -= expToNextLevel;
                    level += 1;
                    expToNextLevel += 15 * level;
                    switch (i)
                    {
                        case 1:
                            maxhealth += 25;
                            Health += 25;
                            break;
                        case 2:
                            strength += 1;
                            RegenerateAttacks();
                            break;
                        default:
                            maxhealth += 25;
                            Health += 25;
                            break;
                    }

                }
                catch (Exception)
                {
                    //Console.WriteLine(e.Message + "\n" + e.StackTrace + "\n" + e.Source + "\n" + e.InnerException);
                    CheckLevelUp();
                }
        }
        protected override void TakeDamage(int dmg, Attack attack)
        {
            if (currentArmor.durability - dmg > 0)
                DamageArmor(dmg);
            else
            {
                Health -= (dmg - currentArmor.durability);
                DamageArmor(dmg);
            }
        }
        public void UsePotion()
        {
            if (healthPotions > 0)
            {
                int heal = 10 * level;
                this.healthPotions -= 1;
                Heal(heal);
                Console.WriteLine($"Вы выпили зелье. Здоровье +{heal}");
            }
            else Console.WriteLine("У Вас нет зелий");
        }
        public void TakePotions(int value) => this.healthPotions += value;
        #endregion
    }
}
