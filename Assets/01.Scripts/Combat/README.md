# 🔥❄️ 화염방사기 & 눈덩이 전투 시스템 - 완전 가이드

Unity 에디터에서 수동으로 설정하여 테스트하는 완전한 가이드입니다.

---

## 📚 가이드 목록

### 🚀 시작하기
1. **[빠른 체크리스트](./QUICK_CHECKLIST.md)** ⭐ **여기서 시작!**
   - 30분 안에 테스트 시작
   - 단계별 체크리스트
   - 핵심 포인트 요약

### 📖 상세 가이드
2. **[화염방사기 & 눈덩이 설정 가이드](./FLAMETHROWER_SNOWBALL_SETUP_GUIDE.md)**
   - 모든 단계 자세한 설명
   - 스크린샷 설명 포함
   - 선택 사항 포함

3. **[Inspector 설정 참고](./INSPECTOR_REFERENCE.md)**
   - 각 컴포넌트별 정확한 값
   - 연결 관계 도표
   - 실시간 값 확인 방법

### 🔧 문제 해결
4. **[문제 해결 가이드](./TROUBLESHOOTING.md)**
   - 증상별 해결 방법
   - 진단 체크리스트
   - 일반적인 실수 정리

---

## ⚡ 초급자용 빠른 시작 (30분)

### 단계 요약
1. **레이어 설정** (5분) - Player, Enemy 레이어 생성
2. **프리팹 생성** (12분) - FlameHitbox, Snowball
3. **플레이어 설정** (8분) - Player GameObject + 컴포넌트
4. **눈사람 설정** (8분) - Snowman GameObject + AI
5. **테스트!** (1분) - Play 버튼 클릭

👉 **[QUICK_CHECKLIST.md](./QUICK_CHECKLIST.md)** 를 열고 따라하세요!

---

## 🎯 이 시스템으로 구현할 수 있는 것

### ✅ 플레이어 기능
- 🔥 **화염방사기**: 지속 피해, 연료 시스템
- 🎮 **WASD 이동**: 자유로운 이동
- 🖱️ **마우스 조준**: 방향 전환
- 💚 **체력 시스템**: 피격, 회복, 무적 시간

### ✅ 눈사람 Enemy 기능
- ❄️ **눈덩이 투사체**: 자동 발사
- 🎯 **자동 조준**: 플레이어 추적
- 🔄 **자동 회전**: 플레이어를 향함
- 💔 **체력 시스템**: 피격, 사망

---

## 📋 필요한 것

### Unity 설정
- **Unity 버전**: 2020.3 이상 (대부분의 버전 호환)
- **Render Pipeline**: Built-in (기본)
- **Physics**: 3D Physics (기본)

### 이미 준비된 스크립트들
이 프로젝트에는 이미 다음 스크립트들이 준비되어 있습니다:

**Core 시스템:**
- `HealthSystem.cs` - 기본 체력 시스템
- `DamageInfo.cs` - 피해 정보
- `DamageType.cs` - 피해 타입 (Fire, Ice 등)

**감지 시스템:**
- `HitBox.cs` - 화염방사기용 히트박스
- `Projectile.cs` - 눈덩이 투사체

**엔티티:**
- `PlayerHealth.cs` - 플레이어 체력
- `EnemyHealth.cs` - 적 체력

**예제:**
- `PlayerFlamethrower.cs` - 화염방사기 기능
- `ProjectileLauncher.cs` - 투사체 발사기
- `SimplePlayerController.cs` - 플레이어 조작
- `SimpleSnowmanAI.cs` - 눈사람 AI

**효과:**
- `DotEffect.cs` - 지속 피해 효과
- `InvincibilityController.cs` - 무적 효과
- `HitFeedback.cs` - 피격 피드백

**UI:**
- `HealthBarUI.cs` - 체력바

---

## 🎮 조작법

### 플레이어
- **W/A/S/D**: 이동
- **마우스 이동**: 조준 (플레이어가 마우스 방향으로 회전)
- **좌클릭 (누르고 있기)**: 화염방사기 발사
- **우클릭**: 근접 공격 (PlayerMeleeAttack 추가 시)

