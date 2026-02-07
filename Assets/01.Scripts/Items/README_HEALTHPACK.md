# 힐팩 아이템 사용 가이드

## 개요
플레이어가 닿으면 체력을 회복하고 사라지는 힐팩 아이템 시스템입니다.

## 주요 기능
- ✅ 플레이어 체력 회복
- ✅ 절대값 또는 비율(%) 기반 회복량 설정
- ✅ 최대 체력일 때 아이템 획득 방지
- ✅ 회전 및 떠다니는 효과
- ✅ 획득 이펙트 및 사운드
- ✅ Scene 뷰에서 아이템 위치 표시 (Gizmo)

---

## 빠른 시작

### 1. 힐팩 프리팹 생성

1. **빈 오브젝트 생성**
   - Hierarchy 창에서 우클릭 → `Create Empty`
   - 이름을 "HealthPack"으로 변경

2. **비주얼 추가**
   - 3D Object → Cube/Sphere 등을 자식으로 추가
   - 또는 힐팩 모델을 자식으로 추가
   - Scale을 적절히 조정 (예: 0.5, 0.5, 0.5)

3. **Collider 설정**
   - HealthPack 오브젝트에 `Collider` 추가 (Box Collider 또는 Sphere Collider)
   - **중요: `Is Trigger` 체크박스를 활성화**

4. **스크립트 추가**
   - HealthPack 오브젝트에 `HealthPack.cs` 스크립트 추가

5. **프리팹으로 저장**
   - HealthPack 오브젝트를 Project 창의 Prefabs 폴더로 드래그

---

## Inspector 설정

### Heal Settings
- **Heal Amount** (기본: 30)
  - 회복량 설정
  - Use Absolute Value가 true면 절대값
  - Use Absolute Value가 false면 최대 체력의 비율(%)
  
- **Use Absolute Value** (기본: true)
  - true: 고정된 체력 회복 (예: 30 HP)
  - false: 비율 기반 회복 (예: 50%면 최대 체력의 50%)

### Effects
- **Pickup Effect Prefab** (선택)
  - 아이템 획득 시 생성될 파티클 이펙트
  - 2초 후 자동으로 제거됨

- **Pickup Sound** (선택)
  - 아이템 획득 시 재생될 사운드
  
- **Sound Volume** (기본: 1.0)
  - 사운드 볼륨 (0.0 ~ 1.0)

### Rotation (Optional)
- **Rotate Item** (기본: true)
  - 아이템 회전 효과 활성화
  
- **Rotation Speed** (기본: 50)
  - 회전 속도 (도/초)
  
- **Rotation Axis** (기본: Vector3.up)
  - 회전 축 (기본값: Y축)

### Floating (Optional)
- **Float Item** (기본: true)
  - 떠다니는 효과 활성화
  
- **Float Amplitude** (기본: 0.3)
  - 위아래로 움직이는 거리
  
- **Float Speed** (기본: 2)
  - 떠다니는 속도

---

## 설정 예시

### 1. 소형 힐팩 (30 HP 회복)
```
Heal Amount: 30
Use Absolute Value: ✓
Rotate Item: ✓
Float Item: ✓
```

### 2. 대형 힐팩 (50% 회복)
```
Heal Amount: 50
Use Absolute Value: ✗ (체크 해제)
Rotate Item: ✓
Float Item: ✓
```

### 3. 완전 회복 힐팩 (100% 회복)
```
Heal Amount: 100
Use Absolute Value: ✗ (체크 해제)
Rotate Item: ✓
Float Item: ✓
```

### 4. 정적인 힐팩 (효과 없음)
```
Heal Amount: 50
Use Absolute Value: ✓
Rotate Item: ✗ (체크 해제)
Float Item: ✗ (체크 해제)
```

---

## 중요 체크리스트

### 필수 설정
- [ ] HealthPack 오브젝트에 **Collider** 컴포넌트 추가
- [ ] Collider의 **Is Trigger 활성화**
- [ ] 플레이어 오브젝트의 Tag가 **"Player"**로 설정됨
- [ ] 플레이어에 **PlayerHealth** 컴포넌트 있음

### 선택 설정
- [ ] 획득 이펙트 프리팹 할당
- [ ] 획득 사운드 클립 할당
- [ ] 회전/떠다니는 효과 조정

---

## 씬에 배치하기

