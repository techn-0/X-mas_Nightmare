﻿# 데미지 시스템 예시 스크립트

이 폴더에는 데미지 시스템을 실제로 사용하는 방법을 보여주는 예시 스크립트들이 포함되어 있습니다.

---

## 🚀 빠른 시작 가이드

**Unity 에디터에서 화염방사기와 눈덩이 공격을 테스트하려면:**

👉 **[../QUICK_CHECKLIST.md](../QUICK_CHECKLIST.md)** 를 확인하세요!

- 30분 안에 테스트 시작 가능
- 단계별 체크리스트 제공
- 모든 설정 방법 포함

---

## 📁 포함된 예시 스크립트

### 1. PlayerFlamethrower.cs ⭐ NEW!
플레이어의 화염방사기 공격

**기능:**
- 지속적인 화염 발사
- 연료 시스템 (소모 및 회복)
- HitBox 기반 범위 공격
- 파티클 효과 연동

**Unity 설정:**
- Component 추가: `PlayerFlamethrower`
- FlameOrigin GameObject 생성 및 연결
- FlameHitbox Prefab 연결
- 자세한 설정: [../QUICK_CHECKLIST.md](../QUICK_CHECKLIST.md)

### 2. ProjectileLauncher.cs ⭐ NEW!
투사체 발사 시스템 (눈사람의 눈덩이)

**기능:**
- 자동/수동 발사 모드
- 발사 각도 조절
- 발사 속도 제어
- 이벤트 시스템

**Unity 설정:**
- Component 추가: `ProjectileLauncher`
- LaunchPoint GameObject 생성 및 연결
- Projectile Prefab (Snowball) 연결
- 자세한 설정: [../QUICK_CHECKLIST.md](../QUICK_CHECKLIST.md)

### 3. SimplePlayerController.cs ⭐ NEW!
테스트용 플레이어 컨트롤러

**기능:**
- WASD 이동
- 마우스 조준 및 회전
- 화염방사기 조작 (좌클릭)
- 근접 공격 조작 (우클릭)

**Unity 설정:**
- Player GameObject에 추가
- 자동으로 컴포넌트 연결

### 4. SimpleSnowmanAI.cs ⭐ NEW!
테스트용 눈사람 AI

**기능:**
- 플레이어 자동 감지
- 플레이어 추적 및 조준
- 자동 투사체 발사
- 발사 간격 랜덤화

**Unity 설정:**
- Snowman GameObject에 추가
- Player GameObject 연결
- 자세한 설정: [../QUICK_CHECKLIST.md](../QUICK_CHECKLIST.md)

### 5. PlayerMeleeAttack.cs
성냥팔이 소녀의 횟불 근접 공격 예시

**기능:**
- 마우스 클릭으로 근접 공격
- 공격 쿨다운 관리
- 애니메이션 이벤트 연동
- HitBox 활성화/비활성화

**사용법:**
```csharp
// Player에 추가
1. PlayerMeleeAttack 컴포넌트 추가
2. Weapon Hit Box 할당
3. Animator 할당 (있으면)
4. Attack Key 설정 (기본: 마우스 좌클릭)
```

---

### 2. PlayerFlamethrower.cs
화염방사기 (DoT 공격) 예시

**기능:**
- 마우스 버튼을 누르고 있는 동안 지속 발사
- 파티클 효과 및 사운드 관리
- DoT 데미지 적용
- HitBox 지속 활성화

**사용법:**
```csharp
// Player에 추가
1. PlayerFlamethrower 컴포넌트 추가
2. Flame Hit Box 할당
   - Inspector에서 Deals Dot Damage: true 설정!
   - Dot Duration: 3
   - Hit Once: false
3. Flame Effect (ParticleSystem) 할당
4. Flame Sound (AudioSource) 할당
5. Fire Key 설정 (기본: 마우스 우클릭)
```

---

### 3. ProjectileLauncher.cs
투사체 발사 (눈덩이 등) 예시

**기능:**
- 특정 방향으로 투사체 발사
- 타겟을 향해 자동 조준
- 플레이어 및 적 AI에서 사용 가능
- 발사 쿨다운 관리

**사용법:**
```csharp
// Player 또는 Enemy에 추가
1. ProjectileLauncher 컴포넌트 추가
2. Projectile Prefab 할당 (Projectile 컴포넌트가 있어야 함)
3. Fire Point 할당 (발사 위치)
4. Attack Cooldown 설정

// 플레이어용
- Use Input: true
- Shoot Key 설정

// 적 AI용
- Use Input: false
- AI 스크립트에서 ShootAtPlayer() 호출
```

**코드 예시:**
```csharp
// 적 AI에서 사용
public class EnemyAI : MonoBehaviour
{
    private ProjectileLauncher launcher;
    
    void Start()
    {
        launcher = GetComponent<ProjectileLauncher>();
    }
    
    void AttackPlayer()
    {
        if (launcher.CanShoot())
        {
            launcher.ShootAtPlayer();
        }
    }
}
```

---

### 4. HealthBarUI.cs
체력바 UI 연동 예시

