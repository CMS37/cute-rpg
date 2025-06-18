# Unity 2D Top-Down RPG

## 프로젝트 개요
- **목표**: 2D 탑다운 RPG의 기본 시스템과 확장 가능한 구조 구현
- **핵심 흐름**:  
  1. 홈씬에서 시작 → CrossMap 이동 → 각 채집/던전 맵 탐험 및 파밍
  2. 픽셀 퍼펙트 16×16 아트스타일

---

## 프로젝트 구조 및 설계

```
/cute-rpg
├─ Assets
│  ├─ Scenes
│  ├─ Sprites
│  ├─ Scripts
│  │   ├─ Actors
│  │   │   ├─ MonsterController.cs
│  │   │   └─ MonsterStates (Idle, Move, Attack, Hit, Dead 등)
│  │   ├─ Player
│  │   │   └─ PlayerController.cs
│  │   ├─ Interfaces
│  │   │   └─ IState.cs
│  │   ├─ System
│  │   │   └─ StateMachine.cs
│  │   ├─ Managers
│  │   │   ├─ GameManager.cs
│  │   │   ├─ InputManager.cs
│  │   │   ├─ InventoryManager.cs
│  │   │   └─ UIManager.cs
│  │   ├─ Stats
│  │   │   └─ CharacterStats.cs
│  │   ├─ Data
│  │   │   └─ (아이템, 드랍테이블 등 SO)
│  │   ├─ World
│  │   │   └─ MapTransition.cs
│  │   └─ UI
│  ├─ Prefabs
│  └─ Resources
└─ README.md
```

---

## 주요 시스템 및 특징

- **FSM(상태머신) 기반 몬스터 AI**  
  - Idle, Move, Attack, Hit, Dead 등 상태별 동작 분리
  - MonsterController 하나로 다양한 몬스터 프리팹에 적용 가능
- **매니저 구조**  
  - GameManager(싱글턴), InputManager, UIManager 등 책임 분리
  - 입력, UI, 게임 상태 등 각자 역할에 집중
- **이벤트 기반 처리**  
  - CharacterStats의 OnDamaged, OnDeath 등 이벤트로 상태머신, UI 등과 느슨하게 연결
- **ScriptableObject 데이터 관리**  
  - 아이템, 드랍테이블 등 SO로 관리, Inspector에서 손쉽게 연결
- **맵/씬 전환, 경계 처리, 픽셀 퍼펙트 카메라 등 2D RPG 필수 시스템 구현**

---

## 향후 개발 예정

- **플레이어 FSM(상태머신) 패턴 적용**
- **몬스터 드랍테이블(아이템 드랍/확률/수량) 구현**
- **NPC 상호작용 및 대화 시스템**
- 퀘스트, 세이브/로드, 스킬 등 추가 확장