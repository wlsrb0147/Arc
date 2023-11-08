using System;

namespace MyFolder.Script.InventoryScript
{
    [Flags]
    public enum CharacterType
    {
        Ai = 1 << 0,
        Carrot = 1 << 1,
        Celline = 1 << 2,
        Eluard = 1 << 3,
        Kreutzer = 1 << 4,
        Maria = 1 << 5,
        Peach = 1 << 6,
        Sizz = 1 << 7,
        Tenzi = 1 << 8,

        Magician = Ai | Sizz,
        Assassin = Carrot | Peach,
        SpearMan = Tenzi,
        SwordMan = Maria | Kreutzer,
        Archer = Celline,
        Whip = Eluard
    }


    public enum ItemType
    {
        Weapon = 1 << 0,
        BothHand = 1 << 1,
        Shield = 1 << 2,
        Helmet = 1 << 3,
        Clothes = 1 << 4,
        Shoes = 1 << 5,
        Brooch = 1 << 6,
        Accessory = 1 << 7,
        Gloves = 1 << 8,
    }

    public enum ClickType
    {
        LeftClick,
        DoubleLeftClick,
        RightClick
    }
}