# 📸 Inspector 설정 참고 가이드

## 🎯 각 컴포넌트별 Inspector 설정값

---

## 1️⃣ FlameHitbox Prefab

```
FlameHitbox (GameObject)
├─ Transform
│  ├─ Position: X=0, Y=0, Z=0
│  ├─ Rotation: X=0, Y=0, Z=0
│  └─ Scale: X=0.5, Y=0.5, Z=1.0
│
├─ Box Collider
│  ├─ Is Trigger: ✅ (체크)
│  ├─ Center: X=0, Y=0, Z=0
│  └─ Size: X=1, Y=1, Z=1
│
├─ Mesh Renderer
│  └─ Materials: (선택사항 - 주황색 반투명)
│
└─ HitBox
   ├─ Base Damage: 10
   ├─ Damage Type: Fire (드롭다운)
   ├─ Deals Dot Damage: ❌ (선택사항)
   ├─ Dot Duration: 3 (DoT 사용 시)
   ├─ Dot Damage Per Tick: 5 (DoT 사용 시)
   ├─ Dot Tick Interval: 0.5 (DoT 사용 시)
   ├─ Target Layer: Enemy (레이어 마스크)
   ├─ Hit Once: ❌ (해제 - 지속 피해)
   ├─ Fire Interval: 0.1
   ├─ Can Hit Multiple: ✅ (체크)
   ├─ Max Targets: 10
   ├─ Hit Effect: None (선택사항)
   └─ Debug: ❌ (필요시 체크)
```

**중요:** Is Trigger와 Target Layers가 핵심!

---

## 2️⃣ Snowball Prefab

```
Snowball (GameObject)
├─ Transform
│  ├─ Position: X=0, Y=0, Z=0
│  ├─ Rotation: X=0, Y=0, Z=0
│  └─ Scale: X=0.3, Y=0.3, Z=0.3
│
├─ Sphere Collider
│  ├─ Is Trigger: ✅ (체크)
│  ├─ Center: X=0, Y=0, Z=0
│  └─ Radius: 0.5
│
├─ Rigidbody
│  ├─ Mass: 0.5
│  ├─ Drag: 0
│  ├─ Angular Drag: 0.05
│  ├─ Use Gravity: ❌ (해제)
│  ├─ Is Kinematic: ❌ (해제)
│  └─ Collision Detection: Continuous
│
├─ Mesh Renderer
│  └─ Materials: (선택사항 - 흰색)
│
├─ Trail Renderer (선택사항)
│  ├─ Time: 0.3
│  ├─ Width: 0.2 → 0.05
│  └─ Color: 흰색 → 투명
│
└─ Projectile
   ├─ Base Damage: 15
   ├─ Damage Type: Ice (드롭다운)
   ├─ Speed: 10
   ├─ Lifetime: 5
   ├─ Source: Enemy (드롭다운)
   ├─ Target Layers: Player (레이어 마스크)
   ├─ Destroy On Hit: ✅ (체크)
   ├─ Pierce Count: 0
   ├─ Hit Effect: None (선택사항)
   ├─ Destroy Effect: None (선택사항)
   └─ Debug: ❌ (필요시 체크)
```

**중요:** Rigidbody와 Is Trigger가 핵심!

---

## 3️⃣ Player GameObject

```
Player (GameObject)
├─ Tag: Player (드롭다운)
├─ Layer: Player (드롭다운)
├─ Transform
│  ├─ Position: X=0, Y=1, Z=0
│  ├─ Rotation: X=0, Y=0, Z=0
│  └─ Scale: X=1, Y=1, Z=1
│
├─ Capsule Collider
│  ├─ Is Trigger: ❌ (해제)
│  ├─ Radius: 0.5
│  ├─ Height: 2
│  └─ Center: X=0, Y=0, Z=0
│
├─ PlayerHealth
│  ├─ Max Health: 100
│  ├─ Regeneration Rate: 5
│  ├─ Invincibility Duration: 1.0
│  └─ Events: (펼침 - 선택사항)
│
├─ PlayerFlamethrower
│  ├─ Flame Origin: FlameOrigin (GameObject 연결)
│  ├─ Hitbox Prefab: FlameHitbox (Prefab 연결)
│  ├─ Flame Length: 3.0
│  ├─ Damage Per Second: 20
│  ├─ Fuel Capacity: 100
│  ├─ Fuel Consumption Rate: 10
│  ├─ Refuel Rate: 5
│  ├─ Fire Key: Mouse0
│  ├─ Flame Effect: None (선택사항 - ParticleSystem)
│  └─ Events: (펼침 - 선택사항)
│
└─ SimplePlayerController
   ├─ Move Speed: 5
   ├─ Rotation Speed: 720
   ├─ Flamethrower: (자동 연결)
   ├─ Melee Attack: (자동 연결)
   ├─ Flamethrower Key: Mouse0
   └─ Melee Key: Mouse1

└─ PlayerModel (자식 GameObject - Visual)
   └─ Capsule Mesh
```

