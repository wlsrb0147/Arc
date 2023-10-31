using System.Collections;
using System.Collections.Generic;
using MyFolder.Script.InventoryScript;
using UnityEngine;using UnityEngine.EventSystems;


public class ItemEvent : MonoBehaviour,IPointerClickHandler
{
    // 필요한 정보 : 아이템의 이름, 아이템의 상태
    // 클릭시 클릭정보를 InvenManager에 전달
    // 여기서는 클릭만 감지함
    
    private float _timer;

    public void OnPointerClick(PointerEventData eventData)
    {
        InventoryCommander.instance.ItemName = gameObject.name;
        
        switch (eventData.button)
        {
            case PointerEventData.InputButton.Left:
            {
                if (Time.unscaledTime - _timer < 0.5f)
                {
                    InventoryCommander.instance.ClickType(ClickType.DoubleLeftClick);
                }
                else
                {
                    InventoryCommander.instance.ClickType(ClickType.LeftClick);
                }

                _timer = Time.unscaledTime;
                break;
            }
            
            case PointerEventData.InputButton.Right:
                InventoryCommander.instance.ClickType(ClickType.RightClick);
                break;
        }
    }
}
