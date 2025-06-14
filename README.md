# 유니티 2D Top-Down RPG 

## 프로젝트 개요
- **목표**: 2D Top-Down 시점의 RPG 기본 틀을 구현해보기 
- **핵심 흐름**:  
  1. 게임 시작 -> 홈씬에서 기본적인 테스트 -> 이동시 CrossMap으로 이동
  2. CrossMap씬 → 동/남/북 경로별 채집 맵 (광산, 숲 등)
  3. 던전 탐험 및 파밍  
- **아트스타일**: 16×16 픽셀 단위 픽셀 아트(픽셀 퍼펙트)  

---

## 프로젝트 구조 및 설계 방향
	```
	/cute-rpg
	├─ Assets
	│  ├─ Scenes
	│  │   ├─ Home.unity
	│  │   ├─ CrossMap.unity
	│  │   ├─ Mine.unity
	│  │   └─ ... (Forest, Dungeon, etc.)
	│  ├─ Sprites
	│  │   ├─ Tiles (Ground, Obstacle)
	│  │   └─ Player (Base, Hair, Body, ...)
	│  ├─ Scripts
	│  │   ├─ Data
	│  │   │   ├─ ItemData.cs
	│  │   │   └─ ItemDatabase.cs
	│  │   ├─ Editor
	│  │   │   └─ ItemDataImporter.cs
	│  │   ├─ Managers
	│  │   │   ├─ GameManager.cs
	│  │   │   ├─ InputManager.cs
	│  │   │   ├─ InventoryManager.cs
	│  │   │   └─ UIManager.cs
	│  │   ├─ World
	│  │   │   ├─ ItemPickup.cs
	│  │   │   └─ MapTransition.cs
	│  │   ├─ UI
	│  │   │   └─ InventoryUI.cs
	│  │   ├─ Player
	│  │   │   └─ PlayerController.cs
	│  │   ├─ Camera
	│  │   │   └─ CameraFollow.cs
	│  │   ├─ System
	│  │   │   └─ SceneTransition.cs
	│  │   └─ Utils
	│  │       └─ IconLoader.cs
	│  ├─ Prefabs
	│  │   ├─ Player.prefab
	│  │   └─ UI (InventorySlot, InventoryCanvas, ...)
	│  └─ Resources
	│      ├─ Items (SO 에셋)
	│      └─ ItemDatabase.asset
	└─ README.md
	```

### 1. 타일맵 기반 월드 구성
- **GroundTilemap**: 바닥 용도, Collider 제거  
- **ObstacleTilemap**: 절벽/울타리/물 등 통과 불가 지형, Tilemap Collider 2D
- **레이어링**:  
  - “Obstacle” 레이어를 만들어 플레이어가 충돌  
  - Physics 2D Layer Collision Matrix에서 Player ↔ Obstacle 충돌 허용

### 2. 플레이어 캐릭터
- **계층 구조 (Player.prefab)**
	```
	Player (GameObject)
	├ Rigidbody2D (Dynamic, Gravity=0, Interpolation=None/Interpolate)
	├ BoxCollider2D
	├ Animator (Controller: PlayerAnimator.controller)
	├ PlayerController.cs
	└─ BaseSprite (SpriteRenderer, Order=0)
	HeadSprite (SpriteRenderer, Order=1)
	BodySprite (SpriteRenderer, Order=2)
	PantsSprite (SpriteRenderer, Order=3)
	ShoesSprite (SpriteRenderer, Order=4)
	HandsSprite (SpriteRenderer, Order=5)
	```
- **애니메이션 (Idle / Run)**
- 각 레이어별 32×32 프레임 6개  
- Animation Clip:  
  - Idle: `Player_Idle.anim` (프레임0~5, Loop)  
  - Run: `Player_Run.anim` (프레임0~5, Loop)
- Animator Controller:  
  - Parameter `Speed` (Float)  
  - Idle ↔ Run 전환 (Speed > 0.1 / Speed < 0.1)

- **PlayerController.cs**
- `Update()`: 입력 처리 → `animator.SetFloat("Speed", value)`  
- `FixedUpdate()`: 물리 이동

### 3. 카메라 시스템
- **Pixel Perfect Camera** (패키지 설치 → Main Camera 컴포넌트 추가)
- Assets Pixels Per Unit = 16  
- Reference Resolution (320×180)

- **CameraFollow.cs**
- `LateUpdate()`:  
  1. `desiredPos = target.position + offset`  
  2. `smoothedPos = Lerp(현재위치, desiredPos, smoothSpeed)`  
  3. `Mathf.Round(...)` 후 `transform.position` 설정  

### 4. 싱글턴 패턴 기반 매니저
- **GameManager.cs (Singleton)**
```csharp
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

	[Header("Core Managers")]
    public InputManager inputManager;
    public UIManager uiManager;
    public QuestManager questManager;
    // …추가 매니저 및 핸들링
}
```

### 향후 확장 예정
- **씬**
- 기존 홈씬을 벗어나 모험 시작
- 숲, 광산, 바다, 던전 정도로 채집맵3개, 던전맵 1개 구현 예정

- **애니메이션**
- 플레이어 상하좌우 애니메이션 추가
- 플레이어 공격 애니메이션 추가
- 오브젝트 애니메이션 추가 (후순위)

- **매니저**
- 인벤토리 - 진행중
- UI - 진행중
- 몬스터
- 세이브&로드
- 퀘스트