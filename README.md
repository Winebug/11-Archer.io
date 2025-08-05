# **Game Title**
<img width="831" height="467" alt="게임인트로" src="https://github.com/user-attachments/assets/bb31ede3-4b2f-45db-8790-d81418aa6f91" />

# **Flow Chart** (아래 Chart는 수정될 수 있음)

<img width="7848" height="2422" alt="image" src="https://github.com/user-attachments/assets/bab016a2-b5ea-4ad3-85f6-4ae25acc3f8d" />

# **Diagram**
<img width="9968" height="6946" alt="image" src="https://github.com/user-attachments/assets/48a3f57b-5897-45cd-9505-b7dc1c1ef733" />

# **Team**
<img width="1728" height="2040" alt="image" src="https://github.com/user-attachments/assets/55b6cded-4a2c-421a-9990-795db67fc4ad" />

# **게임 소개**

- **장르**
    - 탑다운
    - 로그라이크
    - 슈팅

- **랜덤 방 생성**
    - 방의 구조, 적 배치, 아이템 위치를 무작위로 생성
    - 방을 클리어하면 다음 방으로 이동
 
![방생성](https://github.com/user-attachments/assets/d270b2d4-a140-4af5-9520-fe39a52e0c73)

- **캐릭터 이동과 공격**
    - WASD로 이동, 자동으로 가까운 적 공격
    - 특수 공격 스킬(범위 공격, 연속 공격 등) 추가

 ![이동과공격](https://github.com/user-attachments/assets/f06d098c-0639-4489-bb44-22c74254b961)

- **적 AI와 공격 패턴**
    - 적이 플레이어를 추적하거나 특정 패턴으로 공격
    - 적의 종류
        - 근접 공격 적
        - 원거리 공격 적

<img width="13390" height="2754" alt="image" src="https://github.com/user-attachments/assets/2ffb4aca-a4c4-4402-8f7c-06598eed59af" />

- **스킬과 업그레이드 시스템**
    - 방 클리어 시 무작위 스킬 선택(예: 공격 속도 증가, 체력 회복)
    - 스킬 조합으로 플레이어의 전략적 선택 제공

![스킬선택](https://github.com/user-attachments/assets/363f7585-f10e-49c2-9210-17c2456abb4d)

- **보스전**
    - 특정 방에서 보스 등장. 다양한 공격 패턴과 높은 체력
 
![보스전](https://github.com/user-attachments/assets/372e2eee-59e4-4227-9ea1-91a323f07f9d)

- **동료 시스템**
    - 보스방 입장시 랜덤 동료 영입하여 함께 전투 (엘프궁수, 드워프주술사, 리자드맨전사)

![컴패니언 시스템](https://github.com/user-attachments/assets/13c20c39-f0d9-4792-9bdc-48da9b59cd9e)

- **사운드 및 이펙트**
    - 배경음악 및 효과음, 이펙트를 구현
 
![이펙트](https://github.com/user-attachments/assets/f971816d-acd2-49b8-8582-c2324d8fd2a7)

- **게임 시작 화면**
    - 게임 시작 전 **Play**, **Settings**, **Exit** 버튼을 포함한 시작 화면 제작
    - 간단한 배경과 게임 로고 추가
 
![인트로설정](https://github.com/user-attachments/assets/a1bb9564-a9a8-445d-b08f-8ce6de78d71d)

- **타이틀 및 스테이지 클리어 화면**
    - 게임 시작 시 타이틀 화면 표시
    - 스테이지를 클리어하면 클리어 시간, 점수, 다음 스테이지로의 버튼 제공
 
- **튜토리얼 시스템**
    - 초보 플레이어가 게임을 쉽게 이해할 수 있도록 **기본 조작 및 목표 설명** 추가
    - 게임 시작 시 또는 특정 시점에서 튜토리얼 팝업 표시
    - 이동 및 공격 시스템 소개
 
![튜토리얼](https://github.com/user-attachments/assets/db835723-b9bc-4c91-a4b0-e26f65e6527d)

---

# **Commit Convention**

Feat:	새로운 기능 추가  
Fix:	버그 수정 또는 typo  
Refactor:	리팩토링  
Design:	CSS 등 사용자 UI 디자인 변경  
Comment:	필요한 주석 추가 및 변경  
Style:	코드 포맷팅, 세미콜론 누락, 코드 변경이 없는 경우  
Test:	테스트(테스트 코드 추가, 수정, 삭제, 비즈니스 로직에 변경이 없는 경우)  
Chore:	위에 걸리지 않는 기타 변경사항(빌드 스크립트 수정, assets image, 패키지 매니저 등)  
Init:	프로젝트 초기 생성  
Rename:	파일 혹은 폴더명 수정하거나 옮기는 경우  
Remove:	파일을 삭제하는 작업만 수행하는 경우  
