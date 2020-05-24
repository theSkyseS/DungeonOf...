using System;
using static RGR___Dungeon.AttackType;

namespace RGR___Dungeon
{
    abstract class Enemy : Character
    {

        #region Fields
        
        protected int exp;
        protected AttackType defaultAttackType;
        public int Exp => exp;
        #endregion

        #region Methods
        public override void TakeDamage(int dmg, AttackType attackType)
        {
            Health -= dmg;
        }
        public override void InflictAttack(Character attacked)
        {
            Attack UsedAttack = attacks[random.Next(0, attacks.Count)];
            if (UsedAttack.AttackEvent(attacked, UsedAttack, this))
            {
                if (UsedAttack.Type == special) 
                    Health += (13 + 5 * Player.difficulty);
            }
        }
        #endregion
    }

    #region Enemy Types
    class Rat : Enemy
    {
        public Rat()
        {
            name = "Крыса";
            maxhealth = 10 + 20 * Player.difficulty;
            Health = maxhealth;
            exp = 5 + 15 * Player.difficulty;
            defaultAttackType = physical;
            weaknessType = physical;
            resistance = poison;

            attacks.Add(new Attack(3 + 2 * Player.difficulty, 80, "Укус", defaultAttackType));
            attacks.Add(new Attack(8 + 4 * Player.difficulty, 50, "Сильный укус", defaultAttackType));
            weakSpots.Add("удар по голове");
        }
    }
    class DarkKnight : Enemy
    {
        public DarkKnight()
        {
            name = "Падший рыцарь";
            maxhealth = 80 + 25 * Player.difficulty;
            Health = maxhealth;
            exp = 30 + 50 * Player.difficulty;
            defaultAttackType = physical;
            weaknessType = poison;
            resistance = physical;

            attacks.Add(new Attack(17 + 9 * Player.difficulty, 90, "Удар", defaultAttackType));
            attacks.Add(new Attack(0, 100, "Исцеление", special));
            attacks.Add(new Attack(20 + 10 * Player.difficulty, 75, "Сильный удар", defaultAttackType));
            weakSpots.Add("удар по голове");
            weakSpots.Add("удар по рукам");
        }
    }
    class SlimeMonster : Enemy
    {
        public SlimeMonster()
        {
            name = "Слизень";
            maxhealth = 50 + 20 * Player.difficulty;
            Health = maxhealth;
            exp = 15 + 20 * Player.difficulty;
            defaultAttackType = poison;
            weaknessType = physical;
            resistance = poison;

            attacks.Add(new Attack(8 + 6 * Player.difficulty, 95, "Плевок слизью", defaultAttackType));
            attacks.Add(new Attack(13 + 8 * Player.difficulty, 80, "Удар", defaultAttackType));
            weakSpots.Add("удар по торсу");
        }
    }
    class VengefulSpirit : Enemy
    {
        public VengefulSpirit()
        {
            name = "Дух мщения";
            maxhealth = 150 + 30 * Player.difficulty;
            Health = maxhealth;
            exp = 50 + 70 * Player.difficulty;
            defaultAttackType = magic;
            weaknessType = magic;
            resistance = ranged;

            attacks.Add(new Attack(17 + 8 * Player.difficulty, 80, "Поглощение жизни", special));
            attacks.Add(new Attack(24 + 12 * Player.difficulty , 70, "Крик", defaultAttackType));
            attacks.Add(new Attack(3 * Player.difficulty, 99, "Удар", physical));
            weakSpots.Add("удар по торсу");
        }
    }
    class SkeletonArcher : Enemy
    {
        public SkeletonArcher()
        {
            name = "Скелет";
            maxhealth = 75 + 20 * Player.difficulty;
            Health = maxhealth;
            exp = 25 + 30 * Player.difficulty;
            defaultAttackType = ranged;
            weaknessType = poison;
            resistance = ranged;

            attacks.Add(new Attack(10 + 5 * Player.difficulty, 75, "Выстрел", defaultAttackType));
            attacks.Add(new Attack(20 + 10 * Player.difficulty, 60, "Заряженный выстрел", defaultAttackType));
            attacks.Add(new Attack(5 + 4 * Player.difficulty, 90, "Удар луком", physical));
            weakSpots.Add("удар по рукам");
            weakSpots.Add("удар по ногам");
        }
    }
    
    class Zombie : Enemy
    {
        public Zombie()
        {
            name = "Зомби";
            maxhealth = 100 + 25 * Player.difficulty;
            Health = maxhealth;
            exp = 25 + 30 * Player.difficulty;
            defaultAttackType = physical;
            weaknessType = magic;
            resistance = poison;

            attacks.Add(new Attack(6 + 4 * Player.difficulty, 90, "Удар", defaultAttackType));
            attacks.Add(new Attack(15 + 8 * Player.difficulty, 50, "Укус", defaultAttackType));
            attacks.Add(new Attack(2 * Player.difficulty, 95, "Слабый удар", defaultAttackType));
            weakSpots.Add("удар по рукам");
            weakSpots.Add("удар по ногам");
        }
    }
    class Mage : Enemy
    { 
        public Mage()
        {
            name = "Маг-отступник";
            maxhealth = 35 + 15 * Player.difficulty;
            Health = maxhealth;
            exp = 30 + 35 * Player.difficulty;
            defaultAttackType = magic;
            weaknessType = ranged;
            resistance = magic;

            attacks.Add(new Attack(30 + 10 * Player.difficulty, 80, "Огненный шар", defaultAttackType));
            attacks.Add(new Attack(20 + 8 * Player.difficulty, 90, "Удар молнии", defaultAttackType));
            attacks.Add(new Attack(0, 99, "Исцеление", special));
            weakSpots.Add("удар по голове");
        }
    }
    //TODO: NEW MONSTERS
    #endregion
}
