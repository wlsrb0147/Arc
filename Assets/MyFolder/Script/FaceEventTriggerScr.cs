using UnityEngine;
using UnityEngine.EventSystems;

namespace MyFolder.Script
{
    public class FaceEventTriggerScr : MonoBehaviour, IDragHandler, 
        IEndDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        public Transform tempParent;
        public Transform originalParent;
        public Transform tab15Parent;
    
        // private Vector3 CurrentPos;
        public Transform[] pos;
    
        private float width = 140f;
        private float height = 51.58074f;
    
        public static bool isDrag = false;
        public static Vector3 StartingPos;
        public static Transform movingObj;
        public static int moved;
    
        public bool characterUsing;


        private void OnEnable()
        {
            if (!characterUsing && !Inventory.notUsing.Contains(transform))
            {
                Inventory.notUsing.Add(transform);
            }

            if (Inventory.notUsing.Count != 0)
            {
                Inventory.tab15SelectName = Inventory.notUsing[0].gameObject.name;
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            // 좌클릭 포인터 업
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (characterUsing)
                {
                    if (isDrag)
                    {
                        isDrag = false;
                    }
                    Inventory.selectFaceName = gameObject.name;
                }
                else if (!characterUsing)
                {
                    Inventory.tab15SelectName = gameObject.name;
                }
            }
            
            // 우클릭 포인터 업
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                if (characterUsing)
                {
                    if (Inventory.notUsing.Count < 8)
                    {
                        transform.SetParent(tab15Parent);
                        Inventory.notUsing.Add(transform);
                        characterUsing = false;
                        transform.position = new Vector3(300, 0, 0); // 정렬조건 회피용 코드
                        Inventory.needSort = true;
                    }
                }
                else if (!characterUsing)
                {
                    if (Inventory.notUsing.Count > 3)
                    {
                        transform.SetParent(originalParent);
                        Inventory.notUsing.Remove(transform);
                        characterUsing = true;
                        Inventory.tab15SelectName = Inventory.notUsing[0].name;
                        transform.position = pos[8 - Inventory.notUsing.Count].position;
                        Inventory.needSort = true;
                    }
                }
            }
        }
    
        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                StartingPos = transform.position;
                movingObj = transform;
            }
        }
    
        public void OnDrag(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left && characterUsing )
            {       
                isDrag = true;
                transform.SetParent(tempParent);
                transform.position = eventData.position;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left && characterUsing )
            {
                if (transform.position.x > StartingPos.x + width || transform.position.x < StartingPos.x - width)
                {
                    moved = -1;
                }
                else
                {
                    if (transform.position.y > pos[0].position.y - height)
                    {
                        StartingPos = pos[0].position;
                    }
                    // currentPos를 inven.pos에 맞게 정렬
                    else
                    {
                        for (int i = FaceSorting.currentCharaNum - 1; i >= 1; i--)
                        {
                            if (transform.position.y < pos[i].position.y + height)
                            {
                                StartingPos = pos[i].position;
                                break;
                            }
                        }
                    }
                    moved = 1;
                }

                transform.position = StartingPos; // 제자리에 놓음
                transform.SetParent(originalParent);
            }
        }
    }
}
