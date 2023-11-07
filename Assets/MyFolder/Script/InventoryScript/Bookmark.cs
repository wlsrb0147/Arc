using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MyFolder.Script.InventoryScript
{
    public class Bookmark : MonoBehaviour,IPointerClickHandler
    {
        private float _timer;
        
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
    }
}

