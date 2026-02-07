# HealthBar UI 설정 가이드

## 📋 개요
이 가이드는 플레이어 캐릭터의 옆에 붙어있고 항상 카메라를 바라보는 HP 슬라이더 바를 설정하는 방법을 설명합니다.

---

## 🎯 Unity 에디터 설정 단계

### 1단계: World Space Canvas 생성

1. **Hierarchy 창**에서 우클릭 → `UI` → `Canvas` 선택
2. 생성된 Canvas의 이름을 `PlayerHealthBarCanvas`로 변경
3. Canvas의 Inspector 창에서 다음과 같이 설정:
   - **Render Mode**: `World Space`로 변경
   - **Event Camera**: Main Camera를 드래그하여 할당
   - **Sorting Layer**: `UI` (최상위 레이어)
   - **Order in Layer**: `100` (다른 UI보다 위에 표시)

4. Canvas의 **RectTransform** 설정:
   - **Width**: `200`
   - **Height**: `50`
   - **Scale**: `0.01, 0.01, 0.01` (월드 스케일 조정)

---

### 2단계: HP 슬라이더 바 생성

1. **PlayerHealthBarCanvas** 아래에 우클릭 → `UI` → `Slider` 선택
2. 생성된 Slider의 이름을 `HealthSlider`로 변경
3. **Slider Component** 설정:
   - **Direction**: `Left to Right`
   - **Min Value**: `0`
   - **Max Value**: `1`
   - **Whole Numbers**: 체크 해제
   - **Value**: `1` (초기값 100%)

4. **Slider의 RectTransform** 설정:
   - **Anchor Presets**: Stretch-Stretch (전체 화면)
   - **Left**: `10`
   - **Right**: `-10`
   - **Top**: `-10`
   - **Bottom**: `10`

---

### 3단계: Slider 색상 및 디자인 설정

#### Background 설정:
1. `HealthSlider` → `Background` 선택
2. **Image Component**:
   - **Color**: `검은색` 또는 `어두운 회색` (RGB: 0.2, 0.2, 0.2)

#### Fill Area 설정:
1. `HealthSlider` → `Fill Area` → `Fill` 선택
2. **Image Component**:
   - **Color**: `초록색` (RGB: 0, 1, 0) - 코드에서 자동으로 변경됨
   - **Image Type**: `Filled`
   - **Fill Method**: `Horizontal`

#### Handle Slider Area (선택사항):
- 슬라이더 핸들이 필요없으면 `Handle Slider Area` 오브젝트를 삭제하거나 비활성화

---

### 4단계: 텍스트 추가 (선택사항)

1. **HealthSlider** 아래에 우클릭 → `UI` → `Text - TextMeshPro` 선택
   - ⚠️ 처음 사용시 "Import TMP Essentials" 창이 뜨면 `Import` 클릭
2. 이름을 `HealthText`로 변경
3. **TextMeshProUGUI Component** 설정:
   - **Text**: `100 / 100`
   - **Font Size**: `14`
   - **Alignment**: 중앙 정렬 (가운데 버튼)
   - **Color**: `흰색`
   - **Auto Size**: 체크 (자동 크기 조정)
   - **Font Style**: Bold (선택사항)

4. **RectTransform** 설정:
   - **Anchor Presets**: Center-Middle
   - **Width**: `180`
   - **Height**: `30`

**참고:** 일반 Text 대신 TextMeshPro를 사용하면 더 선명하고 깔끔한 텍스트를 표시할 수 있습니다.

---

### 5단계: HealthBarUI 스크립트 추가 및 설정

1. **PlayerHealthBarCanvas** 오브젝트 선택
2. Inspector 창에서 `Add Component` 클릭
3. `HealthBarUI` 검색 후 추가

4. **HealthBarUI Component** 설정:

   #### References 섹션:
   - **Health Component**: Hierarchy에서 Player 오브젝트를 찾아서 `PlayerHealth` 컴포넌트를 드래그하여 할당

   #### UI Elements 섹션:
   - **Health Slider**: `HealthSlider` 오브젝트 할당
   - **Health Text**: `HealthText` 오브젝트 할당 (생성한 경우)
   - **Fill Image**: `HealthSlider` → `Fill Area` → `Fill`의 Image 컴포넌트 할당

   #### Visual Settings 섹션:
   - **Full Health Color**: 밝은 초록색 (RGB: 0, 1, 0)
   - **Mid Health Color**: 노란색 (RGB: 1, 1, 0)
   - **Low Health Color**: 빨간색 (RGB: 1, 0, 0)
   - **Low Health Threshold**: `0.3` (30% 이하)
   - **Mid Health Threshold**: `0.6` (60% 이하)

   #### Position Settings 섹션:
   - **Offset From Character**: `X: 0, Y: -0.5, Z: 0` (플레이어 발 밑)
     - X: 플레이어의 오른쪽(+) 또는 왼쪽(-)
     - Y: 플레이어의 위(+) 또는 아래(-) ⭐ 음수면 아래!
     - Z: 플레이어의 앞(+) 또는 뒤(-)
   - **Follow Character**: ✓ 체크
   - **Face Camera**: ✓ 체크

