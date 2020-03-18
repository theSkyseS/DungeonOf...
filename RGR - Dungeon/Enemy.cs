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
        protected List<string> weakSpots = new List<string>();
        public int exp;
        #endregion

        #region Methods
        protected override void TakeDamage(int dmg, Attack attack)
        {
            if (weakSpots.Contains(attack.name))
            {
                dmg *= 2;
                Console.WriteLine("Вы попали в уязвимое место! Нанесён двойной урон");
            }

            Health -= dmg;
        }
        public override void InflictAttack(Character attacked)
        {
            Random rnd = new Random();
            Attack UsedAttack = attacks[rnd.Next(0, attacks.Count)];
            if (UsedAttack.special) Heal(13 + (int)Math.Pow(2, difficulty));
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

            attacks.Add(new Attack(3 + (int)Math.Pow(2, difficulty), 80, false, "Укус"));
            attacks.Add(new Attack(8 + (int)Math.Pow(2, difficulty), 50, false, "Сильный укус"));
            weakSpots.Add("удар по голове");
        }
    }
    class DarkKnight : Enemy
    {
        public DarkKnight()
        {
            name = "Тёмный Рыцарь(не Бэтмен)";
            maxhealth = 100 + (int)Math.Pow(5, difficulty);
            Health = maxhealth;
            exp = 30;

            attacks.Add(new Attack(13 + (int)Math.Pow(2, difficulty), 90, false, "Удар"));
            attacks.Add(new Attack(0, 100, true, "Исцеление"));
            attacks.Add(new Attack(18 + (int)Math.Pow(2, difficulty), 75, false, "Сильный удар"));
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

            attacks.Add(new Attack(8 + (int)Math.Pow(2, difficulty), 95, false, "Плевок слизью"));
            attacks.Add(new Attack(13 + (int)Math.Pow(2, difficulty), 80, false, "Удар"));
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

            attacks.Add(new Attack(18 + (int)Math.Pow(2, difficulty), 80, true, "Поглощение жизни"));
            attacks.Add(new Attack(28 + (int)Math.Pow(2, difficulty), 70, false, "Крик"));
            attacks.Add(new Attack(3 + (int)Math.Pow(2, difficulty), 100, false, "Удар"));
            weakSpots.Add("удар по торсу");
        }
    }
    class SkeletonArcher : Enemy
    {
        public SkeletonArcher()
        {
            name = "Скелет лучник";
            maxhealth = 75 + (int)Math.Pow(5, difficulty);
            Health = maxhealth;
            exp = 25;

            attacks.Add(new Attack(13 + (int)Math.Pow(2, difficulty), 75, false, "Выстрел"));
            attacks.Add(new Attack(23 + (int)Math.Pow(2, difficulty), 60, false, "Заряженный выстрел"));
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
