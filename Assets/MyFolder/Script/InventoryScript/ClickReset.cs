using System.Collections;
using System.Collections.Generic;
using MyFolder.Script;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickReset : MonoBehaviour,IPointerDownHandler 
{
    public void OnPointerDown(PointerEventData eventData)
    {
        Inventory.selectInventoryItem = null;
        Inventory.selectEquipedItem = null;
    }
}
