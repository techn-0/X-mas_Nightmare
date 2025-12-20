# 연료통 눈사람 AI 구현 완료

## 📝 구현 내용

연료통 눈사람(ExplosiveSnowmanAI)은 다음과 같은 특징을 가진 자폭형 적 AI입니다:

### 주요 기능
1. **플레이어 추적**: 플레이어를 향해 지속적으로 이동
2. **자폭 공격**: 플레이어에게 접근 시 0.3초 후 폭발
3. **처치 시 폭발**: 체력이 0이 되면 0.3초 후 폭발
4. **폭발 범위 데미지**: 폭발 범위 내의 플레이어에게 20 데미지

## 🎮 Unity에서 설정하는 방법

### 1. 연료통 눈사람 프리팹 생성
1. 눈사람 모델을 Hierarchy에 배치
2. 다음 컴포넌트들을 추가:
   - `ExplosiveSnowmanAI` (새로 만든 스크립트)
   - `EnemyHealth` (기존 체력 시스템)
   - `Collider` (충돌 감지용)
   - `Rigidbody` (물리 처리용, 필요시)

### 2. ExplosiveSnowmanAI 설정

#### Target Settings
- **Player**: 플레이어 Transform (자동으로 찾음, "Player" 태그 사용)

#### Movement Settings
- **Move Speed**: 3 (일반 눈사람보다 빠르게 설정)
- **Rotation Speed**: 5

#### Explosion Settings
- **Explosion Distance**: 2 (플레이어와의 거리가 2m 이하면 폭발)
- **Explosion Damage**: 20 (폭발 데미지)
- **Explosion Radius**: 3 (폭발 범위)
- **Explosion Delay**: 0.3 (폭발 전 대기 시간)

#### Visual Effects
- **Explosion Effect**: 폭발 이펙트 프리팹 (선택사항)
  - Cartoon FX Pack 등에서 폭발 이펙트 사용 가능
  - 예: `CFX_Explosion` 프리팹

### 3. EnemyHealth 설정
- **Max HP**: 원하는 체력 설정 (예: 50)
- **Drop Items**: 처치 시 드롭할 아이템 (선택사항)

### 4. 플레이어 태그 확인
- Player 오브젝트에 "Player" 태그가 지정되어 있는지 확인

## 🔧 동작 방식

### 일반 상태
```
1. 플레이어 감지
2. 플레이어를 향해 회전
3. 플레이어에게 접근
4. 거리 체크 (Update마다)
```

### 폭발 시나리오 A: 플레이어 접근
```
1. 플레이어와의 거리 ≤ 2m
2. 0.3초 대기 (explosionDelay)
3. 폭발 실행
   - 폭발 이펙트 생성
   - 범위 3m 내 플레이어 감지
   - 플레이어에게 20 데미지
4. 자신 파괴
```

### 폭발 시나리오 B: 처치당함
```
1. 체력 0 도달 (OnDeath 이벤트)
2. 0.3초 대기 (explosionDelay)
3. 폭발 실행
   - 폭발 이펙트 생성
   - 범위 3m 내 플레이어 감지
   - 플레이어에게 20 데미지
4. 자신 파괴
```

## 📋 추가된 파일

### ExplosiveSnowmanAI.cs
- 경로: `Assets/Scripts/Combat/Entities/ExplosiveSnowmanAI.cs`
- 연료통 눈사람의 AI 로직

### DamageType 업데이트
- 경로: `Assets/Scripts/Combat/Core/DamageType.cs`
- 추가된 타입: `Explosive` (폭발 데미지)

## 🎨 권장 폭발 이펙트

프로젝트에 있는 다음 에셋들을 활용할 수 있습니다:
- `Cartoon FX Pack` 폴더의 폭발 이펙트
- `SpecialSkillsEffectsPack` 폴더의 이펙트

또는 간단하게:
1. Unity의 Particle System으로 직접 제작
2. Asset Store의 무료 폭발 이펙트 사용

## 🐛 디버그 기능

Scene 뷰에서 눈사람을 선택하면 Gizmos로 다음을 표시:
- 🔴 빨간색 원: 폭발 시작 거리 (2m)
- 🟠 주황색 원: 폭발 범위 (3m)

## 🧪 테스트 방법

1. 연료통 눈사람을 씬에 배치
2. 플레이 모드 실행
3. 플레이어로 접근하여 자폭 테스트
4. 원거리 공격으로 처치하여 폭발 테스트
5. Console에서 로그 확인:
   - "연료통 눈사람 폭발로 플레이어에게 20 데미지!"

## 💡 커스터마이징 팁

### 더 위험하게 만들기
- Move Speed를 높여서 더 빠르게 접근
- Explosion Radius를 늘려서 범위 확대
- Explosion Damage를 높여서 더 강한 데미지

### 전략적으로 만들기
- Explosion Delay를 늘려서 회피 가능하게
- Explosion Distance를 줄여서 매우 가까이 와야 폭발
- 폭발 이펙트에 사운드 추가

### 시각 효과 추가
- 폭발 직전에 깜빡이는 효과
- 폭발 카운트다운 UI
- 경고음 재생

