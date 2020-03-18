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
        protected List<string> weakSpots = new List<string>();
        #endregion

        #region Methods
        protected override void TakeDamage(int dmg, Attack attack)
        {
            if (weakSpots.Contains(attack.name))
            {
                dmg *= 2;
                Console.WriteLine("Крит! Вы нанесли двойной урон");
            }

            health -= dmg;
        }
        public override void InflictAttack(Character attacked)
        {
            Random rnd = new Random();
            Attack UsedAttack = attacks[rnd.Next(0, attacks.Count)];
            if (UsedAttack.special) Heal(15);
            UsedAttack.AttackEvent(attacked, UsedAttack, this);
        }
        #endregion
    }

    #region Enemy Types
    class Rat : Enemy
    {
        public Rat()
        {
            attacks.Add(new Attack(5, 80, false, "Укус"));
            attacks.Add(new Attack(10, 50, false, "Сильный укус"));
            health = 15;
            name = "Крыса";
            weakSpots.Add("удар по голове");
        }
    }
    class DarkKnight : Enemy
    {
        public DarkKnight()
        {
            name = "Тёмный Рыцарь(не Бэтмен)";
            health = 100;
            attacks.Add(new Attack(15, 90, false, "Удар"));
            attacks.Add(new Attack(0, 100, true, "Исцеление"));
            attacks.Add(new Attack(20, 75, false, "Сильный удар"));
            weakSpots.Add("удар по голове");
            weakSpots.Add("удар по рукам");
        }
    }
    
    //TODO: NEW MONSTERS
    #endregion
}
