# 눈사람 AI 시스템 설정 가이드

## 개요
눈사람 몹 AI가 구현되었습니다. 이 시스템은:
- 플레이어를 바라보며 추적합니다
- 멀리 있을 때는 눈덩이를 던집니다
- 가까이 있을 때는 근접 공격을 합니다

## 유니티 에디터 설정 방법

### 1. 눈사람 게임 오브젝트 생성

1. **Hierarchy**에서 우클릭 → **3D Object** → **Create Empty**
2. 이름을 `Snowman`으로 변경
3. **Snowamn.fbx** 모델을 Hierarchy의 Snowman 아래로 드래그하여 자식으로 추가
   - 또는 Snowman 오브젝트의 **Add Component** → **Mesh Filter**와 **Mesh Renderer**를 추가하고 Snowamn 메쉬를 할당

### 2. 필수 컴포넌트 추가

Snowman 오브젝트를 선택하고 **Inspector** 창에서:

#### a) EnemyHealth 컴포넌트
1. **Add Component** 클릭
2. `EnemyHealth` 검색 후 추가
3. 설정:
   - **Health System** → **Max HP**: `100` (원하는 값으로 조정)
   - **Drop Settings**: 드롭할 아이템이 있다면 설정 (선택사항)

#### b) SnowmanAI 컴포넌트
1. **Add Component** 클릭
2. `SnowmanAI` 검색 후 추가
3. 설정:
   - **Player**: Scene의 Player 오브젝트를 드래그 (비어있으면 자동으로 "Player" 태그로 찾음)
   - **Move Speed**: `2` (이동 속도)
   - **Rotation Speed**: `5` (회전 속도)
   - **Ranged Attack Distance**: `10` (원거리 공격 거리)
   - **Melee Attack Distance**: `2` (근접 공격 거리)
   - **Ranged Attack Cooldown**: `2` (원거리 공격 쿨다운, 초)
   - **Melee Attack Cooldown**: `1.5` (근접 공격 쿨다운, 초)
   - **Snowball Prefab**: Assets 폴더의 `Snowball.prefab`을 드래그
   - **Throw Point**: 눈덩이가 발사될 위치 (빈 GameObject 생성 후 할당, 비어있으면 눈사람 중심에서 발사)
   - **Melee Damage**: `20` (근접 공격 데미지)
   - **Melee Range**: `2.5` (근접 공격 감지 범위)

#### c) Collider 추가 (충돌 감지용)
1. **Add Component** → **Capsule Collider** (또는 Box Collider)
2. 크기를 눈사람 모델에 맞게 조정
3. **Is Trigger**는 체크 해제 (물리적 충돌 필요)

### 3. 플레이어 설정 확인

1. **Hierarchy**에서 Player 오브젝트 선택
2. **Inspector** → **Tag**가 `Player`로 설정되어 있는지 확인
   - 만약 Player 태그가 없다면:
     - Tag 드롭다운 → **Add Tag...**
     - **+** 버튼 클릭 → 이름을 `Player`로 입력 → **Save**
     - Player 오브젝트로 돌아가서 Tag를 `Player`로 설정

3. Player 오브젝트에 **PlayerHealth** 컴포넌트가 있는지 확인
   - 없다면 **Add Component** → `PlayerHealth` 추가

4. Player의 **Layer** 확인
   - 눈덩이 프리팹의 **Target Layer**가 Player의 Layer와 일치해야 함
   - 기본적으로 Layer 6 (Bit: 64)으로 설정되어 있음
   - Player 오브젝트의 Layer를 확인하고 필요시 조정

### 4. 눈덩이 발사 위치 설정 (선택사항, 더 정확한 발사를 원할 경우)

1. Snowman 오브젝트 선택
2. 우클릭 → **Create Empty**
3. 이름을 `ThrowPoint`로 변경
4. **Transform** → **Position**을 눈사람의 손 또는 머리 위치로 조정
   - 예: Y축을 1.5로 설정하면 눈사람 위쪽에서 발사
5. SnowmanAI 컴포넌트의 **Throw Point**에 이 오브젝트를 드래그

### 5. 레이어 설정

#### 눈덩이 레이어 설정:
1. **Snowball.prefab** 선택
2. **Projectile** 컴포넌트의 **Target Layer** 확인
3. 현재 설정된 레이어: **Layer 6** (m_Bits: 64)
4. Player의 레이어를 확인하고 일치시키기:
   - **Edit** → **Project Settings** → **Tags and Layers**
   - **Layer 6**의 이름 확인 또는 새로 설정
   - Player 오브젝트의 Layer를 해당 레이어로 설정

### 6. 테스트

1. **Play** 버튼 클릭
2. 플레이어를 이동시켜 눈사람 근처로 이동
3. 확인사항:
   - 눈사람이 플레이어를 바라보는가?
   - 눈사람이 플레이어를 향해 다가오는가?
   - 멀리 있을 때 눈덩이를 던지는가?
   - 가까이 있을 때 근접 공격을 하는가? (콘솔에 "눈사람이 근접 공격을 수행했습니다!" 메시지 확인)

### 7. 디버깅 도구

Scene 뷰에서 Snowman 오브젝트를 선택하면:
- **빨간색 원**: 근접 공격 거리
- **노란색 원**: 원거리 공격 거리
- **주황색 원**: 근접 공격 감지 범위

이 시각화를 통해 공격 범위를 조정할 수 있습니다.

## 주요 파라미터 조정 가이드

### 더 빠르게 움직이게 하려면:
- **Move Speed**를 증가 (예: 3~5)

### 더 자주 공격하게 하려면:
- **Ranged Attack Cooldown**을 감소 (예: 1~1.5)
- **Melee Attack Cooldown**을 감소 (예: 0.8~1)

### 더 멀리서 공격하게 하려면:
- **Ranged Attack Distance**를 증가 (예: 15~20)

### 더 강하게 만들려면:
- EnemyHealth의 **Max HP**를 증가
- **Melee Damage**를 증가
- Snowball.prefab의 **Damage**를 증가 (Inspector에서 프리팹 선택 후 수정)

## 문제 해결

### 눈사람이 눈덩이를 던지지 않는 경우:
1. **Snowball Prefab**이 할당되어 있는지 확인
2. Snowball 프리팹에 **Projectile** 컴포넌트가 있는지 확인
3. 콘솔에 경고 메시지가 있는지 확인

### 눈덩이가 플레이어에게 데미지를 주지 않는 경우:
1. Player에 **PlayerHealth** 컴포넌트가 있는지 확인
2. Player의 **Layer**와 Snowball의 **Target Layer**가 일치하는지 확인
3. Player의 Collider가 있는지 확인

### 눈사람이 플레이어를 찾지 못하는 경우:
1. Player 오브젝트의 **Tag**가 `Player`로 설정되어 있는지 확인
2. 또는 SnowmanAI의 **Player** 필드에 직접 할당

## 추가 기능 제안

원한다면 다음 기능을 추가할 수 있습니다:
- 애니메이션 (공격, 이동, 대기)
- 사망 효과
- 순찰 패턴 (플레이어가 멀리 있을 때)
- 체력바 UI

필요한 경우 말씀해주세요!

