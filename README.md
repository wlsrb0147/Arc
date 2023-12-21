# 

![Honeycam 2023-12-14 01-08-50.webp](https://prod-files-secure.s3.us-west-2.amazonaws.com/d6f15c80-0360-4b9c-8d92-2384fa9bb47f/a9d5f83f-8def-4f5b-a6ca-d73d6b57136c/Honeycam_2023-12-14_01-08-50.webp)


### 코드, 논리에 대한 설명은 3번의 pdf파일에 설명되어있습니다.

## 목차

1. 프로젝트 설명

2. 기능 시연

3. 기술서 & 깃허브 링크


## 1. 프로젝트 설명

- 2000년에 발매된 게임 Arcturus를 모방한 프로젝트입니다.
    
    3D환경에서 2D Sprite를 사용한것이 특징입니다.
    
    기본 조작, 인벤토리 및 아이템 필터기능을 구현하였습니다.
    
    움직임의 기준으로, 벽과 바닥은 법선벡터의 각도를 구하여 구분하였고
    
    경사로의 움직임은 경사로의 기울기 벡터값을 구하여 계산하였습니다.
    
    아이템 정보는 scriptable object를 사용하여 저장하였습니다.
    

## 2. 기능 시연

- 기능 목록
    1. 카메라와 Sprite 조율
    2. 움직임 구현과 벡터의 사용 이유
    3. 기울기에 따른 바닥과 벽 판정
    4. 캐릭터 정렬
    5. 아이템 장착 및 해제
    6. 아이템 필터링
    

## 기능 시연

### 2-1. 카메라와 Sprite 조율

![FV_Rot.webp](https://prod-files-secure.s3.us-west-2.amazonaws.com/d6f15c80-0360-4b9c-8d92-2384fa9bb47f/84b52f7f-cece-4490-a5c3-82f4044f54de/FV_Rot.webp)

- 카메라가 회전할 때, Sprite도 같이 회전합니다.
- 카메라가 회전하더라도, 캐릭터의 시선 같은 방향에 고정됩니다.

![FV_Top.webp](https://prod-files-secure.s3.us-west-2.amazonaws.com/d6f15c80-0360-4b9c-8d92-2384fa9bb47f/39dc3421-7301-41c6-b19a-3da04157686f/FV_Top.webp)

- 카메라가 회전할 때의 Sprite 움직임

### 2-2. 움직임 구현과 벡터의 사용 이유

**2-2-1. 기본 움직임**

![Honeycam 2023-12-04 02-07-04.webp](https://prod-files-secure.s3.us-west-2.amazonaws.com/d6f15c80-0360-4b9c-8d92-2384fa9bb47f/45418220-ff8d-426d-beaf-072d18543acb/Honeycam_2023-12-04_02-07-04.webp)

- 캐릭터의 움직임 방향에 맞게 애니메이션이 실행됩니다.
- 카메라의 회전에따라 나침반이 회전하며, 나침반의 침은 움직임을 따라갑니다.
- 리더 캐릭터를 따르는 다섯 동료들은, 리더의 행동을 따라합니다.
- 캐릭터의 움직임은 키보드 혹은 마우스 입력을 받습니다.

**2-2-2. 움직임구현에서 기울기를 사용한 이유**

움직임을 그대로 사용 할 경우, 경사로에서의 이동과 벽과의 충돌에서 문제가 생깁니다.

### 1. 중력을 사용하여 경사면을 움직일 경우

![Honeycam 2023-12-04 02-22-36.webp](https://prod-files-secure.s3.us-west-2.amazonaws.com/d6f15c80-0360-4b9c-8d92-2384fa9bb47f/694a83d8-1ef0-4ae4-b49b-e26e23037c93/Honeycam_2023-12-04_02-22-36.webp)

### 기울기 벡터를 사용하여 움직일 경우

![FM_Vec.webp](https://prod-files-secure.s3.us-west-2.amazonaws.com/d6f15c80-0360-4b9c-8d92-2384fa9bb47f/5073df53-3667-42dd-b323-c1b2798ab8f7/FM_Vec.webp)

1. 언덕을 내려갈 경우, 캐릭터가 공중에 뜨는 문제가 발생합니다.
2. 언덕을 내려갈 경우, 감속이 되는 현상이 발생합니다.

### 2. 상쇄 없이 벽과 충돌 할 결우

![Honeycam 2023-12-04 05-39-14.webp](https://prod-files-secure.s3.us-west-2.amazonaws.com/d6f15c80-0360-4b9c-8d92-2384fa9bb47f/2980757c-1c77-47db-a539-ad89ea9bf434/Honeycam_2023-12-04_05-39-14.webp)

### 벡터의 상쇄를 이용한 경우

![Honeycam 2023-12-04 05-41-17.webp](https://prod-files-secure.s3.us-west-2.amazonaws.com/d6f15c80-0360-4b9c-8d92-2384fa9bb47f/4dbc85eb-b1e4-4794-b069-e8c1617ab816/Honeycam_2023-12-04_05-41-17.webp)

1. 공중에서 벽에 부딛칠경우, 캐릭이 벽에 끼이는 문제가 발생합니다
2. 땅에서 벽에 부딛칠경우, 감속이 과하게 일어나 캐릭터가 정지에 달합니다.

### 2-3. 기울기에 따른 바닥과 벽 판정

![FW_Wall.webp](https://prod-files-secure.s3.us-west-2.amazonaws.com/d6f15c80-0360-4b9c-8d92-2384fa9bb47f/872a5d43-3582-473b-bc98-2b4112c7ce63/FW_Wall.webp)

- 3D 게임에서는 바닥과 벽의 경계가 애매합니다.
- 따라서 바닥과 벽의 인식은 사물의 기울기에 따라 인식하도록 했습니다.
- 바닥에 서 있을경우, 지상판정이 되며 걸을 수 있습니다.
- 벽에 서 있을경우, 공중판정이 되며 미끄러집니다.

### 2-4. 캐릭터 정렬

- 인벤토리에선 캐릭터 아이콘을 드래그하여 순서를 정렬할 수 있습니다.
- 드래그가 유효한 범위를 벗어날 경우, 정렬은 취소됩니다.
- 아이콘을 우클릭하여 사용리스트와 미사용리스트 간 캐릭터의 등록 전환이 가능합니다.
- 사용 리스트에서 제거된 캐릭터의 애니메이션은 재생하지 않습니다.

### 정렬에 따른 캐릭터 순서 변경

![IF_Sort.webp](https://prod-files-secure.s3.us-west-2.amazonaws.com/d6f15c80-0360-4b9c-8d92-2384fa9bb47f/ab34d4b0-8678-482c-bf48-e50acd4b7be1/IF_Sort.webp)

### 정렬 취소

![I_sort.webp](https://prod-files-secure.s3.us-west-2.amazonaws.com/d6f15c80-0360-4b9c-8d92-2384fa9bb47f/3a319fa3-b11a-4e6c-84ed-d17e626e6db8/I_sort.webp)

### 사용 리스트 → 미사용 리스트 전환

![IF_De.webp](https://prod-files-secure.s3.us-west-2.amazonaws.com/d6f15c80-0360-4b9c-8d92-2384fa9bb47f/0e483100-20f8-446f-b33e-709a0f76ea50/IF_De.webp)

### 미사용 리스트 → 사용 리스트 전환

![IF_Ad.webp](https://prod-files-secure.s3.us-west-2.amazonaws.com/d6f15c80-0360-4b9c-8d92-2384fa9bb47f/d887d241-79f8-48ee-9201-5fd4f1c20c4c/IF_Ad.webp)

### 2-5. 아이템 장착 및 해제

- 장착 가능한 아이템의 이름은 진하게 만들어 인식이 쉽게 만들었습니다.
- 캐릭별로 별개의 스텟과 장비창이 존재합니다.
- 아이템을 클릭했을 때, 장착슬롯과 스텟 변동량을 표기합니다.

### 아이템 장착 및 해제

![Item_Dequ.webp](https://prod-files-secure.s3.us-west-2.amazonaws.com/d6f15c80-0360-4b9c-8d92-2384fa9bb47f/17b02d3e-19b2-4520-b00b-4c421d36ef05/Item_Dequ.webp)

![Item_Equ.webp](https://prod-files-secure.s3.us-west-2.amazonaws.com/d6f15c80-0360-4b9c-8d92-2384fa9bb47f/8563a329-e80d-41ec-a32a-af77137c5ec9/Item_Equ.webp)

### 복합 장착 아이템의 처리

![Equip_Ac.webp](https://prod-files-secure.s3.us-west-2.amazonaws.com/d6f15c80-0360-4b9c-8d92-2384fa9bb47f/40192c58-340d-4624-b2d7-05e7e6a4ba28/Equip_Ac.webp)

![Equip_We.webp](https://prod-files-secure.s3.us-west-2.amazonaws.com/d6f15c80-0360-4b9c-8d92-2384fa9bb47f/c950f9a2-1937-42f8-97e3-81ba3ba6c164/Equip_We.webp)

### 캐릭터 고유의 스텟과 장착슬롯

![CharEquip.webp](https://prod-files-secure.s3.us-west-2.amazonaws.com/d6f15c80-0360-4b9c-8d92-2384fa9bb47f/66c83f03-d878-4698-9309-3cec1318e73e/CharEquip.webp)

### 2-6. 아이템 필터링

- 장착 캐릭터와 아이템 타입을 통한 장비 필터링을 구현하였습니다.
- 북마크 기능을 추가하여 아이템을 쉽게 찾을 수 있도록 하였습니다.

### 시즈 캐릭터 필터링

![filter1.webp](https://prod-files-secure.s3.us-west-2.amazonaws.com/d6f15c80-0360-4b9c-8d92-2384fa9bb47f/47870ead-cada-4fdd-b5e7-43e956991d68/filter1.webp)

### 한손무기 필터링

![filter2.webp](https://prod-files-secure.s3.us-west-2.amazonaws.com/d6f15c80-0360-4b9c-8d92-2384fa9bb47f/0aa34952-ae91-4eb7-883a-85adca50d61a/filter2.webp)

### 북마크 기능 활용

![Honeycam 2023-12-04 03-58-50.webp](https://prod-files-secure.s3.us-west-2.amazonaws.com/d6f15c80-0360-4b9c-8d92-2384fa9bb47f/972b4253-e775-488b-b53f-4fccf3112b2c/Honeycam_2023-12-04_03-58-50.webp)

## 3. 코드/논리 & 깃허브 링크

### 코드/논리 : [https://drive.google.com/file/d/1I7p90s4CDGYjOx7H1kBitVpNOOE7ObBD/view?usp=sharing](https://drive.google.com/file/d/1FakOKOw6Gs066TrbbLMNiBjeX_u-y7Ao/view?usp=sharing)https://drive.google.com/file/d/1FakOKOw6Gs066TrbbLMNiBjeX_u-y7Ao/view?usp=sharing

### 깃허브 : https://github.com/wlsrb0147/Arc