**기능:**
- PlayerHealth/EnemyHealth와 자동 연동
- 체력 비율에 따른 색상 변화
- 슬라이더 및 텍스트 업데이트
- HealthSystem 이벤트 자동 구독

**사용법:**
```csharp
// Canvas에 UI 구성
1. Canvas 생성
2. Slider 추가 (체력바)
3. Text 추가 (HP 텍스트, 선택사항)
4. HealthBarUI 컴포넌트 추가
5. Health Component에 PlayerHealth 또는 EnemyHealth 할당
6. Health Slider, Health Text 할당
7. Fill Image 할당 (Slider의 Fill)
8. 색상 설정
```

**UI 구조 예시:**
```
Canvas
└── HealthBar Panel
    ├── HealthBarUI (Script)
    ├── Background Image
    └── Slider
        └── Fill Area
            └── Fill (Image) ← Fill Image에 할당
```

---

## 🎮 통합 예시: 완전한 플레이어 설정

```
Player GameObject
├── PlayerHealth
├── PlayerMeleeAttack
│   └── Weapon Hit Box 연결
├── PlayerFlamethrower
│   └── Flame Hit Box 연결
└── Weapon (자식 오브젝트)
    ├── Torch (근접 공격용)
    │   └── MeleeHitBox
    │       ├── HitBox Component
    │       │   ├── Damage: 20
    │       │   ├── Damage Type: Fire
    │       │   ├── Target Layer: Enemy
    │       │   └── Hit Once: true
    │       └── Box Collider (isTrigger: true)
    └── Flamethrower (원거리 DoT용)
        └── FlameHitBox
            ├── HitBox Component
            │   ├── Damage: 5
            │   ├── Damage Type: Fire
            │   ├── Deals Dot Damage: true
            │   ├── Dot Duration: 3
            │   ├── Target Layer: Enemy
            │   └── Hit Once: false
            └── Cone Collider (isTrigger: true)
```

---

## 🎯 통합 예시: 완전한 적 설정

```
Enemy GameObject (Snowman)
├── EnemyHealth
│   ├── Max HP: 50
│   └── Drop Items: [경험치 오브]
├── ProjectileLauncher
│   ├── Projectile Prefab: SnowballProjectile
│   ├── Fire Point: Head
│   └── Attack Cooldown: 2
└── SimpleEnemyAI (별도 작성 필요)
    └── ProjectileLauncher 사용
```

---

## 💡 실전 팁

### 1. 애니메이션 이벤트 설정
공격 애니메이션에 이벤트 추가:
- 공격 시작 프레임: `OnAttackStart()`
- 공격 종료 프레임: `OnAttackEnd()`

### 2. 레이어 설정 확인
- Player는 Enemy만 공격
- Enemy는 Player만 공격
- Project Settings > Physics에서 설정

### 3. 프리팹 준비
**투사체 프리팹 (SnowballProjectile):**
```
Snowball Prefab
├── Projectile Component
│   ├── Damage: 10
│   ├── Damage Type: Physical
│   ├── Speed: 8
│   ├── Lifetime: 5
│   └── Target Layer: Player
├── Sphere Collider (isTrigger: true)
├── Rigidbody (useGravity: false)
└── Visual (Mesh/Sprite)
```

### 4. 성능 최적화
- HitBox는 필요할 때만 활성화
- 투사체는 Object Pooling 사용 권장
- DoT는 많은 적에게 동시에 적용 시 성능 주의

---

## 🔧 커스터마이징

### 크리티컬 히트 추가
```csharp
public class CriticalMeleeAttack : PlayerMeleeAttack
{
    [SerializeField] private float critChance = 0.2f;
    [SerializeField] private float critMultiplier = 2f;
    
    // HitBox 데미지를 동적으로 변경
}
```

### 콤보 공격 시스템
```csharp
public class ComboMeleeAttack : PlayerMeleeAttack
{
    private int comboCount = 0;
    private float comboWindow = 1f;
    
    // 연속 공격 시 콤보 증가
}
```

### 넉백 효과 추가
```csharp
// PlayerHealth 또는 EnemyHealth의 TakeDamage에서
private void TakeDamage(DamageInfo damageInfo)
{
    // ... 기존 코드 ...
    
    // 넉백 적용
    Rigidbody rb = GetComponent<Rigidbody>();
    if (rb != null)
    {
        rb.AddForce(damageInfo.hitDirection * 5f, ForceMode.Impulse);
    }
}
```

---

## ⚠️ 주의사항

1. **HitBox는 반드시 비활성화 상태로 시작**: Awake/Start에서 `Deactivate()` 호출
2. **공격 전 ResetHitTargets() 호출**: 중복 피격 방지
3. **레이어 설정 확인**: 가장 흔한 문제의 원인
4. **Collider는 isTrigger = true**: 물리 충돌 방지
5. **DoT 사용 시 Hit Once = false**: 지속 피격 판정 필요

---

## 📚 추가 학습 자료

- 상위 폴더의 `README.md`: 전체 시스템 통합 가이드
- 각 클래스의 XML 주석: 상세한 기능 설명
- Unity 문서: Animator Events, Physics Layers

---

**Happy Coding! 🎉**

이 예시 스크립트들은 자유롭게 수정하고 확장할 수 있습니다.

