# 🔥❄️ 화염방사기 & 눈덩이 공격 Unity 에디터 설정 가이드

## 📋 목표
- 플레이어의 화염방사기 공격 구현
- 눈사람의 눈덩이 투사체 공격 구현
- 실시간 테스트 및 디버깅

---

## ⚙️ STEP 1: 기본 레이어 설정 (5분)

### 1-1. 레이어 생성
1. 메뉴: **Edit > Project Settings**
2. 왼쪽에서 **Tags and Layers** 클릭
3. Layers 섹션에서:
   - `Layer 6` → `Player` 입력
   - `Layer 7` → `Enemy` 입력
4. 창 닫기

### 1-2. 충돌 매트릭스 설정
1. 메뉴: **Edit > Project Settings**
2. 왼쪽에서 **Physics** 클릭
3. 아래로 스크롤하여 **Layer Collision Matrix** 찾기
4. 설정:
   - ✅ Player - Enemy 간 충돌 **체크**
   - ❌ Player - Player 충돌 **해제**
   - ❌ Enemy - Enemy 충돌 **해제**
5. 창 닫기

---

## 📁 STEP 2: 프리팹 폴더 생성 (1분)

1. **Project 창**에서 **Assets** 폴더 선택
2. 우클릭 > **Create > Folder**
3. 이름: `Prefabs` 입력
4. Enter

---

## 🔥 STEP 3: 화염 Hitbox 프리팹 생성 (5분)

### 3-1. Hitbox 오브젝트 생성
1. **Hierarchy** 우클릭 > **3D Object > Cube**
2. 이름: `FlameHitbox`로 변경
3. **Inspector**에서 설정:

**Transform:**
- Position: `X=0, Y=0, Z=0`
- Rotation: `X=0, Y=0, Z=0`
- Scale: `X=0.5, Y=0.5, Z=1.0`

### 3-2. Box Collider 설정
1. FlameHitbox 선택된 상태에서
2. **Box Collider** 컴포넌트 찾기
3. **Is Trigger** ✅ **체크** (매우 중요!)

### 3-3. HitBox 컴포넌트 추가
1. **Add Component** 버튼 클릭
2. 검색: `HitBox` 입력
3. **HitBox** 선택하여 추가
4. 설정:
   - **Damage**: `10` (단일 타격 피해)
   - **Damage Type**: 드롭다운에서 `Fire` 선택
   - **Deals Dot Damage**: ❌ 해제 (선택사항 - 화상 지속 피해)
   - **Dot Duration**: `3` (DoT 활성화 시)
   - **Dot Damage Per Tick**: `5` (DoT 활성화 시)
   - **Dot Tick Interval**: `0.5` (DoT 활성화 시)
   - **Target Layer**: 
     1. 드롭다운 클릭
     2. **Nothing** 클릭 (전체 해제)
     3. **Enemy** ✅ 체크 (Enemy만 선택)
   - **Hit Once**: ❌ **해제** (중요! 지속 피해를 위해 해제)
   - **Fire Interval**: `0.1` (0.1초마다 피해)
   - **Can Hit Multiple**: ✅ **체크** (여러 적 동시 타격)
   - **Max Targets**: `10` (최대 10명까지 동시 타격)

### 3-4. 머티리얼 설정 (선택사항)
1. **Project 창**에서 우클릭 > **Create > Material**
2. 이름: `FlameMaterial`
3. Inspector에서:
   - **Albedo** 색상: 주황색 `#FF8C00` 또는 RGB(255, 140, 0)
   - **Rendering Mode**: `Transparent`
   - **Albedo** 색상의 **A(Alpha)**: 약 `100` (반투명)
4. **FlameHitbox** 선택
5. 머티리얼을 **Mesh Renderer > Materials**로 드래그

### 3-5. 프리팹으로 저장
1. **Hierarchy**에서 **FlameHitbox** 선택
2. **Project 창**의 **Prefabs 폴더**로 **드래그**
3. Hierarchy에서 **FlameHitbox 우클릭 > Delete** (씬에서 삭제)

✅ **완료!** FlameHitbox.prefab 생성됨

---

## ❄️ STEP 4: 눈덩이 Projectile 프리팹 생성 (7분)

