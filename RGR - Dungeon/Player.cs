using System;
using System.Collections.Generic;
using static RGR___Dungeon.AttackType;

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
        private int intelligence;
        private int skillPoints;
        private Weapon currentWeapon;
        private Armor currentArmor;
        private List<Skill> passiveSkills = new List<Skill>();
        private List<ActiveSkill> activeSkills = new List<ActiveSkill>();
        private List<ActiveSkill> activeSkillList = new List<ActiveSkill>();
        #endregion

        #region Properties
        public int Level => level;
        public Armor CurrentArmor => currentArmor;
        #endregion
        public Player()
        {
            name = "Игрок";
            attacks.Add(new Attack(25, 75, "удар по торсу", physical));
            attacks.Add(new Attack(30, 45, "удар по голове", physical));
            attacks.Add(new Attack(18, 85, "удар по рукам", physical));
            attacks.Add(new Attack(15, 95, "удар по ногам", physical));
            attacks.Add(new Attack(0, 100, "лечение", special));

            activeSkillList.Add(new FireBall());
            activeSkillList.Add(new Meteor());
            activeSkillList.Add(new CriticalStrike());
            activeSkillList.Add(new LethalBlow());
            activeSkillList.Add(new KnifeThrow());
            activeSkillList.Add(new GhostArrow());

            level = 1;
            maxhealth = 100;
            Health = 100;
            healthPotions = 2;
            score = 0;
            expirience = 0;
            strength = 0;
            agility = 0;
            intelligence = 0;
            statPoints = 0;
            skillPoints = 0;
            expToNextLevel = 20;
            currentWeapon = new NoWeapon();
            currentArmor = new NoArmor();
            resistance = nothing;
            weaknessType = nothing;
        }

        #region methods
        #region attack
        public override void InflictAttack(Character attacked)
        {
            Console.Clear();
            Console.WriteLine(string.Format("Здоровье: {0}, Броня: {3}, Монеты: {1}, Зелья здоровья: {2}",
                                            Health,
                                            score,
                                            healthPotions,
                                            CurrentArmor.durability));
            Console.WriteLine(string.Format("Враг: {0}, Здоровье врага: {1}",
                                            attacked.name,
                                            attacked.Health));
            Console.WriteLine("Выберите действие:\n "
                              + "1 - ударить по торсу,\n "
                              + "2 - ударить по голове,\n "
                              + "3 - ударить по рукам,\n "
                              + "4 - ударить по ногам,\n "
                              + "5 - использовать зелье,\n "
                              + "6 - использовать навык ");
            try
            {
                int i = int.Parse(Console.ReadLine());
                if (i == 6)
                {
                    Console.Clear();
                    Console.WriteLine("Выберите навык для использования:");
                    if (activeSkills.Count == 0)
                    {
                        Console.WriteLine("У вас нет навыков.");
                        Console.ReadKey();
                        InflictAttack(attacked);
                    }
                    else
                    {
                        foreach(ActiveSkill skill in activeSkills)
                        {
                            Console.WriteLine(string.Format(" {0} - {1}. Урон: {2}, До перезарядки: {3}, Время перезарядки: {4}",
                                                            activeSkills.IndexOf(skill) + 1,
                                                            skill.SkillName,
                                                            skill.Damage,
                                                            skill.Cooldown,
                                                            skill.CooldownTime));
                        }
                        int j = int.Parse(Console.ReadLine());
                        if (j > activeSkills.Count)
                        {
                            InflictAttack(attacked);
                        }
                        else
                        {
                            if (activeSkills[j - 1].Cooldown == 0)
                            {
                                activeSkills[j - 1].UseSkill(attacked);
                                activeSkills[j - 1].Cooldown += 1;
                                CooldownTick();
                            }
                            else
                            {
                                Console.WriteLine("Навык на перезарядке.");
                                Console.ReadKey();
                                InflictAttack(attacked);
                            }
                        }
                        
                    }

                }
                else
                {
                    Attack UsedAttack = attacks[i - 1];
                    if (UsedAttack.Type == special) UsePotion();
                    UsedAttack.AttackEvent(attacked, UsedAttack, this);
                    CooldownTick();
                }
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
                if (attack.Type !=special)
                {
                    attack.Damage = attack.BaseDamage + currentWeapon.Damage + strength * 5;
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
                    Console.WriteLine(string.Format("Вы взяли {0}", weapon.WeaponName));
                    RegenerateAttacks();
                }
                else if (choice == 2) Console.WriteLine(string.Format("Вы оставили {0}", currentWeapon.WeaponName));
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
                    resistance = nothing;
                    weaknessType = nothing;
                }
            }
        }
        #endregion
        #region skills
        private void SkillsMenu()
        {
            Console.Clear();
            Console.WriteLine("Ваши текущие навыки: \n");
            foreach (ActiveSkill skill in activeSkills)
            {
                Console.WriteLine(string.Format(" {0} - {1}. Урон: {2}, Время перезарядки: {3}, Очков для улучшения: {4}",
                                                            activeSkills.IndexOf(skill) + 1,
                                                            skill.SkillName,
                                                            skill.Damage,
                                                            skill.CooldownTime,
                                                            skill.PointsToLearn));
            }
            Console.ReadKey();
            while (skillPoints > 0)
            {
                Console.Clear();
                Console.WriteLine($"У вас есть неиспользованные очки навыков: {skillPoints}");
                Console.WriteLine("Выберите:\n 1 - Изучить новый навык\n 2 - Улучшить один из имеющихся навыков\n 3 - Выйти из меню навыков");
                try
                {
                    switch (Console.ReadLine())
                    {
                        case "1":
                            {
                                Console.WriteLine("Навыки, доступные для изучения: ");
                                foreach (ActiveSkill activeSkill in activeSkillList)
                                    Console.WriteLine(string.Format(" {0} - {1}. Урон: {2}, Время перезарядки: {3}, Очков для изучения: {4}",
                                                            activeSkillList.IndexOf(activeSkill) + 1,
                                                            activeSkill.SkillName,
                                                            activeSkill.Damage,
                                                            activeSkill.CooldownTime,
                                                            activeSkill.PointsToLearn));
                                int i = int.Parse(Console.ReadLine());
                                ActiveSkill learningSkill = activeSkillList[i - 1];
                                if(learningSkill.PointsToLearn <= skillPoints)
                                {
                                    skillPoints -= learningSkill.PointsToLearn;
                                    activeSkills.Add(learningSkill);
                                    activeSkillList.Remove(learningSkill);
                                    RegenerateSkills();
                                    Console.WriteLine($"Вы изучили навык: {learningSkill.SkillName}");
                                    Console.ReadKey();
                                }
                                else
                                {
                                    Console.WriteLine("Недостаточно очков навыков для изучения.");
                                    Console.ReadKey();
                                    break;
                                }
                                
                            }
                            break;
                        case "2":
                            {
                                Console.WriteLine("Навыки, доступные для улучшения: ");
                                foreach (ActiveSkill activeSkill in activeSkills)
                                    Console.WriteLine(string.Format(" {0} - {1}. Урон: {2}, Откат: {3}, Уровень: {4}, Очков для улучшения: {5}",
                                                            activeSkills.IndexOf(activeSkill) + 1,
                                                            activeSkill.SkillName,
                                                            activeSkill.Damage,
                                                            activeSkill.CooldownTime,
                                                            activeSkill.level,
                                                            activeSkill.PointsToLearn)); 
                                int i = int.Parse(Console.ReadLine());
                                ActiveSkill learningSkill = activeSkills[i - 1];
                                if (learningSkill.PointsToLearn < skillPoints)
                                {
                                    skillPoints -= learningSkill.PointsToLearn;
                                    learningSkill.level += 1;
                                    RegenerateSkills();
                                    Console.WriteLine($"Вы улучшили навык: {learningSkill.SkillName}");
                                    Console.ReadKey();
                                }
                                else
                                {
                                    Console.WriteLine("Недостаточно очков навыков для улчучшения.");
                                    Console.ReadKey();
                                    break;
                                }
                            }
                            break;
                        case "3": return;
                        default:
                            Console.WriteLine("Введите корректное значение.");
                            Console.ReadKey();
                            break;
                    }
                }
                catch(Exception)
                {
                    Console.WriteLine("Введите корректное значение.");
                    Console.ReadKey();
                }
                
            }

        }
        private void RegenerateSkills()
        {
            foreach (ActiveSkill activeSkill in activeSkills)
            {
                switch(activeSkill.AttackType)
                {
                    case magic: activeSkill.Damage = activeSkill.BaseDamage + activeSkill.level * 6 + intelligence * 8; break;
                    case physical: activeSkill.Damage = activeSkill.BaseDamage + activeSkill.level * 5 + strength * 6; break;
                    case ranged: activeSkill.Damage = activeSkill.BaseDamage + activeSkill.level * 4 + agility * 4; break;
                }
            }
        }
        private void CooldownTick()
        {
            foreach (ActiveSkill activeSkill in activeSkills)
                activeSkill.Cooldown -= 1;
        }
        #endregion

        #region level
        public void CheckLevelUp()
        {
            while (expirience >= expToNextLevel)
            {
                Console.Clear();
                Console.WriteLine("Новый уровень! \n"
                                  + "Очки характеристик + 3\n" 
                                  + "Очки навыков +1");
                Console.ReadKey();
                statPoints += 3;
                skillPoints += 1;
                expirience -= expToNextLevel;
                level += 1;
                expToNextLevel += 15 * level;
            }

        }
        public void SkillsOrStats()
        {
            Console.Clear();
            Console.WriteLine("Выберите действие: \n 1 - Меню навыков \n 2 - Меню характеристик");
            switch (Console.ReadLine())
            {
                case "1": SkillsMenu(); break;
                case "2": StatsMenu(); break;
                default:
                    Console.WriteLine("Введите корректное значение.");
                    SkillsOrStats();
                    break;
            }
        }
        private void StatsMenu()
        {
            Console.Clear();
            Console.WriteLine(string.Format("Ваши характеристики: "
                                            + "\n Здоровье: {0}, "
                                            + "\n Сила: {1}, "
                                            + "\n Ловкость: {2}, "
                                            + "\n Интеллект: {3}",
                                            maxhealth, strength, agility, intelligence));
            Console.ReadKey();
            while (statPoints > 0)
            {
                Console.Clear();
                Console.WriteLine(string.Format("Ваши характеристики: "
                                            + "\n Здоровье: {0}, "
                                            + "\n Сила: {1}, "
                                            + "\n Ловкость: {2}, "
                                            + "\n Интеллект: {3}",
                                            maxhealth, strength, agility, intelligence));
                Console.WriteLine($"\nУ вас есть {statPoints} очков характеристик"
                                  + "\nВыберите характеристику для прокачки:"
                                  + "\n 1 - Здоровье +25"
                                  + "\n 2 - Сила +1"
                                  + "\n 3 - Ловкость +1"
                                  + "\n 4 - Интеллект +1");;
                switch (Console.ReadLine())
                {
                    case "1":
                        maxhealth += 25;
                        Health += 25;
                        statPoints -= 1;
                        break;
                    case "2":
                        strength += 1;
                        RegenerateAttacks();
                        RegenerateSkills();
                        statPoints -= 1;
                        break;
                    case "3":
                        if (agility < 10)
                        {
                            agility += 1;
                            RegenerateAttacks();
                            RegenerateSkills();
                            statPoints -= 1;
                        }
                        else
                        {
                            Console.WriteLine("Вы достигли предела человеческих возможностей."
                                              + "\n вы не можете иметь ловкость выше 10.");
                            Console.ReadKey();
                        }
                        break;
                    case "4":
                        intelligence += 1;
                        statPoints -= 1;
                        RegenerateAttacks();
                        RegenerateSkills();
                        break;
                    default:
                        Console.WriteLine("Неккоректный ввод");
                        Console.ReadKey();
                        break;
                }
            }
        }
        #endregion
        public override void TakeDamage(int dmg)
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
                int heal = 10 * difficulty;
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
