using System;
using static RGR___Dungeon.AttackType;

namespace RGR___Dungeon
{
    [Serializable]
    abstract class Weapon : Entity
    {
        protected int damage;
        protected int durability;
        protected AttackType attackType;
        protected string weaponName;

        public int Damage { get => damage;}
        public int Durability { get => durability; set => durability = value; }
        internal AttackType AttackType { get => attackType; }
        public string WeaponName { get => weaponName; }
    }
    //[Serializable]
    //class Shortsword : Weapon
    //{ 
    //    public Shortsword()
    //    {
    //        weaponName = "короткий меч";
    //        damage = 5 + 2 * Player.difficulty;
    //        durability = 25;
    //        attackType = physical;
    //    }
    //}
    //[Serializable]
    //class Sword : Weapon
    //{
    //    public Sword()
    //    {
    //        weaponName = "меч";
    //        damage = 10 + 4 * Player.difficulty;
    //        durability = 35;
    //        attackType = physical;
    //    }
    //}
    //[Serializable]
    //class Longbow : Weapon
    //{
    //    public Longbow()
    //    {
    //        weaponName = "длинный лук";
    //        damage = 7 + 3 * Player.difficulty;
    //        durability = 25;
    //        attackType = ranged;
    //    }
    //}
    //[Serializable]
    //class Magicstaff : Weapon
    //{
    //    public Magicstaff()
    //    {
    //        weaponName = "магический посох";
    //        damage = 15 + 6 * Player.difficulty;
    //        durability = 15;
    //        attackType = magic;
    //    }
    //}
    //[Serializable]
    //class Acidstaff : Weapon
    //{
    //    public Acidstaff()
    //    {
    //        weaponName = "посох яда";
    //        damage = 10 + 4 * Player.difficulty;
    //        durability = 25;
    //        attackType = poison;
    //    }
    //}
    [Serializable]
    class NoWeapon : Weapon
    {
        public NoWeapon()
        {
            weaponName = "Нет оружия";
            damage = 0;
            durability = 0;
            attackType = nothing;
        }
    }
    [Serializable]
    class RandomWeapon : Weapon
    {
        public RandomWeapon()
        {
            switch(Program.rnd.Next(5))
            {
                case 0: weaponName = "короткий меч"; attackType = physical; break;
                case 1: weaponName = "меч"; attackType = physical; break;
                case 2: weaponName = "длинный лук"; attackType = ranged; break;
                case 3: weaponName = "магический посох"; attackType = magic; break;
                case 4: weaponName = "посох яда"; attackType = poison; break;
            }
            damage = Program.rnd.Next(6 + 3 * Player.difficulty, 12 + 6 * Player.difficulty);
            durability = Program.rnd.Next(10, 30);
        }
    }
}

