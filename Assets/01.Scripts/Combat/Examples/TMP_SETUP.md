# TextMeshPro 사용 안내

## ✅ 수정 완료!

두 스크립트가 모두 **TextMeshProUGUI**를 지원하도록 업데이트되었습니다:

- ✅ `HealthBarUI.cs` - TextMeshProUGUI 지원 추가
- ✅ `PlayerVerticalHealthBar.cs` - TextMeshProUGUI 지원 추가

---

## 🎯 Unity 에디터에서 설정하기

### 1. TextMeshPro 텍스트 생성

```
Hierarchy에서 HealthSlider 선택
→ 우클릭
→ UI
→ Text - TextMeshPro 선택
```

⚠️ **처음 사용시:**
- "Import TMP Essentials" 창이 나타나면 **Import** 클릭
- 필수 리소스가 자동으로 설치됩니다

---

### 2. 스크립트에 할당

**HealthBarUI 컴포넌트의 Inspector에서:**

```
UI Elements 섹션:
├─ Health Slider: HealthSlider 할당
├─ Health Text: HealthText (TextMeshProUGUI) 할당  ← 이제 할당 가능!
└─ Fill Image: HealthSlider/Fill Area/Fill 할당
```

---

## 📝 TextMeshPro vs 일반 Text

| 항목 | 일반 Text | TextMeshPro |
|------|-----------|-------------|
| 선명도 | 보통 | 매우 선명 ✨ |
| 성능 | 보통 | 더 좋음 |
| 효과 | 제한적 | 아웃라인, 그림자 등 다양 |
| 사용 | Legacy | 권장 ⭐ |

**결론:** TextMeshPro 사용을 권장합니다!

---

## 🎨 추천 설정

### HealthText (TextMeshProUGUI) 설정:

```
Text: 100 / 100
Font Size: 14
Alignment: 중앙 정렬
Color: 흰색
Font Style: Bold
Auto Size: ✓ 체크

Extra Settings:
├─ Outline: ✓ 체크 (선택사항)
│   ├─ Color: 검은색
│   └─ Size: 0.2
└─ Face: Dilate: 0.1 (더 두껍게)
```

---

## ✅ 완료!

이제 TextMeshPro 텍스트를 HealthBarUI 스크립트의 Health Text 필드에 할당할 수 있습니다! 🎉

