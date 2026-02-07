# 🎄 눈사람 스포너 시스템 - 완벽 가이드

## 📦 구현 완료!

총 **4개의 파일**이 생성되었습니다:

### 1️⃣ **SnowmanSpawner.cs** ⭐ 추천
간단하고 효율적인 기본 스포너
- 확률 기반 몬스터 스폰
- 스폰 포인트 자동 생성
- 최대 몬스터 수 제한
- 바로 사용 가능!

### 2️⃣ **AdvancedSnowmanSpawner.cs**
웨이브 시스템 포함 고급 스포너
- 웨이브 모드
- 난이도 자동 증가
- 웨이브별 몬스터 설정
- 이벤트 시스템

### 3️⃣ **SpawnerUIController.cs**
웨이브 정보 표시 UI 컨트롤러 (예제)

### 4️⃣ **SpawnerExamples.cs**
다양한 사용 예제 모음

---

## 🚀 빠른 시작 (3분 완성!)

### Step 1: GameObject 생성
1. Hierarchy 우클릭 → Create Empty
2. 이름: "SnowmanSpawner"
3. 위치: 맵 중앙

### Step 2: 스크립트 추가
1. `SnowmanSpawner.cs` 를 GameObject에 드래그

### Step 3: 프리팹 설정
**Inspector에서:**

**Monsters** 리스트:
- Size: `2`
- Element 0:
  - Prefab: `Snowamn` ← 드래그
  - Spawn Weight: `70`
- Element 1:
  - Prefab: `Snowamn_Boom` ← 드래그
  - Spawn Weight: `30`

**Spawn Settings:**
- Spawn Interval: `3`
- Spawn Point Count: `4`
- Spawn Radius: `20`

**Max Active Monsters:** `20`

**Auto Start:** ✅ 체크

### Step 4: 테스트!
▶️ 플레이 버튼 누르기

✅ 눈사람이 스폰되면 성공!

---

## 🎯 주요 기능

### 1. 확률 기반 스폰
```
일반 눈사람: 70%
폭발 눈사람: 30%
```
→ Weight 값으로 확률 조절!

### 2. 스폰 포인트 시각화
Scene 뷰에서:
- 🟡 노란색 원: 스폰 범위
- 🟢 초록색 구체: 스폰 위치
- 🟢 초록색 선: 연결선

### 3. 자동 성능 관리
- 죽은 몬스터 자동 정리
- 최대 수 제한으로 렉 방지
- 효율적인 메모리 관리

### 4. 런타임 제어
```csharp
SnowmanSpawner spawner = FindObjectOfType<SnowmanSpawner>();

spawner.StartSpawning();  // 시작
spawner.StopSpawning();   // 중지
spawner.ClearAllMonsters(); // 전체 제거
```

---

## 🎮 추천 설정

### 🟢 쉬움
```
Spawn Interval: 5초
Max Active: 10
Spawn Points: 4
```

### 🟡 보통
```
Spawn Interval: 3초
Max Active: 20
Spawn Points: 6
```

### 🔴 어려움
```
Spawn Interval: 1.5초
Max Active: 30
Spawn Points: 8
```

---

## 🆚 웨이브 모드가 필요한가요?

### AdvancedSnowmanSpawner 사용하기

**웨이브 설정 예시:**

**Wave 1:**
- Monsters To Spawn: `10`
- Spawn Interval: `2초`
- Prepare Time: `3초`

**Wave 2:**
- Monsters To Spawn: `15`
- Spawn Interval: `1.5초`
- Prepare Time: `5초`

**Wave 3:**
- Monsters To Spawn: `20`
- Spawn Interval: `1초`
- Prepare Time: `5초`

**Use Wave Mode:** ✅ 체크!

---

## 💡 활용 팁

### Tip 1: 플레이어 주변에 스폰
```
Spawner 위치 = 플레이어 위치
Spawn Radius = 20m
```

### Tip 2: 보스 스테이지
```
Auto Start: ❌ 해제
코드에서 제어:
- 보스 등장 전: spawner.StartSpawning()
- 보스 등장 시: spawner.StopSpawning()
```

### Tip 3: 여러 스포너 사용
```
Spawner A: 일반 몬스터 (앞쪽)
Spawner B: 정예 몬스터 (옆쪽)
Spawner C: 보스 전용 (특정 위치)
```

### Tip 4: 시간대별 난이도
```csharp
// 게임 시간에 따라 설정 변경
if (gameTime > 120f) {
    spawner.spawnInterval = 1f;
    spawner.maxActiveMonsters = 50;
}
```

---

## ⚠️ 문제 해결

### 몬스터가 스폰되지 않아요
✅ Monsters 리스트에 프리팹 등록했나요?
✅ Spawn Weight가 0보다 큰가요?
✅ Auto Start가 체크되어 있나요?
✅ Max Active Monsters가 0이 아닌가요?

### 너무 많이 스폰돼요
→ `Max Active Monsters` 값을 줄이세요
→ `Spawn Interval` 값을 높이세요

### 스폰 위치가 이상해요
→ Scene 뷰에서 기즈모 확인
→ `Spawn Radius` 조절
→ Spawner 위치 확인

### 성능이 느려요
→ `Max Active Monsters`를 20 이하로
→ 몬스터 프리팹 최적화
→ 파티클 이펙트 줄이기

---

## 📚 추가 문서

- `SPAWNER_GUIDE.md` - 상세 사용법
- `SPAWNER_COMPARISON.md` - 기본 vs 고급 비교
- `SpawnerExamples.cs` - 코드 예제 모음

---

## ✅ 최종 체크리스트

- [ ] SnowmanSpawner GameObject 생성
- [ ] SnowmanSpawner.cs 스크립트 추가
- [ ] 몬스터 프리팹 등록 (최소 1개)
- [ ] Spawn Weight 설정
- [ ] Spawn Interval 설정 (3초 추천)
- [ ] Spawn Point Count 설정 (4-6개)
- [ ] Max Active Monsters 설정 (20 추천)
- [ ] Auto Start 체크
- [ ] Scene 뷰에서 스폰 포인트 확인
- [ ] 플레이 테스트

---

## 🎉 완료!

이제 게임을 실행하고 눈사람들이 스폰되는 것을 확인하세요!

**문제가 있나요?**
1. Console 창 확인 (경고/에러 메시지)
2. Inspector 설정 재확인
3. 프리팹이 제대로 할당되었는지 확인

**잘 작동하나요?**
- Spawn Interval, Weight 등을 조절하며 실험해보세요!
- 나중에 필요하면 Advanced 버전으로 업그레이드하세요!

즐거운 게임 개발 되세요! 🎮✨

