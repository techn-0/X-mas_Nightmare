
<img width="971" height="547" alt="image (3)" src="https://github.com/user-attachments/assets/e67c7dc7-5e52-4521-8ed7-32cd4f658cfe" />

# **개요**

> 뒤틀린 소원들로 악마가 되어버린 산타클로스 군단에 맞서 행복한 크리스마스를 되찾는 소녀의 이야기
> 

| **항목** | **내용** |
| --- | --- |
| **게임명** | X-mas Nightmare |
| **엔진** | Unity 6 |
| **대회** | 2025 크래프톤 정글 게임잼(3인) / 1등 수상작 |
| **개발 기간** | 1일 (2025.12.20) |
| **장르** | 3D 액션, 핵앤슬래시 |
| **개발 역할** | 클라이언트 개발 100%, 게임 기획 50% |
| **GitHub** | https://github.com/techn-0/X-mas_Nightmare |
| **플레이 영상** | https://youtu.be/dU3i41jHDqY?si=XO6_Z7hIQn3spyIm |

# **핵심 기능 설계**

## **1. 웨이브 시스템 (`AdvancedSnowmanSpawner.cs`)**

**목표**: 게임 난이도를 동적으로 관리할 수 있는 유연한 몬스터 스폰 시스템 구현

**주요 기능**:

- **웨이브 모드**: 각 웨이브마다 다른 몬스터 수, 스폰 간격, 준비 시간 설정
    - 웨이브별 난이도 단계적 상향
    - 각 웨이브 완료 후 모든 몬스터 처치 대기
- **연속 스폰 모드**: 특정 조건 없이 계속 몬스터 스폰
    - 시간에 따른 자동 난이도 증가
    - 최대 활성 몬스터 수 제한으로 성능 최적화
- **확률 기반 몬스터 스폰**: 가중치 시스템으로 다양한 몬스터 타입 비율 조절
    - 웨이브 진행에 따른 조건부 등장 (minWave)
    - 선형 탐색 기반 효율적 확률 선택

- **이벤트 시스템**: 웨이브 진행 상황을 외부에 알림
    - `OnWaveStart`: 웨이브 시작 시 호출
    - `OnWaveComplete`: 웨이브 완료 시 호출
    - `OnAllWavesComplete`: 모든 웨이브 완료 시 호출

**코드 하이라이트**:

```csharp
// 확률 기반 몬스터 스폰
GameObject SelectRandomMonster(int waveNumber)
{
    List<SpawnableMonster> availableMonsters = new List<SpawnableMonster>();
    int totalWeight = 0;
    
    // 진행중인 웨이브 조건에 맞는 몬스터 필터링
    foreach (var monster in monsters)
    {
        if (monster.prefab != null && monster.minWave <= waveNumber)
        {
            availableMonsters.Add(monster);
            totalWeight += monster.spawnWeight;
        }
    }
    
    // 가중치 기반 선택
    int randomValue = Random.Range(0, totalWeight);
    int currentWeight = 0;
    
    foreach (var monster in availableMonsters)
    {
        currentWeight += monster.spawnWeight;
        if (randomValue < currentWeight)
            return monster.prefab;
    }
    return null;
}
```

---

## **2. 전투 시스템**

### **데미지 시스템 (`DamageType.cs`, `DamageInfo.cs`, `HealthSystem.cs`)**

**목표**: 확장 가능하고 재사용성을 갖춘 데미지 시스템 구현

**데미지 타입**:

- `Physical`: 눈사람 근접 공격
- `Fire`: 플레이어 횟불/화염방사기
- `Explosive`: 폭발 눈사람의 폭발 데미지

**HealthSystem 특징** - 모든 엔티티의 기본 체력 관리:

```csharp
public class HealthSystem
{
    [SerializeField] private float maxHP = 100f;
    [SerializeField] private bool isInvincible = false;
    
    // 이벤트 기반 처리
    public UnityEvent<DamageInfo> OnDamaged;      // 데미지 발생 시
    public UnityEvent OnDeath;                    // 사망 시
    public UnityEvent<float> OnHealthChanged;     // 체력 변화 시
}
```

**IDamageable 인터페이스** - 모든 데미지 받는 엔티티의 표준:

```csharp
public interface IDamageable
{
    void TakeDamage(DamageInfo damageInfo);
    float CurrentHP { get; }
    float MaxHP { get; }
    bool IsDead { get; }
}
```

---

### **플레이어 전투**

### **근접 공격 (`PlayerMeleeAttack.cs`)**


<img width="768" height="489" alt="image (4)" src="https://github.com/user-attachments/assets/0f7ae83d-803d-4fe9-9f6f-72bdf3c149e2" />

**주요 기능**:

- **HitBox 기반 판정**: 공격 범위 내의 모든 적에게 데미지
- **공격 쿨다운**: 0.5초 쿨타임으로 연속 공격 방지
- **마우스 조준**: 마우스 위치 방향으로 즉시 회전
- **애니메이션 동기화**: 근접 공격 애니메이션 재생
- **발사음 효과**: 공격 시 효과음 재생

**실행 흐름**:

1. 마우스 좌클릭 감지
2. `AimTowardsMouse()`: 마우스 방향으로 캐릭터 회전
3. `animator.SetTrigger("MeleeAttack")`: 공격 애니메이션 재생
4. `OnAttackStart()`: HitBox 활성화 및 판정 시작
5. `OnAttackEnd()`: HitBox 비활성화 및 쿨다운 적용

### **화염방사기 (`PlayerFlamethrower.cs`)**

<img width="824" height="478" alt="image (5)" src="https://github.com/user-attachments/assets/7c333959-f7c0-410b-892b-a7cb43acc1d1" />


**주요 기능**:

- **지속 데미지 (DoT)**: 마우스 우클릭 누르는 동안 계속 데미지
- **마우스 조준**: 화염 방사 중 부드럽게 마우스 추적 (Lerp 기반)
- **파티클 이펙트**: 화염 이펙트 실시간 재생/중지
- **사운드 루프**: 발사 중 루프 사운드 지속 재생
- **HitBox DoT 모드**: `dealsDotDamage: true`, `hitOnce: false` 필수 설정

**특징**:

- 적응형 조준: 마우스를 지면과의 교차점으로 계산
- `ResetHitTargets()` 호출로 여러 적 동시 공격 가능

---

## **3. 적 AI 시스템**

### **눈사람 AI (`SnowmanAI.cs`)**

<img width="445" height="324" alt="image (6)" src="https://github.com/user-attachments/assets/d12acb9a-9106-4e00-8f22-6e48a4ee553a" />
<img width="584" height="427" alt="image (7)" src="https://github.com/user-attachments/assets/80cb9887-7989-4981-a8b6-238177bc1d32" />


**AI 상태 머신**:

```
거리 > 10m: 플레이어 추적
10m >= 거리 > 2m: 다가가며 눈덩이 던지기 (2초 쿨타임)
거리 <= 2m: 근접 공격 (1.5초 쿨타임)
```

**주요 기능**:

- **거리 기반 행동**: 플레이어와의 거리에 따라 동적 행동
- **원거리 공격**: 눈덩이 발사 (2초 쿨타임)
- **근접 공격**: 근접 거리에서 직접 데미지 (1.5초 쿨타임)
- **자동 플레이어 추적**: 태그 기반으로 플레이어 자동 찾기
- **방향 회전**: 플레이어를 항상 바라보며 이동

**데미지 전달**:

```csharp
DamageInfo damageInfo = new DamageInfo(
    meleeDamage,                    // 20f
    DamageType.Physical,            // 물리 데미지
    player.position,                // 피격 위치
    hitDirection,                   // 피격 방향
    gameObject                      // 공격 소유자
);
damageable.TakeDamage(damageInfo);
```

