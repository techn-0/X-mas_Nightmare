# 풀아머 산타 보스 - 빠른 시작 가이드

## 🎯 빠른 설정 (5분 완성)

### 1단계: 컴포넌트 추가
산타 GameObject에 다음을 추가:
```
- EnemyHealth (체력 500 이상)
- BossSantaAI
- BossDeathSequence
- (선택) BossSantaSetupHelper
```

### 2단계: 발사 위치 설정

#### 자동 설정 (BossSantaSetupHelper 사용)
1. Left Arm Bone, Right Arm Bone, Back Bone 할당
2. Inspector 우클릭 → 컨텍스트 메뉴:
   - "Create Fire Points"
   - "Create Missile Points"
   - "Create Explosion Points"
3. 생성된 Transform들을 BossSantaAI와 BossDeathSequence에 할당

#### 수동 설정
**머신건**:
- 왼쪽/오른쪽 팔에 빈 GameObject → BossSantaAI에 할당

**미사일**:
- 등에 빈 GameObject 10개 → BossSantaAI의 Missile Fire Points 배열에 할당

**폭발**:
- 몸 곳곳에 빈 GameObject → BossDeathSequence의 Explosion Points 배열에 할당

### 3단계: 프리팹 할당

**BossSantaAI**:
- Bullet Prefabs: 배열 크기 5로 설정
  - Slot 0-4: 총알 프리팹들 (예: `Snowball` 5번 또는 서로 다른 프리팹)
- Missile Prefab: `Snowball` 프리팹
  - 플레이어를 향해 직선으로 날아감

**BossDeathSequence**:
- Explosion Effect Prefab: `Explosion05 BOOOOOM Red` 프리팹

### 4단계: 레이어 설정
총알과 미사일의 Target Layer를 "Player"로 설정

---

## 🎮 보스 패턴

### 기본 패턴 (체력 > 50%)
1. 플레이어에게 천천히 접근
2. 머신건 2회 난사 (각 10발, 초당 5발)
3. 1초 재장전 그로기
4. 반복

### 페이즈 2 (체력 ≤ 50%)
1. 머신건 2회 난사
2. 1초 재장전
3. **미사일 10발 포격** (1초 간격)
4. 반복

### 사망
여러 부위에서 순차적으로 폭발 후 파괴

---

## ⚙️ 주요 설정값

| 항목 | 기본값 | 설명 |
|------|--------|------|
| Move Speed | 1f | 매우 느린 이동 |
| Bullets Per Second | 5 | 초당 5발 |
| Shots Per Salvo | 2 | 2회 난사 |
| Reload Time | 1초 | 그로기 시간 |
| Missile Count | 10 | 미사일 개수 |
| Missile Fire Interval | 1초 | 미사일 간격 |
| Health Threshold | 0.5 | 50%에서 미사일 활성화 |

---

## 🔧 난이도 조정

**쉽게**:
- Move Speed: 0.5
- Bullets Per Salvo: 5
- Reload Time: 2초

**어렵게**:
- Move Speed: 2
- Bullets Per Second: 10
- Reload Time: 0.5초
- Missile Fire Interval: 0.5초

---

## ✅ 체크리스트

- [ ] EnemyHealth 추가 및 체력 설정
- [ ] BossSantaAI 추가
- [ ] BossDeathSequence 추가
- [ ] 발사 위치 생성 (팔 2개, 미사일 10개)
- [ ] 폭발 위치 생성 (원하는 개수)
- [ ] 총알 프리팹 배열 할당 (5개 권장)
- [ ] 미사일 프리팹 할당
- [ ] 폭발 이펙트 할당
- [ ] Target Layer 설정
- [ ] Player 태그 확인

---

## 🐛 문제 해결

**총알이 안 나감**:
- Fire Point가 제대로 할당되었는지 확인
- Bullet Prefabs 배열에 프리팹이 할당되었는지 확인
- 총알 프리팹에 Projectile 컴포넌트 있는지 확인

**총알이 한 종류만 나옴**:
- Bullet Prefabs 배열 크기가 5인지 확인
- 각 슬롯에 서로 다른 프리팹이 할당되었는지 확인

**미사일이 발사 안 됨**:
- 체력이 50% 이하인지 확인
- Missile Fire Points 배열 크기 확인

**플레이어가 안 맞음**:
- Target Layer가 "Player" 레이어와 일치하는지 확인
- Player GameObject에 "Player" 태그 있는지 확인

**폭발이 안 나옴**:
- Explosion Points 배열에 Transform들이 할당되었는지 확인
- Explosion Effect Prefab이 할당되었는지 확인

