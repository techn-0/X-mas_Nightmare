# 보스 스폰 타이머 시스템 사용 가이드

## 개요
30초 카운트다운 후 보스를 특정 위치에 스폰하는 시스템입니다.
TextMeshPro(TMP)를 사용하여 UI에 남은 시간을 표시합니다.

## 구성 요소

### 1. BossSpawnTimer.cs
보스 스폰 타이머의 핵심 로직을 담당하는 스크립트입니다.

### 2. BossSpawnTimerUI.cs (옵션)
타이머와 함께 추가적인 UI 효과(색상 변경, 펄스 효과, 경고 메시지 등)를 제공합니다.

## 설정 방법

### 기본 설정

1. **빈 GameObject 생성**
   - Hierarchy에서 우클릭 > Create Empty
   - 이름을 "BossSpawnTimer"로 변경

2. **BossSpawnTimer 컴포넌트 추가**
   - 생성한 GameObject를 선택
   - Inspector에서 Add Component > BossSpawnTimer

3. **필수 설정**
   - **Timer Text**: Canvas에 있는 TextMeshProUGUI 컴포넌트를 드래그 앤 드롭
   - **Boss Prefab**: 스폰할 보스 프리팹을 드래그 앤 드롭
   - **Spawn Point**: 보스가 스폰될 위치의 Transform을 드래그 앤 드롭

### 상세 설정

#### 타이머 설정
- **Spawn Time**: 보스 스폰까지 대기 시간 (기본: 30초)
- **Timer Text**: 시간을 표시할 TextMeshProUGUI
- **Start On Awake**: 게임 시작 시 자동으로 타이머 시작 여부

#### 보스 설정
- **Boss Prefab**: 스폰할 보스 프리팹
- **Spawn Point**: 보스 스폰 위치
- **Use Random Position In Radius**: 랜덤 위치 스폰 사용 여부
- **Spawn Radius**: 랜덤 스폰 반경 (Use Random Position In Radius가 true일 때)

#### UI 설정
- **Timer Panel**: 타이머가 표시될 패널 (카운트다운 중에만 활성화)
- **Timer Format**: 시간 표시 형식 (예: "{0:00}" → "05", "{0:0}" → "5")
- **Timer Prefix Text**: 타이머 앞에 표시될 텍스트 (예: "보스 등장까지: ")
- **Timer Suffix Text**: 타이머 뒤에 표시될 텍스트 (예: "초")

#### 이벤트
- **On Timer Start**: 타이머 시작 시 호출
- **On Boss Spawned**: 보스 스폰 시 호출
- **On Timer Tick**: 매 초마다 호출

#### 사운드 (옵션)
- **Tick Sound**: 초 단위로 재생될 사운드
- **Spawn Sound**: 보스 스폰 시 재생될 사운드
- **Audio Source**: AudioSource 컴포넌트 (없으면 자동 생성)

## UI 설정 예시

### Canvas 구조
```
Canvas
├── BossTimerPanel (Panel)
│   ├── TimerText (TextMeshProUGUI) ← "보스 등장까지: 30초"
│   └── WarningText (TextMeshProUGUI) ← "⚠️ 강력한 적이 다가오고 있습니다!"
└── (기타 UI 요소들)
```

### TextMeshProUGUI 설정
1. Canvas에 우클릭 > UI > TextMeshPro - Text
2. 이름을 "TimerText"로 변경
3. Font Size, Color, Alignment 등을 원하는 대로 설정
4. BossSpawnTimer의 Timer Text 필드에 드래그 앤 드롭

## 스폰 위치 설정

### 방법 1: 빈 GameObject 사용
1. Hierarchy에서 우클릭 > Create Empty
2. 이름을 "BossSpawnPoint"로 변경
3. Scene 뷰에서 원하는 위치로 이동
4. BossSpawnTimer의 Spawn Point에 드래그 앤 드롭

### 방법 2: 기존 오브젝트 사용
- 맵의 특정 위치(예: 플랫폼, 특정 지역 등)의 Transform을 Spawn Point로 사용

### Scene 뷰에서 확인
- BossSpawnTimer GameObject를 선택하면 Scene 뷰에 스폰 위치가 빨간 원으로 표시됩니다
- Use Random Position In Radius가 활성화되어 있으면 반경도 함께 표시됩니다

