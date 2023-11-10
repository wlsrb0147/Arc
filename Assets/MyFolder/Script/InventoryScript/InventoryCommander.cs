using System;
using UnityEngine;

namespace MyFolder.Script.InventoryScript
{
    public class InventoryCommander : MonoBehaviour
    {
        public static InventoryCommander instance;
        public int ItemSlot { get; set; }
        public string SelectedInvenItemName { get; set; }

        
        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);
        }
        
        public void HandleSlotTextColor(bool enter, int slotNum)
        {
            switch (slotNum)
            {
                case 0:
                case 2:
                case 3:
                case 4: 
                    ItemManager.instance.ChangeColor(enter,slotNum);
                    break;
                case 1:
                case 5:
                    if (ItemManager.instance.IsDualEquipped(1))
                    {
                        ItemManager.instance.ChangeColor(enter,1);
                        ItemManager.instance.ChangeColor(enter,5);
                    }
                    else
                        ItemManager.instance.ChangeColor(enter,slotNum);
                    break;
                case 6:
                case 7:
                    if (ItemManager.instance.IsDualEquipped(6))
                    {
                        ItemManager.instance.ChangeColor(enter,6);
                        ItemManager.instance.ChangeColor(enter,7);
                    }
                    else
                        ItemManager.instance.ChangeColor(enter,slotNum);
                    break;
            }
        }

        private void Reset()
        {
            ItemManager.instance.UnableAllCheck();
            for (int i = 0; i < 6; i++)
            {
                ItemManager.instance.SetStat(i,0);    
            }
        }

        public void FaceClick()
        {
            Reset();
        }

        public void FilterPanelEnabled()
        {
            ItemManager.instance.SaveCurrentOn();
        }
        
        public void BookmarkCommand()
        {
            ItemManager.instance.ToggleItem(SelectedInvenItemName);
            ItemManager.instance.RefreshItem();
        }
        
        public void ClickType(ClickType clickType,bool isEquipped)
        {
            Reset();
            
            if (isEquipped) // true면 장착 아이템 클릭
            {
                switch (clickType)
                {
                    case InventoryScript.ClickType.LeftClick:
                        ItemManager.instance.EquippedItemSelect(ItemSlot);
                        ItemManager.instance.CalculateItemValue(ItemSlot);
                        break;
                    case InventoryScript.ClickType.RightClick:
                    case InventoryScript.ClickType.DoubleLeftClick:
                        ItemManager.instance.UnequipItemBySlot(ItemSlot);
                        UI_Manager.instance.isSomethingChanged = true;
                        break;
                }
            }
            else
            {
                switch (clickType)
                {
                    case InventoryScript.ClickType.LeftClick:
                        ItemManager.instance.InvenSelectItemCheck(SelectedInvenItemName);
                        ItemManager.instance.CalculateItemValue(SelectedInvenItemName);
                        break;
                    case InventoryScript.ClickType.RightClick:
                    case InventoryScript.ClickType.DoubleLeftClick:
                        ItemManager.instance.ChangeItem(SelectedInvenItemName);
                        UI_Manager.instance.isSomethingChanged = true;
                        break;
                }
            }
        }

        public static void CreateItemCommand()
        {
            ItemManager.instance.CreateItem();
        }

        public static void DeleteItemCommand()
        {
            ItemManager.instance.DeleteItem();
        }

        public void DiscardFilter()
        {
            ItemManager.instance.DiscardFilter();
            UI_Manager.instance.FilterOff();
        }

        public void ApplyFilter()
        {
            ItemManager.instance.ApplyFilter();
            UI_Manager.instance.FilterOff();
        }

        public void SetItemType(int type)
        {
            ItemManager.instance.ItemFilter(type);
        }

        public void SetCharType(int type)
        {
            ItemManager.instance.CharFilter(type);
        }
        public void ActiveFilterTab()
        {
            UI_Manager.instance.FilterOn();
        }

        public void AllItemCheck(bool Value)
        {
          ItemManager.instance.ItemAll(Value);   
        }
        
        public void AllCharCheck(bool Value)
        {
            ItemManager.instance.CharAll(Value);
        }
        
    }
}