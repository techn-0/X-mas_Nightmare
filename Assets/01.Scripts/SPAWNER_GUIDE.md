# 눈사람 스포너 사용 가이드

## 📦 설치 방법

1. **빈 GameObject 생성**
   - Hierarchy에서 우클릭 → Create Empty
   - 이름을 "SnowmanSpawner"로 변경

2. **스크립트 추가**
   - SnowmanSpawner 스크립트를 GameObject에 추가

## ⚙️ 인스펙터 설정

### 📌 스폰할 몬스터 설정
- **Monsters** 리스트 크기 설정 (예: 2개)
  - **Element 0:**
    - Prefab: `Snowamn` 프리팹 드래그
    - Spawn Weight: 70 (70% 확률)
  - **Element 1:**
    - Prefab: `Snowamn_Boom` 프리팹 드래그
    - Spawn Weight: 30 (30% 확률)

> 💡 Spawn Weight는 상대적 확률입니다. 70:30 = 70% vs 30%

### 🎯 스폰 설정
- **Spawn Interval**: 3초 (스폰 간격)
- **Spawn Point Count**: 4개 (스폰 포인트 개수)
- **Spawn Radius**: 20m (스포너로부터 거리)

### 🚫 제한 설정
- **Max Active Monsters**: 20 (최대 동시 몬스터 수, 0=무제한)
- **Auto Start**: ✅ 체크 (게임 시작시 자동 스폰)

### 🐛 디버그
- **Show Spawn Points**: ✅ 체크 (스폰 포인트 시각화)

## 🎮 사용 방법

### 자동 스폰
```csharp
// Auto Start를 체크하면 자동으로 시작됩니다
```

### 코드로 제어
```csharp
SnowmanSpawner spawner = FindObjectOfType<SnowmanSpawner>();

// 스폰 시작
spawner.StartSpawning();

// 스폰 중지
spawner.StopSpawning();

// 모든 몬스터 제거
spawner.ClearAllMonsters();
```

## 🎨 Scene 뷰에서 확인
- **노란색 원**: 스폰 범위
- **초록색 구체**: 스폰 포인트 위치
- **초록색 선**: 스포너 중심에서 스폰 포인트까지의 연결

## 💡 추천 설정

### 쉬운 난이도
- Spawn Interval: 5초
- Max Active Monsters: 10
- Spawn Point Count: 4

### 보통 난이도
- Spawn Interval: 3초
- Max Active Monsters: 20
- Spawn Point Count: 6

### 어려운 난이도
- Spawn Interval: 1.5초
- Max Active Monsters: 30
- Spawn Point Count: 8

## 🔧 확률 시스템 예시

### 균등 확률
- 눈사람: Weight 50
- 폭발 눈사람: Weight 50
- 결과: 50% vs 50%

### 일반 몬스터 위주
- 눈사람: Weight 80
- 폭발 눈사람: Weight 20
- 결과: 80% vs 20%

### 3종류 몬스터
- 일반: Weight 60
- 폭발: Weight 30
- 보스: Weight 10
- 결과: 60% vs 30% vs 10%

## ⚠️ 주의사항

1. **프리팹 필수**: Monsters 리스트에 최소 1개 이상의 프리팹 등록
2. **Weight 값**: 모든 Weight가 0이면 스폰되지 않음
3. **성능**: Max Active Monsters로 성능 관리 (추천: 20-30)
4. **스폰 범위**: 플레이어가 볼 수 있는 범위 밖에서 스폰 추천

## 🚀 고급 기능

### 웨이브 시스템 추가 (선택사항)
나중에 웨이브 시스템을 추가하려면:
- 시간에 따라 `spawnInterval` 감소
- 웨이브마다 `maxActiveMonsters` 증가
- 특정 웨이브에서만 특정 몬스터 스폰

### 이벤트 기반 스폰
특정 조건에서만 스폰하려면:
- `autoStart`를 ✅ 해제
- 코드에서 `StartSpawning()` 호출

```csharp
// 플레이어가 특정 지역에 들어갈 때
void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("Player"))
    {
        spawner.StartSpawning();
    }
}
```

## 📝 체크리스트

- [ ] SnowmanSpawner GameObject 생성
- [ ] SnowmanSpawner 스크립트 추가
- [ ] 몬스터 프리팹 2개 이상 등록
- [ ] Spawn Weight 설정 (합이 100 추천)
- [ ] Spawn Interval 설정 (3초 추천)
- [ ] Spawn Point Count 설정 (4-6개 추천)
- [ ] Max Active Monsters 설정 (20 추천)
- [ ] Scene 뷰에서 스폰 포인트 확인
- [ ] 게임 테스트 실행

완료! 🎉

