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
        private int statPoints;
        private int agility;
        private Weapon currentWeapon;
        public Armor currentArmor;
        Random random = new Random();
        #endregion

        #region Properties
        public int Level => level;
        #endregion

        public Player()
        {
            name = "Игрок";
            attacks.Add(new Attack(25, 75, "удар по торсу", AttackType.physical));
            attacks.Add(new Attack(30, 45, "удар по голове", AttackType.physical));
            attacks.Add(new Attack(18, 85, "удар по рукам", AttackType.physical));
            attacks.Add(new Attack(15, 95, "удар по ногам", AttackType.physical));
            attacks.Add(new Attack(0, 100, "лечение", AttackType.special));
            level = 1;
            maxhealth = 100;
            Health = 100;
            healthPotions = 2;
            score = 0;
            expirience = 0;
            strength = 0;
            agility = 0;
            statPoints = 0;
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
                if (UsedAttack.Type == AttackType.special) UsePotion();
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
                if (attack.Type !=AttackType.special)
                {
                    attack.Damage = attack.BaseDamage + currentWeapon.Damage + strength * 6;
                    attack.SuccessChance = attack.BaseChance + agility * 2;
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
                Console.WriteLine(string.Format("Вы нашли {0}. Защита: {1}, Штраф к улонению {2}, Устойчивость: {3}, Уязвимость: {4}",
                                                armor.ArmorName,
                                                armor.durability,
                                                armor.AgilityPenalty,
                                                armor.Resistance,
                                                armor.Weakness));
                if (currentArmor.GetType() == typeof(NoArmor))
                {
                    Console.WriteLine("\nУ Вас нет брони");
                }
                else
                {
                    Console.WriteLine(string.Format("\nВаша броня:{0}. Защита: {1}, Штраф к уклонению {2} ,Устойчивость: {3}, Уязвимость: {4}",
                                                currentArmor.ArmorName,
                                                currentArmor.durability,
                                                currentArmor.AgilityPenalty,
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
                                      + "очки характеристик + 2");
                    statPoints += 2;
                    expirience -= expToNextLevel;
                    level += 1;
                    expToNextLevel += 15 * level;
                    while (statPoints > 0)
                    {
                        Console.WriteLine("\nВыберите характеристику для прокачки:"
                                          + "\n 1 - Здоровье +25"
                                          + "\n 2 - Сила +1"
                                          + "\n 3 - Ловкость +1");
                        int i = int.Parse(Console.ReadLine());
                        switch (i)
                        {
                            case 1:
                                maxhealth += 25;
                                Health += 25;
                                statPoints -= 1;
                                break;
                            case 2:
                                strength += 1;
                                RegenerateAttacks();
                                statPoints -= 1;
                                break;
                            case 3:
                                if (agility < 10)
                                {
                                    agility += 1;
                                    RegenerateAttacks();
                                    statPoints -= 1;
                                }
                                else
                                    Console.WriteLine("Вы достигли предела человеческих возможностей."
                                                      + "\n вы не можете иметь ловкость выше 10.");
                                break;
                            default:
                                maxhealth += 25;
                                Health += 25;
                                statPoints -= 1;
                                break;
                        }
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

            if (random.Next(101) + currentArmor.AgilityPenalty >= agility * 3)
            {
                if (currentArmor.durability - dmg > 0)
                    DamageArmor(dmg);
                else
                {
                    Health -= (dmg - currentArmor.durability);
                    DamageArmor(dmg);
                }
            }
            else
            {
                Console.WriteLine("Вы смогли увернуться от атаки противника");
                Console.ReadKey();
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