### 4-1. Snowball 오브젝트 생성
1. **Hierarchy** 우클릭 > **3D Object > Sphere**
2. 이름: `Snowball`로 변경
3. **Inspector**에서 설정:

**Transform:**
- Position: `X=0, Y=0, Z=0`
- Rotation: `X=0, Y=0, Z=0`
- Scale: `X=0.3, Y=0.3, Z=0.3`

### 4-2. Rigidbody 추가
1. **Add Component** 클릭
2. 검색: `Rigidbody` 입력
3. **Rigidbody** 선택하여 추가
4. 설정:
   - **Mass**: `0.5`
   - **Drag**: `0`
   - **Angular Drag**: `0.05`
   - **Use Gravity**: ❌ **해제** (직선으로 날아가도록)
   - **Is Kinematic**: ❌ **해제**
   - **Collision Detection**: `Continuous`

### 4-3. Sphere Collider 설정
1. **Sphere Collider** 컴포넌트 찾기
2. **Is Trigger**: ✅ **체크** (매우 중요!)

### 4-4. Projectile 컴포넌트 추가
1. **Add Component** 클릭
2. 검색: `Projectile` 입력
3. **Projectile** 선택하여 추가
4. 설정:
   - **Damage**: `15`
   - **Damage Type**: 드롭다운에서 `Physical` 선택
   - **Speed**: `10`
   - **Lifetime**: `5`
   - **Target Layer**:
     1. 드롭다운 클릭
     2. **Nothing** 클릭
     3. **Player** ✅ 체크
   - **Hit Effect Prefab**: 비워두기 (나중에 추가 가능)

### 4-5. 머티리얼 설정 (선택사항)
1. **Project 창** 우클릭 > **Create > Material**
2. 이름: `SnowballMaterial`
3. Inspector에서:
   - **Albedo** 색상: 흰색 (기본값) 또는 밝은 파란색
4. **Snowball** 선택
5. 머티리얼을 **Mesh Renderer > Materials**로 드래그

### 4-6. Trail 효과 추가 (선택사항)
1. **Snowball** 선택
2. **Add Component** > **Effects > Trail Renderer**
3. 설정:
   - **Time**: `0.3`
   - **Width**: Curve 그래프 조정 (시작 0.2, 끝 0.05)
   - **Color**: 흰색 → 투명 (Gradient 편집)

### 4-7. 프리팹으로 저장
1. **Hierarchy**에서 **Snowball** 선택
2. **Project 창**의 **Prefabs 폴더**로 **드래그**
3. Hierarchy에서 **Snowball 우클릭 > Delete**

✅ **완료!** Snowball.prefab 생성됨

---

## 👤 STEP 5: 플레이어 생성 (10분)

### 5-1. 플레이어 오브젝트 생성
1. **Hierarchy** 우클릭 > **Create Empty**
2. 이름: `Player`
3. **Inspector**에서:
   - **Tag**: `Player` 선택
   - **Layer**: `Player` 선택
   - **Transform Position**: `X=0, Y=1, Z=0`

### 5-2. 플레이어 Visual 추가
1. **Player** 우클릭 > **3D Object > Capsule**
2. 이름: `PlayerModel`
3. Transform:
   - Local Position: `X=0, Y=0, Z=0`
   - Local Rotation: `X=0, Y=0, Z=0`
   - Local Scale: `X=1, Y=1, Z=1`

### 5-3. 플레이어 Collider 추가
1. **Player** (부모 오브젝트) 선택
2. **Add Component** > **Capsule Collider**
3. 설정:
   - **Radius**: `0.5`
   - **Height**: `2`
   - **Center**: `X=0, Y=0, Z=0`
   - **Is Trigger**: ❌ **해제**

### 5-4. PlayerHealth 추가
1. **Player** 선택
2. **Add Component** > 검색: `PlayerHealth`
3. 설정:
   - **Max Health**: `100`
   - **Regeneration Rate**: `5` (초당 회복량)
   - **Invincibility Duration**: `1.0`

### 5-5. PlayerFlamethrower 추가
1. **Player** 선택
2. **Add Component** > 검색: `PlayerFlamethrower`
3. 일단 기본값으로 두기 (나중에 설정)

