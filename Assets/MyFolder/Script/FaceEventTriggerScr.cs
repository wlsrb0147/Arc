using MyFolder.Script.InventoryScript;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MyFolder.Script
{
    public class FaceEventTriggerScr : MonoBehaviour, IDragHandler,
        IEndDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        public static bool isDrag;
        public static Vector3 StartingPos;
        public static Transform movingObj;
        public static int moved;
        public Transform tempParent;
        public Transform originalParent;
        public Transform tab15Parent;

        // private Vector3 CurrentPos;
        public Transform[] pos;

        public bool characterUsing;
        private readonly float height = 51.58074f;

        private readonly float width = 140f;


        private void OnEnable()
        {
            if (!characterUsing && !Inventory.notUsing.Contains(transform))
            {
                Inventory.notUsing.Add(transform);
                transform.SetParent(tab15Parent);
            }

            if (Inventory.notUsing.Count != 0) Inventory.tab15SelectName = Inventory.notUsing[0].gameObject.name;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left && characterUsing)
            {
                isDrag = true;
                transform.SetParent(tempParent);
                transform.position = eventData.position;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left && characterUsing)
            {
                if (transform.position.x > StartingPos.x + width || transform.position.x < StartingPos.x - width)
                {
                    moved = -1;
                }
                else
                {
                    if (transform.position.y > pos[0].position.y - height)
                        StartingPos = pos[0].position;
                    // currentPos를 inven.pos에 맞게 정렬
                    else
                        for (var i = FaceSorting.currentCharaNum - 1; i >= 1; i--)
                            if (transform.position.y < pos[i].position.y + height)
                            {
                                StartingPos = pos[i].position;
                                break;
                            }

                    moved = 1;
                }

                transform.position = StartingPos; // 제자리에 놓음
                transform.SetParent(originalParent);
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

        public void OnPointerUp(PointerEventData eventData)
        {
            InventoryCommander.instance.FaceClick();
            // 좌클릭 포인터 업
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (characterUsing)
                {
                    if (isDrag) isDrag = false;

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
                        UI_Manager.instance.needWait = true;
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
                        UI_Manager.instance.needWait = true;       
                        transform.SetParent(originalParent);
                        Inventory.notUsing.Remove(transform);
                        characterUsing = true;
                        if (!Inventory.notUsing.Find(i=>i.name == Inventory.tab15SelectName))
                        {
                            Inventory.tab15SelectName = Inventory.notUsing[0].name;
                        }
                        transform.position = pos[8 - Inventory.notUsing.Count].position;
                        Inventory.needSort = true;
                    }
                }
            }
            UI_Manager.instance.isSomethingChanged = true;
        }
    }
}