---

## 🎨 위치 조정 팁

### 오프셋 조정 예시:

- **발 밑 (기본)**: `(0, -0.5, 0)` ⭐ 추천
- **더 아래**: `(0, -1.0, 0)`
- **오른쪽 아래**: `(0.8, -0.5, 0)`
- **왼쪽 아래**: `(-0.8, -0.5, 0)`
- **머리 위**: `(0, 2.5, 0)`
- **오른쪽 옆**: `(1.5, 0.5, 0)`

**💡 중요:** Y값이 음수(-)이면 플레이어 **아래**, 양수(+)이면 **위**에 표시됩니다!

게임을 플레이하면서 **Position Settings**의 **Offset From Character** 값을 실시간으로 조정하여 원하는 위치를 찾을 수 있습니다.

---

## ✅ 최종 체크리스트

- [ ] Canvas의 Render Mode가 `World Space`로 설정됨
- [ ] Canvas의 Scale이 `0.01, 0.01, 0.01`로 설정됨
- [ ] HealthSlider가 올바르게 생성됨
- [ ] HealthBarUI 스크립트가 추가됨
- [ ] Health Component에 PlayerHealth가 할당됨
- [ ] Health Slider, Health Text, Fill Image가 모두 할당됨
- [ ] Follow Character와 Face Camera가 체크됨
- [ ] 게임을 실행하여 HP 바가 플레이어를 따라다니는지 확인
- [ ] 카메라를 움직여도 HP 바가 항상 정면을 향하는지 확인

---

## 🔧 대안: PlayerVerticalHealthBar 사용

더 풍부한 기능(애니메이션, 피격 효과 등)이 필요하면 `PlayerVerticalHealthBar.cs`를 사용할 수 있습니다.

### 사용 방법:
1. 위의 1~4단계를 동일하게 진행
2. 5단계에서 `HealthBarUI` 대신 `PlayerVerticalHealthBar` 스크립트 추가
3. 세로 방향 HP 바를 원하면 Slider의 Direction을 `Bottom to Top`으로 변경

---

## 🎮 테스트 방법

1. **Play Mode** 진입
2. 플레이어가 데미지를 받을 때 HP 바가 감소하는지 확인
3. 플레이어를 이동시켜도 HP 바가 따라다니는지 확인
4. 카메라를 회전시켜도 HP 바가 항상 정면을 향하는지 확인
5. HP 바 색상이 체력에 따라 변하는지 확인 (초록색 → 노란색 → 빨간색)

---

## 🐛 문제 해결

### ⚠️ 플레이 시 UI 위치가 틀어지는 경우:

**중요한 확인 사항:**

1. **Canvas Position이 (0, 0, 0)인지 확인**
   - Canvas의 RectTransform → Position을 (0, 0, 0)으로 설정
   - 스크립트가 자동으로 위치를 조정하므로 Canvas는 원점에 있어야 함

2. **Canvas가 Player의 자식이 아닌지 확인**
   ```
   잘못된 구조:
   Player
   └── PlayerHealthBarCanvas  ❌
   
   올바른 구조:
   Player
   PlayerHealthBarCanvas  ✅ (별도의 오브젝트)
   ```
   
3. **Canvas의 Render Mode가 World Space인지 확인**

4. **Main Camera가 Scene에 존재하고 태그가 "MainCamera"인지 확인**

**더 자세한 해결 방법은 `UI_POSITION_FIX.md` 참조**

---

### HP 바가 보이지 않는 경우:
- Canvas의 Render Mode가 World Space인지 확인
- Canvas의 Scale이 너무 작거나 크지 않은지 확인
- Event Camera가 할당되어 있는지 확인

### HP 바가 플레이어를 따라가지 않는 경우:
- Follow Character가 체크되어 있는지 확인
- Health Component에 PlayerHealth가 할당되어 있는지 확인

### HP 바가 카메라를 바라보지 않는 경우:
- Face Camera가 체크되어 있는지 확인
- Main Camera가 씬에 존재하는지 확인

### HP 바가 업데이트되지 않는 경우:
- Health Slider, Fill Image가 올바르게 할당되어 있는지 확인
- PlayerHealth 스크립트가 플레이어 오브젝트에 있는지 확인
- Console 창에서 에러 메시지 확인

---

## 📝 추가 정보

- 이 HP 바는 World Space에 존재하므로 게임 월드에 실제로 배치됩니다
- Screen Space Overlay UI와는 달리 카메라 거리에 따라 크기가 변합니다
- 여러 플레이어나 적에게 동일한 방식으로 HP 바를 추가할 수 있습니다

