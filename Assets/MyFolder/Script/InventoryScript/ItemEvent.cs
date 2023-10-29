using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MyFolder.Script.InventoryScript
{
    public class ItemEvent : MonoBehaviour,IPointerClickHandler
    {
        private int _itemType;
        private int _characterType;
        private bool _equiped = false;
        private float _time;
        private float _currentTime;

        private ItemDatabase _db;
        private ItemManager _manager;
        private Items item;
        private ItemType _type;
        
        private void Awake()
        {
            _db = GameObject.FindWithTag("DB").GetComponent<ItemDatabase>();
            _manager = ItemManager.instance;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            
            _currentTime = Time.unscaledTime;
            float deltaTime = _currentTime - _time;
            
            
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (Inventory.selectInventoryItem == item.name && deltaTime < 1f )
                {
                    _manager.DeleteItem(item);
                }
                else
                {
                    Inventory.selectInventoryItem = item.name;   
                }
            }

            if (eventData.button == PointerEventData.InputButton.Right)
            {
                _manager.DeleteItem(item);
            }
            _time = Time.unscaledTime;
        }
    }
}
