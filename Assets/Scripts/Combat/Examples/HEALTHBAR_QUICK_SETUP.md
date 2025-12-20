# HP 슬라이더 바 - 빠른 설정 가이드

## 🚀 5분 안에 설정하기

### 1. Canvas 만들기
```
Hierarchy → 우클릭 → UI → Canvas
이름: PlayerHealthBarCanvas
```

**Inspector 설정:**
- Render Mode: `World Space`
- Scale: `0.01, 0.01, 0.01`
- Width: `200`, Height: `50`

---

### 2. Slider 추가
```
PlayerHealthBarCanvas → 우클릭 → UI → Slider
이름: HealthSlider
```

**설정:**
- Min: `0`, Max: `1`
- Direction: `Left to Right`

**텍스트 추가 (선택사항):**
```
HealthSlider → 우클릭 → UI → Text - TextMeshPro
이름: HealthText
```
- Font Size: `14`
- Alignment: 중앙 정렬
- Color: 흰색

---

### 3. 스크립트 추가
```
PlayerHealthBarCanvas 선택 → Add Component → HealthBarUI
```

**필수 할당:**
- Health Component: Player의 `PlayerHealth` 컴포넌트
- Health Slider: `HealthSlider`
- Fill Image: `HealthSlider/Fill Area/Fill`의 Image

**위치 설정:**
- Offset From Character: `(0, -0.5, 0)` - 플레이어 발 밑
- ✓ Follow Character
- ✓ Face Camera

---

### 4. 완료! ✅

Play를 눌러서 테스트하세요.

---

## 📍 위치 조정

원하는 위치로 이동하려면 `Offset From Character` 값을 변경:

| 위치 | X | Y | Z |
|------|---|---|---|
| 발 밑 (기본) | 0 | -0.5 | 0 |
| 더 아래 | 0 | -1.0 | 0 |
| 오른쪽 아래 | 0.8 | -0.5 | 0 |
| 머리 위 | 0 | 2.5 | 0 |
| 오른쪽 옆 | 1.5 | 0.5 | 0 |

**💡 Y값이 음수(-)면 플레이어 아래, 양수(+)면 위에 표시됩니다**

---

더 자세한 내용은 `HEALTHBAR_SETUP_GUIDE.md`를 참조하세요!

