# 눈사람 몹 AI 시스템 - 구현 완료

## 📋 구현 내용

### 생성된 파일
1. **SnowmanAI.cs** - 눈사람 AI 시스템 메인 코드
   - 위치: `Assets/Scripts/Combat/Entities/SnowmanAI.cs`
   
2. **SNOWMAN_AI_SETUP_GUIDE.md** - 상세 설정 가이드
   - 위치: `Assets/Scripts/Combat/Entities/SNOWMAN_AI_SETUP_GUIDE.md`
   
3. **SNOWMAN_AI_QUICK_CHECKLIST.md** - 빠른 체크리스트
   - 위치: `Assets/Scripts/Combat/Entities/SNOWMAN_AI_QUICK_CHECKLIST.md`

## 🎯 구현된 기능

### AI 동작
✅ **플레이어 추적**
- 항상 플레이어를 바라봄
- 플레이어 방향으로 이동

✅ **원거리 공격 (눈덩이 던지기)**
- 플레이어가 멀리 있을 때 자동으로 눈덩이 발사
- 쿨다운 시스템으로 공격 빈도 조절
- 기존 Projectile 시스템 활용

✅ **근접 공격**
- 플레이어와 가까이 있을 때 근접 공격
- 눈덩이 던지기 중단하고 근접 공격으로 전환
- 데미지 시스템과 연동

### 코드 특징
✅ **간단하고 명확한 구조**
- 약 200줄의 간결한 코드
- 명확한 주석과 문서화
- 기존 Combat 시스템과 완벽히 통합

✅ **커스터마이징 가능**
- Inspector에서 모든 파라미터 조절 가능
- 이동 속도, 공격 거리, 데미지 등 자유롭게 설정

✅ **디버깅 도구**
- Scene 뷰에서 공격 범위 시각화
- Gizmos로 거리 확인 가능

## 🛠️ 유니티 에디터 설정 (간단 버전)

### 1. 눈사람 오브젝트 생성
```
1. Hierarchy → Create Empty → 이름: "Snowman"
2. Snowamn.fbx를 자식으로 추가
3. Collider 추가 (Capsule Collider)
```

### 2. 컴포넌트 추가
```
1. EnemyHealth 컴포넌트 추가
   - Max HP: 100

2. SnowmanAI 컴포넌트 추가
   - Snowball Prefab: Snowball.prefab 드래그
   - Player: 비워두기 (자동으로 찾음) 또는 Player 오브젝트 드래그
```

### 3. 플레이어 확인
```
1. Player 오브젝트 Tag = "Player"
2. PlayerHealth 컴포넌트 있는지 확인
```

### 4. 테스트
```
Play 버튼 → 플레이어 이동 → 눈사람 동작 확인
```

## 📊 기본 파라미터

| 파라미터 | 기본값 | 설명 |
|---------|--------|------|
| Move Speed | 2 | 이동 속도 |
| Rotation Speed | 5 | 회전 속도 |
| Ranged Attack Distance | 10 | 원거리 공격 시작 거리 |
| Melee Attack Distance | 2 | 근접 공격 전환 거리 |
| Ranged Attack Cooldown | 2초 | 눈덩이 투척 간격 |
| Melee Attack Cooldown | 1.5초 | 근접 공격 간격 |
| Melee Damage | 20 | 근접 공격 데미지 |

## 🔧 활용된 기존 시스템

- ✅ **Projectile 시스템**: 눈덩이 발사에 활용
- ✅ **IDamageable 인터페이스**: 플레이어 데미지 처리
- ✅ **EnemyHealth**: 눈사람 체력 관리
- ✅ **DamageInfo**: 데미지 정보 전달
- ✅ **HealthSystem**: 체력 시스템 통합

## 📝 사용 방법

1. 상세 가이드: `SNOWMAN_AI_SETUP_GUIDE.md` 참고
2. 빠른 설정: `SNOWMAN_AI_QUICK_CHECKLIST.md` 참고

## ⚙️ 커스터마이징

### 더 어렵게 만들기
- Move Speed 증가 (예: 3~4)
- Attack Cooldown 감소 (예: 1초)
- Melee Damage 증가 (예: 30)

### 더 쉽게 만들기
- Move Speed 감소 (예: 1~1.5)
- Attack Cooldown 증가 (예: 3~4초)
- Melee Damage 감소 (예: 10~15)

## 🎮 테스트 시나리오

1. **추적 테스트**: 플레이어가 도망가도 쫓아오는지
2. **원거리 공격 테스트**: 멀리 있을 때 눈덩이를 던지는지
3. **근접 공격 테스트**: 가까이 있을 때 근접 공격하는지
4. **전환 테스트**: 거리에 따라 공격 방식이 바뀌는지

## 완료! 🎉

이제 유니티 에디터에서 위의 설정만 하면 눈사람 AI가 작동합니다!

