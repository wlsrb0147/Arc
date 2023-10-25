namespace MyFolder.Script
{
    public class CharacterStat
    {
        public CharacterStat(string name, CharacterType type,int str, int intel, int vit, int agi, int luk)
        {
            Name = name;
            Type = type;
            charBase.Initialize(str, intel, vit, agi, luk);
        }
        
        public string Name { get; private set; }
        public CharacterType Type { get; private set; }
        public Attribute charBase = new();
        
        public Stat hp = new ();
        public Stat mp = new ();
        public Stat exp = new ();
    
        public int Level { get; private set; } = 1;
    
        public int iStr = 0; // 근력
        public int iAgi = 0; // 속도
        public int iVit = 0; // 맷집
        public int iInt = 0; // 회피력
        public int iLuk = 0; // 스피드
        public int iBar = 0; // 배리어
        
        public int Atk => charBase.Str + iStr;
        public int Def => charBase.Vit + iVit;
        public int Spd => charBase.Agi + iAgi;
        public int Mag => charBase.Int + iInt;
        public int Cri => charBase.Luk + iLuk;
        public int Bar => iBar;
    }

    public class ItemSlot
    {
        public Items EquippedItems { get; private set; } 
        public ItemType AllowedItemType { get; private set; }
        public CharacterType characterType;

        public ItemSlot(CharacterType characterType)
        {
            this.characterType = characterType;
        }
        
        
        public bool Equip(Items items)
        {
            if (characterType == (items.allowedCharacterType & characterType))
            {
                if (EquippedItems == null) 
                {
                    EquippedItems = items;   
                }
                else
                {
                    
                }
                return true;
            }
            else return false;
        }
    }
}