### 5-6. Flame Origin 생성
1. **Player** 우클릭 > **Create Empty**
2. 이름: `FlameOrigin`
3. Transform:
   - Local Position: `X=0, Y=0.5, Z=0.7` (플레이어 앞쪽)
   - Local Rotation: `X=0, Y=0, Z=0`

### 5-7. PlayerFlamethrower 설정
1. **Player** 선택
2. **PlayerFlamethrower** 컴포넌트 찾기
3. 설정:
   - **Flame Origin**: 
     1. 오른쪽 동그라미 아이콘 클릭
     2. **FlameOrigin** 더블클릭 선택
   - **Hitbox Prefab**:
     1. **Project 창**의 **Prefabs/FlameHitbox** 찾기
     2. 드래그하여 이 필드에 드롭
   - **Flame Length**: `3.0`
   - **Damage Per Second**: `20`
   - **Fuel Capacity**: `100`
   - **Fuel Consumption Rate**: `10`
   - **Refuel Rate**: `5`
   - **Fire Key**: `Mouse0` (기본값)

### 5-8. 플레이어 컨트롤러 추가 (이동 및 조작용)
1. **Player** 선택
2. **Add Component** > 검색: `SimplePlayerController`
3. 설정은 자동으로 연결됨

### 5-9. 플레이어 색상 변경 (선택사항)
1. **PlayerModel** 선택
2. **Mesh Renderer > Materials > Element 0** 클릭
3. **Albedo** 색상: 파란색 등 원하는 색

✅ **플레이어 완료!**

---

## ⛄ STEP 6: 눈사람 Enemy 생성 (10분)

### 6-1. 눈사람 오브젝트 생성
1. **Hierarchy** 우클릭 > **Create Empty**
2. 이름: `Snowman`
3. **Inspector**에서:
   - **Tag**: `Enemy` (없으면 **Add Tag**에서 생성)
   - **Layer**: `Enemy` 선택
   - **Transform Position**: `X=0, Y=1, Z=10` (플레이어 앞 10m)

### 6-2. 눈사람 Visual 추가

**하단 구체:**
1. **Snowman** 우클릭 > **3D Object > Sphere**
2. 이름: `Body_Bottom`
3. Transform:
   - Local Position: `X=0, Y=0, Z=0`
   - Local Scale: `X=1.2, Y=1.2, Z=1.2`

**상단 구체:**
1. **Snowman** 우클릭 > **3D Object > Sphere**
2. 이름: `Body_Top`
3. Transform:
   - Local Position: `X=0, Y=1, Z=0`
   - Local Scale: `X=0.8, Y=0.8, Z=0.8`

### 6-3. 눈사람 Collider 추가
1. **Snowman** (부모 오브젝트) 선택
2. **Add Component** > **Capsule Collider**
3. 설정:
   - **Radius**: `0.6`
   - **Height**: `2`
   - **Center**: `X=0, Y=0.5, Z=0`
   - **Is Trigger**: ❌ **해제**

### 6-4. EnemyHealth 추가
1. **Snowman** 선택
2. **Add Component** > 검색: `EnemyHealth`
3. 설정:
   - **Max Health**: `50`
   - **Regeneration Rate**: `0` (적은 회복 안 함)
   - **Invincibility Duration**: `0.5`

### 6-5. ProjectileLauncher 추가
1. **Snowman** 선택
2. **Add Component** > 검색: `ProjectileLauncher`
3. 일단 기본값으로 두기

### 6-6. Launch Point 생성
1. **Snowman** 우클릭 > **Create Empty**
2. 이름: `LaunchPoint`
3. Transform:
   - Local Position: `X=0, Y=1.2, Z=0.5` (눈사람 상단 앞쪽)
   - Local Rotation: `X=0, Y=0, Z=0`

### 6-7. ProjectileLauncher 설정
1. **Snowman** 선택
2. **ProjectileLauncher** 컴포넌트 찾기
3. 설정:
   - **Projectile Prefab**:
     1. **Project 창**의 **Prefabs/Snowball** 찾기
     2. 드래그하여 이 필드에 드롭
   - **Fire Point**:
     1. 오른쪽 동그라미 아이콘 클릭
     2. **LaunchPoint** 더블클릭 선택
   - **Projectile Speed**: `10` (발사 속도)
   - **Attack Cooldown**: `1.5` (1.5초마다 발사)
   - **Auto Target**: ✅ **체크** (자동으로 타겟 추적)
   - **Use Input**: ❌ **해제** (적 AI는 입력 사용 안 함)
   - **Shoot Key**: `E` (Use Input 활성화 시에만 사용)

