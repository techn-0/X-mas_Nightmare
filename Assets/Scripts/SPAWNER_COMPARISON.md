# 스포너 비교 가이드

## 🎯 어떤 스포너를 사용해야 할까?

### ✅ SnowmanSpawner (기본 버전) - **추천**
**이런 경우 사용:**
- 간단하고 빠르게 구현하고 싶을 때
- 계속해서 몬스터가 스폰되는 게임
- 서바이벌/엔드리스 모드
- 복잡한 설정이 필요 없을 때

**장점:**
- ✅ 코드가 간단함
- ✅ 설정이 쉬움
- ✅ 즉시 사용 가능
- ✅ 성능 최적화

### 🚀 AdvancedSnowmanSpawner (고급 버전)
**이런 경우 사용:**
- 웨이브 시스템이 필요할 때
- 난이도가 점점 증가해야 할 때
- 특정 웨이브에서만 특정 몬스터 등장
- 웨이브 클리어 보상 시스템
- 스토리 모드

**장점:**
- ✅ 웨이브 시스템
- ✅ 난이도 자동 증가
- ✅ 웨이브별 몬스터 제한
- ✅ 이벤트 시스템
- ✅ 두 가지 모드 (일반/웨이브)

---

## 📊 기능 비교표

| 기능 | SnowmanSpawner | AdvancedSnowmanSpawner |
|------|----------------|------------------------|
| **기본 스폰** | ✅ | ✅ |
| **확률 기반 스폰** | ✅ | ✅ |
| **스폰 포인트** | ✅ | ✅ |
| **최대 몬스터 수 제한** | ✅ | ✅ |
| **웨이브 시스템** | ❌ | ✅ |
| **난이도 자동 증가** | ❌ | ✅ |
| **웨이브별 몬스터** | ❌ | ✅ |
| **이벤트 시스템** | ❌ | ✅ |
| **학습 난이도** | 쉬움 | 보통 |
| **코드 복잡도** | 낮음 | 중간 |

---

## 🎮 사용 예시

### 시나리오 1: 서바이벌 게임
```
추천: SnowmanSpawner

설정:
- Spawn Interval: 2초
- Max Active Monsters: 30
- Auto Start: ✅
```

### 시나리오 2: 웨이브 디펜스 게임
```
추천: AdvancedSnowmanSpawner

설정:
- Use Wave Mode: ✅
- Wave 1: 10마리, 3초 간격
- Wave 2: 15마리, 2초 간격
- Wave 3: 20마리, 1.5초 간격
```

### 시나리오 3: 보스 스테이지 전 잡몹
```
추천: SnowmanSpawner

설정:
- Auto Start: ❌
- 코드로 제어:
  spawner.StartSpawning();
  보스 등장시 spawner.StopSpawning();
```

### 시나리오 4: 점점 어려워지는 게임
```
추천: AdvancedSnowmanSpawner

설정:
- Use Wave Mode: ❌
- Increase Difficulty: ✅
- Difficulty Increase Rate: 0.01
- Min Spawn Interval: 0.5초
```

---

## 🔄 마이그레이션 (기본 → 고급)

기본 버전에서 고급 버전으로 전환하려면:

1. **GameObject에 AdvancedSnowmanSpawner 추가**
2. **기존 설정 복사:**
   - Monsters 리스트
   - Spawn Interval
   - Spawn Point Count
   - Spawn Radius
   - Max Active Monsters

3. **새로운 기능 설정:**
   - Use Wave Mode: 선택
   - Increase Difficulty: 선택

4. **기존 SnowmanSpawner 제거 또는 비활성화**

---

## 💡 추천 설정

### 처음 시작하는 경우
→ **SnowmanSpawner** 사용
- 간단하고 빠르게 테스트 가능
- 나중에 고급 버전으로 쉽게 전환 가능

### 완성도 높은 게임을 만들 경우
→ **AdvancedSnowmanSpawner** 사용
- 웨이브 시스템으로 긴장감 UP
- 클리어 목표가 명확함
- 보상 시스템 추가하기 좋음

---

## 🎯 빠른 선택 가이드

**Q: 게임에 "웨이브" 개념이 있나요?**
- YES → AdvancedSnowmanSpawner
- NO → 다음 질문으로

**Q: 시간이 지날수록 어려워져야 하나요?**
- YES → AdvancedSnowmanSpawner
- NO → 다음 질문으로

**Q: 간단하게 만들고 싶나요?**
- YES → SnowmanSpawner ✅
- NO → AdvancedSnowmanSpawner

---

## 📝 최종 추천

### 🥇 1순위: SnowmanSpawner
- 대부분의 경우 충분함
- 간단하고 효율적
- **지금 당장 시작하기 좋음**

### 🥈 2순위: AdvancedSnowmanSpawner
- 웨이브 시스템 필요시
- 복잡한 게임 디자인
- 확장성 중요시

**결론: 일단 SnowmanSpawner로 시작하고, 필요하면 나중에 업그레이드!**