### **폭발 눈사람 AI (`ExplosiveSnowmanAI.cs`)**
<img width="427" height="337" alt="image (8)" src="https://github.com/user-attachments/assets/969846e3-7ddd-4bc1-886c-fcee16052167" />
<img width="665" height="534" alt="image (9)" src="https://github.com/user-attachments/assets/6413ca60-5372-4234-88e1-d1f2abf795b7" />



**특수 기능**:

- 사망 시 범위 폭발 데미지
- 플레이어에게 접근하면 자폭
- 폭발 효과와 사운드 연출

### **보스 산타 AI (`BossSantaAI.cs`)**
<img width="665" height="534" alt="image (9)" src="https://github.com/user-attachments/assets/b8a086d8-b258-47d6-87de-aa34aef200e9" />
<img width="699" height="578" alt="image (10)" src="https://github.com/user-attachments/assets/a9d9026f-79aa-4790-b67d-67b6cd03fc79" />



**목표**: 1일 게임잼의 클라이맥스를 위한 도전적인 보스 구현

**공격 패턴**:

**1. 머신건 난사** (2회)

- 난사당 10발씩 (총 2회 = 20발)
- 발사 후 재장전으로 무방비 상태 → 반격 기회 제공

**2. 미사일 공격** (체력 50% 이하에서 활성화)

- 1초마다 1발씩 지속 발사
- 플레이어 현재 위치를 향해 발사
- 머신건 패턴과 독립적으로 진행

**3. 공격 패턴 관리**:

```csharp
// 공격 패턴 코루틴
IEnumerator AttackPattern()
{
    // 2회 난사 진행
    for (int i = 0; i < shotsPerSalvo; i++)
    {
        yield return StartCoroutine(MachineGunSalvo());
    }
    
    // 재장전 그로기 (0.5초) - 반격 타이밍
    yield return new WaitForSeconds(reloadTime);
}
```

**게임플레이 특징**:

- 느린 이동속도로 플레이어 회피 필수
- 체력 임계값으로 2페이즈 난이도 상향
- 미사일은 공격 패턴과 독립적으로 지속 발사
- 5개 총알 프리팹 순환으로 다양한 발사 표현

---

## **4. 투사체 시스템 (`Projectile.cs`)**

**목표**: 플레이어와 적 모두에게 사용 가능한 범용 투사체 시스템

**주요 기능**:

- **일반 투사체**: 눈덩이, 총알, 미사일 등 모든 발사체 지원
- **데미지 타입 지정**: DamageType.Physical/Fire/Explosive 선택 가능
- **충돌 감지**:
    - `OnTriggerEnter`: IDamageable 엔티티 감지
    - `OnCollisionEnter`: 지형/벽 충돌 감지
- **히트 이펙트**: 충돌 위치에 이펙트 생성 (2초 후 파괴)
- **사운드 재생**: 발사음과 충돌음 분리 재생
- **자동 파괴**: 5초 수명 후 자동 제거

```csharp
// 발사: 방향과 속도 설정 후 수명 타이머 시작
public void Launch(Vector3 direction)
{
    rb.linearVelocity = direction.normalized * speed;
    transform.rotation = Quaternion.LookRotation(direction);
    PlaySound(launchSound);
    Destroy(gameObject, lifetime);  // 5초 후 자동 파괴
}

private void OnTriggerEnter(Collider other)
{
    if (hasHit) return;

    // 소유자와의 충돌 무시 — 자체 피해 방지
    if (owner != null && (other.gameObject == owner || other.transform.IsChildOf(owner.transform)))
        return;

    IDamageable damageable = other.GetComponent<IDamageable>()
                          ?? other.GetComponentInParent<IDamageable>();

    if (damageable != null && !damageable.IsDead)
    {
        hasHit = true;
        DamageInfo damageInfo = new DamageInfo(
            damage, damageType,
            other.ClosestPoint(transform.position),
            rb.linearVelocity.normalized,
            owner != null ? owner : gameObject
        );
        damageable.TakeDamage(damageInfo);
        OnHit(other.ClosestPoint(transform.position));
    }
}
```