### 6-8. 눈사람 테스트용 간단 AI (선택사항)

**참고**: 현재 프로젝트에 AI 스크립트가 없으므로, 두 가지 방법으로 테스트 가능합니다:

**방법 1: 수동 테스트 (간단)**
1. **Snowman** 선택
2. **ProjectileLauncher** 컴포넌트에서:
   - **Use Input**: ✅ **체크**
   - **Shoot Key**: `E`
3. Play 모드에서 **E키**를 눌러 수동으로 발사 테스트

**방법 2: 간단한 AI 스크립트 생성 (고급)**
1. **Assets/Scripts/Combat/Examples** 폴더에 새 스크립트 생성:
   - 이름: `SimpleEnemyAI.cs`
2. 스크립트 내용:
```csharp
using UnityEngine;

namespace Game.Combat.Examples
{
    public class SimpleEnemyAI : MonoBehaviour
    {
        [SerializeField] private float fireInterval = 1.5f;
        private ProjectileLauncher launcher;
        private float nextFireTime;

        void Start()
        {
            launcher = GetComponent<ProjectileLauncher>();
        }

        void Update()
        {
            if (Time.time >= nextFireTime)
            {
                launcher?.ShootAtPlayer();
                nextFireTime = Time.time + fireInterval;
            }
        }
    }
}
```
3. **Snowman**에 **SimpleEnemyAI** 컴포넌트 추가
4. **Fire Interval**: `1.5` 설정

### 6-9. 눈사람 색상 변경 (선택사항)
1. **Body_Bottom**과 **Body_Top** 각각 선택
2. **Mesh Renderer > Materials > Element 0** 클릭
3. **Albedo** 색상: 흰색 (기본값)

✅ **눈사람 완료!**

---

## 🎮 STEP 7: 바닥 및 카메라 설정 (3분)

### 7-1. 바닥 생성
1. **Hierarchy** 우클릭 > **3D Object > Plane**
2. 이름: `Ground`
3. Transform:
   - Position: `X=0, Y=0, Z=0`
   - Rotation: `X=0, Y=0, Z=0`
   - Scale: `X=5, Y=1, Z=5` (큰 바닥)

### 7-2. 카메라 설정
1. **Main Camera** 선택
2. Transform:
   - Position: `X=0, Y=15, Z=-10`
   - Rotation: `X=50, Y=0, Z=0`

이렇게 하면 위에서 내려다보는 탑다운 뷰가 됩니다.

---

## ▶️ STEP 8: 테스트 실행! (2분)

### 8-1. 저장
1. **File > Save** (또는 `Ctrl+S`)
2. 씬 이름: `FlamethrowerTest` 또는 원하는 이름

### 8-2. 플레이 모드 시작
1. **Play 버튼** (상단 중앙) 클릭 ▶️

### 8-3. 조작법
- **WASD**: 플레이어 이동
- **마우스 이동**: 플레이어 방향 전환 (마우스 커서 방향으로 회전)
- **좌클릭 (누르고 있기)**: 화염방사 발사
- **눈사람**: 자동으로 플레이어를 향해 눈덩이 발사

### 8-4. 확인 사항
✅ 플레이어가 WASD로 이동하는가?
✅ 플레이어가 마우스 방향을 향하는가?
✅ 좌클릭 시 화염이 나가는가? (주황색 박스가 생성됨)
✅ 화염이 눈사람에게 닿으면 체력이 깎이는가?
✅ 눈사람이 자동으로 눈덩이를 발사하는가? (흰색 구체)
✅ 눈덩이가 플레이어에게 닿으면 체력이 깎이는가?

### 8-5. Console 확인
1. **Window > General > Console** 열기 (또는 `Ctrl+Shift+C`)
2. 로그 확인:
   - "Flamethrower started firing"
   - "Projectile fired"
   - "Projectile hit ..."
   - "Dealt X damage to ..."

---

## 🐛 STEP 9: 문제 해결 (Troubleshooting)

### 문제 1: 화염이 눈사람을 감지하지 못함
**증상**: 화염이 나가지만 피해를 주지 않음

