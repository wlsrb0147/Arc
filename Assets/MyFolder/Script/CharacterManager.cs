using System;
using System.Collections.Generic;
using MyFolder.Script.InventoryScript;
using UnityEngine;

namespace MyFolder.Script
{
    public class CharacterManager : MonoBehaviour
    {
        public static CharacterManager instance;
        public List<CharacterStat> info = new();

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);
            info.Add(new CharacterStat("Ai", CharacterType.Ai, 10, 11, 12, 13, 14)
            {
                hp = new Stat { Value = 51, MaxValue = 51 },
                mp = new Stat { Value = 51, MaxValue = 51 },
                exp = new Stat { Value = 10, MaxValue = 150 }
            });

            info.Add(new CharacterStat("Carrot", CharacterType.Carrot, 20, 21, 22, 23, 24)
            {
                hp = new Stat { Value = 52, MaxValue = 52 },
                mp = new Stat { Value = 52, MaxValue = 52 },
                exp = new Stat { Value = 20, MaxValue = 150 }
            });

            info.Add(new CharacterStat("Maria", CharacterType.Maria, 30, 31, 32, 33, 34)
            {
                hp = new Stat { Value = 50, MaxValue = 50 },
                mp = new Stat { Value = 50, MaxValue = 50 },
                exp = new Stat { Value = 30, MaxValue = 150 }
            });

            info.Add(new CharacterStat("Peach", CharacterType.Peach, 40, 41, 42, 43, 44)
            {
                hp = new Stat { Value = 50, MaxValue = 50 },
                mp = new Stat { Value = 50, MaxValue = 50 },
                exp = new Stat { Value = 40, MaxValue = 150 }
            });

            info.Add(new CharacterStat("Celline", CharacterType.Celline, 50, 51, 52, 53, 54)
            {
                hp = new Stat { Value = 50, MaxValue = 50 },
                mp = new Stat { Value = 50, MaxValue = 50 },
                exp = new Stat { Value = 50, MaxValue = 150 }
            });

            info.Add(new CharacterStat("Sizz", CharacterType.Sizz, 60, 61, 62, 63, 64)
            {
                hp = new Stat { Value = 50, MaxValue = 50 },
                mp = new Stat { Value = 50, MaxValue = 50 },
                exp = new Stat { Value = 60, MaxValue = 150 }
            });

            info.Add(new CharacterStat("Eluard", CharacterType.Eluard, 70, 71, 72, 73, 74)
            {
                hp = new Stat { Value = 50, MaxValue = 50 },
                mp = new Stat { Value = 50, MaxValue = 50 },
                exp = new Stat { Value = 70, MaxValue = 150 }
            });

            info.Add(new CharacterStat("Kreutzer", CharacterType.Kreutzer, 80, 81, 82, 83, 84)
            {
                hp = new Stat { Value = 50, MaxValue = 50 },
                mp = new Stat { Value = 50, MaxValue = 50 },
                exp = new Stat { Value = 80, MaxValue = 150 }
            });

            info.Add(new CharacterStat("Tenzi", CharacterType.Tenzi, 90, 91, 92, 93, 94)
            {
                hp = new Stat { Value = 50, MaxValue = 50 },
                mp = new Stat { Value = 50, MaxValue = 50 },
                exp = new Stat { Value = 90, MaxValue = 150 }
            });
        }

        public void ItemEquipped(int charNum,int equipmentNum)
        {
            Items item = ItemManager.instance.equippedItems[charNum][equipmentNum]; 
            info[charNum].iAgi += item.attribute.Agi;
            info[charNum].iInt += item.attribute.Int;
            info[charNum].iLuk += item.attribute.Luk;
            info[charNum].iStr += item.attribute.Str;
            info[charNum].iVit += item.attribute.Vit;
            info[charNum].iBar += item.barrier;
        }

        public void ItemUnequipped(int charNum, int equipmentNum)
        {
            Items item = ItemManager.instance.equippedItems[charNum][equipmentNum]; 
            info[charNum].iAgi -= item.attribute.Agi;
            info[charNum].iInt -= item.attribute.Int;
            info[charNum].iLuk -= item.attribute.Luk;
            info[charNum].iStr -= item.attribute.Str;
            info[charNum].iVit -= item.attribute.Vit;
            info[charNum].iBar -= item.barrier;
        }
    }
}