# 🧪 데미지 시스템 테스트 가이드 (수동 설정)

## 📋 목표
- 플레이어 박스가 적 박스를 공격
- 적 박스가 플레이어 박스를 공격
- 데미지 확인 및 사망 확인

---

## ⚙️ 1단계: 레이어 설정 (필수!)

### 1-1. 레이어 생성
1. **Edit > Project Settings** 클릭
2. 왼쪽 메뉴에서 **Tags and Layers** 선택
3. **Layers** 섹션에서:
   - `Layer 6`에 `Player` 입력
   - `Layer 7`에 `Enemy` 입력

### 1-2. 충돌 매트릭스 설정
1. **Edit > Project Settings** 클릭
2. 왼쪽 메뉴에서 **Physics** 선택
3. 아래로 스크롤하여 **Layer Collision Matrix** 찾기
4. 다음과 같이 체크/해제:
   - ✅ **Player와 Enemy** 체크 (서로 충돌)
   - ❌ **Player와 Player** 해제 (플레이어끼리 충돌 X)
   - ❌ **Enemy와 Enemy** 해제 (적끼리 충돌 X)

---

## 🎮 2단계: 테스트 씬 준비

### 2-1. 새 씬 생성
1. **File > New Scene** (또는 `Ctrl+N`)
2. **Basic (Built-in)** 선택
3. **File > Save As** → `TestCombatScene` 이름으로 저장

### 2-2. 카메라 설정
1. **Main Camera** 선택
2. Inspector에서:
   - Position: `X=0, Y=10, Z=-5`
   - Rotation: `X=45, Y=0, Z=0`
   - (탑다운 뷰로 설정)

---

## 👤 3단계: 플레이어 생성

### 3-1. 플레이어 오브젝트 생성
1. Hierarchy에서 **우클릭 > 3D Object > Cube**
2. 이름을 `Player`로 변경
3. Inspector에서:
   - **Position**: `X=0, Y=0.5, Z=0`
   - **Layer**: `Player` 선택

### 3-2. PlayerHealth 추가
1. Player 선택된 상태에서
2. Inspector 하단 **Add Component** 클릭
3. 검색창에 `PlayerHealth` 입력
4. **Player Health (Script)** 선택
5. Inspector에서 확인:
   - Max HP: `100` (기본값)

### 3-3. 플레이어 무기(HitBox) 생성
1. Player 선택
2. Hierarchy에서 **우클릭 > Create Empty**
3. 이름을 `Weapon`으로 변경
4. Weapon 선택 > **우클릭 > 3D Object > Cube**
5. 이름을 `HitBox`로 변경
6. HitBox Inspector 설정:
   - **Position**: `X=1, Y=0, Z=0` (플레이어 오른쪽)
   - **Scale**: `X=1.5, Y=0.5, Z=0.5` (작게)

### 3-4. HitBox 컴포넌트 설정
1. HitBox 선택
2. **Add Component** > `HitBox` 검색 > 추가
3. HitBox (Script) 설정:
   - **Damage**: `20`
   - **Damage Type**: `Fire`
   - **Target Layer**: `Enemy` 선택
   - **Hit Once**: ✅ 체크

### 3-5. HitBox Collider 설정
1. HitBox에 이미 있는 **Box Collider** 선택
2. **Is Trigger**: ✅ 체크 (중요!)


### 3-7. 플레이어 공격 스크립트 추가
1. Player 선택
2. **Add Component** > `PlayerMeleeAttack` 검색 > 추가
3. Player Melee Attack (Script) 설정:
   - **Weapon Hit Box**: HitBox를 드래그해서 할당
   - **Use Mouse Left Click**: ✅ 체크 (마우스 좌클릭 사용)
   - **Alternative Key**: `Space` (키보드 대체키)
   - **Attack Cooldown**: `0.5`

> **참고**: New Input System을 사용하므로 마우스 좌클릭 또는 스페이스바로 공격 가능합니다.

### 3-8. 플레이어 색상 변경 (선택사항)
1. Player 선택
2. Inspector에서 **Mesh Renderer > Materials**
3. **Default-Material** 더블클릭
4. **Albedo** 색상을 파란색으로 변경

---

## 👾 4단계: 적(Enemy) 생성

### 4-1. 적 오브젝트 생성
1. Hierarchy에서 **우클릭 > 3D Object > Cube**
2. 이름을 `Enemy`로 변경
3. Inspector에서:
   - **Position**: `X=5, Y=0.5, Z=0` (플레이어 오른쪽에)
   - **Layer**: `Enemy` 선택

### 4-2. EnemyHealth 추가
1. Enemy 선택
2. **Add Component** > `EnemyHealth` 검색 > 추가
3. Inspector에서:
   - Max HP: `50`

