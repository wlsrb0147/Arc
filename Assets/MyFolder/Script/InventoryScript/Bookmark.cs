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
        private string _name;
        private void Start()
        {
            _name = name;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            switch (eventData.button)
            {
                case PointerEventData.InputButton.Left:
                {
                    if (Time.unscaledTime - _timer < 0.5f)
                    {
                        InventoryCommander.instance.BookmarkCommand(_name);
                    }
                    _timer = Time.unscaledTime;
                    break;
                }
                case PointerEventData.InputButton.Right:
                    InventoryCommander.instance.BookmarkCommand(_name);
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

