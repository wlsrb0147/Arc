using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;


namespace MyFolder.Script.InventoryScript
{
    public class ItemManager : MonoBehaviour
    {
        public static ItemManager instance;
        public GameObject itemSlotPrefab; // 아이템 슬롯 프리팹
        public Transform content; // 스크롤 뷰의 Content
        public Transform content2; // 스크롤 뷰의 Content
        public List<Items> inventoryItemsList = new();
        public GameObject[] ItemSlot;
        public Items[] equipedItem = new Items[8];
        public ItemslotStr[] slots = new ItemslotStr[8];
        public Dictionary<ItemType, Items> type2Equip = new();
        public Dictionary<ItemType, ItemslotStr> type2Slot= new();
        
        public struct ItemslotStr
        {
            public GameObject Slot1 { get; set; } 
            public GameObject Slot2 { get; set; }
        }
        
        private void Start()
        {
            for (int i = 0; i < slots.Length; i++)
            {
                slots[i] = new ItemslotStr
                {
                    Slot1 = ItemSlot[i * 2],
                    Slot2 = ItemSlot[i * 2 + 1]
                };
            }
            
        }

        public void Additem(Items items)
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
            }
            
            if (inventoryItemsList.Contains(items))
            {
                items.quantity++;
            }
            else
            {
                inventoryItemsList.Add(items);
                items.quantity = 1;
            }
                
            UpdateInventoryUI(content); // 인벤토리 UI 업데이트
            UpdateInventoryUI(content2);
        }

        public void DeleteItem(Items items)
        {
            if (inventoryItemsList.Contains(items))
            {
                items.quantity--;
            }
            
            if (items.quantity <= 0)
            {
                inventoryItemsList.Remove(items);
            }
                
            UpdateInventoryUI(content); // 인벤토리 UI 업데이트
            UpdateInventoryUI(content2);
        }

        
        private void UpdateInventoryUI(Transform contents) // 이건 획득순 정렬
        {
            foreach (Transform child in contents)
            {
                Destroy(child.gameObject); // 기존 UI 삭제
            }

            foreach (Items item in inventoryItemsList)
            {
                GameObject itemSlot = Instantiate(itemSlotPrefab, contents);
                itemSlot.name = item.ItemName;
                itemSlot.transform.Find("Item_Name").GetComponent<Text>().text = item.ItemName;
                itemSlot.transform.Find("Image").GetComponent<Image>().sprite = item.ItemIcon;
                itemSlot.transform.Find("Item_Have").GetComponent<Text>().text = item.quantity.ToString();
            }
        }

        public void GetType(ItemType type)
        {
            switch (type)
            {
                case ItemType.Helmet:
                    break;
                case ItemType.Weapon:
                    break;
                case ItemType.Clothes:
                    break;
                case ItemType.Shoes:
                    break;
                case ItemType.Brooch:
                    break;
                case ItemType.Shild:
                    break;
                case ItemType.BothHand:
                    break;
                case ItemType.Gloves:
                    break;
            }
        }
    }
}