### 4-3. 적 무기(HitBox) 생성
1. Enemy 선택
2. Hierarchy에서 **우클릭 > Create Empty**
3. 이름을 `EnemyWeapon`으로 변경
4. EnemyWeapon 선택 > **우클릭 > 3D Object > Cube**
5. 이름을 `EnemyHitBox`로 변경
6. EnemyHitBox Inspector 설정:
   - **Position**: `X=-1, Y=0, Z=0` (적 왼쪽)
   - **Scale**: `X=1.5, Y=0.5, Z=0.5`

### 4-4. EnemyHitBox 컴포넌트 설정
1. EnemyHitBox 선택
2. **Add Component** > `HitBox` 검색 > 추가
3. HitBox (Script) 설정:
   - **Damage**: `10`
   - **Damage Type**: `Physical`
   - **Target Layer**: `Player` 선택
   - **Hit Once**: ✅ 체크

### 4-5. EnemyHitBox Collider 설정
1. EnemyHitBox의 **Box Collider** 선택
2. **Is Trigger**: ✅ 체크

### 4-6. 적 공격 스크립트 추가
1. Enemy 선택
2. **Add Component** > `PlayerMeleeAttack` 검색 > 추가
   (적도 같은 스크립트 사용 가능)
3. 설정:
   - **Weapon Hit Box**: EnemyHitBox 드래그해서 할당
   - **Use Mouse Left Click**: ❌ 해제
   - **Alternative Key**: `Q` 로 변경 (Q키로 적 공격)
   - **Attack Cooldown**: `1`

> **참고**: 플레이어는 마우스 좌클릭/스페이스, 적은 Q키로 설정하여 구분합니다.

### 4-7. 적 색상 변경
1. Enemy 선택
2. **Mesh Renderer > Materials > Default-Material**
3. **Albedo** 색상을 빨간색으로 변경

---

## 🎯 5단계: 테스트!

### 5-1. HitBox 초기 상태 확인
**중요!** HitBox GameObject가 비활성화되어 있는지 확인하세요.

1. **Player/Weapon/HitBox** 선택
2. Inspector 상단의 체크박스 **해제** (GameObject 비활성화) - 이미 되어 있을 것임
3. **Enemy/EnemyWeapon/EnemyHitBox** 선택
4. Inspector 상단의 체크박스 **해제** - 이미 되어 있을 것임

> **참고**: HitBox 스크립트의 Activate() 메서드가 자동으로 GameObject를 켜고 끕니다.

### 5-2. 플레이 모드 실행
1. **Play 버튼** 클릭 (또는 `Ctrl+P`)

### 5-3. 테스트 방법
1. **WASD 또는 화살표 키**: 플레이어 이동 (적에게 가까이 다가가세요!)
2. **마우스 좌클릭 또는 스페이스바**: 플레이어가 적 공격
3. **Q 키**: 적이 플레이어 공격

> **중요**: 플레이어와 적이 충분히 가까워야 HitBox가 Enemy에 닿습니다! WASD로 적에게 다가가세요.

### 5-4. 확인 사항
**Console 창** (Window > General > Console)에서 확인:
- ✅ `Enemy took 20 Fire damage. HP: 30/50`
- ✅ `Player took 10 Physical damage. HP: 90/100`
- ✅ `Enemy died!` (3번 공격 후)
- ✅ `Player died!` (10번 공격 후)

**Scene 창**에서 확인:
- ✅ 피격 시 깜빡임 (무적 시간)
- ✅ 사망 시 오브젝트 사라짐

---

## 🐛 문제 해결

### 데미지가 안 들어가요!
1. **레이어 확인**
   - Player는 `Player` 레이어
   - Enemy는 `Enemy` 레이어
   
2. **Layer Collision Matrix 확인**
   - Edit > Project Settings > Physics
   - Player와 Enemy가 체크되어 있는지
   
3. **Is Trigger 확인**
   - HitBox의 Collider에서 Is Trigger 체크
   
4. **Target Layer 확인**
   - Player HitBox: Target Layer = Enemy
   - Enemy HitBox: Target Layer = Player

5. **HitBox가 활성화되지 않음**
   - Console에 "HitBox activated" 같은 메시지가 없으면
   - PlayerMeleeAttack이 제대로 연결되지 않은 것

### 공격 버튼을 눌러도 반응이 없어요!
1. **Weapon Hit Box 할당 확인**
   - PlayerMeleeAttack의 Weapon Hit Box에 HitBox가 할당되었는지
   
2. **Animator 경고 무시**
   - Animator가 없어도 공격은 작동합니다
   - 경고는 무시해도 됩니다

### 한 번만 맞고 더 이상 안 맞아요!
1. **Hit Once 설정 확인**
   - Hit Once가 체크되어 있으면 한 공격당 한 번만 맞음
   - 다시 공격하려면 버튼을 떼고 다시 눌러야 함

