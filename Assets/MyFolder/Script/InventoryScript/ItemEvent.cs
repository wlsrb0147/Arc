using UnityEngine;
using UnityEngine.EventSystems;

namespace MyFolder.Script.InventoryScript
{
    public class ItemEvent : MonoBehaviour,IPointerClickHandler
    {
        private int _itemType;
        private int _characterType;
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                Inventory.selectInventoryItem = gameObject.name;
            }
        }
    }
}