### 방법 1: 수동 배치
1. Prefabs 폴더에서 HealthPack 프리팹을 씬으로 드래그
2. Transform 위치 조정
3. 원하는 만큼 복사하여 배치

### 방법 2: 적이 죽을 때 드롭 ⭐ 추천!
1. 적 오브젝트 선택 (Hierarchy)
2. Inspector에서 **Enemy Health** 컴포넌트 찾기
3. **Drop Settings** 설정:
   ```
   Drop Items:
     Size: 1
     Element 0: [힐팩 프리팹 드래그]
   Drop Chance: 0.5 (50% 확률)
   ```
4. 완료! 이제 적을 죽이면 힐팩이 나옵니다

**자세한 내용**: `HOW_TO_MAKE_ENEMIES_DROP_HEALTHPACK.md` 참고

### 방법 3: 코드로 스폰
```csharp
public GameObject healthPackPrefab;

void SpawnHealthPack(Vector3 position)
{
    Instantiate(healthPackPrefab, position, Quaternion.identity);
}
```

---

## 동작 원리

1. **플레이어 감지**
   - OnTriggerEnter를 통해 플레이어와 충돌 감지
   - Tag가 "Player"인지 확인

2. **체력 회복**
   - PlayerHealth 컴포넌트 찾기
   - 현재 체력이 최대 체력보다 낮은지 확인
   - Heal() 메서드 호출하여 체력 회복

3. **이펙트 재생**
   - 파티클 이펙트 생성 (있는 경우)
   - 사운드 재생 (있는 경우)

4. **오브젝트 제거**
   - Destroy(gameObject)로 힐팩 제거

---

## 문제 해결

### 아이템을 먹어도 체력이 회복되지 않아요
- 플레이어에 PlayerHealth 컴포넌트가 있는지 확인
- 플레이어의 Tag가 "Player"인지 확인
- Collider의 Is Trigger가 체크되어 있는지 확인

### 아이템이 바닥으로 떨어져요
- HealthPack 오브젝트에 Rigidbody가 있다면 제거하거나
- Rigidbody의 Is Kinematic을 활성화

### 플레이어가 통과해버려요
- Collider의 Is Trigger가 체크되어 있는지 확인
- Collider 크기가 적절한지 확인

### 최대 체력인데도 아이템을 먹어요
- 스크립트가 자동으로 방지합니다
- Console에 "체력이 이미 최대입니다" 메시지가 표시됨

---

## 커스터마이징 아이디어

### 다양한 힐팩 종류
- **소형**: 30 HP 회복
- **중형**: 50 HP 회복
- **대형**: 100 HP 회복 또는 50%
- **완전 회복**: 100% 회복

### 추가 기능
- 일정 시간 후 자동으로 사라지기
- 획득 시 버프 효과 추가
- 희귀도에 따른 다른 색상/이펙트
- 회전 속도 랜덤화

### 스폰 시스템
- 적 처치 시 확률적으로 드롭
- 특정 위치에 주기적으로 리스폰
- 보스전 전에 자동 스폰

---

## 예제 사용 코드

### 적이 죽을 때 힐팩 드롭
```csharp
public class EnemyDrop : MonoBehaviour
{
    [SerializeField] private GameObject healthPackPrefab;
    [SerializeField] private float dropChance = 0.3f; // 30% 확률
    
    public void OnDeath()
    {
        if (Random.value <= dropChance)
        {
            Instantiate(healthPackPrefab, transform.position, Quaternion.identity);
        }
    }
}
```

### 일정 시간 후 자동 제거 (HealthPack.cs에 추가)
```csharp
[Header("Lifetime")]
[SerializeField] private bool autoDestroy = false;
[SerializeField] private float lifetime = 30f;

private void Start()
{
    // ...기존 코드...
    
    if (autoDestroy)
    {
        Destroy(gameObject, lifetime);
    }
}
```

---

## 관련 파일
- `Assets/Scripts/Items/HealthPack.cs` - 힐팩 아이템 스크립트
- `Assets/Scripts/Combat/Entities/PlayerHealth.cs` - 플레이어 체력 시스템

---

## 추가 도움말
문제가 해결되지 않으면 다음을 확인하세요:
1. Console 창의 에러 메시지
2. 플레이어 Tag 설정
3. Collider 설정 (Is Trigger)
4. PlayerHealth 컴포넌트 존재 여부

