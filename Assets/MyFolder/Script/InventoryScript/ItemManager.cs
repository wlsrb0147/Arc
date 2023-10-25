using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace MyFolder.Script.InventoryScript
{
    public class ItemManager : MonoBehaviour
    { 
        public GameObject itemSlotPrefab; // 아이템 슬롯 프리팹
        public Transform content; // 스크롤 뷰의 Content
        public Transform content2; // 스크롤 뷰의 Content
        public List<Items> inventoryItemsList = new();
        
        public void Additem(Items items)
        {
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
    }
}