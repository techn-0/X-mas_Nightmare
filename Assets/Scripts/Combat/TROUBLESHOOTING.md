# 🔧 문제 해결 가이드 (Troubleshooting)

## 🎯 증상별 해결 방법

---

## ❌ 문제 1: "화염이 눈사람을 맞추지 못합니다"

### 증상
- 좌클릭 시 주황색 박스(화염)는 나타남
- 눈사람에게 닿아도 피해가 없음
- Console에 피해 로그가 안 나옴

### 원인 및 해결

#### 1-1. Layer 설정 확인
**확인:**
1. **Snowman** GameObject 선택
2. Inspector 최상단 확인
3. **Layer**: `Enemy`로 설정되었는지?

**해결:**
- Snowman 선택 > Inspector 상단 Layer 드롭다운 > `Enemy` 선택

#### 1-2. FlameHitbox Target Layers 확인
**확인:**
1. **Project 창** > **Prefabs/FlameHitbox** 선택
2. Inspector에서 **HitBox** 컴포넌트 찾기
3. **Target Layers**: `Enemy`가 체크되었는지?

**해결:**
1. FlameHitbox Prefab 선택
2. HitBox > Target Layers 클릭
3. `Nothing` 클릭 (모두 해제)
4. `Enemy` ✅ 체크
5. Ctrl+S (저장)

#### 1-3. Is Trigger 확인
**확인:**
1. FlameHitbox Prefab 선택
2. **Box Collider** 컴포넌트 찾기
3. **Is Trigger**: 체크되었는지?

**해결:**
- FlameHitbox > Box Collider > Is Trigger ✅ 체크

#### 1-4. Layer Collision Matrix 확인
**확인:**
1. **Edit > Project Settings**
2. **Physics** 선택
3. **Layer Collision Matrix** 섹션
4. **Player**와 **Enemy** 교차점이 체크되었는지?

**해결:**
- Player 행과 Enemy 열이 만나는 곳 ✅ 체크
- 반대(Enemy 행과 Player 열)도 자동으로 체크됨

#### 1-5. Collider 크기 확인
**확인:**
1. Play 모드 진입
2. **Scene 뷰** 보기
3. Snowman에 초록색 캡슐 윤곽선이 보이는지?
4. FlameHitbox가 그 윤곽선과 겹치는지?

**해결:**
- Snowman > Capsule Collider > Center/Radius/Height 조정
- 또는 FlameHitbox > Transform > Scale 조정

---

## ❌ 문제 2: "눈덩이가 발사되지 않습니다"

### 증상
- 눈사람이 서있기만 함
- 눈덩이가 안 나옴
- Console에 "Projectile fired" 로그가 안 나옴

### 원인 및 해결

#### 2-1. Projectile Prefab 연결 확인
**확인:**
1. **Snowman** GameObject 선택
2. **ProjectileLauncher** 컴포넌트 찾기
3. **Projectile Prefab**: `Snowball`이 연결되었는지?

**해결:**
1. Snowman 선택
2. ProjectileLauncher > Projectile Prefab 필드
3. **Project 창**에서 **Prefabs/Snowball** 찾기
4. Snowball을 Projectile Prefab 필드로 **드래그**

#### 2-2. Launch Point 연결 확인
**확인:**
1. Snowman 선택
2. ProjectileLauncher > **Launch Point**: 연결되었는지?

**해결:**
1. Hierarchy에서 **Snowman > LaunchPoint** 존재 확인
2. 없으면: Snowman 우클릭 > Create Empty > 이름: LaunchPoint
3. ProjectileLauncher > Launch Point 필드 클릭
4. LaunchPoint GameObject 선택

#### 2-3. Auto Fire 확인
**확인:**
1. Snowman 선택
2. ProjectileLauncher > **Auto Fire**: 체크되었는지?

**해결:**
- ProjectileLauncher > Auto Fire ✅ 체크

#### 2-4. Snowball Prefab 설정 확인
**확인:**
1. **Project 창** > Prefabs/Snowball 선택
2. **Rigidbody** 컴포넌트가 있는지?
3. **Projectile** 컴포넌트가 있는지?

**해결:**
- Snowball Prefab 선택
- Add Component > Rigidbody (없으면)
- Add Component > Projectile (없으면)

#### 2-5. SimpleSnowmanAI 확인
**확인:**
1. Snowman 선택
2. **SimpleSnowmanAI** 컴포넌트가 있는지?
3. **Player**: Player GameObject가 연결되었는지?
4. **Detection Range**: 너무 작지 않은지? (기본 15)

**해결:**
1. SimpleSnowmanAI 없으면 Add Component로 추가
2. Player 필드에 Player GameObject 드래그
3. Detection Range를 15 이상으로 설정
4. Play 모드에서 플레이어를 눈사람 가까이 이동

#### 2-6. Console 에러 확인
**확인:**
1. **Window > General > Console** (Ctrl+Shift+C)
2. 빨간색 에러가 있는지?

**해결:**
- "NullReferenceException": 위의 연결 확인
- "Missing Prefab": Snowball Prefab 재생성

---

