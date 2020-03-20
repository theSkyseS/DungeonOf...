using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGR___Dungeon
{
    abstract class Weapon : Entity
    {
        protected int damage;
        protected int durability;
        protected AttackType attackType;
        protected string weaponName;

        public int Damage { get => damage;}
        public int Durability { get => durability; set => durability = value; }
        internal AttackType AttackType { get => attackType;}
        public string WeaponName { get => weaponName;}
    }
    class Shortsword : Weapon
    { 
        public Shortsword()
        {
            weaponName = "короткий меч";
            damage = 5;
            durability = 25;
            attackType = AttackType.physical;
        }
    }
    class Sword : Weapon
    {
        public Sword()
        {
            weaponName = "меч";
            damage = 10;
            durability = 35;
            attackType = AttackType.physical;
        }
    }
    class Longbow : Weapon
    {
        public Longbow()
        {
            weaponName = "длинный лук";
            damage = 7;
            durability = 25;
            attackType = AttackType.ranged;
        }
    }
    class Magicstaff : Weapon
    {
        public Magicstaff()
        {
            weaponName = "магический посох";
            damage = 15;
            durability = 15;
            attackType = AttackType.magic;
        }
    }
    class Acidstaff : Weapon
    {
        public Acidstaff()
        {
            weaponName = "посох яда";
            damage = 10;
            durability = 25;
            attackType = AttackType.poison;
        }
    }
}

