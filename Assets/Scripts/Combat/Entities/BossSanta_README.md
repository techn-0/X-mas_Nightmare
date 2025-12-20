# 풀아머 산타 보스 시스템

## 개요
풀아머 산타는 머신건 난사와 미사일 포격을 사용하는 보스 적입니다.

## 구성 요소

### 1. BossSantaAI.cs
보스의 메인 AI 스크립트입니다.

#### 주요 기능
- **이동**: 플레이어를 천천히 추적 (매우 느림)
- **머신건 공격**: 양 팔에서 교차로 총알 난사
- **미사일 포격**: 체력 50% 이하에서 등에서 미사일 발사
- **그로기 타임**: 2회 난사 후 1초 재장전 그로기

#### 인스펙터 설정

**Target Settings**
- `Player`: 플레이어 Transform (자동 검색됨)

**Movement Settings**
- `Move Speed`: 이동 속도 (기본값: 1f - 매우 느림)
- `Rotation Speed`: 회전 속도 (기본값: 3f)
- `Attack Stop Distance`: 공격 시작 거리 (기본값: 8f)

**Machine Gun Settings**
- `Bullet Prefabs`: 총알 프리팹 배열 (5개 권장, 순환 발사)
- `Left Arm Fire Point`: 왼쪽 팔 발사 위치 Transform
- `Right Arm Fire Point`: 오른쪽 팔 발사 위치 Transform
- `Bullets Per Second`: 초당 발사 속도 (기본값: 5 = 초당 5발)
- `Shots Per Salvo`: 난사 횟수 (기본값: 2)
- `Bullets Per Salvo`: 각 난사당 총알 수 (기본값: 10)
- `Reload Time`: 재장전 그로기 시간 (기본값: 1초)

**Missile Settings**
- `Missile Prefab`: 미사일 프리팹 (Projectile 컴포넌트 사용)
- `Missile Fire Points`: 미사일 발사 위치 배열 (10개 권장)
- `Missile Count`: 발사할 미사일 개수 (기본값: 10)
- `Missile Fire Interval`: 미사일 발사 간격 (기본값: 1초)
- `Health Threshold For Missiles`: 미사일 활성화 체력 비율 (기본값: 0.5 = 50%)

**참고**: 미사일은 지정된 위치에서 플레이어를 향해 직선으로 날아갑니다.

**Vulnerability**
- `Is Vulnerable While Firing`: 발사 중 무방비 상태 (기본값: true)

### 2. BossDeathSequence.cs
보스 사망 시 순차적 폭발 이펙트를 재생합니다.

#### 주요 기능
- **순차 폭발**: 여러 부위에서 차례대로 폭발
- **커스터마이징**: 폭발 위치, 개수, 간격 조절 가능

#### 인스펙터 설정

**Explosion Settings**
- `Explosion Effect Prefab`: 폭발 이펙트 프리팹
- `Explosion Points`: 폭발 위치 배열 (Transform)
- `Delay Between Explosions`: 폭발 간 딜레이 (기본값: 0.3초)
- `Explosion Effect Duration`: 이펙트 지속 시간 (기본값: 3초)

**Final Destruction**
- `Destroy Delay`: 마지막 폭발 후 파괴 대기 시간 (기본값: 2초)

## 설정 방법

### 1. 기본 설정

1. 풀아머 산타 GameObject에 다음 컴포넌트 추가:
   - `EnemyHealth` (체력 관리)
   - `BossSantaAI` (보스 AI)
   - `BossDeathSequence` (사망 시퀀스)

2. EnemyHealth 설정:
   - Max HP: 500 이상 권장

### 2. 발사 위치 설정

#### 머신건 발사 위치
1. 산타의 왼쪽 팔에 빈 GameObject 생성 → "LeftArmFirePoint"
2. 산타의 오른쪽 팔에 빈 GameObject 생성 → "RightArmFirePoint"
3. BossSantaAI에 할당

#### 미사일 발사 위치
1. 산타의 등에 빈 GameObject 10개 생성:
   - "MissilePoint_01"
   - "MissilePoint_02"
   - ...
   - "MissilePoint_10"
2. BossSantaAI의 Missile Fire Points 배열에 순서대로 할당

