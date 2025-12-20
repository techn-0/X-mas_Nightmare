# 눈사람 AI 빠른 체크리스트

## 필수 설정 (5분 안에 완료)

### ✅ 1단계: 눈사람 오브젝트 준비
- [ ] Hierarchy에 빈 GameObject 생성하고 이름을 "Snowman"으로 변경
- [ ] Snowamn.fbx 모델을 자식으로 추가
- [ ] Collider 추가 (Capsule Collider 권장)

### ✅ 2단계: 컴포넌트 추가
- [ ] **EnemyHealth** 컴포넌트 추가
  - Max HP: 100
- [ ] **SnowmanAI** 컴포넌트 추가

### ✅ 3단계: SnowmanAI 설정
- [ ] **Snowball Prefab**: `Assets/Snowball.prefab` 드래그
- [ ] **Player**: Player 오브젝트 드래그 (또는 비워두기)
- [ ] 나머지는 기본값 사용 가능

### ✅ 4단계: 플레이어 확인
- [ ] Player 오브젝트의 **Tag**가 "Player"인지 확인
- [ ] Player에 **PlayerHealth** 컴포넌트가 있는지 확인
- [ ] Player의 **Layer** 확인 (기본적으로 Layer 6)

### ✅ 5단계: 테스트
- [ ] Play 버튼 클릭
- [ ] 플레이어를 눈사람 근처로 이동
- [ ] 눈사람이 추적하고 공격하는지 확인

## 기본 파라미터 값

```
SnowmanAI 컴포넌트:
- Move Speed: 2
- Rotation Speed: 5
- Ranged Attack Distance: 10
- Melee Attack Distance: 2
- Ranged Attack Cooldown: 2
- Melee Attack Cooldown: 1.5
- Melee Damage: 20
- Melee Range: 2.5
```

## 문제 발생 시

1. **눈덩이를 안 던짐** → Snowball Prefab 할당 확인
2. **플레이어를 안 찾음** → Player 태그 확인
3. **데미지가 안 들어감** → PlayerHealth 컴포넌트 확인, Layer 확인

상세한 내용은 `SNOWMAN_AI_SETUP_GUIDE.md`를 참고하세요.