## ❌ 문제 3: "눈덩이가 플레이어를 통과합니다"

### 증상
- 눈덩이는 발사됨
- 플레이어에게 닿아도 피해가 없음
- 눈덩이가 사라지지 않고 계속 날아감

### 원인 및 해결

#### 3-1. Player Layer 확인
**확인:**
1. **Player** GameObject 선택
2. Inspector 최상단 **Layer**: `Player`인지?

**해결:**
- Player 선택 > Layer 드롭다운 > `Player` 선택

#### 3-2. Snowball Target Layers 확인
**확인:**
1. **Project 창** > Prefabs/Snowball 선택
2. **Projectile** 컴포넌트 찾기
3. **Target Layers**: `Player`가 체크되었는지?

**해결:**
1. Snowball Prefab 선택
2. Projectile > Target Layers 클릭
3. `Nothing` 클릭
4. `Player` ✅ 체크
5. Ctrl+S

#### 3-3. Is Trigger 확인
**확인:**
1. Snowball Prefab 선택
2. **Sphere Collider** > **Is Trigger**: 체크되었는지?

**해결:**
- Snowball > Sphere Collider > Is Trigger ✅ 체크

#### 3-4. Rigidbody 설정 확인
**확인:**
1. Snowball Prefab 선택
2. **Rigidbody** 컴포넌트 확인
3. **Is Kinematic**: 해제되었는지?
4. **Use Gravity**: 해제되었는지? (직선 발사용)

**해결:**
- Rigidbody > Is Kinematic ❌ 해제
- Rigidbody > Use Gravity ❌ 해제

#### 3-5. Player Collider 확인
**확인:**
1. Player GameObject 선택
2. **Capsule Collider** 컴포넌트 존재하는지?
3. **Is Trigger**: 해제되었는지? (Player는 Trigger 아님)

**해결:**
- Player > Capsule Collider 없으면 Add Component
- Is Trigger ❌ 해제 (일반 충돌체)

---

## ❌ 문제 4: "플레이어가 조작되지 않습니다"

### 증상
- WASD 눌러도 안 움직임
- 마우스 움직여도 안 돌아감
- 좌클릭해도 화염 안 나옴

### 원인 및 해결

#### 4-1. SimplePlayerController 확인
**확인:**
1. **Player** GameObject 선택
2. **SimplePlayerController** 컴포넌트가 있는지?

**해결:**
- Player 선택 > Add Component > `SimplePlayerController` 추가

#### 4-2. Main Camera 확인
**확인:**
1. **Hierarchy**에서 **Main Camera** 존재하는지?
2. Main Camera 선택 > **Tag**: `MainCamera`인지?

**해결:**
- Main Camera가 없으면: Create > Camera
- Tag를 MainCamera로 설정

#### 4-3. 입력 설정 확인
**확인:**
1. **Edit > Project Settings**
2. **Input Manager** 선택
3. **Horizontal**, **Vertical**, **Mouse X**, **Mouse Y** 존재하는지?

**해결:**
- 기본 Unity 프로젝트면 자동으로 있음
- 없으면 프로젝트가 손상되었을 수 있음

#### 4-4. Script 에러 확인
**확인:**
1. **Console 창** 확인 (Ctrl+Shift+C)
2. 빨간색 컴파일 에러가 있는지?

**해결:**
- 에러 메시지 읽고 해당 스크립트 수정
- 또는 스크립트 재생성

---

## ❌ 문제 5: "화염/눈덩이가 보이지 않습니다"

### 증상
- 피해는 정상 작동
- Console 로그도 정상
- 하지만 시각적으로 아무것도 안 보임

### 원인 및 해결

#### 5-1. Game 뷰 확인
**확인:**
1. **Game 뷰** 탭 클릭 (Scene 뷰가 아닌)
2. Play 모드에서 확인

**해결:**
- Scene 뷰가 아닌 Game 뷰에서 플레이

#### 5-2. 카메라 위치 확인
**확인:**
1. **Main Camera** 선택
2. Transform Position 확인
3. 플레이어와 눈사람이 카메라 시야에 있는지?

**해결:**
- Main Camera > Position: (0, 15, -10)
- Rotation: (50, 0, 0)
- 또는 Scene 뷰에서 원하는 각도로 카메라 이동 후
  GameObject > Align View to Selected (Ctrl+Shift+F)

#### 5-3. Mesh Renderer 확인
**확인:**
1. FlameHitbox/Snowball Prefab 선택
2. **Mesh Renderer** 컴포넌트가 활성화되었는지? (체크박스)

**해결:**
- Mesh Renderer 컴포넌트 좌측 체크박스 ✅ 체크

#### 5-4. 머티리얼 확인
**확인:**
1. Prefab 선택
2. Mesh Renderer > Materials > Element 0 확인
3. `None (Material)` 또는 핑크색이면 문제

**해결:**
- Project 창 > Create > Material
- Material을 Mesh Renderer > Materials로 드래그

---

## ❌ 문제 6: "Console에 수많은 로그가 찍힙니다"

### 증상
- Console 창이 로그로 도배됨
- 게임이 느려짐

