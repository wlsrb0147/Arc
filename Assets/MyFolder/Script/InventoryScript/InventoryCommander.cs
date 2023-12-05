using System;
using UnityEngine;

namespace MyFolder.Script.InventoryScript
{
    public class InventoryCommander : MonoBehaviour
    {
        public static InventoryCommander instance;
        
        private void Awake()
        {
            if (!instance)
                instance = this;
            else
                Destroy(gameObject);
        }
        
        public void HandleSlotTextColor(bool enter, int slotNum)
        {
            switch (slotNum)
            {
                case 0 or 2 or 3 or 4:
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

        public void BookmarkCommand(string itemName)
        {
            ItemManager.instance.ToggleItem(itemName);
            ItemManager.instance.RefreshItem();
        }


        public void ClickType(ClickType clickType, int itemSlot)
        {
            Reset();
            switch (clickType)
            {
                case InventoryScript.ClickType.LeftClick:
                    ItemManager.instance.EquippedItemSelect(itemSlot);
                    ItemManager.instance.CalculateItemValue(itemSlot);
                    break;
                case InventoryScript.ClickType.RightClick:
                case InventoryScript.ClickType.DoubleLeftClick:
                    ItemManager.instance.UnequipItemBySlot(itemSlot);
                    UI_Manager.instance.isSomethingChanged = true;
                    break;
            }
        }
        
        public void ClickType(ClickType clickType, string itemName)
        {
            Reset();
            switch (clickType)
            {
                case InventoryScript.ClickType.LeftClick:
                    ItemManager.instance.InvenSelectItemCheck(itemName);
                    ItemManager.instance.CalculateItemValue(itemName);
                    break;
                case InventoryScript.ClickType.RightClick:
                case InventoryScript.ClickType.DoubleLeftClick:
                    ItemManager.instance.ChangeItem(itemName);
                    UI_Manager.instance.isSomethingChanged = true;
                    break;
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