**특징**:

- 소유자(owner)와의 충돌 무시로 자체 피해 방지
- 지형 폭발 옵션으로 지면 충돌 감지 가능

---

# 트러블 슈팅

## **1. 첫 공격 시 히트박스 이펙트가 나타나지 않는 문제**

**증상**: 게임 플레이 중 첫 번째 공격 시 히트박스(화염 이펙트)가 화면에 표시되지 않음. 두 번째 공격부터는 정상 작동

**원인**:

- `PlayerMeleeAttack.cs` Awake()에서 히트박스를 비활성화(`gameObject.SetActive(false)`)한 상태로 초기화
- 첫 공격 시 `OnAttackStart()`에서 활성화하려고 해도, Unity의 물리 엔진은 GameObject 활성화에 1프레임의 지연 필요
- 콜라이더가 비활성화 상태에서 활성화될 때, 같은 프레임의 `OnTriggerEnter` 콜백이 스킵되는 문제 발생

**해결**:

```csharp
// Awake()에서 히트박스를 활성화 상태로 두되, 피격 목록만 초기화
weaponHitBox.ResetHitTargets();  // 피격 목록 초기화
// gameObject.SetActive(false) 제거 - 콜라이더는 활성화 상태 유지
```

**학습점**: 물리 엔진의 타이밍 이슈는 GameObject 활성화/비활성화보다 **콜라이더 감지 로직을 분리**하는 것으로 해결 가능

---

## **2. 플레이어가 보스와 충돌 시 튕겨져 나가는 문제**

**증상**: 플레이어가 산타 보스와 부딪치면 강하게 튕겨져 나갔으며, 정상적인 게임플레이 불가능

**원인**:

- 플레이어: `CharacterController` (물리 기반 X)
- 보스: `Rigidbody` + `Collider` (물리 시뮬레이션)
- 두 시스템의 충돌 시 물리 엔진이 보스를 밀어내려는 힘을 생성하여 플레이어가 튕겨남

**해결** (Inspector에서 Rigidbody Constraints 설정):

1. **Freeze Position**: Y축만 체크
    - X, Z축: AI 코드에서 `MoveTowardsPlayer()`로 수평 이동 제어
    - Y축: 중력으로 떨어지지 않게 고정
2. **Freeze Rotation**: X, Z축 체크
    - X, Z축: 옆으로/앞뒤로 기울어지지 않게 고정

**결과**:

- 물리 계산으로 밀려나지 않으면서도 AI 이동 정상 작동
- 보스가 플레이어를 추적하며 회전하는 기능 유지

**학습점**:

- Rigidbody의 Constraints는 **축별로 세밀하게 제어** 가능
- AI 이동과 물리 엔진의 균형을 인스펙터 설정만으로 조정 가능
- CharacterController와 Rigidbody의 병행 사용 시 각 축의 역할을 명확히 이해해야 함

---

# 회고

 1일의 시간 제약 속에서 클라이언트 개발 전체를 담당했기 때문에, AI를 적극적으로 활용했다. 다만 단순히 코드 생성을 맡기는 방식이 아니라, 클래스 구조와 변수 설계 틀을 먼저 직접 작성한 뒤 핵심 요구사항과 함께 AI에게 세부 구현을 맡기고, AI가 작성한 코드는 동작 원리를 직접 파악한 뒤에만 프로젝트에 통합했다.

 덕분에 히트박스 타이밍 이슈, 보스 물리 충돌 문제 등 AI가 생성한 코드에서 발생한 버그도 직접 원인을 분석하고 수정할 수 있었다.
 이를 통해 웨이브 시스템, 보스 AI, 데미지 시스템 등 게임의 전체 아키텍처를 1일 안에 완성할 수 있었고, AI를 효과적으로 활용하려면 명확한 설계 능력과 요구사항을 구체화하는 능력이 전제되어야 한다는 것을 체감했다.