## 고급 기능 (BossSpawnTimerUI 사용)

### 설정 방법
1. BossSpawnTimer GameObject에 BossSpawnTimerUI 컴포넌트 추가
2. 추가 UI 요소 설정:
   - **Warning Panel**: 경고 메시지를 표시할 패널
   - **Warning Text**: 경고 메시지 TextMeshProUGUI
   - **Timer Canvas Group**: 타이머 페이드 효과용 CanvasGroup

### 제공 기능
- **색상 변경**: 시간에 따라 타이머 색상 자동 변경 (흰색 → 노란색 → 빨간색)
- **펄스 효과**: 시간이 얼마 남지 않았을 때 타이머가 펄스 효과
- **페이드 효과**: 긴급 시간에 깜빡임 효과
- **경고 메시지**: 타이머 시작 시 경고 메시지 표시

## 코드에서 사용하기

### 타이머 수동 시작
```csharp
BossSpawnTimer timer = GetComponent<BossSpawnTimer>();
timer.StartTimer();
```

### 타이머 중지
```csharp
timer.StopTimer();
```

### 즉시 보스 스폰
```csharp
timer.SpawnBossImmediately();
```

### 남은 시간 확인
```csharp
float remainingTime = timer.GetRemainingTime();
Debug.Log($"남은 시간: {remainingTime}초");
```

### 스폰된 보스 참조
```csharp
GameObject spawnedBoss = timer.GetSpawnedBoss();
if (spawnedBoss != null)
{
    // 보스와 상호작용
}
```

## 예제 시나리오

### 시나리오 1: 웨이브 완료 후 보스 스폰
```csharp
public class WaveManager : MonoBehaviour
{
    [SerializeField] private BossSpawnTimer bossTimer;
    
    public void OnWaveComplete()
    {
        Debug.Log("웨이브 완료! 보스 타이머 시작!");
        bossTimer.StartTimer();
    }
}
```

### 시나리오 2: 특정 조건 달성 시 보스 스폰
```csharp
public class GameManager : MonoBehaviour
{
    [SerializeField] private BossSpawnTimer bossTimer;
    private int enemiesKilled = 0;
    
    public void OnEnemyKilled()
    {
        enemiesKilled++;
        
        if (enemiesKilled >= 50)
        {
            bossTimer.StartTimer();
        }
    }
}
```

### 시나리오 3: 버튼 클릭으로 보스 스폰 (테스트용)
```csharp
// BossSpawnTimer의 OnTimerStart 이벤트에 함수 연결하거나
// UI 버튼의 OnClick 이벤트에 BossSpawnTimer.StartTimer() 연결
```

## 문제 해결

### 타이머가 표시되지 않음
- TimerText가 올바르게 연결되었는지 확인
- TextMeshPro가 프로젝트에 임포트되었는지 확인 (Window > TextMeshPro > Import TMP Essential Resources)
- Timer Panel이 활성화되어 있는지 확인

### 보스가 스폰되지 않음
- Boss Prefab이 설정되었는지 확인
- Spawn Point가 설정되었는지 확인
- Console에 에러 메시지가 있는지 확인

### 스폰 위치가 이상함
- Scene 뷰에서 Spawn Point의 위치를 확인
- Use Random Position In Radius가 너무 큰 값으로 설정되지 않았는지 확인

## 팁

1. **테스트**: Start On Awake를 false로 설정하고, UI 버튼으로 타이머를 시작하면 테스트하기 편리합니다.

2. **디버깅**: Console 로그를 통해 타이머 상태를 확인할 수 있습니다.

3. **여러 보스**: 여러 종류의 보스를 스폰하고 싶다면, BossSpawnTimer를 여러 개 만들고 각각 다른 보스 프리팹을 설정하세요.

4. **반복 스폰**: 보스가 죽으면 타이머를 다시 시작하도록 설정할 수 있습니다:
```csharp
public void OnBossDeath()
{
    bossTimer.ResetTimer();
    bossTimer.StartTimer();
}
```

## 추가 개선 아이디어

- 웨이브 시스템과 연동
- 보스 스폰 시 카메라 연출
- 보스 체력 바 자동 연결
- 배경 음악 변경
- 파티클 이펙트 추가

