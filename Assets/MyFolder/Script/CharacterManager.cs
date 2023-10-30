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
            info.Add(new CharacterStat("Ai", CharacterType.Ai, 10, 10, 10, 10, 10)
            {
                hp = new Stat() { Value = 50, MaxValue = 50 },
                mp = new Stat() { Value = 50, MaxValue = 50 },
                exp = new Stat() { Value = 10, MaxValue = 150 }
            });
        
            info.Add(new CharacterStat("Carrot", CharacterType.Carrot, 10, 10, 10, 10, 10)
            {
                hp = new Stat() { Value = 50, MaxValue = 50 },
                mp = new Stat() { Value = 50, MaxValue = 50 },
                exp = new Stat() { Value = 20, MaxValue = 150 }
            });
        
            info.Add(new CharacterStat("Maria", CharacterType.Maria, 10, 10, 10, 10, 10)
            {
                hp = new Stat() { Value = 50, MaxValue = 50 },
                mp = new Stat() { Value = 50, MaxValue = 50 },
                exp = new Stat() { Value = 30, MaxValue = 150 }
            });
        
            info.Add(new CharacterStat("Peach", CharacterType.Peach, 10, 10, 10, 10, 10)
            {
                hp = new Stat() { Value = 50, MaxValue = 50 },
                mp = new Stat() { Value = 50, MaxValue = 50 },
                exp = new Stat() { Value = 40, MaxValue = 150 }
            });
        
            info.Add(new CharacterStat("Celline", CharacterType.Celline, 10, 10, 10, 10, 10)
            {
                hp = new Stat() { Value = 50, MaxValue = 50 },
                mp = new Stat() { Value = 50, MaxValue = 50 },
                exp = new Stat() { Value = 50, MaxValue = 150 }
            });
        
            info.Add(new CharacterStat("Sizz", CharacterType.Sizz, 10, 10, 10, 10, 10)
            {
                hp = new Stat() { Value = 50, MaxValue = 50 },
                mp = new Stat() { Value = 50, MaxValue = 50 },
                exp = new Stat() { Value = 60, MaxValue = 150 }
            });
        
            info.Add(new CharacterStat("Eluard", CharacterType.Eluard, 10, 10, 10, 10, 10)
            {
                hp = new Stat() { Value = 50, MaxValue = 50 },
                mp = new Stat() { Value = 50, MaxValue = 50 },
                exp = new Stat() { Value = 70, MaxValue = 150 }
            });
        
            info.Add(new CharacterStat("Kreutzer", CharacterType.Kreutzer, 10, 10, 10, 10, 10)
            {
                hp = new Stat() { Value = 50, MaxValue = 50 },
                mp = new Stat() { Value = 50, MaxValue = 50 },
                exp = new Stat() { Value = 80, MaxValue = 150 }
            });

            info.Add(new CharacterStat("Tenzi", CharacterType.Tenzi, 10, 10, 10, 10, 10)
            {
                hp = new Stat() { Value = 50, MaxValue = 50 },
                mp = new Stat() { Value = 50, MaxValue = 50 },
                exp = new Stat() { Value = 90, MaxValue = 150 }
            });
        }
    }
}