### 눈사람 (자동)
- 자동으로 플레이어 감지
- 자동으로 플레이어를 향해 회전
- 자동으로 눈덩이 발사

---

## 🔑 핵심 개념

### Layer System
- **Player Layer**: 플레이어 전용
- **Enemy Layer**: 적 전용
- **Target Layers**: 어떤 레이어를 공격할지 설정

### Trigger vs Collider
- **Trigger (Is Trigger ✅)**: 통과하면서 이벤트 발생 (피해 감지용)
- **Collider (Is Trigger ❌)**: 물리적 충돌 (이동 막기용)

### Prefab
- 재사용 가능한 GameObject 템플릿
- 화염 박스와 눈덩이는 반복 생성되므로 Prefab 필요

---

## 📊 기본 값 (밸런스)

### 플레이어
- **체력**: 100
- **화염 피해**: 초당 20 (0.1초마다 2 피해)
- **화염 길이**: 3m
- **연료**: 100 (초당 10 소모, 초당 5 회복)

### 눈사람
- **체력**: 50
- **눈덩이 피해**: 15
- **발사 속도**: 1.5초에 1발
- **감지 범위**: 15m

**이 값들은 자유롭게 조정 가능합니다!**

---

## 🎨 커스터마이징

### 화염방사기 강화
```
PlayerFlamethrower 컴포넌트:
- Damage Per Second: 20 → 50 (더 강한 피해)
- Flame Length: 3 → 5 (더 긴 사거리)
- Fuel Consumption Rate: 10 → 5 (더 오래 사용)
```

### 눈사람 강화
```
Snowball Prefab:
- Base Damage: 15 → 30 (더 강한 피해)

ProjectileLauncher:
- Fire Rate: 1.5 → 0.5 (더 빠른 발사)
- Launch Force: 15 → 25 (더 빠른 속도)
```

### 비주얼 개선
- **Particle System**: 화염 효과 추가
- **Trail Renderer**: 눈덩이 궤적
- **Material**: 색상과 투명도 조정
- **Post Processing**: 히트 효과, 화면 흔들림

---

## 📁 파일 구조

```
Assets/Scripts/Combat/
├─ Core/                    # 핵심 시스템
│  ├─ HealthSystem.cs
│  ├─ DamageInfo.cs
│  └─ DamageType.cs
│
├─ Detection/               # 감지 시스템
│  ├─ HitBox.cs            (화염방사기용)
│  └─ Projectile.cs        (눈덩이용)
│
├─ Entities/               # 플레이어/적
│  ├─ PlayerHealth.cs
│  └─ EnemyHealth.cs
│
├─ Examples/               # 예제 코드
│  ├─ PlayerFlamethrower.cs        ⭐
│  ├─ ProjectileLauncher.cs        ⭐
│  ├─ SimplePlayerController.cs    ⭐
│  ├─ SimpleSnowmanAI.cs          ⭐
│  ├─ PlayerMeleeAttack.cs
│  └─ HealthBarUI.cs
│
├─ Effects/                # 효과
│  ├─ DotEffect.cs
│  └─ ...
│
├─ Feedback/               # 피드백
│  ├─ HitFeedback.cs
│  └─ InvincibilityController.cs
│
└─ [가이드 문서들]
   ├─ README.md                               (이 파일)
   ├─ QUICK_CHECKLIST.md                      ⭐ 시작!
   ├─ FLAMETHROWER_SNOWBALL_SETUP_GUIDE.md    📖 상세
   ├─ INSPECTOR_REFERENCE.md                  📊 참고
   └─ TROUBLESHOOTING.md                      🔧 문제해결
```

---

## ✅ 성공 체크리스트

### 설정 완료 후 확인
- [ ] Play 버튼 클릭 시 에러 없음
- [ ] WASD로 플레이어 이동
- [ ] 마우스로 플레이어 회전
- [ ] 좌클릭 시 화염 발사 (주황색 박스)
- [ ] 화염이 눈사람 맞으면 체력 감소
- [ ] 눈사람이 자동으로 눈덩이 발사 (흰색 구체)
- [ ] 눈덩이가 플레이어 맞으면 체력 감소
- [ ] Console에 피해 로그 표시

