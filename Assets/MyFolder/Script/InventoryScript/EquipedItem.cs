using System.Collections;
using System.Collections.Generic;
using MyFolder.Script;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipedItem : MonoBehaviour, IPointerClickHandler,IDragHandler,IEndDragHandler,IPointerDownHandler
{
    private Vector3 now;
    
    public void OnPointerDown(PointerEventData eventData)
    {
        now = transform.position;
        Inventory.selectEquipedItem = gameObject.name;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = now;
        Inventory.selectEquipedItem = null;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        
    }
}