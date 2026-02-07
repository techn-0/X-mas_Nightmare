# 빠른 시작 가이드 - 게임 오버 시스템

## ⚡ 5분 설정 가이드

### 1단계: GameManager 설정 (1분)
```
1. Hierarchy 우클릭 → Create Empty
2. 이름: "GameManager"
3. Add Component → GameManager.cs
```

### 2단계: UI 생성 (3분)
```
1. Hierarchy 우클릭 → UI → Canvas (없으면 생성)

2. Canvas 우클릭 → UI → Panel
   - 이름: "GameOverPanel"
   - Image Color: 검은색 (A: 180)
   - Add Component → Canvas Group
   - Canvas Group Alpha: 0
   - 체크박스 해제 (비활성화)

3. GameOverPanel 우클릭 → UI → Text - TextMeshPro
   - 이름: "GameOverText"
   - Width: 800, Height: 200
   - Font Size: 120
   - Alignment: Center (가운데 정렬)
   - Color: Red
```

### 3단계: 스크립트 연결 (1분)
```
1. GameOverPanel 선택
2. Add Component → GameOverUI.cs
3. 드래그 앤 드롭:
   - Game Over Panel ← GameOverPanel
   - Game Over Text ← GameOverText
```

## ✅ 완료!

이제 게임을 실행하면:
- 플레이어 사망 시 → **YOU DIE** (빨간색)
- 보스 사망 시 → **YOU WIN** (금색)
- 모든 몬스터 AI 자동 정지

## 🎨 색상 커스터마이징

GameOverUI Inspector에서:
- Player Death Color: 빨간색 (255, 0, 0)
- Boss Death Color: 금색 (255, 215, 0)

원하는 색상으로 변경 가능!

---

더 자세한 내용은 `README_GAMEOVER_SYSTEM.md` 참고