### Inspector 확인
- [ ] Player > PlayerFlamethrower > Current Fuel (실시간 변화)
- [ ] Player > PlayerHealth > Current Health (실시간 변화)
- [ ] Snowman > EnemyHealth > Current Health (실시간 변화)

### Scene 뷰 확인
- [ ] Gizmos 켜짐 (Scene 뷰 우측 상단)
- [ ] 화염박스 주변 초록색 와이어 박스
- [ ] 눈사람 주변 노란색 감지 범위 원

---

## 🚀 다음 단계

### 1. 기본 테스트 완료 후
- [ ] 여러 눈사람 추가 (Ctrl+D로 복제)
- [ ] 체력바 UI 추가 (HealthBarUI 사용)
- [ ] 파티클 효과 강화
- [ ] 사운드 추가 (AudioSource)

### 2. 고급 기능 추가
- [ ] DOT 효과 (DotEffect 스크립트 활용)
- [ ] 크리티컬 히트
- [ ] 다양한 적 타입
- [ ] 보스 패턴

### 3. 게임화
- [ ] 레벨 디자인
- [ ] 웨이브 시스템
- [ ] 점수 시스템
- [ ] 게임 오버/승리 조건

---

## 💡 팁과 트릭

### 빠른 테스트
1. Play 모드에서 값 조정 (즉시 확인 가능)
2. 좋은 값 발견하면 메모
3. Play 중지 후 Edit 모드에서 재설정

### 성능 최적화
- Object Pooling (투사체 재사용)
- Hitbox 생명 시간 제한
- Particle System Max Particles 제한

### 디버깅
- Console 로그 활용
- Gizmos로 시각화
- Scene 뷰와 Game 뷰 동시에 보기

---

## 🐛 자주 하는 실수

### ❌ 하지 마세요
1. Layer를 설정하지 않음
2. Is Trigger를 체크 안 함
3. Target Layers를 설정 안 함
4. Prefab 연결을 안 함
5. Play 모드에서 값 변경 후 저장 안 함

### ✅ 이렇게 하세요
1. 모든 GameObject에 올바른 Layer 설정
2. 피해 감지용은 모두 Is Trigger 체크
3. Target Layers 정확히 설정 (Player ↔ Enemy)
4. 모든 필드 연결 확인
5. Edit 모드에서만 최종 값 설정

---

## 📞 도움말

### 문제가 생기면
1. **Console 확인** (Ctrl+Shift+C) - 에러 메시지 읽기
2. **Inspector 확인** - 빨간 글씨 (Missing) 찾기
3. **TROUBLESHOOTING.md** - 증상별 해결책
4. **QUICK_CHECKLIST.md** - 모든 체크리스트 확인

### 유용한 단축키
- **Ctrl+S**: 씬 저장
- **Ctrl+D**: 복제
- **F**: 선택한 오브젝트로 포커스
- **Ctrl+Shift+C**: Console 열기
- **Space**: Play/Stop 토글

---

## 📖 추가 학습 자료

### Unity 공식 문서
- [Physics.OverlapSphere](https://docs.unity3d.com/ScriptReference/Physics.OverlapSphere.html)
- [OnTriggerEnter](https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnTriggerEnter.html)
- [Rigidbody](https://docs.unity3d.com/ScriptReference/Rigidbody.html)

### 관련 개념
- Layer Mask
- Trigger vs Collider
- Prefab System
- Unity Events

---

## 🎉 완성!

이제 준비가 끝났습니다!

**[QUICK_CHECKLIST.md](./QUICK_CHECKLIST.md)** 를 열고

30분 안에 화염방사기와 눈덩이 전투를 테스트해보세요!

**행운을 빕니다! 🔥❄️🎮**

---

## 📝 버전 정보

- **버전**: 1.0
- **최종 업데이트**: 2025-12-20
- **Unity 호환**: 2020.3 이상
- **제작**: Combat System Example

---

**질문이나 문제가 있으면 TROUBLESHOOTING.md를 먼저 확인하세요!**