### 계속 데미지가 들어가요!
1. **HitBox 비활성화 확인**
   - 초기에 HitBox GameObject가 꺼져있어야 함
   - Hierarchy에서 회색으로 표시되어야 정상

---

## 🎨 추가 테스트 (선택사항)

### A. 화염방사기(DoT) 테스트

#### A-1. 플레이어에 화염방사기 추가
1. Player 선택
2. **Add Component** > `PlayerFlamethrower` 추가
3. Player에 새로운 자식 오브젝트 생성:
   - **우클릭 > Create Empty** → `Flamethrower`
   - Flamethrower 하위에 **Cube** 추가 → `FlameHitBox`
   - Position: `X=2, Y=0, Z=0`
   - Scale: `X=2, Y=0.5, Z=1`

#### A-2. FlameHitBox 설정
1. FlameHitBox 선택
2. **Add Component** > `HitBox`
3. 설정:
   - Damage: `5`
   - Damage Type: `Fire`
   - **Deals Dot Damage**: ✅ 체크
   - **Dot Duration**: `3`
   - **Dot Damage Per Tick**: `5`
   - **Dot Tick Interval**: `0.5`
   - Target Layer: `Enemy`
   - **Hit Once**: ❌ 해제 (지속 공격)
4. Box Collider의 **Is Trigger**: ✅ 체크
5. FlameHitBox GameObject **비활성화**

#### A-3. PlayerFlamethrower 설정
1. Player 선택
2. Player Flamethrower (Script):
   - **Flame Hit Box**: FlameHitBox 할당
   - **Use Mouse Right Click**: ✅ 체크
   - **Alternative Key**: `F` (F키로도 사용 가능)

#### A-4. 테스트
- **마우스 우클릭 누르고 있기 또는 F키**: 화염방사
- Console에서 틱 데미지 확인: `Enemy took 5 Fire damage` (0.5초마다)

---

### B. 투사체 테스트

#### B-1. 투사체 프리팹 생성
1. Hierarchy에서 **우클릭 > 3D Object > Sphere**
2. 이름: `Projectile`
3. Scale: `X=0.3, Y=0.3, Z=0.3` (작게)
4. **Add Component** > `Projectile` 추가
5. 설정:
   - Damage: `15`
   - Damage Type: `Physical`
   - Speed: `10`
   - Lifetime: `5`
   - Target Layer: `Enemy`
6. **Add Component** > `Rigidbody` 추가
   - Use Gravity: ❌ 해제
7. Sphere Collider:
   - **Is Trigger**: ✅ 체크
8. Projectile을 **Project 창의 Assets 폴더로 드래그** (프리팹 생성)
9. Hierarchy에서 Projectile **삭제**

#### B-2. 플레이어에 발사기 추가
1. Player 선택
2. **Add Component** > `ProjectileLauncher` 추가
3. 설정:
   - **Projectile Prefab**: Project 창의 Projectile 프리팹 할당
   - **Fire Point**: Player 자신 할당
   - **Use Input**: ✅ 체크
   - **Shoot Key**: `E`

#### B-3. 테스트
- **E 키**: 투사체 발사
- 투사체가 Enemy와 충돌하면 데미지

---

## 📊 최종 테스트 체크리스트

### 씬 구성 확인
```
Hierarchy:
├── Main Camera
├── Directional Light
├── Player (Layer: Player, 파란색)
│   ├── PlayerHealth
│   ├── PlayerMeleeAttack
│   ├── (선택) PlayerFlamethrower
│   ├── (선택) ProjectileLauncher
│   └── Weapon
│       └── HitBox (비활성화)
│           ├── HitBox (Script)
│           └── Box Collider (Is Trigger ✅)
└── Enemy (Layer: Enemy, 빨간색)
    ├── EnemyHealth
    ├── PlayerMeleeAttack
    └── EnemyWeapon
        └── EnemyHitBox (비활성화)
            ├── HitBox (Script)
            └── Box Collider (Is Trigger ✅)
```

### 기능 확인
- [ ] 플레이어가 적 공격 (마우스 좌클릭 또는 스페이스)
- [ ] 적이 플레이어 공격 (Q 키)
- [ ] Console에 데미지 로그 출력
- [ ] 피격 시 깜빡임 (무적 시간)
- [ ] 체력 0 되면 사망 (오브젝트 사라짐)
- [ ] (선택) 화염방사기 (마우스 우클릭 또는 F키)
- [ ] (선택) 투사체 발사 (E 키)

---

## 🎉 완료!

이제 데미지 시스템이 정상적으로 작동합니다!

### 다음 단계
1. 팀원들과 공유
2. 실제 캐릭터 모델로 교체
3. 애니메이션 연동
4. AI 통합
5. UI 체력바 추가

---

**예상 소요 시간: 15-20분**

