# 
![Honeycam 2023-12-26 14-26-40](https://github.com/wlsrb0147/Arc/assets/50743287/fc8caabc-30c2-4ea5-a586-29eb1556cec6)


## 목차

1. 프로젝트 설명

2. 코드/논리 pdf파일 & 시연영상

3. 기능 시연




## 1. 프로젝트 설명

 2000년에 발매된 게임 Arcturus를 모방한 프로젝트입니다.<br/>
 3D환경에서 2D Sprite를 사용한것이 특징입니다.

 기본 조작, 인벤토리 및 아이템 필터기능을 구현하였습니다.

 움직임의 기준으로, 벽과 바닥은 법선벡터의 각도를 구하여 구분하였고,<br/>
 경사로의 움직임은 경사로의 기울기 벡터값을 구하여 계산하였습니다.
    
  아이템 정보는 scriptable object를 사용하여 저장하였습니다.
    
## 2. 코드/논리 링크 & 시연영상
</br>
코드/논리 자료 : https://drive.google.com/file/d/1FakOKOw6Gs066TrbbLMNiBjeX_u-y7Ao/view?usp=sharing
</br>(목차 포함 8p)</br></br>

시연영상 : https://youtu.be/w0WUOJ53-rs </br></br>

## 3. 기능 시연

- 기능 목록
    1. 카메라와 Sprite 조율
    2. 움직임 구현과 벡터의 사용 이유
    3. 기울기에 따른 바닥과 벽 판정
    4. 캐릭터 정렬
    5. 아이템 장착 및 해제
    6. 아이템 필터링
    

## 기능 시연
![image](https://github.com/user-attachments/assets/29e42dfa-e069-4b47-87eb-57edacf944b1)

캐릭터 관련 스크립트</br>
Assets/MyFolder/Script/Leader.cs<br/>

캐릭터 스텟 : CharacterManager<br/>
아이템 정보 저장 : ItemDatabase<br/>
인벤토리 정보 : UIManager & InventoryCommander<br/>
아이템 장착 및 스텟 : ItemManager<br/>

### 2-1. 카메라와 Sprite 조율
![Honeycam 2023-12-26 14-27-04](https://github.com/wlsrb0147/Arc/assets/50743287/e1d14d52-7ec3-4fb2-b275-5c2f41809743)


- 카메라가 회전할 때, Sprite도 같이 회전합니다.
- 카메라가 회전하더라도, 캐릭터의 시선 같은 방향에 고정됩니다.

![Honeycam 2023-12-26 14-27-08](https://github.com/wlsrb0147/Arc/assets/50743287/ae8fc73f-4aac-4568-803d-f870ce00b819)

- 카메라가 회전할 때의 Sprite 움직임

### 2-2. 움직임 구현과 벡터의 사용 이유

**2-2-1. 기본 움직임**
![Honeycam 2023-12-26 14-27-13](https://github.com/wlsrb0147/Arc/assets/50743287/9118bf21-8b96-4928-8dbe-abd8d545338f)



- 캐릭터의 움직임 방향에 맞게 애니메이션이 실행됩니다.
- 카메라의 회전에따라 나침반이 회전하며, 나침반의 침은 움직임을 따라갑니다.
- 리더 캐릭터를 따르는 다섯 동료들은, 리더의 행동을 따라합니다.
- 캐릭터의 움직임은 키보드 혹은 마우스 입력을 받습니다.

**2-2-2. 움직임구현에서 기울기를 사용한 이유**

움직임을 그대로 사용 할 경우, 경사로에서의 이동과 벽과의 충돌에서 문제가 생깁니다.

### 1. 중력을 사용하여 경사면을 움직일 경우
![Honeycam 2023-12-26 14-27-19](https://github.com/wlsrb0147/Arc/assets/50743287/ab06ac54-9fb3-4450-be67-92b878fbdcb3)



### 기울기 벡터를 사용하여 움직일 경우
![Honeycam 2023-12-26 14-27-23](https://github.com/wlsrb0147/Arc/assets/50743287/90a464f3-2052-404f-a4d3-8d84dff9c206)



1. 언덕을 내려갈 경우, 캐릭터가 공중에 뜨는 문제가 발생합니다.
2. 언덕을 내려갈 경우, 감속이 되는 현상이 발생합니다.

### 2. 상쇄 없이 벽과 충돌 할 결우
![Honeycam 2023-12-26 14-27-27](https://github.com/wlsrb0147/Arc/assets/50743287/0db0e8de-fd1f-433a-8ccd-7f2daefb4627)


### 벡터의 상쇄를 이용한 경우
![Honeycam 2023-12-26 14-27-31](https://github.com/wlsrb0147/Arc/assets/50743287/2f72bf88-9b83-4c20-967a-a6c050361239)


1. 공중에서 벽에 부딛칠경우, 캐릭이 벽에 끼이는 문제가 발생합니다
2. 땅에서 벽에 부딛칠경우, 감속이 과하게 일어나 캐릭터가 정지에 달합니다.

### 2-3. 기울기에 따른 바닥과 벽 판정
![Honeycam 2023-12-26 14-27-38](https://github.com/wlsrb0147/Arc/assets/50743287/aae00206-02a5-4d34-a4ea-26bcb31f0fd7)


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
![Honeycam 2023-12-26 14-27-42](https://github.com/wlsrb0147/Arc/assets/50743287/a5f3a9f2-c2a9-4cf1-86c4-a6c46635a5a3)


### 정렬 취소
![Honeycam 2023-12-26 14-27-48](https://github.com/wlsrb0147/Arc/assets/50743287/3b9ec97c-def5-4d0a-981a-817075f0e8c3)


### 사용 리스트 → 미사용 리스트 전환
![Honeycam 2023-12-26 14-27-54](https://github.com/wlsrb0147/Arc/assets/50743287/3bb0cf9f-a2ae-49ac-8867-d20ec17d78fb)


### 미사용 리스트 → 사용 리스트 전환
![Honeycam 2023-12-26 14-27-58](https://github.com/wlsrb0147/Arc/assets/50743287/35c7c9b6-cdca-4422-a1a9-8312144b7a86)


### 2-5. 아이템 장착 및 해제

- 장착 가능한 아이템의 이름은 진하게 만들어 인식이 쉽게 만들었습니다.
- 캐릭별로 별개의 스텟과 장비창이 존재합니다.
- 아이템을 클릭했을 때, 장착슬롯과 스텟 변동량을 표기합니다.

### 아이템 장착 및 해제
![Honeycam 2023-12-26 14-28-02](https://github.com/wlsrb0147/Arc/assets/50743287/9b2ae983-f130-413b-a133-2a063c6a317d)

![Honeycam 2023-12-26 14-28-07](https://github.com/wlsrb0147/Arc/assets/50743287/937f9b22-cb3d-41a7-81ad-2ca287a1000d)


### 복합 장착 아이템의 처리
![Honeycam 2023-12-26 14-28-12](https://github.com/wlsrb0147/Arc/assets/50743287/c8a7a0d4-0854-4afa-ad5e-ff4e871f68f6)

![Honeycam 2023-12-26 14-28-17](https://github.com/wlsrb0147/Arc/assets/50743287/75c046a3-d3fc-40fe-aaa0-6b1c495b57b3)


### 캐릭터 고유의 스텟과 장착슬롯
![Honeycam 2023-12-26 14-28-21](https://github.com/wlsrb0147/Arc/assets/50743287/6cf677b7-270d-460e-b7ec-46bba6caf476)


### 2-6. 아이템 필터링

- 장착 캐릭터와 아이템 타입을 통한 장비 필터링을 구현하였습니다.
- 북마크 기능을 추가하여 아이템을 쉽게 찾을 수 있도록 하였습니다.

### 시즈 캐릭터 필터링
![Honeycam 2023-12-26 14-28-26](https://github.com/wlsrb0147/Arc/assets/50743287/3ba4c8d3-375d-4286-a75b-c213fac647f7)


### 한손무기 필터링
![Honeycam 2023-12-26 14-28-30](https://github.com/wlsrb0147/Arc/assets/50743287/927a1fce-a376-4ecc-8b6d-48f7d0d6a0d8)


### 북마크 기능 활용
![Honeycam 2023-12-26 14-28-35](https://github.com/wlsrb0147/Arc/assets/50743287/699bb4f1-f64a-4306-a7d0-0d78580ba973)

