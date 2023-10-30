using System.Collections;
using System.Collections.Generic;
using MyFolder.Script.InventoryScript;
using UnityEngine;using UnityEngine.EventSystems;


public class ItemEvent : MonoBehaviour,IPointerClickHandler
{
    // 필요한 정보 : 아이템의 이름, 아이템의 상태
    // 클릭시 클릭정보를 InvenManager에 전달
    // 여기서는 클릭만 감지함

    private const bool Equipped = false;


    public void OnPointerClick(PointerEventData eventData)
    {
        InventoryManager.instance.SetInfo(gameObject.name, Equipped);
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            
        }

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            
        }
    }

    
    public void Delete()
    {
        Destroy(gameObject);
    }
}
