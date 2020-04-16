using System;

namespace RGR___Dungeon
{
    [Serializable]
    abstract class Armor
    {
        public int durability;
        protected AttackType resistance;
        protected AttackType weakness;
        protected string armorName;
        protected int agilityPenalty;

        public AttackType Resistance => resistance;
        public AttackType Weakness => weakness;
        public string ArmorName => armorName;
        public int AgilityPenalty => agilityPenalty;
    }
    [Serializable]
    class ClothArmor : Armor
    {
        public ClothArmor()
        {
            armorName = "Броня из ткани";
            resistance = AttackType.magic;
            weakness = AttackType.physical;
            agilityPenalty = 0;
            durability = 20 + 10 * Player.difficulty;
        }
    }
    [Serializable]
    class PlateArmor : Armor
    {
        public PlateArmor()
        {
            armorName = "Латная броня";
            resistance = AttackType.physical;
            weakness = AttackType.poison;
            agilityPenalty = 20;
            durability = 45 + 20 * Player.difficulty;
        }
    }
    [Serializable]
    class LeatherArmor : Armor
    {
        public LeatherArmor()
        {
            armorName = "Кожанная броня";
            resistance = AttackType.ranged;
            weakness = AttackType.magic;
            agilityPenalty = 10;
            durability = 25 + 15 * Player.difficulty;
        }
    }
    [Serializable]
    class ChitinArmor : Armor
    {
        public ChitinArmor()
        {
            armorName = "Хитиновая броня";
            resistance = AttackType.poison;
            weakness = AttackType.physical;
            agilityPenalty = 15;
            durability = 35 + 15 * Player.difficulty;
        }
    }
    [Serializable]
    class NoArmor : Armor
    {
        public NoArmor()
        {
            armorName = "нет брони";
            resistance = AttackType.nothing;
            weakness = AttackType.nothing;
            durability = 0;
            agilityPenalty = 0;
        }
    }
}