**해결책**:
1. **Snowman** 선택 > Inspector 상단에서 **Layer가 Enemy**인지 확인
2. **FlameHitbox** Prefab 선택:
   - **Box Collider > Is Trigger** ✅ 확인
   - **HitBox > Target Layers에 Enemy** 체크되었는지 확인
3. **Edit > Project Settings > Physics**:
   - **Layer Collision Matrix**에서 Player-Enemy 충돌 체크 확인

### 문제 2: 눈덩이가 발사되지 않음
**증상**: 눈사람이 아무것도 안 함

**해결책**:
1. **Snowman** 선택
2. **ProjectileLauncher** 컴포넌트 확인:
   - **Projectile Prefab** 할당되었는지
   - **Fire Point** 할당되었는지
3. **수동 테스트**: 
   - **Use Input** 체크
   - **Shoot Key**: `E`
   - Play 모드에서 E 키 눌러보기
4. **AI 사용 시**:
   - **SimpleEnemyAI** 컴포넌트가 추가되었는지 확인
   - **Player 태그**가 제대로 설정되었는지 확인

### 문제 3: 눈덩이가 플레이어를 통과함
**증상**: 눈덩이가 플레이어에게 닿아도 피해 없음

**해결책**:
1. **Player** 선택 > Inspector 상단에서 **Layer가 Player**인지 확인
2. **Snowball** Prefab 선택:
   - **Sphere Collider > Is Trigger** ✅ 확인
   - **Projectile > Target Layers에 Player** 체크되었는지 확인
   - **Rigidbody**가 추가되었는지 확인

### 문제 4: 플레이어가 조작되지 않음
**증상**: WASD나 마우스 입력이 작동 안 함

**해결책**:
1. **Player** 선택
2. **SimplePlayerController** 컴포넌트가 추가되었는지 확인
3. Console에 에러가 있는지 확인

### 문제 5: 화염/눈덩이가 보이지 않음
**증상**: 공격은 되는데 시각적으로 안 보임

**해결책**:
- **Scene 뷰**와 **Game 뷰**를 모두 확인
- **카메라 위치**가 적절한지 확인
- **Mesh Renderer**가 활성화되었는지 확인

---

## 📊 STEP 10: 디버깅 및 확인 (실시간)

### 10-1. 체력 확인
**Play 모드에서:**
1. **Player** 선택
2. **PlayerHealth** 컴포넌트 보기
3. **Current Health** 값이 실시간으로 변하는지 확인

1. **Snowman** 선택
2. **EnemyHealth** 컴포넌트 보기
3. **Current Health** 값이 실시간으로 변하는지 확인

### 10-2. 연료 확인
**Play 모드에서:**
1. **Player** 선택
2. **PlayerFlamethrower** 컴포넌트 보기
3. **Current Fuel** 값 확인 (좌클릭 중 감소, 떼면 회복)

### 10-3. Gizmos 활용
**Scene 뷰에서:**
1. Scene 뷰 상단의 **Gizmos** 버튼이 켜져있는지 확인
2. 플레이 중 다음이 보임:
   - 초록색 와이어 박스: HitBox 감지 범위
   - 노란색 원: 눈사람 감지 범위
   - 파란색 선: 눈사람이 바라보는 방향

### 10-4. Console 로그 필터링
1. **Console 창** 열기
2. 우측 상단:
   - 💬 **Log**: 일반 정보
   - ⚠️ **Warning**: 경고
   - ❌ **Error**: 에러
3. 원하는 것만 켜서 확인

---

## 🎨 STEP 11: 비주얼 개선 (선택사항)

### 11-1. 화염 파티클 효과
1. **FlameOrigin** 우클릭 > **Effects > Particle System**
2. 이름: `FlameEffect`
3. Particle System 설정:
   - **Start Color**: 주황색/노란색
   - **Start Size**: `0.3`
   - **Start Speed**: `3`
   - **Start Lifetime**: `0.5`
   - **Emission > Rate over Time**: `50`
   - **Shape > Shape**: `Cone`
   - **Shape > Angle**: `20`
   - **Shape > Radius**: `0.1`
4. **Player** 선택
5. **PlayerFlamethrower > Flame Effect**에 FlameEffect 드래그

