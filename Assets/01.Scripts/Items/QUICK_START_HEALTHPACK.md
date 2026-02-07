# 힐팩 아이템 빠른 시작 가이드 ⚡

## 5분 안에 힐팩 만들기!

### 1단계: 프리팹 생성 (1분)
1. Hierarchy 우클릭 → `Create Empty`
2. 이름: `HealthPack`
3. 자식으로 Cube 추가 (크기: 0.5, 0.5, 0.5)
4. Cube를 초록색으로 설정

### 2단계: 컴포넌트 설정 (2분)
1. **HealthPack에 Sphere Collider 추가**
   - Add Component → Sphere Collider
   - ✅ **Is Trigger 체크**
   - Radius: 0.5

2. **HealthPack 스크립트 추가**
   - Add Component → HealthPack

### 3단계: 스크립트 설정 (1분)
- Heal Amount: `30` (또는 원하는 값)
- Use Absolute Value: ✅ 체크
- Rotate Item: ✅ 체크
- Float Item: ✅ 체크

### 4단계: 프리팹 저장 (30초)
- Project 창에 "Prefabs" 폴더 생성 (없다면)
- HealthPack을 Prefabs 폴더로 드래그

### 5단계: 테스트 (30초)
1. 씬에 힐팩 배치
2. 플레이어가 "Player" Tag인지 확인
3. 플레이 모드 실행
4. 플레이어로 힐팩에 다가가기
5. ✅ 체력 회복 & 아이템 사라짐!

---

## 체크리스트 ✓
- [ ] HealthPack 오브젝트 생성
- [ ] Collider 추가 & Is Trigger 체크
- [ ] HealthPack 스크립트 추가
- [ ] 플레이어 Tag = "Player"
- [ ] 테스트 완료!

---

## 다양한 힐팩 만들기

### 소형 힐팩 (30 HP)
```
Heal Amount: 30
Use Absolute Value: ✓
색상: 연한 초록
크기: 0.3
```

### 대형 힐팩 (50% 회복)
```
Heal Amount: 50
Use Absolute Value: ✗
색상: 진한 초록
크기: 0.6
```

### 완전 회복 힐팩 (100%)
```
Heal Amount: 100
Use Absolute Value: ✗
색상: 금색
크기: 0.8
```

---

## 문제 해결 (30초 진단)

❌ **아이템을 먹어도 반응 없음**
→ Is Trigger 체크했나요?
→ 플레이어 Tag가 "Player"인가요?

❌ **아이템이 바닥으로 떨어짐**
→ Rigidbody 제거하세요!

❌ **체력이 회복되지 않음**
→ 플레이어에 PlayerHealth 컴포넌트 있나요?

---

완료! 🎉 이제 게임에서 힐팩을 사용할 수 있습니다!

자세한 내용은 `README_HEALTHPACK.md` 참고

