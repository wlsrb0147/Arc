using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro.EditorUtilities;
using Unity.Collections;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace MyFolder.Script.InventoryScript
{
    public class ItemManager : MonoBehaviour
    {
        public static ItemManager instance;
        public Image[] equipmentsImg;
        public Text[] equipmentsText;
        public List<Items> inventoryItems;
        public GameObject itemSlot;
        public Transform content1;
        public Transform content2;
        public GameObject[] check;
        public Image[] upAndDownImage;
        public Text[] upAndDownValue;
        public Sprite heart;
        
        private readonly Dictionary<CharacterType, int> _charType2Int = new();
        private readonly Dictionary<ItemType, int> _itemType2Int = new();
        public readonly Items[][] equippedItems = new Items[9][];
        private const string DefaultText = "(Beer있음)";

        
        public Toggle[] charFilter;
        public Toggle[] equFilter;
        private int _charFilter = 0b111111111;
        private int _itemFilter = 0b111111111;
        private int _tempCharFilter = 0; // 필터의 임시값
        private int _tempItemFilter = 0; // 필터의 임시값
        private static readonly int[] CharWeight;
        private static readonly int[] EquWeight;
        private readonly bool[] _charFilterBool = new bool[9]; // toggle의 처음 bool값을 저장
        private readonly bool[] _equFilterBool = new bool[9]; // toggle의 처음 bool값을 저장
        private readonly int[] _charSign = new int[9];
        private readonly int[] _itemSign = new int[9];
        private readonly int[] _savedCharSign = new int[9];
        private readonly int[] _savedEquSign = new int[9];

        public Toggle Bookmark;

        static ItemManager()
        {
            EquWeight = new[] {1,2,4,8,16,32,64,128,256};
            CharWeight = new[] {1,2,4,8,16,32,64,128,256};
        }

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
                _charSign[i] = 1;
                _itemSign[i] = 1;
            }
        }

        public void ChangeItem(string itemName)
        {
            var item = inventoryItems.Find(i => i.ItemName == itemName);
            EquipItemToCharacter(item);

            if (item.quantity is 0) inventoryItems.Remove(item);

            RefreshItem();
        }

        public void InvenSelectItemCheck(string itemName)
        {
            var type = inventoryItems.Find(i => i.ItemName == itemName).itemType;

            switch (type)
            {
                case ItemType.Helmet:
                    check[0].SetActive(true);
                    break;
                case ItemType.Weapon:
                    check[1].SetActive(true);
                    break;
                case ItemType.Clothes:
                    check[2].SetActive(true);
                    break;
                case ItemType.Shoes:
                    check[3].SetActive(true);
                    break;
                case ItemType.Brooch:
                    check[4].SetActive(true);
                    break;
                case ItemType.Shield:
                    check[5].SetActive(true);
                    break;
                case ItemType.Accessory:
                    var currentCharacter = UI_Manager.instance.GetType(Inventory.selectFaceName);;
                    var charIndex = _charType2Int[currentCharacter];
                    if (equippedItems[charIndex][6] &&equippedItems[charIndex][6].itemType != ItemType.Gloves )
                    {
                        check[7].SetActive(true);  
                    }
                    else
                    {
                        check[6].SetActive(true);
                    }
                    
                    break;
                case ItemType.Gloves:
                    check[6].SetActive(true);
                    check[7].SetActive(true);
                    break;
                case ItemType.BothHand:
                    check[1].SetActive(true);
                    check[5].SetActive(true);
                    break;
            }
        }
        
        public void UnableAllCheck()
        {
            for (int i = 0; i < 8; i++)
            {
                check[i].SetActive(false);
            }
        }
        
        public void EquippedItemSelect(int index)
        {
            var dual = DetermineEquippedSelectedItem(index);
            if (dual)
            {
                switch (index)
                {
                    case 1: // 무기
                    case 5: // 방패
                        check[1].SetActive(transform);
                        check[5].SetActive(transform);
                        break;
                    case 6: // 악세사리1
                    case 7: // 엑사사리2
                        check[6].SetActive(transform);
                        check[7].SetActive(transform);
                        break;
                }
            }
            else
            {
                check[index].SetActive(transform);
            }
        }
        
        private Items GetItemInfo(int index)
        {
            var currentCharacter = UI_Manager.instance.GetType(Inventory.selectFaceName);;
            var charIndex = _charType2Int[currentCharacter];
            Items equippedItem = null;
            var dual = DetermineEquippedSelectedItem(index);
            
            if (dual)
            {
                switch (index)
                {
                    case 1: // 무기
                    case 5: // 방패
                        equippedItem = equippedItems[charIndex][1];
                        break;
                    case 6: // 악세사리1
                    case 7: // 엑사사리2
                        equippedItem = equippedItems[charIndex][6];
                        break;
                }
            }
            else
            {
                equippedItem = equippedItems[charIndex][index];
            }

            return equippedItem;
        }

        private Items GetInvenItemInfo(string itemName)
        {
            var item = inventoryItems.Find(i => i.ItemName == itemName);
            return item;
        }

        public void ToggleItem(string itemName)
        {
            var item = inventoryItems.Find(i => i.ItemName == itemName);
            item.bookmark = !item.bookmark;
        }

        public void CalculateItemValue(int item)
        {
            Items sel = GetItemInfo(item);
            if (!sel)
            {
                return;
            }
            int[] stat = new int[6];
            stat[0] = -sel.barrier;
            stat[1] = -sel.attribute.Str;
            stat[2] = -sel.attribute.Vit;
            stat[3] = -sel.attribute.Agi;
            stat[4] = -sel.attribute.Int;
            stat[5] = -sel.attribute.Luk;
            
            for (int i = 0; i < 6; i++)
            {
                SetStat(i,stat[i]);
            }
        }

        public void CalculateItemValue(string item)
        {
            var currentCharacter = UI_Manager.instance.GetType(Inventory.selectFaceName);;
            var charIndex = _charType2Int[currentCharacter];
            Items selectedItem = GetInvenItemInfo(item);
            if (!selectedItem)
            {
                return;
            }
            Items equippedItem1 = null;
            Items equippedItem2 = null;
            ItemType type = selectedItem.itemType;
            
            switch (type)
            {
                case ItemType.BothHand:
                    if (!equippedItems[charIndex][1])
                    {
                        equippedItem1 = equippedItems[charIndex][5];
                    }
                    else
                    {
                        equippedItem1 = equippedItems[charIndex][1];
                        equippedItem2 = equippedItems[charIndex][5];
                    }
                    break;
                case ItemType.Accessory:
                    if (equippedItems[charIndex][6] && equippedItems[charIndex][6].itemType != ItemType.Gloves)
                    {
                        equippedItem1 = equippedItems[charIndex][7];
                    }
                    else
                    {
                        equippedItem1 = equippedItems[charIndex][6];
                    }
                    break;
                case ItemType.Gloves:
                    equippedItem1 = equippedItems[charIndex][6];
                    equippedItem2 = equippedItems[charIndex][7];
                    break;

                case ItemType.Helmet:
                case ItemType.Brooch:
                case ItemType.Weapon:
                case ItemType.Shield:
                case ItemType.Clothes:
                case ItemType.Shoes:
                default:
                    equippedItem1 = equippedItems[charIndex][_itemType2Int[type]];
                    break;
            }

            int[] stat = new int[6];

            if (equippedItem2)
            {
                stat[0] = selectedItem.barrier - equippedItem1.barrier - equippedItem2.barrier;
                stat[1] = selectedItem.attribute.Str - equippedItem1.attribute.Str - equippedItem2.attribute.Str;
                stat[2] = selectedItem.attribute.Vit - equippedItem1.attribute.Vit - equippedItem2.attribute.Vit;
                stat[3] = selectedItem.attribute.Agi - equippedItem1.attribute.Agi - equippedItem2.attribute.Agi;
                stat[4] = selectedItem.attribute.Int - equippedItem1.attribute.Int - equippedItem2.attribute.Int;
                stat[5] = selectedItem.attribute.Luk - equippedItem1.attribute.Luk - equippedItem2.attribute.Luk;
            }
            else if (equippedItem1)
            {
                stat[0] = selectedItem.barrier - equippedItem1.barrier;
                stat[1] = selectedItem.attribute.Str - equippedItem1.attribute.Str;
                stat[2] = selectedItem.attribute.Vit - equippedItem1.attribute.Vit;
                stat[3] = selectedItem.attribute.Agi - equippedItem1.attribute.Agi;
                stat[4] = selectedItem.attribute.Int - equippedItem1.attribute.Int;
                stat[5] = selectedItem.attribute.Luk - equippedItem1.attribute.Luk;
            }
            else
            {
                stat[0] = selectedItem.barrier;
                stat[1] = selectedItem.attribute.Str;
                stat[2] = selectedItem.attribute.Vit;
                stat[3] = selectedItem.attribute.Agi;
                stat[4] = selectedItem.attribute.Int;
                stat[5] = selectedItem.attribute.Luk;
            }
            
            

            for (int i = 0; i < 6; i++)
            {
                SetStat(i,stat[i]);
            }
        }

        public void SetStat(int i,int stat)
        {
            if (stat > 0)
            {
                upAndDownValue[i].text = stat.ToString();
            }
            else if (stat < 0)
            {
                upAndDownValue[i].text = stat.ToString();
            }
            else
            {
                upAndDownValue[i].text = "";
            }
        }
        
        public void UnequipItemBySlot(int index)
        {
            var dual = DetermineEquippedSelectedItem(index);
            switch (index)
            {
                case 0: // 헬멧
                case 1: // 무기
                case 2: // 옷
                case 3: // 신발
                case 4: // 브로치
                case 6: // 악세사리1
                    RemoveItemFromCharacter(index);
                    break;
                case 5: // 방패, 무기슬롯 장비타입 확인필요, 무기는 1번
                    RemoveItemFromCharacter(dual ? 1 : index);
                    break;
                case 7: // 악세사리2, 악세사리1 장비타입 필요, 6번 확인 필요
                    RemoveItemFromCharacter(dual ? 6 : index);
                    break;
            }
            
            if (dual)
            {
                var y = index switch
                {
                    1 => 5,
                    5 => 1,
                    6 => 7,
                    7 => 6,
                    _ => -1
                };
                ChangeColor(false,y);
            }
        }
        
        public bool IsDualEquipped(int x)
        {
            CharacterType currentCharacter = UI_Manager.instance.GetType(Inventory.selectFaceName);
            int charIndex = _charType2Int[currentCharacter];
            
            return equippedItems[charIndex][x]!=null 
                   && equippedItems[charIndex][x].itemType is ItemType.BothHand or ItemType.Gloves;
        }
        
        public void ChangeColor(bool enter, int slotNum)
        {
            equipmentsText[slotNum+8].color = enter ? Color.red : Color.black;
        }
        
        public void RefreshItem()
        {
            var content = content1.gameObject.activeInHierarchy ? content1 : content2;

            foreach (Transform child in content) Destroy(child.gameObject); // 기존 UI 삭제
            
            var iFilter = _itemFilter == 0 ? 0b111111111 : _itemFilter;
            var cFilter = _charFilter == 0 ? 0b111111111 : _charFilter;
            
            
            foreach (var items in inventoryItems)
            {
                if ((items.itemType & (ItemType)iFilter) != 0 &&
                    (items.allowedCharacterType & (CharacterType)cFilter) != 0)
                {

                    if (Bookmark.isOn && !items.bookmark) { continue; }
                    var slotPrefab = Instantiate(itemSlot, content);
                    
                    // if문, 조건 사용, Enum사용해야할듯
                    
                    
                    
                    slotPrefab.name = items.ItemName;

                    var item1 = slotPrefab.transform.GetChild(0).GetComponent<Text>();
                    var item2 = slotPrefab.transform.GetChild(1).GetComponent<Text>();
                    var item3 = slotPrefab.transform.GetChild(2).GetComponent<Image>();
                    var item4 = slotPrefab.transform.GetChild(3).GetComponent<Image>();

                    item1.text = items.ItemName;
                    item2.text = items.quantity.ToString();
                    item3.sprite = items.ItemIcon;
                    if (items.bookmark)
                    {
                        item4.sprite = heart;
                        item4.color = new Color(1, 1, 1, 1);
                    }

                    item4.name = items.ItemName;

                    // 착용가능여부 재고, if문으로 text 색깔을 정해야함
                    var currentCharacter = UI_Manager.instance.GetType(Inventory.selectFaceName);
                    if ((currentCharacter & items.allowedCharacterType) != currentCharacter)
                    {
                        item1.color = new Color(0.7f, 0.7f, 0.7f, 1f);
                        item2.color = new Color(0.7f, 0.7f, 0.7f, 1f);
                    }
                    else
                    {
                        item1.color = Color.black;
                        item2.color = Color.black;
                    }
                }
            }

            RefreshEquipment();
        }
        
        public void RefreshEquipment()
        {
            var charIndex = _charType2Int[UI_Manager.instance.GetType(Inventory.selectFaceName)];
            
            for (var i = 0; i < 8; i++)
            {
                var currentItem = equippedItems[charIndex][i];

                if (!currentItem)
                {
                    equipmentsImg[i].sprite = null;
                    equipmentsText[i].text = DefaultText;
                    
                    equipmentsImg[i+8].sprite = null;
                    equipmentsText[i + 8].text = DefaultText;
                }
                else
                {
                    equipmentsImg[i].sprite = currentItem.ItemIcon;
                    equipmentsText[i].text = currentItem.ItemName;
                    
                    equipmentsImg[i+8].sprite = currentItem.ItemIcon;
                    equipmentsText[i+8].text = currentItem.ItemName;
                }
            }

            if (equippedItems[charIndex][6]?.itemType == ItemType.Gloves)
            {
                equipmentsImg[7].sprite = equippedItems[charIndex][6].ItemIcon;
                equipmentsText[7].text = equippedItems[charIndex][6].ItemName;
                
                equipmentsImg[15].sprite = equippedItems[charIndex][6].ItemIcon;
                equipmentsText[15].text = equippedItems[charIndex][6].ItemName;
            }

            if (equippedItems[charIndex][1]?.itemType == ItemType.BothHand)
            {
                equipmentsImg[5].sprite = equippedItems[charIndex][1].ItemIcon;
                equipmentsText[5].text = equippedItems[charIndex][1].ItemName;
                
                equipmentsImg[13].sprite = equippedItems[charIndex][1].ItemIcon;
                equipmentsText[13].text = equippedItems[charIndex][1].ItemName;
            }

        }

        public void ItemFilter(int value)
        {
            _itemSign[value] *= -1;
            _tempItemFilter += EquWeight[value] * _itemSign[value];
        }
        
        public void CharFilter(int value)
        {
            _charSign[value] *= -1;
            _tempCharFilter += CharWeight[value] * _charSign[value];
        }

        public void ItemAll(bool value)
        {
            if (value)
            {
                for (int i = 0; i < 9; i++)
                {
                    equFilter[i].isOn = true;
                    _itemSign[i] = 1;
                }   
                _tempItemFilter = 0b111111111 - _itemFilter;
            }
            else
            {
                for (int i = 0; i < 9; i++)
                {
                    equFilter[i].isOn = false;
                    _itemSign[i] = -1;
                } 
                _tempItemFilter = -_itemFilter ;
            }
        }

        public void CharAll(bool value)
        {
            if (value)
            {
                for (int i = 0; i < 9; i++)
                {
                    charFilter[i].isOn = true;
                    _charSign[i] = 1;
                } 
                _tempCharFilter = 0b111111111 - _charFilter;
            }
            else
            {
                for (int i = 0; i < 9; i++)
                {
                    charFilter[i].isOn = false;
                    _charSign[i] = -1;
                }
                _tempCharFilter = -_charFilter;
            }
        }
        
        public void ApplyFilter()
        {
            _charFilter += _tempCharFilter;
            _itemFilter += _tempItemFilter;
            _tempCharFilter = 0;
            _tempItemFilter = 0;
            RefreshItem();
        }

        public void DiscardFilter()
        {
            for (int i = 0; i < 9; i++)
            {
                charFilter[i].isOn = _charFilterBool[i];
                equFilter[i].isOn = _equFilterBool[i];
                _charSign[i] = _savedCharSign[i];
                _itemSign[i] = _savedEquSign[i];
            }
            _tempCharFilter = 0;
            _tempItemFilter = 0;
        }

        public void SaveCurrentOn()
        {
            for (int i = 0; i < 9; i++)
            {
                _charFilterBool[i] = charFilter[i].isOn;
                _equFilterBool[i] = equFilter[i].isOn;
                _savedCharSign[i] = _charSign[i];
                _savedEquSign[i] = _itemSign[i];
            }
        }
        
        private void EquipItemToCharacter(Items items)
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
                        RemoveItemFromCharacter(_itemType2Int[ItemType.BothHand]);
                        equippedItems[charIndex][itemIndex] = items;
                    }
                    else
                    {
                        // 장비타입 벗고, 현재장비 장착
                        RemoveItemFromCharacter(itemIndex);
                        equippedItems[charIndex][itemIndex] = items;
                    }
                    CharacterManager.instance.ItemEquipped(charIndex,itemIndex);
                    break;

                case ItemType.BothHand:
                    bothHand = IsBothHand();
                    if (bothHand) // 현재 장착중인 아이템이 양손무기면
                    {
                        // 무기 벗고, 현재 장비 장착
                        RemoveItemFromCharacter(itemIndex);
                        equippedItems[charIndex][itemIndex] = items;
                    }
                    else
                    {
                        // 무기 방패 벗고, 현재장비 장착
                        RemoveItemFromCharacter(_itemType2Int[ItemType.Weapon]);
                        RemoveItemFromCharacter(_itemType2Int[ItemType.Shield]);
                        equippedItems[charIndex][itemIndex] = items;
                    }
                    CharacterManager.instance.ItemEquipped(charIndex,itemIndex);
                    break;

                case ItemType.Accessory:
                    glove = IsGlove();
                    if (glove)
                    {
                        // 장갑꼈으면 장갑 벗음
                        RemoveItemFromCharacter(_itemType2Int[ItemType.Gloves]);
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
                            itemIndex++;
                            RemoveItemFromCharacter(itemIndex);
                            equippedItems[charIndex][itemIndex] = items;
                        }
                    }
                    CharacterManager.instance.ItemEquipped(charIndex,itemIndex);
                    break;
                case ItemType.Gloves:
                    glove = IsGlove();
                    if (glove)
                    {
                        RemoveItemFromCharacter(itemIndex);
                        equippedItems[charIndex][itemIndex] = items;
                    }
                    else
                    {
                        RemoveItemFromCharacter(_itemType2Int[ItemType.Accessory]);
                        RemoveItemFromCharacter(_itemType2Int[ItemType.Accessory] + 1);
                        equippedItems[charIndex][itemIndex] = items;
                    }

                    CharacterManager.instance.ItemEquipped(charIndex,itemIndex);
                    break;

                // 모자, 브로치, 옷, 신발
                case ItemType.Helmet:
                case ItemType.Brooch:
                case ItemType.Clothes:
                case ItemType.Shoes:
                default:
                    RemoveItemFromCharacter(itemIndex);
                    equippedItems[charIndex][itemIndex] = items;
                    CharacterManager.instance.ItemEquipped(charIndex,itemIndex);
                    break;
            }

            items.quantity--;
            return;
            
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
        
        private void RemoveItemFromCharacter(int index)
        {
            var currentCharacter = UI_Manager.instance.GetType(Inventory.selectFaceName);
            var charIndex = _charType2Int[currentCharacter];
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
            CharacterManager.instance.ItemUnequipped(charIndex,index); // 스텟 갱신
            equippedItems[charIndex][index] = null;
        }
        
        private bool DetermineEquippedSelectedItem(int index)
        {
            var currentCharacter = UI_Manager.instance.GetType(Inventory.selectFaceName);;
            var charIndex = _charType2Int[currentCharacter];
            
            var dual = false;
            if (index is 1 or 5 or 6 or 7)
            {
                var x = index is 1 or 5 ? 1 : 6;
                Items equipItem = equippedItems[charIndex][x];
                dual = equipItem != null && equipItem.itemType is ItemType.BothHand or ItemType.Gloves;
            }
            return dual;
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
    }
}