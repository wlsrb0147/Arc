using UnityEngine;
using UnityEngine.EventSystems;

namespace MyFolder.Script.InventoryScript
{
    public class FilterBkg : MonoBehaviour,IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                InventoryCommander.instance.ApplyFilter();
            }

            if (eventData.button == PointerEventData.InputButton.Right)
            {
                InventoryCommander.instance.DiscardFilter();
            }
        }
    }
}
