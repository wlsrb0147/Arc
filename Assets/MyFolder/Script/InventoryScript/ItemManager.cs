using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyFolder.Script.InventoryScript
{
    public class ItemManager : MonoBehaviour
    {
        public static ItemManager instance;
        public GameObject[] EquipmentsUI;
        public List<Items> inventoryItems;
        public GameObject itemSlot;
        public Transform content1;
        public Transform content2;

        private readonly Dictionary<CharacterType, int> _charType2Int = new();
        private readonly Dictionary<ItemType, int> _itemType2Int = new();
        public readonly Items[][] equippedItems = new Items[9][];

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            _charType2Int.Add(CharacterType.Ai, 0);
            _charType2Int.Add(CharacterType.Carrot, 1);
            _charType2Int.Add(CharacterType.Celline, 2);
            _charType2Int.Add(CharacterType.Eluard, 3);
            _charType2Int.Add(CharacterType.Kreutzer, 4);
            _charType2Int.Add(CharacterType.Maria, 5);
            _charType2Int.Add(CharacterType.Peach, 6);
            _charType2Int.Add(CharacterType.Sizz, 7);
            _charType2Int.Add(CharacterType.Tenzi, 8);

            _itemType2Int.Add(ItemType.Helmet, 0);
            _itemType2Int.Add(ItemType.Weapon, 1);
            _itemType2Int.Add(ItemType.Clothes, 2);
            _itemType2Int.Add(ItemType.Shoes, 3);
            _itemType2Int.Add(ItemType.Brooch, 4);
            _itemType2Int.Add(ItemType.Shield, 5);
            _itemType2Int.Add(ItemType.Accessory, 6);
            _itemType2Int.Add(ItemType.BothHand, 1);
            _itemType2Int.Add(ItemType.Gloves, 6);


            for (var i = 0; i < 9; i++)
            {
                equippedItems[i] = new Items[8];
            }
        }

        public void ChangeItem(string itemName)
        {
            var item = inventoryItems.Find(i => i.ItemName == itemName);
            InventoryItemEquip(item);

            if (item.quantity is 0) inventoryItems.Remove(item);

            RefreshItem();
        }

        private void InventoryItemEquip(Items items)
        {
            var currentCharacter = UI_Manager.instance.GetType(Inventory.selectFaceName);
            var allowedType = items.allowedCharacterType;
            var itemType = items.itemType;

            if ((currentCharacter & allowedType) != currentCharacter) return;

            var charIndex = _charType2Int[currentCharacter];
            var itemIndex = _itemType2Int[itemType];

            bool bothHand, glove;

            switch (itemType)
            {
                case ItemType.Weapon:
                case ItemType.Shield:
                    bothHand = IsBothHand();
                    if (bothHand) // 현재 장착중인 아이템이 양손무기면
                    {
                        // 양손무기 벗고, 현재 장비 장착
                        UnequipItem(_itemType2Int[ItemType.BothHand]);
                        equippedItems[charIndex][itemIndex] = items;
                    }
                    else
                    {
                        // 장비타입 벗고, 현재장비 장착
                        UnequipItem(itemIndex);
                        equippedItems[charIndex][itemIndex] = items;
                    }

                    break;

                case ItemType.BothHand:
                    bothHand = IsBothHand();
                    if (bothHand) // 현재 장착중인 아이템이 양손무기면
                    {
                        // 무기 벗고, 현재 장비 장착
                        UnequipItem(itemIndex);
                        equippedItems[charIndex][itemIndex] = items;
                    }
                    else
                    {
                        // 무기 방패 벗고, 현재장비 장착
                        UnequipItem(_itemType2Int[ItemType.Weapon]);
                        UnequipItem(_itemType2Int[ItemType.Shield]);
                        equippedItems[charIndex][itemIndex] = items;
                    }

                    break;

                case ItemType.Accessory:
                    glove = IsGlove();
                    if (glove)
                    {
                        // 장갑꼈으면 장갑 벗음
                        UnequipItem(_itemType2Int[ItemType.Gloves]);
                        equippedItems[charIndex][itemIndex] = items;
                    }
                    else
                    {
                        if (equippedItems[charIndex][itemIndex] == null)
                        {
                            equippedItems[charIndex][itemIndex] = items;
                        }
                        else
                        {
                            UnequipItem(itemIndex + 1);
                            equippedItems[charIndex][itemIndex + 1] = items;
                        }
                    }

                    break;
                case ItemType.Gloves:
                    glove = IsGlove();
                    if (glove)
                    {
                        UnequipItem(itemIndex);
                        equippedItems[charIndex][itemIndex] = items;
                    }
                    else
                    {
                        UnequipItem(_itemType2Int[ItemType.Accessory]);
                        UnequipItem(_itemType2Int[ItemType.Accessory] + 1);
                        equippedItems[charIndex][itemIndex] = items;
                    }

                    break;

                // 모자, 브로치, 옷, 신발
                case ItemType.Helmet:
                case ItemType.Brooch:
                case ItemType.Clothes:
                case ItemType.Shoes:
                default:
                    UnequipItem(itemIndex);
                    equippedItems[charIndex][itemIndex] = items;
                    break;
            }

            items.quantity--;
            return;

            void DebugItem()
            {
                if (equippedItems[charIndex][itemIndex] != null)
                    Debug.Log(equippedItems[charIndex][itemIndex].ItemName);
            }

            void UnequipItem(int index)
            {
                var equipItem = equippedItems[charIndex][index];

                if (equipItem is null) return;

                var item = inventoryItems.Find(i => i == equipItem);

                if (item == null)
                {
                    inventoryItems.Add(equipItem);
                    equipItem.quantity = 1;
                }
                else
                {
                    equipItem.quantity++;
                }

                equippedItems[charIndex][index] = null;
            }

            bool IsBothHand()
            {
                var i = equippedItems[charIndex][_itemType2Int[ItemType.Weapon]];
                if (i == null) return false;
                return i.itemType is ItemType.BothHand;
            }

            bool IsGlove()
            {
                var i = equippedItems[charIndex][_itemType2Int[ItemType.Accessory]];
                if (i == null) return false;
                return i.itemType is ItemType.Gloves;
            }
        }

        public void CreateItem()
        {
            var x = Random.Range(0, ItemDatabase.instance.Allitems.Count);
            var item = ItemDatabase.instance.Allitems[x];

            if (inventoryItems.Find(i => i == item))
            {
                item.quantity++;
            }
            else
            {
                inventoryItems.Add(item);
                item.quantity = 1;
            }

            RefreshItem();
        }

        public void DeleteItem()
        {
            if (inventoryItems.Count > 0)
            {
                var x = Random.Range(0, inventoryItems.Count);
                inventoryItems.Remove(inventoryItems[x]);
            }

            RefreshItem();
        }

        public void RefreshItem()
        {
            var content = content1.gameObject.activeInHierarchy ? content1 : content2;

            foreach (Transform child in content) Destroy(child.gameObject); // 기존 UI 삭제

            foreach (var items in inventoryItems)
            {
                var slotPrefab = Instantiate(itemSlot, content);
                slotPrefab.name = items.ItemName;
                slotPrefab.transform.Find("Item_Name").GetComponent<Text>().text = items.ItemName;
                slotPrefab.transform.Find("Image").GetComponent<Image>().sprite = items.ItemIcon;
                slotPrefab.transform.Find("Item_Have").GetComponent<Text>().text = items.quantity.ToString();
            }

            var charIndex = _charType2Int[UI_Manager.instance.GetType(Inventory.selectFaceName)];

            for (var i = 0; i < EquipmentsUI.Length; i++)
            {
                var currentItem = equippedItems[charIndex][i];

                if (currentItem == null)
                    EquipmentsUI[i].transform.Find("Spr").GetComponent<Image>().sprite = null;
                else
                    EquipmentsUI[i].transform.Find("Spr").GetComponent<Image>().sprite = currentItem.ItemIcon;
            }

            if (equippedItems[charIndex][6]?.itemType == ItemType.Gloves)
                EquipmentsUI[7].transform.Find("Spr").GetComponent<Image>().sprite =
                    equippedItems[charIndex][6].ItemIcon;
            if (equippedItems[charIndex][1]?.itemType == ItemType.BothHand)
                EquipmentsUI[5].transform.Find("Spr").GetComponent<Image>().sprite =
                    equippedItems[charIndex][1].ItemIcon;
        }
    }
}