# **개요**

> 뒤틀린 소원들로 악마가 되어버린 산타클로스 군단에 맞서 행복한 크리스마스를 되찾는 소녀의 이야기
> 

| **항목** | **내용** |
| --- | --- |
| **게임명** | X-mas Nightmare |
| **엔진** | Unity 6 |
| **대회** | 2025 크래프톤 정글 게임잼(3인) / 1등 수상작 |
| **개발 기간** | 1일 (2025.12.20) |
| **장르** | 3D 액션, 핵앤슬래시 |
| **개발 역할** | 클라이언트 개발 100%, 게임 기획 50% |
| **GitHub** | https://github.com/techn-0/X-mas_Nightmare |
| **플레이 영상** | https://youtu.be/dU3i41jHDqY?si=XO6_Z7hIQn3spyIm |

# **핵심 기능 설계**

## **1. 웨이브 시스템 (`AdvancedSnowmanSpawner.cs`)**

**목표**: 게임 난이도를 동적으로 관리할 수 있는 유연한 몬스터 스폰 시스템 구현

**주요 기능**:

- **웨이브 모드**: 각 웨이브마다 다른 몬스터 수, 스폰 간격, 준비 시간 설정
    - 웨이브별 난이도 단계적 상향
    - 각 웨이브 완료 후 모든 몬스터 처치 대기
- **연속 스폰 모드**: 특정 조건 없이 계속 몬스터 스폰
    - 시간에 따른 자동 난이도 증가
    - 최대 활성 몬스터 수 제한으로 성능 최적화
- **확률 기반 몬스터 스폰**: 가중치 시스템으로 다양한 몬스터 타입 비율 조절
    - 웨이브 진행에 따른 조건부 등장 (minWave)
    - 선형 탐색 기반 효율적 확률 선택

- **이벤트 시스템**: 웨이브 진행 상황을 외부에 알림
    - `OnWaveStart`: 웨이브 시작 시 호출
    - `OnWaveComplete`: 웨이브 완료 시 호출
    - `OnAllWavesComplete`: 모든 웨이브 완료 시 호출

**코드 하이라이트**:

```csharp
// 확률 기반 몬스터 스폰
GameObject SelectRandomMonster(int waveNumber)
{
    List<SpawnableMonster> availableMonsters = new List<SpawnableMonster>();
    int totalWeight = 0;
    
    // 진행중인 웨이브 조건에 맞는 몬스터 필터링
    foreach (var monster in monsters)
    {
        if (monster.prefab != null && monster.minWave <= waveNumber)
        {
            availableMonsters.Add(monster);
            totalWeight += monster.spawnWeight;
        }
    }
    
    // 가중치 기반 선택
    int randomValue = Random.Range(0, totalWeight);
    int currentWeight = 0;
    
    foreach (var monster in availableMonsters)
    {
        currentWeight += monster.spawnWeight;
        if (randomValue < currentWeight)
            return monster.prefab;
    }
    return null;
}
```

---

## **2. 전투 시스템**

### **데미지 시스템 (`DamageType.cs`, `DamageInfo.cs`, `HealthSystem.cs`)**

**목표**: 확장 가능하고 재사용성을 갖춘 데미지 시스템 구현

**데미지 타입**:

- `Physical`: 눈사람 근접 공격
- `Fire`: 플레이어 횟불/화염방사기
- `Explosive`: 폭발 눈사람의 폭발 데미지

**HealthSystem 특징** - 모든 엔티티의 기본 체력 관리:

```csharp
public class HealthSystem
{
    [SerializeField] private float maxHP = 100f;
    [SerializeField] private bool isInvincible = false;
    
    // 이벤트 기반 처리
    public UnityEvent<DamageInfo> OnDamaged;      // 데미지 발생 시
    public UnityEvent OnDeath;                    // 사망 시
    public UnityEvent<float> OnHealthChanged;     // 체력 변화 시
}
```

**IDamageable 인터페이스** - 모든 데미지 받는 엔티티의 표준:

```csharp
public interface IDamageable
{
    void TakeDamage(DamageInfo damageInfo);
    float CurrentHP { get; }
    float MaxHP { get; }
    bool IsDead { get; }
}
```

---

### **플레이어 전투**

### **근접 공격 (`PlayerMeleeAttack.cs`)**

![image.png](attachment:e7932969-ca7a-4a15-8a8e-7877441db4d8:image.png)

**주요 기능**:

- **HitBox 기반 판정**: 공격 범위 내의 모든 적에게 데미지
- **공격 쿨다운**: 0.5초 쿨타임으로 연속 공격 방지
- **마우스 조준**: 마우스 위치 방향으로 즉시 회전
- **애니메이션 동기화**: 근접 공격 애니메이션 재생
- **발사음 효과**: 공격 시 효과음 재생

**실행 흐름**:

1. 마우스 좌클릭 감지
2. `AimTowardsMouse()`: 마우스 방향으로 캐릭터 회전
3. `animator.SetTrigger("MeleeAttack")`: 공격 애니메이션 재생
4. `OnAttackStart()`: HitBox 활성화 및 판정 시작
5. `OnAttackEnd()`: HitBox 비활성화 및 쿨다운 적용

### **화염방사기 (`PlayerFlamethrower.cs`)**

![image.png](attachment:157562d8-c7fd-43b6-acd6-fbda5ced3c12:image.png)

**주요 기능**:

- **지속 데미지 (DoT)**: 마우스 우클릭 누르는 동안 계속 데미지
- **마우스 조준**: 화염 방사 중 부드럽게 마우스 추적 (Lerp 기반)
- **파티클 이펙트**: 화염 이펙트 실시간 재생/중지
- **사운드 루프**: 발사 중 루프 사운드 지속 재생
- **HitBox DoT 모드**: `dealsDotDamage: true`, `hitOnce: false` 필수 설정

**특징**:

- 적응형 조준: 마우스를 지면과의 교차점으로 계산
- `ResetHitTargets()` 호출로 여러 적 동시 공격 가능

---

## **3. 적 AI 시스템**

### **눈사람 AI (`SnowmanAI.cs`)**

![image.png](attachment:0290cd61-e0bb-450f-b4d7-889cc4385e49:0dcaa576-9c16-4ca4-b2c4-51a161e73abb.png)

![image.png](attachment:2157c744-3a79-41fe-ad9e-fe62ba05d43c:image.png)

**AI 상태 머신**:

```
거리 > 10m: 플레이어 추적
10m >= 거리 > 2m: 다가가며 눈덩이 던지기 (2초 쿨타임)
거리 <= 2m: 근접 공격 (1.5초 쿨타임)
```

**주요 기능**:

- **거리 기반 행동**: 플레이어와의 거리에 따라 동적 행동
- **원거리 공격**: 눈덩이 발사 (2초 쿨타임)
- **근접 공격**: 근접 거리에서 직접 데미지 (1.5초 쿨타임)
- **자동 플레이어 추적**: 태그 기반으로 플레이어 자동 찾기
- **방향 회전**: 플레이어를 항상 바라보며 이동

**데미지 전달**:

```csharp
DamageInfo damageInfo = new DamageInfo(
    meleeDamage,                    // 20f
    DamageType.Physical,            // 물리 데미지
    player.position,                // 피격 위치
    hitDirection,                   // 피격 방향
    gameObject                      // 공격 소유자
);
damageable.TakeDamage(damageInfo);
```

### **폭발 눈사람 AI (`ExplosiveSnowmanAI.cs`)**

