using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RGR___Dungeon.AttackType;

namespace RGR___Dungeon
{
    [Serializable]
    abstract class Skill
    {
        public int level;
        protected string skillName;
        protected int pointsToLearn;

        public int PointsToLearn => pointsToLearn;
        public string SkillName => skillName;
    }
    [Serializable]
    abstract class ActiveSkill : Skill
    {
        protected int baseDamage;
        protected int damage;
        protected int cooldownTime;
        protected int cooldown;
        protected AttackType attackType;

        public int Cooldown 
        { 
            get => cooldown; 
            set
            {
                if (cooldown - value <= 0) cooldown = 0;
                else cooldown = value;
            }
        }
        public int CooldownTime => cooldownTime;
        public int Damage { get => damage; set => damage = value; }
        public int BaseDamage => baseDamage;
        public AttackType AttackType => attackType;

        public void UseSkill(Character attacked)
        {
            Console.WriteLine(string.Format("Вы использовали {0} на {1}",
                                             skillName, attacked.name));
            int dmg = damage;
            if (AttackType == attacked.WeaknessType)
            {
                dmg = (int)(dmg * 1.5);
                Console.WriteLine($"{attacked.name} уязвим к данному типу урона.");
            }
            else if (AttackType == attacked.Resistance)
            {
                dmg = (int)(dmg / 1.5);
                Console.WriteLine($"{attacked.name} устойчив к данному типу урона.");
            }
            Console.WriteLine($"Урон: {dmg}");
            attacked.TakeDamage(dmg);
            cooldown = cooldownTime;
        }
    }
    [Serializable]
    class FireBall : ActiveSkill
    {
        public FireBall()
        {
            skillName = "Огненный шар";
            baseDamage = 35;
            damage = baseDamage;
            cooldownTime = 4;
            level = 0;
            pointsToLearn = 2;
            attackType = magic;
        }
    }
    [Serializable]
    class Meteor : ActiveSkill
    {
        public Meteor()
        {
            skillName = "Метеор";
            baseDamage = 60;
            damage = baseDamage;
            cooldownTime = 8;
            level = 0;
            pointsToLearn = 3;
            attackType = magic;
        }
    }
    [Serializable]
    class CriticalStrike : ActiveSkill
    {
        public CriticalStrike()
        {
            skillName = "Точный удар";
            baseDamage = 30;
            damage = baseDamage;
            cooldownTime = 3;
            level = 0;
            pointsToLearn = 1;
            attackType = physical;
        }
    }
    [Serializable]
    class LethalBlow : ActiveSkill
    {
        public LethalBlow()
        {
            skillName = "Cмертельный удар";
            baseDamage = 45;
            damage = baseDamage;
            cooldownTime = 5;
            level = 0;
            pointsToLearn = 2;
            attackType = physical;
        }
    }
    [Serializable]
    class KnifeThrow : ActiveSkill
    {
        public KnifeThrow()
        {
            skillName = "Бросок ножа";
            baseDamage = 20;
            damage = baseDamage;
            cooldownTime = 1;
            level = 0;
            pointsToLearn = 1;
            attackType = ranged;
        }
    }
    [Serializable]
    class GhostArrow : ActiveSkill
    {
        public GhostArrow()
        {
            skillName = "Призрачная стрела";
            baseDamage = 25;
            damage = baseDamage;
            cooldownTime = 2;
            level = 0;
            pointsToLearn = 2;
            attackType = ranged;
        }
    }
}
