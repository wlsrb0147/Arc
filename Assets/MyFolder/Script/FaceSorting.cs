using System.Collections.Generic;
using MyFolder.Script;
using UnityEngine;

public class FaceSorting : MonoBehaviour
{
    public static int currentCharaNum;
    public Transform[] pos;
    public Transform[] tab15Pos;
    public Inventory inven;

    public RuntimeAnimatorController[] ani;
    public Animator[] currentAni;

    private readonly Dictionary<string, RuntimeAnimatorController> _changeAni = new();

    private readonly float height = 51.58074f;
    private readonly Vector3[] startPos = new Vector3[6];
    private readonly Transform[] tempPos = new Transform[6];
    private bool findMoving;

    private int movingNum;

    private void Update()
    {
        // 드래그 시작시
        if (FaceEventTriggerScr.isDrag && findMoving == false)
        {
            for (var i = 0; i < currentCharaNum; i++)
                if (tempPos[i].name == FaceEventTriggerScr.movingObj.name) // 드래그하는 게임오브젝트 판별
                {
                    movingNum = i;
                    findMoving = true;
                }
        }
        // 드래그 중일때
        else if (FaceEventTriggerScr.isDrag && findMoving)
        {
            for (var i = 0; i < movingNum; i++) // 아래에서 위로감
            {
                if (tempPos[i].position != pos[i].position // 한번 움직였고
                    && tempPos[i].position.y > tempPos[movingNum].position.y - height) // 포지션이 바뀌었을떄
                    tempPos[i].position = pos[i].position;


                if (tempPos[i].position.y < tempPos[movingNum].position.y + height) // 움직인적 없을때
                    tempPos[i].position = pos[i + 1].position;
            }

            for (var i = movingNum + 1; i < currentCharaNum; i++) // 위에서 아래로 감
            {
                if (tempPos[i].position != pos[i].position //한번 움직였고
                    && tempPos[i].position.y < tempPos[movingNum].position.y + height) // 초기 위치가 현재 위치보다 위에있을 때
                    tempPos[i].position = pos[i].position; // 제자리로 

                if (tempPos[i].position.y > tempPos[movingNum].position.y - height)
                    tempPos[i].position = pos[i - 1].position;
            }
        }
        // 드래그를 끝냈을 때
        else if (!FaceEventTriggerScr.isDrag && FaceEventTriggerScr.moved != 0)
        {
            /*
             * moved = -1, 움직임 취소
             * moved = 1, 움직임 성공, 갱신 전
             * moved = 0, 갱신 완료
             */
            if (FaceEventTriggerScr.moved == -1) // 움직임을 취소했을 때
            {
                // 움직이기 전 초기위치로 돌아감
                for (var i = 0; i < currentCharaNum; i++) tempPos[i].position = startPos[i];
                FaceEventTriggerScr.moved = 0;
            }
            else /*if (EventTriggerScr.moved == 1) */ // 움직임에 성공했을 떄
            {
                for (var i = 0; i < currentCharaNum; i++) startPos[i] = tempPos[i].position;
                FaceEventTriggerScr.moved = 0;
            }

            // 여기서부터 초기화 알고리즘

            Sorting();

            findMoving = false;
        }

        if (Inventory.needSort)
        {
            Sorting();
            if (inven.check.transform.position.x > 80) Inventory.selectFaceName = tempPos[0].name;
            Inventory.needSort = false;
        }

        for (var i = 0; i < Inventory.notUsing.Count; i++) Inventory.notUsing[i].position = tab15Pos[i].position;
    }

    private void OnEnable()
    {
        _changeAni.Clear();
        _changeAni.Add("AiBox", ani[0]);
        _changeAni.Add("CarrotBox", ani[1]);
        _changeAni.Add("CellineBox", ani[2]);
        _changeAni.Add("EluardBox", ani[3]);
        _changeAni.Add("KreutzerBox", ani[4]);
        _changeAni.Add("MariaBox", ani[5]);
        _changeAni.Add("PeachBox", ani[6]);
        _changeAni.Add("SizzBox", ani[7]);
        _changeAni.Add("TenziBox", ani[8]);

        Sorting();
        Inventory.selectFaceName = tempPos[0].gameObject.name;
    }

    private void Sorting()
    {
        // 여기서 face[i]가 setActive(false)를 감지해야함
        for (var i = 0; i < 6; i++) tempPos[i] = null;

        currentCharaNum = 0;

        for (var i = 0; i < 9; i++)
        for (var j = 0; j < 6; j++)
            if (inven.face[i].position.y >= pos[j].position.y - height
                && inven.face[i].position.y < pos[j].position.y + height
                && inven.face[i].position.x < 160f) // face[i]의 위치가 pos[j] 위치인걸 인식
            {
                tempPos[j] = inven.face[i]; // tempPos[j]에 face[i] 대입
                currentCharaNum++;
            }

        // 위 함수의 결과물 : 
        // 위부터 현재위치에 맞게 tempPos에 face[i] 대입

        var shiftCount = 0;
        for (var i = 0; i < 6; i++)
        {
            if (tempPos[i] == null) // 지금 상태가 null 이면, shift++하고 다음으로 넘어감
            {
                shiftCount++;
                continue;
            }

            if (shiftCount > 0 && i - shiftCount >= 0) //  null이 존재할 때, i를 null갯수만큼 앞으로 당김
            {
                tempPos[i - shiftCount] = tempPos[i];
                tempPos[i] = null;
            }
        }

        for (var i = 0; i < currentCharaNum; i++)
        {
            tempPos[i].position = pos[i].position;
            startPos[i] = pos[i].position;
        }

        for (var i = 0; i < 6; i++)
            if (tempPos[i] != null)
                currentAni[i].runtimeAnimatorController = _changeAni[tempPos[i].name];
            else
                currentAni[i].runtimeAnimatorController = null;
    }
}