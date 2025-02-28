<div align="center">

# Back_to_the_Gungeon - TopDown 2D Shooter 

<p align="center"><img src="Assets\04.Images\GunjeonTitle.png" alt="프로젝트 로고" width="300"/></p>

</div>

---

## 📌 프로젝트 개요  

- **프로젝트명** : Back_to_the_Gungeon  
- **개발환경** : C#, Unity  
- **프로젝트 기간** : 2025.02.21 ~ 2025.02.28  
- **개발 인원** : 파이브아이즈(Five I's) 10조 5명  
  - 김효중, 황희돈, 박규태, 남태훈, 이준범  

---

## ⏳ 개발 타임라인  

| 날짜 | 내용 |  
|------|-------|  
| **02.21 (금)** | 역할 분담 및 와이어 프레임 작성 |  
| **02.22 (월)** | 스켈레톤 코드 작성 후 기본 기능 개발 |  
| **02.23 (화)** | 1차 병합 및 레벨 디자인 구성 |  
| **02.24 (수)** | 2차 병합 후 수정 사항 피드백 |  
| **02.25 (목)** | 지속적으로 피드백과 디버깅, 3차 병합 및 최종 테스트 |  
| **02.26 (금)** | 최종 완성 및 프로젝트 결과 발표 |  

---

## 🎮 주요 기능  

- **전투 시스템** : 무기, 투사체, 적 AI, 보스전  
- **레벨 디자인** : 랜덤 맵 생성, 방 기반 전투  
- **UI 및 UX** : 체력바, 적 조준 인터페이스 

---

## 👥 팀원 소개 및 업무 분담  

| 이름 | 역할 | 업무 상세내용 |  
|------|------|------------|  
| 김효중 | 팀장 | 원거리 무기 데이터 구현 및 구성 <br> 스킬 업그레이드 구현 <br> 보스몬스터 구현 |  
| 황희돈 | 팀원 | 플레이어 조작 및 Entity의 BaseController 구성 <br> 조이스틱 기능 <br> 적 탐색 및 조준 기능 <br> 로고 및 플레이어 그래픽 제작 |  
| 박규태 | 팀원 | 게임 컨셉 기획 <br> 레벨 디자인 및 함정 제작 <br> 레벨(맵) 랜덤 생성 기능 |  
| 남태훈 | 팀원 | 적 몬스터 제작 <br> 플레이어 추적 AI 구성 |  
| 이준범 | 팀원 | 전반적인 UI 구성 <br> 튜토리얼 구성 및 기능 <br> 퀘스트 기능 |  

---

## 🎮 게임 소개  

> **Back_tO_the_Gungeon** 은 **탑다운 로그라이크 슈팅 게임**입니다.  
> 플레이어는 랜덤한 방들을 지나며 몬스터들을 처지하고 전진해야합니다,  
> 레벨을 클리어하며 강력한 보스와 맞서 싸우게 됩니다.  

### 🔹 특징  
- 랜덤 생성되는 방 기반 던전  
- 간편한 조작  
- 도전적인 보스전 
- 2D 스타일의 그래픽  

---

## 🛠 트러블슈팅  
 
#### 1️⃣ **Navigation Mesh가 생성되는 맵마다 적용이 되지 않는 현상**
- 문제: AI Navigion Mesh가 제대로 적용되지 못해 적이 움직이지 않는 현상이 발생
- 원인: 씬을 시작할 때 기본적인 맵이 존재하지 않아 시작 시 Enemy Manager 오브젝트를 통해 생성되는 Nav Mesh를 생성할 공간이 정해지지 않아 발생
- 해결: 맵을 생성할 때 Nav Mesh도 같이 인식할 수 있도록 전역으로 관리하던 Enemy Manager를 맵 프리팹마다 삽입, 맵 생성 시 Nav Mesh도 같이 생성되게 하여 해결
#### 2️⃣ **총알(투사체)의 충돌 누락**
- 문제: 총알의 충돌이 누락되는 현상 발생
- 원인: 총알의 RigidBody의 충돌검사 방식 설정 오류로 인해 총알이 충돌되지 않았음
- 해결: RigidBody의 충돌검사 방식을 Continuous로 변경하여 총알의 충돌을 개선함.
#### 3️⃣ **보상 화면 선택 시 오류 발생**
- 문제: 플레이어의 보상을 선택하는 보상 화면에서 보상 선택 시 게임 오브젝트에 접근이 불가한 문제 발생
- 원인: 보상 선택 시 비동기 처리 로직이 멀티 쓰레드로 인해 게임 오브젝트에 접근이 불가능한 현상
- 해결: 게임 오브젝트에 접근이 불가한 점을 스크립트 역전 주입으로 해결
#### 4️⃣ **몬스터 애니메이션 미작동**
- 문제: 몬스터의 공격 애니메이션이 작동하지 않는 현상 발생
- 원인: AnimationHandler의 참조 누락
- 해결: AnimationHandler를 필수메서드와 Hash정보를 담은 추상클래스로하여 각 AinmationHandler에 상속하고 필수기능이 누락되거나 참조값이 달라지는 현상 방지
#### 5️⃣ **조이스틱 동시 관리 문제**
- 문제: 터치 조이스틱 기능에서 UI오브젝트를 동시에 관리하기 어려운 문제 발견
- 원인: worldToScreenPoint로 구현시 모바일 환경에서 다중 터치 호환 문제가 발생
- 해결: unity EventSystem 인터페이스를 활용하여 해결 모바일 환경에서의 다중 터치 호환 문제 해결
#### 6️⃣ **애니메이션 실행 문제**
- 문제: 두가지 애니메이션 동시 실행시 하나의 애니메이션만 실행
- 원인: 애니메이터 레이어를 추가하지 않은 상태로 두가지 애니메이션이 중복되어 하나만 출력됨
- 시도: 애니메이터 레이어 추가. 하지만 레이어 추가 시 레이어마다 엔트리 애니메이션이 자동으로 지정되는 문제점 발생
- 해결: Idle_Dummy 상태를 추가하여 추가 레이어에서 기본 상태를 유지시키는 것으로 해결
#### 7️⃣ **무기, 스킨 변경 문제**
- 문제: 무기 및 플레이어 컴포넌트 내에 게임오브젝트 리스트로 관리 시 성능 저하 및 관리가 난해한 문제
- 해결: Scriptable Class를 활용해 데이터 컨테이너를 활용해 문제를 해결

---

## ⏯️ 게임 시연 영상



---

## 📎 프로젝트 실행 방법  

1. **Unity 필요(22.3.17버전 권장)**  
2. **Lobby 씬에서 게임시작**