### 11-2. 히트 효과
1. **Project 창** 우클릭 > **Create > Material**
2. 이름: `HitFlash`
3. Albedo: 빨간색
4. 피격 시 잠깐 보이도록 설정 (스크립트로 제어)

---

## 📈 STEP 12: 값 조정 (밸런싱)

### 플레이어 화염방사기 조정
**강하게 만들려면:**
- **Damage Per Second**: 20 → 50
- **Flame Length**: 3 → 5
- **Fire Interval**: 0.1 → 0.05 (더 빠른 타격)

**약하게 만들려면:**
- **Fuel Consumption Rate**: 10 → 20 (빨리 소모)
- **Refuel Rate**: 5 → 2 (느리게 회복)

### 눈사람 공격 조정
**강하게 만들려면:**
- **Snowball > Damage**: 15 → 30
- **ProjectileLauncher > Attack Cooldown**: 1.5 → 0.5 (더 빠른 발사)
- **Snowball > Speed**: 10 → 20 (더 빠른 속도)

**약하게 만들려면:**
- **ProjectileLauncher > Attack Cooldown**: 1.5 → 3.0 (느린 발사)
- **Snowball > Speed**: 10 → 5 (느린 속도)
- **SimpleEnemyAI > Fire Interval**: 1.5 → 3.0 (AI 사용 시)

---

## ✅ 완료 체크리스트

### 설정 완료
- [ ] Layer 6: Player, Layer 7: Enemy 생성
- [ ] Layer Collision Matrix 설정
- [ ] Prefabs 폴더 생성
- [ ] FlameHitbox.prefab 생성 및 설정
- [ ] Snowball.prefab 생성 및 설정

### 플레이어 완료
- [ ] Player GameObject 생성
- [ ] Tag: Player, Layer: Player 설정
- [ ] PlayerHealth 컴포넌트 추가
- [ ] PlayerFlamethrower 컴포넌트 추가
- [ ] FlameOrigin 생성 및 연결
- [ ] FlameHitbox Prefab 연결
- [ ] SimplePlayerController 추가

### 눈사람 완료
- [ ] Snowman GameObject 생성
- [ ] Tag: Enemy, Layer: Enemy 설정
- [ ] EnemyHealth 컴포넌트 추가
- [ ] ProjectileLauncher 컴포넌트 추가
- [ ] LaunchPoint 생성 및 연결
- [ ] Snowball Prefab 연결
- [ ] SimpleEnemyAI 추가 (선택사항, AI 테스트용)

### 테스트 완료
- [ ] Play 모드 실행
- [ ] 플레이어 이동 확인
- [ ] 화염방사기 발사 확인
- [ ] 눈덩이 발사 확인
- [ ] 피해 적용 확인
- [ ] Console 로그 확인

---

## 🚀 다음 단계

완료했다면:
1. **여러 눈사람 생성**: Snowman 복제 (Ctrl+D) 후 다른 위치에 배치
2. **체력바 UI 추가**: HealthBarUI 스크립트 활용
3. **사운드 추가**: Audio Source 컴포넌트로 효과음
4. **이펙트 강화**: Unity Particle System으로 화려한 효과
5. **DOT 효과 테스트**: 화염에 지속 피해 추가
6. **다양한 적 타입**: 다른 투사체, 다른 능력

---

## 💡 팁

### 빠른 테스트
- **Play 모드에서 값 조정 가능** (단, 저장 안 됨)
- 테스트 중 좋은 값 발견하면 **Play 모드 중지 전에 메모**
- Scene 뷰와 Game 뷰 동시에 보기: 탭을 나란히 배치

### 성능 확인
- **Stats 버튼** (Game 뷰 우측 상단): FPS, Batches 등 확인
- 너무 많은 Hitbox/Projectile이 생성되면 프레임 드롭

### 저장 습관
- **Ctrl+S**: 씬 저장
- **자주 저장**: Unity는 가끔 크래시 할 수 있음
- **버전 관리**: Git 등 사용 권장

---

## 📞 도움이 필요하면

1. **Console 에러 메시지** 확인
2. **Inspector에서 빨간 글씨** 찾기 (missing reference)
3. **Gizmos로 시각화** (Scene 뷰에서 확인)
4. **Debug.Log 추가**: 스크립트에 로그 출력

**끝! 즐거운 테스트 되세요! 🎮🔥❄️**

