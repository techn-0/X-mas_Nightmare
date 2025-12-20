# ⚡ 빠른 설정 체크리스트

## 🎯 30분 안에 테스트 시작하기

### ✅ Phase 1: 기본 설정 (5분)
1. [ ] Edit > Project Settings > Tags and Layers
   - Layer 6: `Player`
   - Layer 7: `Enemy`
2. [ ] Edit > Project Settings > Physics
   - Layer Collision Matrix: Player ↔ Enemy 체크
3. [ ] Assets 폴더에 `Prefabs` 폴더 생성

---

### ✅ Phase 2: FlameHitbox Prefab (5분)
1. [ ] Hierarchy > Create > 3D Object > Cube → 이름: `FlameHitbox`
2. [ ] Transform Scale: (0.5, 0.5, 1.0)
3. [ ] Box Collider > Is Trigger: ✅
4. [ ] Add Component > `HitBox`
   - Base Damage: `10`
   - Damage Type: `Fire`
   - Source: `Player`
   - Target Layers: `Enemy`만 체크
   - Fire Interval: `0.1`
   - Can Hit Multiple: ✅
5. [ ] Hierarchy에서 Prefabs 폴더로 드래그
6. [ ] Hierarchy에서 삭제

---

### ✅ Phase 3: Snowball Prefab (7분)
1. [ ] Hierarchy > Create > 3D Object > Sphere → 이름: `Snowball`
2. [ ] Transform Scale: (0.3, 0.3, 0.3)
3. [ ] Add Component > `Rigidbody`
   - Use Gravity: ❌
   - Collision Detection: `Continuous`
4. [ ] Sphere Collider > Is Trigger: ✅
5. [ ] Add Component > `Projectile`
   - Base Damage: `15`
   - Damage Type: `Ice`
   - Speed: `10`
   - Lifetime: `5`
   - Source: `Enemy`
   - Target Layers: `Player`만 체크
   - Destroy On Hit: ✅
   - Pierce Count: `0`
6. [ ] Hierarchy에서 Prefabs 폴더로 드래그
7. [ ] Hierarchy에서 삭제

---

### ✅ Phase 4: Player 생성 (8분)
1. [ ] Hierarchy > Create Empty → 이름: `Player`
   - Tag: `Player`
   - Layer: `Player`
   - Position: (0, 1, 0)
2. [ ] Player > Create > 3D Object > Capsule (Visual용)
3. [ ] Player > Add Component > `Capsule Collider`
4. [ ] Player > Add Component > `PlayerHealth`
5. [ ] Player > Add Component > `PlayerFlamethrower`
6. [ ] Player > Create Empty → 이름: `FlameOrigin`
   - Local Position: (0, 0.5, 0.7)
7. [ ] PlayerFlamethrower 설정:
   - Flame Origin: `FlameOrigin` 연결
   - Hitbox Prefab: `FlameHitbox` 연결
   - Flame Length: `3.0`
   - Damage Per Second: `20`
   - Fuel Capacity: `100`
   - Fuel Consumption Rate: `10`
   - Refuel Rate: `5`
8. [ ] Player > Add Component > `SimplePlayerController`

---

### ✅ Phase 5: Snowman 생성 (8분)
1. [ ] Hierarchy > Create Empty → 이름: `Snowman`
   - Tag: `Enemy` (없으면 생성)
   - Layer: `Enemy`
   - Position: (0, 1, 10)
2. [ ] Snowman > Create > 3D Object > Sphere (Body_Bottom)
   - Local Position: (0, 0, 0)
   - Local Scale: (1.2, 1.2, 1.2)
3. [ ] Snowman > Create > 3D Object > Sphere (Body_Top)
   - Local Position: (0, 1, 0)
   - Local Scale: (0.8, 0.8, 0.8)
4. [ ] Snowman > Add Component > `Capsule Collider`
   - Radius: `0.6`
   - Height: `2`
   - Center: (0, 0.5, 0)
5. [ ] Snowman > Add Component > `EnemyHealth`
6. [ ] Snowman > Add Component > `ProjectileLauncher`
7. [ ] Snowman > Create Empty → 이름: `LaunchPoint`
   - Local Position: (0, 1.2, 0.5)
8. [ ] ProjectileLauncher 설정:
   - Projectile Prefab: `Snowball` 연결
   - Launch Point: `LaunchPoint` 연결
   - Launch Force: `15`
   - Fire Rate: `1.5`
   - Auto Fire: ✅
   - Launch Angle: `0`
9. [ ] Snowman > Add Component > `SimpleSnowmanAI`
   - Player: `Player` 연결
   - Detection Range: `15`

---

### ✅ Phase 6: 환경 설정 (2분)
1. [ ] Hierarchy > Create > 3D Object > Plane → 이름: `Ground`
   - Position: (0, 0, 0)
   - Scale: (5, 1, 5)
2. [ ] Main Camera
   - Position: (0, 15, -10)
   - Rotation: (50, 0, 0)

---

### ✅ Phase 7: 테스트! (1분)
1. [ ] Ctrl+S (씬 저장)
2. [ ] Play 버튼 ▶️
3. [ ] 조작:
   - **WASD**: 이동
   - **마우스**: 방향
   - **좌클릭**: 화염방사

---

## 🔍 빠른 확인

### 작동하는지 확인
- [ ] 플레이어가 WASD로 움직임
- [ ] 플레이어가 마우스 방향으로 회전
- [ ] 좌클릭 시 주황색 박스(화염) 생성
- [ ] 화염이 눈사람 닿으면 체력 감소
- [ ] 눈사람이 자동으로 흰색 구체(눈덩이) 발사
- [ ] 눈덩이가 플레이어 닿으면 체력 감소

### Console 확인
- [ ] Window > General > Console (Ctrl+Shift+C)
- [ ] "Flamethrower started firing" 로그 보임
- [ ] "Projectile fired" 로그 보임
- [ ] "Dealt X damage" 로그 보임

---

## ⚠️ 문제 발생 시

| 문제 | 해결 |
|------|------|
| 화염이 적 안 맞춤 | FlameHitbox > Is Trigger 체크, Target Layers에 Enemy 체크 |
| 눈덩이 안 나감 | ProjectileLauncher > Projectile Prefab, Launch Point 확인 |
| 눈덩이 통과 | Snowball > Is Trigger 체크, Target Layers에 Player 체크 |
| 조작 안 됨 | SimplePlayerController 추가 확인 |
| Layer 에러 | Snowman Layer: Enemy, Player Layer: Player 확인 |

---

## 📝 핵심 포인트

### 반드시 Trigger여야 함!
- ✅ FlameHitbox > Box Collider > Is Trigger
- ✅ Snowball > Sphere Collider > Is Trigger

### 반드시 연결되어야 함!
- ✅ PlayerFlamethrower > Flame Origin
- ✅ PlayerFlamethrower > Hitbox Prefab
- ✅ ProjectileLauncher > Projectile Prefab
- ✅ ProjectileLauncher > Launch Point
- ✅ SimpleSnowmanAI > Player

### Layer 확인!
- ✅ Player GameObject > Layer: Player
- ✅ Snowman GameObject > Layer: Enemy
- ✅ FlameHitbox > Target Layers: Enemy만
- ✅ Snowball > Target Layers: Player만

---

**설정 완료 시간: 약 30-35분**
**즐거운 테스트 되세요! 🎮**