### 3. 폭발 위치 설정

1. 산타의 몸 여러 부위에 빈 GameObject 생성:
   - "ExplosionPoint_Head"
   - "ExplosionPoint_Chest"
   - "ExplosionPoint_LeftArm"
   - "ExplosionPoint_RightArm"
   - "ExplosionPoint_Legs"
   - 등...
2. BossDeathSequence의 Explosion Points 배열에 원하는 순서대로 할당

### 4. 프리팹 설정

#### 총알 프리팹
- **다중 총알 사용** (5개 권장):
  1. 서로 다른 총알 프리팹 5개 준비 (또는 같은 프리팹도 가능)
  2. 각각 `Projectile` 컴포넌트 필요
  3. BossSantaAI의 `Bullet Prefabs` 배열 크기를 5로 설정
  4. 각 슬롯에 총알 프리팹 할당
  5. 발사 시 0→1→2→3→4→0... 순서로 순환 발사
  
- **단일 총알 사용**:
  1. 배열 크기를 1로 설정
  2. 하나의 총알 프리팹만 할당
  
- 기존 Snowball 프리팹 사용 가능
- 또는 새로운 총알 프리팹 생성:
  1. GameObject에 `Projectile` 컴포넌트 추가
  2. Damage, Speed, Target Layer 설정
  3. 프리팹으로 저장

#### 미사일 프리팹
- Snowball 프리팹 사용 (또는 복사하여 크기/색상 변경)
- `Projectile` 컴포넌트만 있으면 됨
- 발사 시 지정된 위치에서 플레이어를 향해 직선으로 날아감

**간단 설정**:
1. Snowball 프리팹 복사
2. 이름 변경 (예: `Missile`)
3. 원하면 크기/색상/속도 조정
4. BossSantaAI의 Missile Prefab에 할당

#### 폭발 이펙트
- 프로젝트의 "Explosion05 BOOOOOM Red" 프리팹 사용 가능
- 또는 원하는 폭발 이펙트 할당

## 보스 패턴

### 체력 50% 초과
1. 플레이어에게 천천히 다가감
2. 공격 거리(8m) 도달 시 공격 시작
3. 머신건 난사 (양 팔 교차 발사):
   - 초당 5발
   - 각 난사당 10발
   - 2회 난사
4. 1초 재장전 그로기
5. 패턴 반복

### 체력 50% 이하
1. 머신건 2회 난사
2. 1초 재장전 그로기
3. **미사일 포격 추가**:
   - 10개 미사일 발사
   - 1초 간격으로 발사
   - 발사 중 무방비 상태
4. 패턴 반복

### 사망 시
1. 설정된 폭발 위치에서 순차적으로 폭발 이펙트 재생
2. 설정한 딜레이만큼 대기 후 오브젝트 파괴

## 팁

### 밸런스 조정
- **난이도 상승**:
  - Move Speed 증가
  - Bullets Per Second 증가
  - Reload Time 감소
  - Missile Fire Interval 감소
  
- **난이도 하락**:
  - Move Speed 감소
  - Bullets Per Salvo 감소
  - Reload Time 증가
  - Health Threshold For Missiles 낮춤 (0.3 = 30%)

### 비주얼 강화
- Trail Effect Prefab에 파티클 이펙트 추가
- 총구 화염 이펙트 추가
- 재장전 시 애니메이션 추가

### 디버깅
- Scene 뷰의 Gizmos로 공격 범위 확인
- Console 로그로 패턴 진행 상황 확인
- 폭발 위치는 Scene 뷰에서 빨간 구체로 표시됨

## 필수 컴포넌트
- EnemyHealth
- Collider (트리거가 아닌 일반)
- 총알/미사일 프리팹에 Projectile 컴포넌트
- Player GameObject에 "Player" 태그

## 주의사항
- 미사일 발사 중에는 무방비 상태가 되므로 플레이어가 공격할 기회를 줌
- 발사 위치(Fire Points)는 정확히 설정해야 함
- Target Layer를 "Player" 레이어로 설정 필요
- 폭발 위치는 원하는 순서대로 배열에 추가 (첫 번째부터 순차 폭발)

