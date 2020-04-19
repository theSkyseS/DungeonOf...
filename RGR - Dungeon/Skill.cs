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
        abstract public new Type GetType();
        public int level;
        protected string name;
        protected int pointsToLearn;

        public int PointsToLearn => pointsToLearn;
        public string Name => name;
    }
    [Serializable]
    abstract class ActiveSkill : Skill
    {
        public override Type GetType()
        {
            return typeof(ActiveSkill);
        }
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
                if (value <= 0) cooldown = 0;
                else cooldown = value;
            }
        }
        public int CooldownTime => cooldownTime;
        public int Damage { get => damage; set => damage = value; }
        public int BaseDamage => baseDamage;
        public AttackType AttackType => attackType;

        public virtual void UseSkill(Character attacked, Player player)
        {
            Console.WriteLine(string.Format("{2} использовали {0} на {1}",
                                             name, attacked.name, player.name));
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
            attacked.TakeDamage(dmg, attackType);
            Cooldown = cooldownTime;
        }
    }
    [Serializable]
    class MagicArmor : ActiveSkill
    {
        public MagicArmor()
        {
            name = "Магическая броня";
            baseDamage = 0;
            damage = baseDamage;
            cooldownTime = 3;
            level = 0;
            pointsToLearn = 2;
            attackType = special;
        }
        public override void UseSkill(Character attacked , Player player)
        {
            Console.WriteLine($"Вы использовали {name}");
            player.CurrentArmor.durability += 20;
            Cooldown = CooldownTime;
        }
    }
    [Serializable]
    class FireBall : ActiveSkill
    {
        public FireBall()
        {
            name = "Огненный шар";
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
            name = "Метеор";
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
            name = "Точный удар";
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
            name = "Cмертельный удар";
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
            name = "Бросок ножа";
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
            name = "Призрачная стрела";
            baseDamage = 25;
            damage = baseDamage;
            cooldownTime = 2;
            level = 0;
            pointsToLearn = 2;
            attackType = ranged;
        }
    }
    [Serializable]
    abstract class PassiveSkill : Skill
    {
        protected string description;
        public override Type GetType()
        {
            return typeof(PassiveSkill);
        }

        public string Description => description;
    }
    [Serializable]
    class StoneSkin : PassiveSkill
    {
        public StoneSkin()
        {
            name = "Каменная кожа";
            description = "Уменьшает получаемый Вами урон от физических атак на 5 едениц за уровень";
            level = 0;
            pointsToLearn = 2;
        }
    }
    [Serializable]
    class GoodStudent : PassiveSkill
    {
        public GoodStudent()
        {
            name = "Прилежный ученик";
            description = "Увеличивает количество получаемого опыта за убийство монстров";
            level = 0;
            pointsToLearn = 6;
        }
    }
    [Serializable]
    class Dodge : PassiveSkill
    {
        public Dodge()
        {
            name = "Уклонение";
            description = "Увеличивает шанс увернутся от атаки противника на 10%";
            level = 0;
            pointsToLearn = 8;
        }
    }
    [Serializable]
    class MagicNullify : PassiveSkill
    {
        public MagicNullify()
        {
            name = "Невосприимчивость к магии";
            description = "Уменьшает получаемый Вами урон от магических атак на 5 едениц за уровень";
            level = 0;
            pointsToLearn = 2;
        }
    }

}
