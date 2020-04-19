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
            damage = random.Next(6 + 3 * Player.difficulty, 12 + 6 * Player.difficulty);
            durability = random.Next(5, 40);
            switch (random.Next(5))
            {
                case 0: 
                    weaponName = "короткий меч"; 
                    attackType = physical;
                    damage -= 2 * Player.difficulty;
                    break;
                case 1: 
                    weaponName = "меч"; 
                    attackType = physical; 
                    break;
                case 2: 
                    weaponName = "длинный лук"; 
                    attackType = ranged; 
                    break;
                case 3: 
                    weaponName = "магический посох"; 
                    attackType = magic;
                    damage += 3 * Player.difficulty;
                    break;
                case 4: 
                    weaponName = "посох яда"; 
                    attackType = poison; 
                    damage += 5 * Player.difficulty; 
                    break;
            } 
        }
    }
}