**자식 GameObject:**
```
└─ FlameOrigin (Empty GameObject)
   └─ Transform Local Position: X=0, Y=0.5, Z=0.7
```

**중요:** Flame Origin과 Hitbox Prefab 연결이 핵심!

---

## 4️⃣ Snowman GameObject

```
Snowman (GameObject)
├─ Tag: Enemy (드롭다운, 없으면 생성)
├─ Layer: Enemy (드롭다운)
├─ Transform
│  ├─ Position: X=0, Y=1, Z=10
│  ├─ Rotation: X=0, Y=0, Z=0
│  └─ Scale: X=1, Y=1, Z=1
│
├─ Capsule Collider
│  ├─ Is Trigger: ❌ (해제)
│  ├─ Radius: 0.6
│  ├─ Height: 2
│  └─ Center: X=0, Y=0.5, Z=0
│
├─ EnemyHealth
│  ├─ Max Health: 50
│  ├─ Regeneration Rate: 0
│  ├─ Invincibility Duration: 0.5
│  └─ Events: (펼침 - 선택사항)
│
├─ ProjectileLauncher
│  ├─ Projectile Prefab: Snowball (Prefab 연결)
│  ├─ Launch Point: LaunchPoint (GameObject 연결)
│  ├─ Launch Force: 15
│  ├─ Fire Rate: 1.5
│  ├─ Auto Fire: ✅ (체크)
│  ├─ Launch Angle: 0
│  ├─ Use Gravity: ❌ (해제)
│  ├─ Launch Sound: None (선택사항)
│  └─ Events: (펼침 - 선택사항)
│
└─ SimpleSnowmanAI
   ├─ Player: Player (GameObject 연결)
   ├─ Projectile Launcher: (자동 연결)
   ├─ Detection Range: 15
   ├─ Rotation Speed: 180
   ├─ Auto Find Player: ✅ (체크)
   ├─ Fire Interval: 1.5
   ├─ Randomize Interval: ✅ (체크)
   └─ Interval Variation: 0.5

└─ Body_Bottom (자식 GameObject - Visual)
   ├─ Sphere Mesh
   └─ Transform Local Position: X=0, Y=0, Z=0
       Local Scale: X=1.2, Y=1.2, Z=1.2

└─ Body_Top (자식 GameObject - Visual)
   ├─ Sphere Mesh
   └─ Transform Local Position: X=0, Y=1, Z=0
       Local Scale: X=0.8, Y=0.8, Z=0.8

└─ LaunchPoint (자식 Empty GameObject)
   └─ Transform Local Position: X=0, Y=1.2, Z=0.5
```

**중요:** Player 연결과 Projectile Prefab, Launch Point 연결이 핵심!

---

## 5️⃣ Main Camera

```
Main Camera (GameObject)
├─ Tag: MainCamera
├─ Transform
│  ├─ Position: X=0, Y=15, Z=-10
│  ├─ Rotation: X=50, Y=0, Z=0
│  └─ Scale: X=1, Y=1, Z=1
│
└─ Camera
   ├─ Clear Flags: Skybox
   ├─ Culling Mask: Everything
   ├─ Projection: Perspective
   └─ Field of View: 60
```

---

## 6️⃣ Ground

```
Ground (GameObject)
├─ Transform
│  ├─ Position: X=0, Y=0, Z=0
│  ├─ Rotation: X=0, Y=0, Z=0
│  └─ Scale: X=5, Y=1, Z=5
│
└─ Mesh Collider (자동 생성)
   └─ Convex: ❌
```

