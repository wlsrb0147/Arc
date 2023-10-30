using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyFolder.Script.InventoryScript
{
    public class ItemList : MonoBehaviour
    {
        public List<Items> inventoryItems;
        public Items[][] equippedItems = new Items[9][];
        public static ItemList instance;

        private Dictionary<CharacterType, int> _charType2Int;
        private Dictionary<>
        
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            _charType2Int.Add(CharacterType.Ai,0);
            _charType2Int.Add(CharacterType.Carrot,1);
            _charType2Int.Add(CharacterType.Celline,2);
            _charType2Int.Add(CharacterType.Eluard,3);
            _charType2Int.Add(CharacterType.Kreutzer,4);
            _charType2Int.Add(CharacterType.Maria,5);
            _charType2Int.Add(CharacterType.Peach,6);
            _charType2Int.Add(CharacterType.Sizz,7);
            _charType2Int.Add(CharacterType.Tenzi,8);

            for (int i = 0; i < 9; i++)
            { 
                equippedItems[i] = new Items[8];
            }
            
            
        }

        public void ItemEquip(CharacterType type,)
        {
            
        }
        
    }
}
