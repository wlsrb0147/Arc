using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace MyFolder.Script.InventoryScript
{
    public class ItemManager : MonoBehaviour
    {
        public List<Items> inventoryItems;
        public Items[][] equippedItems = new Items[9][];
        public GameObject itemSlot;
        public Transform content1;
        public Transform content2;
        
        public static ItemManager instance;

        private readonly Dictionary<CharacterType, int> _charType2Int = new();
        private readonly Dictionary<ItemType, int> _itemType2Int = new();
        
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
            
            _itemType2Int.Add(ItemType.Helmet,0);
            _itemType2Int.Add(ItemType.Weapon,1);
            _itemType2Int.Add(ItemType.Clothes,2);
            _itemType2Int.Add(ItemType.Shoes,3);
            _itemType2Int.Add(ItemType.Brooch,4);
            _itemType2Int.Add(ItemType.Shield,5);
            _itemType2Int.Add(ItemType.Accessory,6);
            _itemType2Int.Add(ItemType.BothHand,-1);
            _itemType2Int.Add(ItemType.Gloves,-1);
            

            for (int i = 0; i < 9; i++)
            { 
                equippedItems[i] = new Items[8];
            }
            
            
        }
        public void ChangeItem(string itemName,bool equipped)
        {
            // 아이템 타입이 캐릭과 맞을때만 실행해야함
            var item = inventoryItems.Find(i => i.ItemName == itemName);
            
            if (equipped)
            {
                // 장착 캐릭에서 
            }
            else
            {
                InventoryItemEquip(item);
                item.quantity--; 
            }

            if (item.quantity == 0)
            {
                inventoryItems.Remove(item);
            }
        }
        
        public void RefreshItem()
        {
            foreach (Transform child in content1)
            {
                Destroy(child.gameObject); // 기존 UI 삭제
            }
            foreach (Transform child in content2)
            {
                Destroy(child.gameObject); // 기존 UI 삭제
            }
            
            foreach (var items in inventoryItems)
            {
                GameObject slotPrefab1 = Instantiate(itemSlot, content1);
                slotPrefab1.name = items.ItemName;
                slotPrefab1.transform.Find("Item_Name").GetComponent<Text>().text = items.ItemName;
                slotPrefab1.transform.Find("Image").GetComponent<Image>().sprite = items.ItemIcon;
                slotPrefab1.transform.Find("Item_Have").GetComponent<Text>().text = items.quantity.ToString();

                GameObject slotPrefab2 = Instantiate(itemSlot, content2);
                slotPrefab2.name = items.ItemName;
                slotPrefab2.transform.Find("Item_Name").GetComponent<Text>().text = items.ItemName;
                slotPrefab2.transform.Find("Image").GetComponent<Image>().sprite = items.ItemIcon;
                slotPrefab2.transform.Find("Item_Have").GetComponent<Text>().text = items.quantity.ToString();
            }
        }
        
        
        public void CreateItem()
        {
            var x = Random.Range(0, ItemDatabase.instance.Allitems.Count);
            Items item = ItemDatabase.instance.Allitems[x];

            if (inventoryItems.Find(i => i == item))
            {
                item.quantity++;
            }
            else
            {
                inventoryItems.Add(item);
                item.quantity = 1;
            }
        }
        public void DeleteItem()
        {
            if (inventoryItems.Count > 0)
            {
                int x = Random.Range(0, inventoryItems.Count);
                inventoryItems.Remove(inventoryItems[x]);
            }
        }
        
        
        // 아이템을 장착하는 로직 실행중
        // 장착하려는 아이템의 타입 필요함
        // 장착하려는 아이템의 캐릭터 정보 필요함
        // 전부 아이템에 존재함
        // 인벤토리에 selectedFaceName 존재함
        
        private void InventoryItemEquip(Items items)
        {
            var allowedType = items.allowedCharacterType;
            var itemType = items.itemType;
            var currentCharacter = UI_Manager.instance.GetType(Inventory.selectFaceName);

            if ((currentCharacter & allowedType) != currentCharacter)
            {
                return;
            }
            
            var charIndex = _charType2Int[currentCharacter];
            var itemIndex = _itemType2Int[itemType];

            switch (itemType)
            {
                case ItemType.BothHand:
                    CheckAndLog(_itemType2Int[ItemType.Weapon]);
                    CheckAndLog(_itemType2Int[ItemType.Shield]);
                    
                    equippedItems[charIndex][_itemType2Int[ItemType.Weapon]] = items;
                    equippedItems[charIndex][_itemType2Int[ItemType.Shield]] = items;
                    break;
                
                case ItemType.Gloves:
                    CheckAndLog(6);
                    CheckAndLog(7);
                    
                    equippedItems[charIndex][6] = items;
                    equippedItems[charIndex][7] = items;
                    break;
                
                case ItemType.Accessory:
                    if (equippedItems[charIndex][6] == null)
                    {
                        equippedItems[charIndex][6] = items; 
                    }
                    else
                    {
                        CheckAndLog(7);
                        equippedItems[charIndex][7] = items; 
                    }
                    break;

                case ItemType.Helmet:
                case ItemType.Brooch:
                case ItemType.Weapon:
                case ItemType.Shield:
                case ItemType.Clothes:
                case ItemType.Shoes:
                default:
                    CheckAndLog(itemIndex);
                    equippedItems[charIndex][itemIndex] = items;
                    break;
            }
            
            return;

            // 양손무기, 장갑 처리 안해줌
            // 양손무기, 양손장갑을 벗을 때 두개 증가하는 문제 발생
            // 양손무기, 양손장갑을 낀 상태에서 장비 착용시 한쪽만 벗는 문제 발생
            void CheckAndLog(int index)
            {
                if (equippedItems[charIndex][index] != null)
                {
                    var item = inventoryItems.Find(i => i == equippedItems[charIndex][index]);
                    if (item != null)
                    {
                        item.quantity++;
                    }
                    else
                    {
                        inventoryItems.Add(equippedItems[charIndex][index]);
                        equippedItems[charIndex][index].quantity = 1;
                    }
                }
            }
        }
    }
}