---

## 🔑 핵심 연결 관계

### Player
```
PlayerFlamethrower
  ↓ Flame Origin (참조)
  └─→ FlameOrigin (GameObject)
  
  ↓ Hitbox Prefab (참조)
  └─→ FlameHitbox (Prefab)
```

### Snowman
```
ProjectileLauncher
  ↓ Launch Point (참조)
  └─→ LaunchPoint (GameObject)
  
  ↓ Projectile Prefab (참조)
  └─→ Snowball (Prefab)

SimpleSnowmanAI
  ↓ Player (참조)
  └─→ Player (GameObject)
```

---

## ⚙️ 레이어 마스크 설정 방법

### FlameHitbox > Target Layers
1. `Target Layers` 필드 클릭
2. 드롭다운 메뉴 열림
3. **Nothing** 클릭 (모두 해제)
4. **Enemy** ✅ 체크

결과: `Enemy` 표시됨

### Snowball > Target Layers
1. `Target Layers` 필드 클릭
2. 드롭다운 메뉴 열림
3. **Nothing** 클릭 (모두 해제)
4. **Player** ✅ 체크

결과: `Player` 표시됨

---

## 🎨 색상 참고 (선택사항)

### FlameHitbox 머티리얼
- **Albedo**: RGB(255, 140, 0) - 주황색
- **Rendering Mode**: Transparent
- **Alpha**: 100

### Snowball 머티리얼
- **Albedo**: RGB(255, 255, 255) - 흰색
- **Rendering Mode**: Opaque

### Player Visual
- **Albedo**: RGB(51, 128, 255) - 파란색

### Snowman Visual
- **Albedo**: RGB(255, 255, 255) - 흰색

---

## 📊 실시간 값 확인 (Play 모드)

### Player 확인
```
PlayerHealth
  ├─ Current Health: 100 → 실시간 감소/증가
  ├─ Is Invincible: false → 피격 시 잠깐 true
  └─ Health Percentage: 1.0 → 0.0

PlayerFlamethrower
  ├─ Current Fuel: 100 → 발사 시 감소
  ├─ Is Firing: false → 좌클릭 시 true
  └─ Fuel Percentage: 1.0 → 0.0
```

### Snowman 확인
```
EnemyHealth
  ├─ Current Health: 50 → 피격 시 감소
  ├─ Is Invincible: false → 피격 시 잠깐 true
  └─ Health Percentage: 1.0 → 0.0

ProjectileLauncher
  ├─ Can Fire: true → Fire Rate에 따라 변경
  └─ (내부 타이머는 보이지 않음)
```

---

## 🐛 Inspector 에러 표시

### Missing Reference (빨간 글씨)
```
PlayerFlamethrower
  ├─ Flame Origin: None (Missing) ← 빨간색
  └─ Hitbox Prefab: None (Missing) ← 빨간색
```
**해결:** 해당 필드에 올바른 GameObject/Prefab 드래그

### Layer 경고
```
HitBox
  └─ Target Layers: Nothing ← 노란색 경고
```
**해결:** 최소 하나의 레이어 선택

### Null Reference
Console에 "NullReferenceException" 에러
**해결:** Inspector에서 모든 필수 필드 연결 확인

---

## ✅ 올바르게 설정된 상태

### FlameHitbox
- Box Collider의 Is Trigger: ✅ 초록색 체크
- HitBox의 Target Layers: "Enemy" 표시

### Snowball
- Sphere Collider의 Is Trigger: ✅ 초록색 체크
- Rigidbody 존재
- Projectile의 Target Layers: "Player" 표시

### Player
- PlayerFlamethrower의 Flame Origin: "FlameOrigin" 표시
- PlayerFlamethrower의 Hitbox Prefab: "FlameHitbox" 표시

### Snowman
- ProjectileLauncher의 Projectile Prefab: "Snowball" 표시
- ProjectileLauncher의 Launch Point: "LaunchPoint" 표시
- SimpleSnowmanAI의 Player: "Player" 표시

---

**이 값들을 참고하여 Inspector를 정확히 설정하세요!**
**모든 체크박스와 연결이 올바르면 정상 작동합니다! ✅**

