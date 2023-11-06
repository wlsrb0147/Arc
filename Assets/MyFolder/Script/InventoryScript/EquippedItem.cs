using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace MyFolder.Script.InventoryScript
{
    public class EquippedItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        private float _timer;
        private int _x;

        private void OnEnable()
        {
            transform.GetChild(3).gameObject.SetActive(false); // 체크표시 해제
        }

        private void Start()
        {
            _x = gameObject.name switch
            {
                "Helmet" => 0,
                "Weapon" => 1,
                "Clothes" => 2,
                "Shoes" => 3,
                "Brooch" => 4,
                "Shield" => 5,
                "Accessory1" => 6,
                "Accessory2" => 7,
                _ => _x
            };
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            InventoryCommander.instance.HandleSlotTextColor(true,_x);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            InventoryCommander.instance.HandleSlotTextColor(false,_x);
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            InventoryCommander.instance.ItemSlot = _x;
            switch (eventData.button) // 인벤토리 아이템 클릭
            {
                case PointerEventData.InputButton.Left:
                {
                    if (Time.unscaledTime - _timer < 0.5f)
                    {
                        InventoryCommander.instance.ClickType(ClickType.DoubleLeftClick, true);
                    }
                    else
                    {
                        InventoryCommander.instance.ClickType(ClickType.LeftClick, true);
                    }
                    _timer = Time.unscaledTime;
                    break;
                }

                case PointerEventData.InputButton.Right:
                    InventoryCommander.instance.ClickType(ClickType.RightClick, true);
                    break;
            }
        }
    }
}
