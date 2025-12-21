# 힐팩 아이템 시스템 - 완성! 🎉

## 생성된 파일들

### 📜 스크립트 (Assets/Scripts/Items/)
1. **HealthPack.cs** - 메인 힐팩 아이템 스크립트
2. **HealthPackDropper.cs** - 적이 죽을 때 힐팩 드롭
3. **HealthPackSpawner.cs** - 주기적으로 힐팩 스폰

### 📖 문서
1. **README_HEALTHPACK.md** - 상세 사용 가이드
2. **QUICK_START_HEALTHPACK.md** - 5분 빠른 시작
3. **SUMMARY_HEALTHPACK.md** - 이 파일

---

## 주요 기능

### HealthPack.cs
✅ 플레이어 체력 회복
✅ 절대값/비율(%) 회복량 설정
✅ 최대 체력일 때 획득 방지
✅ 회전 & 떠다니는 효과
✅ 획득 이펙트 & 사운드
✅ 자동으로 사라짐

### HealthPackDropper.cs
✅ 확률 기반 드롭
✅ 강제 드롭 (보스용)
✅ 다중 드롭 지원

### HealthPackSpawner.cs
✅ 주기적 스폰
✅ 최대 개수 제한
✅ 랜덤 위치 스폰
✅ 자동/수동 시작

---

## 빠른 사용법

### 1. 기본 힐팩 만들기 (2분)
```
1. 빈 오브젝트 생성 (HealthPack)
2. Sphere Collider 추가 (Is Trigger ✓)
3. HealthPack.cs 스크립트 추가
4. 프리팹으로 저장
```

### 2. 적이 힐팩 드롭하게 만들기
```csharp
// 적 오브젝트에 HealthPackDropper.cs 추가
// EnemyHealth의 OnDeath 이벤트에 연결:
healthPackDropper.TryDropHealthPack();
```

### 3. 자동으로 힐팩 스폰하기
```
1. 빈 오브젝트 생성 (HealthPackSpawner)
2. HealthPackSpawner.cs 추가
3. Health Pack Prefab 할당
4. Auto Start ✓ 체크
```

---

## 설정 예시

### 소형 힐팩 (30 HP)
- Heal Amount: 30
- Use Absolute Value: ✓
- 초록색 작은 큐브

### 대형 힐팩 (50% 회복)
- Heal Amount: 50
- Use Absolute Value: ✗
- 초록색 큰 구체

### 완전 회복 (100%)
- Heal Amount: 100
- Use Absolute Value: ✗
- 금색 별 모양

---

## 체크리스트

### 필수 설정
- [ ] PlayerHealth 컴포넌트가 플레이어에 있음
- [ ] 플레이어 Tag = "Player"
- [ ] Collider의 Is Trigger 활성화
- [ ] HealthPack 스크립트 추가

### 선택 설정
- [ ] 획득 이펙트 프리팹
- [ ] 획득 사운드
- [ ] 회전/떠다니는 효과 조정

---

## 코드 사용 예시

### 적이 죽을 때 드롭
```csharp
public class EnemyHealth : MonoBehaviour
{
    private HealthPackDropper dropper;
    
    void Start()
    {
        dropper = GetComponent<HealthPackDropper>();
    }
    
    void Die()
    {
        dropper.TryDropHealthPack(); // 30% 확률로 드롭
    }
}
```

### 보스가 항상 드롭
```csharp
public class BossHealth : MonoBehaviour
{
    private HealthPackDropper dropper;
    
    void Die()
    {
        dropper = GetComponent<HealthPackDropper>();
        dropper.ForceDropHealthPack(); // 무조건 드롭
        dropper.DropMultipleHealthPacks(3); // 3개 드롭
    }
}
```

### 특정 위치에 스폰
```csharp
public GameObject healthPackPrefab;

void SpawnAtPosition(Vector3 position)
{
    Instantiate(healthPackPrefab, position, Quaternion.identity);
}
```

---

## 문제 해결

| 문제 | 해결 방법 |
|------|----------|
| 아이템을 먹어도 반응 없음 | Is Trigger 체크, Player Tag 확인 |
| 아이템이 바닥으로 떨어짐 | Rigidbody 제거 |
| 체력이 회복되지 않음 | PlayerHealth 컴포넌트 확인 |
| 최대 체력인데 먹어짐 | 자동으로 방지됨 (로그 확인) |

---

## 추가 기능 아이디어

### 시간 제한 힐팩
```csharp
// HealthPack.cs의 Start()에 추가:
Destroy(gameObject, 30f); // 30초 후 자동 제거
```

### 희귀 힐팩
```csharp
// 회복량을 랜덤으로:
healAmount = Random.Range(30f, 100f);
```

### 체력 표시 UI
```csharp
// 힐팩 위에 "+30 HP" 텍스트 표시
```

---

## 다음 단계

1. ✅ 기본 힐팩 프리팹 만들기
2. ✅ 씬에 배치하기
3. ✅ 테스트하기
4. 이펙트/사운드 추가하기
5. 다양한 종류의 힐팩 만들기
6. 적 드롭 시스템 연동하기
7. 자동 스폰 시스템 설정하기

---

## 지원 및 문의

문제가 발생하면:
1. Console 창에서 에러 확인
2. README_HEALTHPACK.md 참고
3. QUICK_START_HEALTHPACK.md 체크리스트 확인

---

**만든 날짜**: 2025-12-21  
**버전**: 1.0  
**상태**: ✅ 완료

즐거운 게임 개발 되세요! 🎮