### 원인 및 해결

#### 6-1. Debug 모드 확인
**확인:**
1. FlameHitbox Prefab 선택
2. **HitBox** > **Debug**: 체크되었는지?

**해결:**
- HitBox > Debug ❌ 해제
- Projectile > Debug ❌ 해제

#### 6-2. Console 필터 사용
**확인:**
1. Console 창 우측 상단
2. 💬 Log / ⚠️ Warning / ❌ Error 버튼

**해결:**
- 일반 로그 끄기: 💬 버튼 클릭하여 해제
- Warning과 Error만 보기

---

## ❌ 문제 7: "Play 모드에서 설정이 저장 안 됩니다"

### 증상
- Play 모드에서 값 조정
- Play 중지하면 원래대로 돌아감

### 원인 및 해결

#### 이것은 Unity의 정상 동작입니다!

**해결책:**
1. Play 모드에서 좋은 값 발견하면
2. **메모장에 기록** (예: Damage: 20 → 30)
3. Play 모드 **중지**
4. Inspector에서 **수동으로 값 재설정**
5. Ctrl+S로 씬 저장

**팁:**
- Play 모드에서는 테스트만
- 실제 값 변경은 Edit 모드에서

---

## ❌ 문제 8: "Cannot add script component because..."

### 증상
- Add Component 시 에러
- "The script don't exist on the assembly"

### 원인 및 해결

#### 8-1. Script 이름 확인
**확인:**
1. Project 창에서 스크립트 선택
2. 파일 이름과 내부 class 이름이 같은지?

**해결:**
```csharp
// 파일명: SimplePlayerController.cs
// 내부 class 이름도 동일해야 함
public class SimplePlayerController : MonoBehaviour
```

#### 8-2. 컴파일 에러 확인
**확인:**
1. Console 창에서 빨간색 에러 확인

**해결:**
- 모든 컴파일 에러 수정 후 재시도

#### 8-3. Unity 재시작
**해결:**
1. Unity 완전 종료
2. 재시작
3. Assets > Reimport All

---

## ⚡ 빠른 진단 체크리스트

### 화염방사기 문제
- [ ] Player Layer = Player?
- [ ] Snowman Layer = Enemy?
- [ ] FlameHitbox > Is Trigger = ✅?
- [ ] FlameHitbox > Target Layers = Enemy?
- [ ] PlayerFlamethrower > Flame Origin 연결?
- [ ] PlayerFlamethrower > Hitbox Prefab 연결?

### 눈덩이 문제
- [ ] Player Layer = Player?
- [ ] Snowman Layer = Enemy?
- [ ] Snowball > Is Trigger = ✅?
- [ ] Snowball > Rigidbody 존재?
- [ ] Snowball > Target Layers = Player?
- [ ] ProjectileLauncher > Projectile Prefab 연결?
- [ ] ProjectileLauncher > Launch Point 연결?
- [ ] ProjectileLauncher > Auto Fire = ✅?
- [ ] SimpleSnowmanAI > Player 연결?

### 일반 문제
- [ ] Main Camera 존재?
- [ ] SimplePlayerController 추가?
- [ ] Console 에러 없음?
- [ ] Layer Collision Matrix 설정?
- [ ] 씬 저장 (Ctrl+S)?

---

## 🆘 최후의 수단

### 모든 것을 시도했지만 여전히 안 될 때

1. **새 씬 생성**
   - File > New Scene
   - 처음부터 다시 설정

2. **프리팹 재생성**
   - 기존 FlameHitbox/Snowball 삭제
   - 가이드대로 처음부터 다시 생성

3. **스크립트 재임포트**
   - Project 창 > Scripts 폴더 우클릭
   - Reimport

4. **Unity 재시작**
   - Unity 완전 종료 후 재시작
   - 프로젝트 다시 열기

5. **Library 폴더 삭제**
   - Unity 종료
   - 프로젝트 폴더의 Library 폴더 삭제
   - Unity 재시작 (자동으로 재생성됨)

---

## 📞 추가 도움말

### Console 에러 메시지 해석

**"NullReferenceException"**
→ Inspector에서 연결 안 된 필드가 있음

**"MissingReferenceException"**
→ 삭제된 GameObject/Prefab을 참조함

**"The object of type 'X' has been destroyed"**
→ 이미 파괴된 오브젝트를 접근함

**"Can't add component"**
→ 컴파일 에러 또는 스크립트 이름 불일치

### Scene 뷰 단축키

- **F**: 선택한 GameObject로 포커스
- **Ctrl+Shift+F**: 카메라를 현재 뷰로 정렬
- **Q/W/E/R**: Transform 도구 변경
- **T**: Rect Transform 도구

### 유용한 Debug 방법

1. **Gizmos 켜기**: Scene 뷰 우측 상단 Gizmos 버튼
2. **Console Collapse**: 중복 로그 접기
3. **Pause on Error**: Console 우측 상단 ⏸️ 버튼

---

**문제가 해결되지 않으면 Console 에러 메시지를 확인하세요!**
**대부분의 문제는 연결 누락이나 Layer 설정입니다! ✅**

