using MyFolder.Script.InventoryScript;

namespace MyFolder.Script
{
    public class CharacterStat
    {
        public Attribute charBase = new();
        public Stat exp = new();

        public Stat hp = new();
        public int iAgi = 0; // 속도
        public int iBar = 0; // 배리어
        public int iInt = 0; // 회피력
        public int iLuk = 0; // 스피드

        public int iStr = 0; // 근력
        public int iVit = 0; // 맷집
        public Stat mp = new();

        public CharacterStat(string name, CharacterType type, int str, int intel, int vit, int agi, int luk)
        {
            Name = name;
            Type = type;
            charBase.Initialize(str, intel, vit, agi, luk);
        }

        public string Name { get; private set; }
        public CharacterType Type { get; private set; }

        public int Level { get; private set; } = 1;

        public int Atk => charBase.Str + iStr;
        public int Def => charBase.Vit + iVit;
        public int Spd => charBase.Agi + iAgi;
        public int Mag => charBase.Int + iInt;
        public int Cri => charBase.Luk + iLuk;
        public int Bar => iBar;
    }
}