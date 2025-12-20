# UI 위치 틀어짐 문제 해결 가이드

## 🐛 증상
플레이 모드에서 HP 바가 이상한 위치에 나타나거나, 캐릭터를 제대로 따라가지 않는 문제

---

## ✅ 해결 방법

### 1️⃣ Canvas 설정 확인

**PlayerHealthBarCanvas** 오브젝트를 선택하고 Inspector에서 확인:

#### Canvas Component:
```
✓ Render Mode: World Space (반드시!)
✓ Event Camera: Main Camera 할당
```

#### RectTransform:
```
Pos X: 0
Pos Y: 0
Pos Z: 0

Width: 200
Height: 50

Scale X: 0.01
Scale Y: 0.01
Scale Z: 0.01
```

**⚠️ 중요:** Canvas의 Position은 (0, 0, 0)이어야 합니다!
- 스크립트가 자동으로 위치를 조정하므로 Canvas 자체는 원점에 있어야 합니다.

---

### 2️⃣ Canvas가 Player의 자식이 아닌지 확인

**잘못된 구조:**
```
Player
└── PlayerHealthBarCanvas  ❌ 이렇게 하지 마세요!
    └── HealthSlider
```

**올바른 구조:**
```
Player (PlayerHealth 컴포넌트 있음)

PlayerHealthBarCanvas  ✅ 별도의 오브젝트로 생성
└── HealthSlider
```

**수정 방법:**
1. Hierarchy에서 `PlayerHealthBarCanvas`를 드래그
2. Hierarchy의 빈 공간에 드롭하여 최상위 오브젝트로 이동
3. Canvas의 Position을 (0, 0, 0)으로 리셋

---

### 3️⃣ 오프셋 값 조정

**HealthBarUI Component**의 **Position Settings** 섹션:

```
Offset From Character:
  X: 0     (플레이어 중앙)
  Y: -0.5  (플레이어 아래 - 발 밑)
  Z: 0     (앞뒤 이동 없음)

✓ Follow Character
✓ Face Camera
```

**권장 오프셋 값:**

| 위치 | X | Y | Z | 설명 |
|------|---|---|---|------|
| 발 밑 | 0 | -0.5 | 0 | 기본 권장 ⭐ |
| 더 아래 | 0 | -1.0 | 0 | 발 훨씬 아래 |
| 오른쪽 아래 | 0.8 | -0.5 | 0 | 오른쪽 발 밑 |
| 왼쪽 아래 | -0.8 | -0.5 | 0 | 왼쪽 발 밑 |
| 머리 위 | 0 | 2.5 | 0 | 상단 표시 |
| 오른쪽 위 | 1.5 | 0.5 | 0 | 측면 표시 |

**💡 팁:**
- Y값이 **음수(-)** 이면 플레이어 **아래**에 표시됩니다
- Y값이 **양수(+)** 이면 플레이어 **위**에 표시됩니다
- 플레이어 키가 2m라면, Y: -0.5는 발 밑 0.5m 아래를 의미합니다

---

### 4️⃣ 실시간 테스트 및 조정

**Play Mode에서 값 조정하기:**

1. **Play 버튼** 클릭하여 게임 실행
2. Hierarchy에서 `PlayerHealthBarCanvas` 선택
3. Inspector에서 **Offset From Character** 값을 실시간으로 조정
4. 원하는 위치를 찾으면 값을 기록
5. **Stop** 버튼을 누르기 전에 값을 복사
6. Stop 후 다시 같은 값으로 설정

**⚠️ 주의:** Play Mode에서 변경한 값은 Stop하면 사라집니다!

---

### 5️⃣ Canvas Scaler 설정 (선택사항)

Canvas에 **Canvas Scaler** 컴포넌트가 있다면:

```
UI Scale Mode: Constant Pixel Size
Scale Factor: 1
Reference Pixels Per Unit: 100
```

---

## 🔧 추가 문제 해결

### 문제: HP 바가 너무 크거나 작게 보임

**해결:** Canvas의 Scale 조정
```
Scale: 0.01, 0.01, 0.01  (기본값)
Scale: 0.005, 0.005, 0.005  (더 작게)
Scale: 0.02, 0.02, 0.02  (더 크게)
```

---

### 문제: HP 바가 카메라 회전에 따라 기울어짐

**해결:** 코드가 이미 수정되었습니다!
- Y축만 회전하도록 변경되어 수평 상태 유지

---

### 문제: HP 바가 캐릭터를 따라가지 않음

**체크리스트:**
- [ ] Health Component에 PlayerHealth가 할당되어 있는가?
- [ ] Follow Character가 체크되어 있는가?
- [ ] PlayerHealth 스크립트가 Player 오브젝트에 있는가?
- [ ] Console에 에러 메시지가 없는가?

---

### 문제: HP 바가 플레이어 뒤에 보임

**원인:** Sorting Order 또는 Z-Fighting 문제

**해결 1:** Canvas 설정 변경
```
Canvas Component:
├─ Sorting Layer: UI
└─ Order in Layer: 100 (높은 값)
```

**해결 2:** 오프셋 Z값 조정
```
Offset From Character:
  Z: 0.5  (약간 앞으로)
```

---

### 문제: 플레이어가 멀리 가면 HP 바가 안 보임

**원인:** World Space UI는 거리에 따라 크기가 변함

**해결 1:** Canvas Scale 증가
```
Scale: 0.02, 0.02, 0.02
```

**해결 2:** 카메라 Far Clipping Plane 확인
```
Main Camera:
└─ Far: 1000 (충분히 큰 값)
```

---

## 📋 올바른 설정 요약

### Canvas 설정:
```yaml
Render Mode: World Space
Event Camera: Main Camera
Position: (0, 0, 0)
Scale: (0.01, 0.01, 0.01)
Sorting Layer: UI
Order in Layer: 100
```

### HealthBarUI 설정:
```yaml
Health Component: Player의 PlayerHealth 컴포넌트
Health Slider: HealthSlider 오브젝트
Fill Image: HealthSlider/Fill Area/Fill의 Image
Health Text: HealthText (TextMeshProUGUI)
Offset From Character: (1.5, 0.5, 0)
Follow Character: ✓
Face Camera: ✓
```

---

## 🎯 빠른 리셋 방법

문제가 계속되면 처음부터 다시 설정:

1. **PlayerHealthBarCanvas 삭제**
2. **새로운 Canvas 생성** (UI → Canvas)
3. **HEALTHBAR_SETUP_GUIDE.md** 가이드를 정확히 따라 재설정
4. Canvas Position이 (0, 0, 0)인지 확인
5. Canvas가 Player의 자식이 아닌지 확인

---

## ✅ 최종 확인

Play Mode에서 다음을 확인:

- [ ] HP 바가 플레이어 옆에 나타남
- [ ] 플레이어 이동 시 HP 바가 따라감
- [ ] 카메라 회전 시 HP 바가 항상 정면을 향함
- [ ] HP 바가 기울어지지 않고 수평 상태 유지
- [ ] 체력 변화 시 HP 바가 업데이트됨

모든 항목이 체크되면 성공! 🎉

