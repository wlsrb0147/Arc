using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MyFolder.Script.InventoryScript
{
    public class Bookmark : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
    {
        private float _timer;
        private bool asd = false;
        
        public void OnPointerClick(PointerEventData eventData)
        {
            InventoryCommander.instance.SelectedInvenItemName = gameObject.name;

            switch (eventData.button)
            {
                case PointerEventData.InputButton.Left:
                {
                    if (Time.unscaledTime - _timer < 0.5f)
                    {
                        InventoryCommander.instance.BookmarkCommand();
                    }
                    _timer = Time.unscaledTime;
                    break;
                }
                case PointerEventData.InputButton.Right:
                    InventoryCommander.instance.BookmarkCommand();
                    break;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Image spr = gameObject.GetComponent<Image>();
            if (spr.color == Color.clear)
            {
                spr.color = Color.white;
                asd = true;
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (asd)
            { 
                gameObject.GetComponent<Image>().color = Color.clear;
            }
        }
    }
}