![image.png](attachment:9f5c3079-ddee-447e-a444-830802a0e8f9:b22f2c6a-aa89-4ac1-8ba8-1543d5e10510.png)

![image.png](attachment:672a09b4-8d6d-4081-9a0b-b949d72eb0fd:38b0dde3-5766-424c-8523-646be6cf0238.png)

**특수 기능**:

- 사망 시 범위 폭발 데미지
- 플레이어에게 접근하면 자폭
- 폭발 효과와 사운드 연출

### **보스 산타 AI (`BossSantaAI.cs`)**

![image.png](attachment:469ae7d1-d9c5-4143-8597-ab6c86756e42:image.png)

![image.png](attachment:6e4d7762-a2d3-466b-938b-241b052f2347:image.png)

**목표**: 1일 게임잼의 클라이맥스를 위한 도전적인 보스 구현

**공격 패턴**:

**1. 머신건 난사** (2회)

- 난사당 10발씩 (총 2회 = 20발)
- 발사 후 재장전으로 무방비 상태 → 반격 기회 제공

**2. 미사일 공격** (체력 50% 이하에서 활성화)

- 1초마다 1발씩 지속 발사
- 플레이어 현재 위치를 향해 발사
- 머신건 패턴과 독립적으로 진행

**3. 공격 패턴 관리**:

```csharp
// 공격 패턴 코루틴
IEnumerator AttackPattern()
{
    // 2회 난사 진행
    for (int i = 0; i < shotsPerSalvo; i++)
    {
        yield return StartCoroutine(MachineGunSalvo());
    }
    
    // 재장전 그로기 (0.5초) - 반격 타이밍
    yield return new WaitForSeconds(reloadTime);
}
```

**게임플레이 특징**:

- 느린 이동속도로 플레이어 회피 필수
- 체력 임계값으로 2페이즈 난이도 상향
- 미사일은 공격 패턴과 독립적으로 지속 발사
- 5개 총알 프리팹 순환으로 다양한 발사 표현

---

## **4. 투사체 시스템 (`Projectile.cs`)**

**목표**: 플레이어와 적 모두에게 사용 가능한 범용 투사체 시스템

**주요 기능**:

- **일반 투사체**: 눈덩이, 총알, 미사일 등 모든 발사체 지원
- **데미지 타입 지정**: DamageType.Physical/Fire/Explosive 선택 가능
- **충돌 감지**:
    - `OnTriggerEnter`: IDamageable 엔티티 감지
    - `OnCollisionEnter`: 지형/벽 충돌 감지
- **히트 이펙트**: 충돌 위치에 이펙트 생성 (2초 후 파괴)
- **사운드 재생**: 발사음과 충돌음 분리 재생
- **자동 파괴**: 5초 수명 후 자동 제거

```csharp
// 발사: 방향과 속도 설정 후 수명 타이머 시작
public void Launch(Vector3 direction)
{
    rb.linearVelocity = direction.normalized * speed;
    transform.rotation = Quaternion.LookRotation(direction);
    PlaySound(launchSound);
    Destroy(gameObject, lifetime);  // 5초 후 자동 파괴
}

private void OnTriggerEnter(Collider other)
{
    if (hasHit) return;

    // 소유자와의 충돌 무시 — 자체 피해 방지
    if (owner != null && (other.gameObject == owner || other.transform.IsChildOf(owner.transform)))
        return;

    IDamageable damageable = other.GetComponent<IDamageable>()
                          ?? other.GetComponentInParent<IDamageable>();

    if (damageable != null && !damageable.IsDead)
    {
        hasHit = true;
        DamageInfo damageInfo = new DamageInfo(
            damage, damageType,
            other.ClosestPoint(transform.position),
            rb.linearVelocity.normalized,
            owner != null ? owner : gameObject
        );
        damageable.TakeDamage(damageInfo);
        OnHit(other.ClosestPoint(transform.position));
    }
}
```

