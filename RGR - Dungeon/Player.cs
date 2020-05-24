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
        private int expirience;
        private int strength;
        private int expToNextLevel;
        private int level;
        private int statPoints;
        private int agility;
        private int intelligence;
        private int skillPoints;
        private Weapon currentWeapon;
        private Armor currentArmor;
        private List<Skill> Skills = new List<Skill>();
        private List<Skill> SkillList = new List<Skill>();
        #endregion

        #region Properties
        public int Level => level;
        public Armor CurrentArmor => currentArmor;
        public int Expirience => expirience;
        #endregion
        public Player()
        {
            name = "Игрок";
            attacks.Add(new Attack(25, 75, "удар по торсу", physical));
            attacks.Add(new Attack(30, 45, "удар по голове", physical));
            attacks.Add(new Attack(18, 85, "удар по рукам", physical));
            attacks.Add(new Attack(15, 95, "удар по ногам", physical));
            attacks.Add(new Attack(0, 100, "лечение", special));

            SkillList.Add(new MagicArmor());
            SkillList.Add(new FireBall());
            SkillList.Add(new Meteor());
            SkillList.Add(new CriticalStrike());
            SkillList.Add(new LethalBlow());
            SkillList.Add(new KnifeThrow());
            SkillList.Add(new GhostArrow());

            SkillList.Add(new StoneSkin());
            SkillList.Add(new GoodStudent());
            SkillList.Add(new Dodge());
            SkillList.Add(new MagicNullify());

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
                    if (Skills.Count == 0)
                    {
                        Console.WriteLine("У вас нет навыков.");
                        Console.ReadKey();
                        InflictAttack(attacked);
                    }
                    else
                    {
                        foreach(ActiveSkill skill in Skills)
                        {
                            Console.WriteLine(string.Format(" {0} - {1}. Урон: {2}, До перезарядки: {3}, Время перезарядки: {4}",
                                                            Skills.IndexOf(skill) + 1,
                                                            skill.Name,
                                                            skill.Damage,
                                                            skill.Cooldown,
                                                            skill.CooldownTime));
                        }
                        int j = int.Parse(Console.ReadLine());
                        ActiveSkill activeSkill = (ActiveSkill) Skills[j - 1];
                        if (activeSkill.Cooldown == 0)
                        {
                            activeSkill.UseSkill(attacked, this);
                            activeSkill.Cooldown += 1;
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
                else
                {
                    Attack UsedAttack = attacks[i - 1];
                    if (UsedAttack.Type == special) UsePotion();
                    UsedAttack.AttackEvent(attacked, UsedAttack, this);
                    CooldownTick();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "\n" + e.StackTrace + "\n" + e.Source + "\n" + e.InnerException);

                Console.WriteLine("Введите корректное значение");
                InflictAttack(attacked);
            }

        }
        private void RegenerateAttacks()
        {
            foreach (Attack attack in attacks)
            {
                if (attack.Type != special)
                {
                    switch (currentWeapon.AttackType)
                    {
                        case physical: 
                        case nothing:
                            attack.Damage = attack.BaseDamage + currentWeapon.Damage + strength * 5;
                            attack.SuccessChance = attack.BaseChance + agility * 2;
                            break;
                        case ranged:
                            attack.Damage = attack.BaseDamage + currentWeapon.Damage + agility * 5;
                            attack.SuccessChance = attack.BaseChance + agility * 3;
                            break;
                        case magic:
                            attack.Damage = attack.BaseDamage + currentWeapon.Damage + intelligence * 5;
                            attack.SuccessChance = attack.BaseChance + agility * 1;
                            break;
                        default:
                            attack.Damage = attack.BaseDamage + currentWeapon.Damage;
                            break;
                    }
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
            foreach (ActiveSkill skill in Skills.FindAll(x => x.GetType() == typeof(ActiveSkill)))
            {
                Console.WriteLine(string.Format(" {0} - {1}. Урон: {2}, Уровень {3}, Время перезарядки: {4}, Очков для улучшения: {5}",
                                                Skills.IndexOf(skill) + 1,
                                                skill.Name,
                                                skill.Damage,
                                                skill.level,
                                                skill.CooldownTime,
                                                skill.PointsToLearn));
            }
            foreach (PassiveSkill passiveSkill in Skills.FindAll(x => x.GetType() == typeof(PassiveSkill)))
            {
                Console.WriteLine(string.Format(" {0} - {1}. Уровень: {2}, \n  Описание: {3}, Очков для улучшения: {4}",
                                                Skills.IndexOf(passiveSkill) + 1,
                                                passiveSkill.Name,
                                                passiveSkill.level,
                                                passiveSkill.Description,
                                                passiveSkill.PointsToLearn));
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
                                Console.WriteLine("Навыки, доступные для изучения:");
                                Console.WriteLine(" Активные навыки:");
                                foreach (ActiveSkill skill in SkillList.FindAll(x => x.GetType() == typeof(ActiveSkill)))
                                    Console.WriteLine(string.Format(" {0} - {1}. Урон: {2}, Время перезарядки: {3}, Очков для улучшения: {4}",
                                                                    SkillList.IndexOf(skill) + 1,
                                                                    skill.Name,
                                                                    skill.Damage,
                                                                    skill.CooldownTime,
                                                                    skill.PointsToLearn));
                                Console.WriteLine(" Пассивные навыки:");
                                foreach (PassiveSkill skill in SkillList.FindAll(x=> x.GetType() == typeof(PassiveSkill)))
                                    Console.WriteLine(string.Format(" {0} - {1}. Описание: {2}, Очков для улучшения: {3}",
                                                                    SkillList.IndexOf(skill) + 1,
                                                                    skill.Name,
                                                                    skill.Description,
                                                                    skill.PointsToLearn));
                                int i = int.Parse(Console.ReadLine());
                                if (i > SkillList.Count + 1) break;
                                Skill learningSkill = SkillList[i - 1];
                                if(learningSkill.PointsToLearn <= skillPoints)
                                {
                                    learningSkill.level += 1;
                                    SkillList.Remove(learningSkill);
                                    Skills.Add(learningSkill);
                                    skillPoints -= learningSkill.PointsToLearn;
                                    RegenerateSkills();
                                }
                                else
                                {
                                    Console.WriteLine("Вы накопили недостаточно знаний для изучения этого навыка");
                                    Console.ReadKey();
                                }
                            }
                            break;
                        case "2":
                            {
                                Console.WriteLine("Навыки, доступные для улучшения:");
                                foreach (ActiveSkill skill in Skills.FindAll(x => x.GetType() == typeof(ActiveSkill)))
                                    Console.WriteLine(string.Format(" {0} - {1}. Урон: {2}, Уровень {3}, Время перезарядки: {4}, Очков для улучшения: {5}",
                                                                    Skills.IndexOf(skill) + 1,
                                                                    skill.Name,
                                                                    skill.Damage,
                                                                    skill.level,
                                                                    skill.CooldownTime,
                                                                    skill.PointsToLearn));
                                foreach (PassiveSkill skill in Skills.FindAll(x => x.GetType() == typeof(PassiveSkill)))
                                    Console.WriteLine(string.Format(" {0} - {1}. Уровень: {2}, \n  Описание: {3}, Очков для улучшения: {4}",
                                                                    Skills.IndexOf(skill) + 1,
                                                                    skill.Name,
                                                                    skill.level,
                                                                    skill.Description,
                                                                    skill.PointsToLearn));
                                int i = int.Parse(Console.ReadLine());
                                if (i > Skills.Count + 1) break;
                                Skill learningSkill = Skills[i - 1];
                                if (learningSkill.PointsToLearn <= skillPoints)
                                {
                                    learningSkill.level += 1;
                                    skillPoints -= learningSkill.PointsToLearn;
                                    RegenerateSkills();
                                }
                                else
                                {
                                    Console.WriteLine("Вы накопили недостаточно знаний для изучения этого навыка");
                                    Console.ReadKey();
                                }
                            }
                            break;
                        case "3": return;
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine("Введите корректное значение.");
                    Console.ReadKey();
                }
                
            }

        }
        private void RegenerateSkills()
        {
            foreach (ActiveSkill activeSkill in Skills)
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
            foreach (ActiveSkill activeSkill in Skills.FindAll(x => x.GetType() == typeof(ActiveSkill)))
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
        public void TakeExpririence(int exp)
        {
            if (Skills.Contains(new GoodStudent()))
            {
                int skillLevel = Skills[Skills.IndexOf(new GoodStudent())].level;
                exp += exp * (skillLevel);
            }
            expirience += exp;
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
        public override void TakeDamage(int dmg, AttackType attackType)
        {
            
            if (Skills.Exists(x => x.Name == "Каменная кожа") && (attackType == physical || attackType == ranged))
            {
                Skill findSkill = Skills.Find(x => x.Name == "Каменная кожа");
                dmg -= 5 * findSkill.level;
            }
            if (Skills.Exists(x => x.Name == "Невосприимчивость к магии") && attackType == magic)
            {
                Skill findSkill = Skills.Find(x => x.Name == "Невосприимчивость к магии");
                dmg -= 5 * findSkill.level;
            }
            if (Skills.Exists(x => x.Name == "Уклонение"))
            {
                Skill findSkill = Skills.Find(x => x.Name == "Уклонение");
                if (random.Next(101) + currentArmor.AgilityPenalty >= agility * 3 + findSkill.level * 10)
                {
                    if (currentArmor.durability - dmg > 0)
                        DamageArmor(dmg);
                    else
                    {
                        Health -= (dmg - currentArmor.durability);
                        DamageArmor(dmg);
                    }
                }
            }
            else if (random.Next(101) + currentArmor.AgilityPenalty >= agility * 3)
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
                Health += heal;
                Console.WriteLine($"Вы выпили зелье. Здоровье +{heal}");
            }
            else Console.WriteLine("У Вас нет зелий");
        }
        public void TakePotions(int value) => healthPotions += value;
        #endregion
    }
}
