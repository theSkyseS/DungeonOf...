using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGR___Dungeon
{
    abstract class Enemy : Character
    {

        #region Fields
        public static int difficulty = 1;
        protected int exp;
        protected AttackType defaultAttackType;

        public int Exp => exp;
        #endregion

        #region Methods
        protected override void TakeDamage(int dmg, Attack attack)
        {
            Health -= dmg;
        }
        public override void InflictAttack(Character attacked)
        {
            Random rnd = new Random();
            Attack UsedAttack = attacks[rnd.Next(0, attacks.Count)];
            if (UsedAttack.Special) Heal(13 + (int)Math.Pow(2, difficulty));
            UsedAttack.AttackEvent(attacked, UsedAttack, this);
        }
        #endregion
    }

    #region Enemy Types
    class Rat : Enemy
    {
        public Rat()
        {
            name = "Крыса";
            maxhealth = 15 + (int)Math.Pow(5, difficulty);
            Health = maxhealth;
            exp = 5;
            defaultAttackType = AttackType.physical;
            weaknessType = AttackType.physical;
            resistance = AttackType.poison;

            attacks.Add(new Attack(3 + (int)Math.Pow(2, difficulty), 80, false, "Укус", defaultAttackType));
            attacks.Add(new Attack(8 + (int)Math.Pow(2, difficulty), 50, false, "Сильный укус", defaultAttackType));
            weakSpots.Add("удар по голове");
        }
    }
    class DarkKnight : Enemy
    {
        public DarkKnight()
        {
            name = "Падший рыцарь";
            maxhealth = 100 + (int)Math.Pow(5, difficulty);
            Health = maxhealth;
            exp = 30;
            defaultAttackType = AttackType.physical;
            weaknessType = AttackType.poison;
            resistance = AttackType.physical;

            attacks.Add(new Attack(13 + (int)Math.Pow(2, difficulty), 90, false, "Удар", defaultAttackType));
            attacks.Add(new Attack(0, 100, true, "Исцеление"));
            attacks.Add(new Attack(18 + (int)Math.Pow(2, difficulty), 75, false, "Сильный удар", defaultAttackType));
            weakSpots.Add("удар по голове");
            weakSpots.Add("удар по рукам");
        }
    }
    class SlimeMonster : Enemy
    {
        public SlimeMonster()
        {
            name = "Слизень";
            maxhealth = 50 + (int)Math.Pow(5, difficulty);
            Health = maxhealth;
            exp = 15;
            defaultAttackType = AttackType.poison;
            weaknessType = AttackType.physical;
            resistance = AttackType.poison;

            attacks.Add(new Attack(8 + (int)Math.Pow(2, difficulty), 95, false, "Плевок слизью", defaultAttackType));
            attacks.Add(new Attack(13 + (int)Math.Pow(2, difficulty), 80, false, "Удар", defaultAttackType));
            weakSpots.Add("удар по торсу");
        }
    }
    class VengefulSpirit : Enemy
    {
        public VengefulSpirit()
        {
            name = "Дух мщения";
            maxhealth = 150 + (int)Math.Pow(5, difficulty);
            Health = maxhealth;
            exp = 50;
            defaultAttackType = AttackType.magic;
            weaknessType = AttackType.magic;
            resistance = AttackType.ranged;

            attacks.Add(new Attack(18 + (int)Math.Pow(2, difficulty), 80, true, "Поглощение жизни", defaultAttackType));
            attacks.Add(new Attack(28 + (int)Math.Pow(2, difficulty), 70, false, "Крик", defaultAttackType));
            attacks.Add(new Attack(3 + (int)Math.Pow(2, difficulty), 100, false, "Удар"));
            weakSpots.Add("удар по торсу");
        }
    }
    class SkeletonArcher : Enemy
    {
        public SkeletonArcher()
        {
            name = "Скелет";
            maxhealth = 75 + (int)Math.Pow(5, difficulty);
            Health = maxhealth;
            exp = 25;
            defaultAttackType = AttackType.ranged;
            weaknessType = AttackType.poison;
            resistance = AttackType.ranged;

            attacks.Add(new Attack(13 + (int)Math.Pow(2, difficulty), 75, false, "Выстрел", defaultAttackType));
            attacks.Add(new Attack(23 + (int)Math.Pow(2, difficulty), 60, false, "Заряженный выстрел", defaultAttackType));
            attacks.Add(new Attack(8 + (int)Math.Pow(2, difficulty), 90, false, "Удар луком"));
            weakSpots.Add("удар по рукам");
            weakSpots.Add("удар по ногам");
        }
    }
    
    class Zombie : Enemy
    {
        public Zombie()
        {
            name = "Зомби";
            maxhealth = 100 + (int)Math.Pow(5, difficulty);
            Health = maxhealth;
            exp = 25;
            defaultAttackType = AttackType.physical;
            weaknessType = AttackType.magic;
            resistance = AttackType.poison;

            attacks.Add(new Attack(13 + (int)Math.Pow(2, difficulty), 90, false, "Удар"));
            attacks.Add(new Attack(23 + (int)Math.Pow(2, difficulty), 50, false, "Укус"));
            attacks.Add(new Attack(3 + (int)Math.Pow(2, difficulty), 95, false, "Слабый удар"));
            weakSpots.Add("удар по рукам");
            weakSpots.Add("удар по ногам");
        }
    }
    //TODO: NEW MONSTERS
    #endregion
}
