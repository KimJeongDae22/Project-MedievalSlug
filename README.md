# 🎮 MedievalSlug

2D 액션 어드벤처 게임으로, 활과 근접 무기를 사용해 몬스터들과 전투하고 퀘스트를 완료하는 게임입니다.

## ✨ 주요 기능

### 🏹 전투 시스템
- **원거리 공격**: 다양한 화살 타입 (일반, 불, 얼음, 독)
- **근접 공격**: 검을 이용한 근거리 전투
- **탑승 시스템**: 전차 탑승으로 강력한 화력 지원

### 🎯 게임플레이
- **몬스터 처치**: AI 기반 몬스터와의 전투
- **아이템 수집**: 체력, 무기, 점수 아이템
- **퀘스트 시스템**: NPC와의 상호작용을 통한 미션 수행
- **파괴 가능한 환경**: 오브젝트 파괴로 아이템 획득

### 🚗 탑승 시스템
- F키로 전차 탑승/하차
- 탑승 중 화살 개수 무한, 전차 근접 공격
- 전차 전용 UI 및 체력 시스템

## 🎮 조작법

| 키 | 기능 |
|---|---|
| 방향키 | 이동 |
| Space | 점프 |
| A | 근접 공격 |
| S | 원거리 공격 |
| F | 전차 탑승/하차 |
| E | 포탈 이동 |

## 🏗️ 프로젝트 구조

### 📁 Core Systems
- **CharacterManager**: 플레이어 컴포넌트 통합 관리
- **GameManager**: 게임 상태 및 점수 관리
- **UIManager**: UI 시스템 통합 관리
- **SceneLoadManager**: 씬 전환 및 로딩 시스템

### 📁 Player
- **PlayerController**: 플레이어 이동 및 입력 처리
- **PlayerRangedHandler**: 원거리 무기 시스템
- **PlayerMeleeHandler**: 근접 무기 시스템
- **PlayerStatHandler**: 플레이어 스탯 관리
- **PlayerItemCollector**: 아이템 획득 처리

### 📁 Monster
- **Monster**: 몬스터 기본 클래스
- **MonsterStateMachine**: AI 상태 머신
- **MonsterSpawner**: 몬스터 생성 시스템

### 📁 Item & Quest
- **ItemDropManager**: 아이템 드롭 시스템
- **QuestManager**: 퀘스트 관리 시스템
- **NPC**: NPC 상호작용 시스템

### 📁 Vehicle
- **VehicleController**: 전차 시스템
- **VehicleItemCollector**: 전차 탑승 중 아이템 처리

## 🛠️ 기술 스택

- **Unity 2022.3 LTS**
- **C# 스크립팅**
- **Unity Input System**
- **Object Pooling**: 성능 최적화
- **Singleton Pattern**: 매니저 클래스들
- **State Machine**: 몬스터 AI

## 🎯 주요 디자인 패턴

### Singleton Pattern
게임 매니저들의 전역 접근을 위해 사용
```csharp
public class GameManager : Singleton<GameManager>
```

### Object Pooling
투사체와 사운드 이펙트의 성능 최적화
```csharp
public class ObjectPoolManager : Singleton<ObjectPoolManager>
```

### State Machine
몬스터 AI의 체계적인 상태 관리
```csharp
public abstract class MonsterBaseState
```

## 🎨 씬 구성

- **IntroScene**: 게임 인트로 영상
- **StartScene**: 메인 메뉴
- **CharacterSelectScene**: 캐릭터 선택
- **MainScene**: 메인 게임플레이
- **BossScene**: 보스 전투
- **EndingCreditScene**: 엔딩 크레딧

## 🎵 오디오 시스템

- **AudioManager**: BGM 및 SFX 관리
- **SoundSource**: 오디오 풀링 시스템
- 무기별 차별화된 사운드 이펙트

## 👥 개발팀

**팀명**: 1박 3일

---

🎮 재미있게 플레이하세요!
