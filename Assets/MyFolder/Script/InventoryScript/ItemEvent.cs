using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MyFolder.Script.InventoryScript
{
    public class ItemEvent : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        // 필요한 정보 : 아이템의 이름, 아이템의 상태
        // 클릭시 클릭정보를 InvenManager에 전달
        // 여기서는 클릭만 감지함

        private float _timer;
        private Text _text1;
        private Text _text2;
        private Color _color;

        private void Awake()
        {
            _text1 = transform.GetChild(0).GetComponent<Text>();
            _text2 = transform.GetChild(1).GetComponent<Text>();
        }

        private void Start()
        {
            _color = _text1.color;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _text1.color = Color.red;
            _text2.color = Color.red;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _text1.color = _color;
            _text2.color = _color;
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            InventoryCommander.instance.SelectedInvenItemName = gameObject.name;
            
            switch (eventData.button) // 인벤토리 아이템 클릭
            {
                case PointerEventData.InputButton.Left:
                {
                    if (Time.unscaledTime - _timer < 0.5f)
                        InventoryCommander.instance.ClickType(ClickType.DoubleLeftClick, false);
                    else
                    {
                        InventoryCommander.instance.ClickType(ClickType.LeftClick, false);
                    }

                    _timer = Time.unscaledTime;
                    break;
                }

                case PointerEventData.InputButton.Right:
                    InventoryCommander.instance.ClickType(ClickType.RightClick, false);
                    break;
            }
            
        }
    
    }
}