**특징**:

- 소유자(owner)와의 충돌 무시로 자체 피해 방지
- 지형 폭발 옵션으로 지면 충돌 감지 가능

---

# 트러블 슈팅

## **1. 첫 공격 시 히트박스 이펙트가 나타나지 않는 문제**

**증상**: 게임 플레이 중 첫 번째 공격 시 히트박스(화염 이펙트)가 화면에 표시되지 않음. 두 번째 공격부터는 정상 작동

**원인**:

- `PlayerMeleeAttack.cs` Awake()에서 히트박스를 비활성화(`gameObject.SetActive(false)`)한 상태로 초기화
- 첫 공격 시 `OnAttackStart()`에서 활성화하려고 해도, Unity의 물리 엔진은 GameObject 활성화에 1프레임의 지연 필요
- 콜라이더가 비활성화 상태에서 활성화될 때, 같은 프레임의 `OnTriggerEnter` 콜백이 스킵되는 문제 발생

**해결**:

```csharp
// Awake()에서 히트박스를 활성화 상태로 두되, 피격 목록만 초기화
weaponHitBox.ResetHitTargets();  // 피격 목록 초기화
// gameObject.SetActive(false) 제거 - 콜라이더는 활성화 상태 유지
```

**학습점**: 물리 엔진의 타이밍 이슈는 GameObject 활성화/비활성화보다 **콜라이더 감지 로직을 분리**하는 것으로 해결 가능

---

## **2. 플레이어가 보스와 충돌 시 튕겨져 나가는 문제**

**증상**: 플레이어가 산타 보스와 부딪치면 강하게 튕겨져 나갔으며, 정상적인 게임플레이 불가능

**원인**:

- 플레이어: `CharacterController` (물리 기반 X)
- 보스: `Rigidbody` + `Collider` (물리 시뮬레이션)
- 두 시스템의 충돌 시 물리 엔진이 보스를 밀어내려는 힘을 생성하여 플레이어가 튕겨남

**해결** (Inspector에서 Rigidbody Constraints 설정):

1. **Freeze Position**: Y축만 체크
    - X, Z축: AI 코드에서 `MoveTowardsPlayer()`로 수평 이동 제어
    - Y축: 중력으로 떨어지지 않게 고정
2. **Freeze Rotation**: X, Z축 체크
    - X, Z축: 옆으로/앞뒤로 기울어지지 않게 고정

**결과**:

- 물리 계산으로 밀려나지 않으면서도 AI 이동 정상 작동
- 보스가 플레이어를 추적하며 회전하는 기능 유지

**학습점**:

- Rigidbody의 Constraints는 **축별로 세밀하게 제어** 가능
- AI 이동과 물리 엔진의 균형을 인스펙터 설정만으로 조정 가능
- CharacterController와 Rigidbody의 병행 사용 시 각 축의 역할을 명확히 이해해야 함

---

# 회고

 1일의 시간 제약 속에서 클라이언트 개발 전체를 담당했기 때문에, AI를 적극적으로 활용했다. 다만 단순히 코드 생성을 맡기는 방식이 아니라, 클래스 구조와 변수 설계 틀을 먼저 직접 작성한 뒤 핵심 요구사항과 함께 AI에게 세부 구현을 맡기고, AI가 작성한 코드는 동작 원리를 직접 파악한 뒤에만 프로젝트에 통합했다.

 덕분에 히트박스 타이밍 이슈, 보스 물리 충돌 문제 등 AI가 생성한 코드에서 발생한 버그도 직접 원인을 분석하고 수정할 수 있었다.
 이를 통해 웨이브 시스템, 보스 AI, 데미지 시스템 등 게임의 전체 아키텍처를 1일 안에 완성할 수 있었고, AI를 효과적으로 활용하려면 명확한 설계 능력과 요구사항을 구체화하는 능력이 전제되어야 한다는 것을 체감했다.
