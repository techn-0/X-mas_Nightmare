# 게임 오버 시스템 설정 가이드

이 가이드는 게임 오버 UI (YOU DIE / YOU WIN) 시스템을 Unity 씬에 설정하는 방법을 설명합니다.

## 📋 구현된 기능

1. ✅ 플레이어 사망 시 **YOU DIE** (빨간색) 표시
2. ✅ 보스 사망 시 **YOU WIN** (금색) 표시
3. ✅ 게임 종료 시 모든 몬스터 AI 정지
4. ✅ 페이드 인 애니메이션 효과

## 🔧 Unity 에디터 설정 방법

### 1. GameManager 설정

1. Hierarchy에서 빈 GameObject 생성 (우클릭 → Create Empty)
2. 이름을 `GameManager`로 변경
3. `GameManager.cs` 스크립트를 GameObject에 추가

### 2. UI Canvas 설정

#### 2-1. Canvas 생성
1. Hierarchy에서 UI → Canvas 생성 (이미 있으면 기존 것 사용)
2. Canvas 설정:
   - Render Mode: Screen Space - Overlay
   - Canvas Scaler 추가 (UI → Canvas → Add Component → Canvas Scaler)
   - UI Scale Mode: Scale With Screen Size
   - Reference Resolution: 1920 x 1080

#### 2-2. Game Over Panel 생성
1. Canvas 우클릭 → UI → Panel
2. 이름을 `GameOverPanel`로 변경
3. Inspector에서 설정:
   - Anchor: Stretch (전체 화면)
   - Image 컴포넌트:
     - Color: 검은색 (R:0, G:0, B:0, A:180) - 반투명 배경
4. Canvas Group 컴포넌트 추가:
   - Add Component → Canvas Group
   - Alpha: 0 (처음에는 투명)

#### 2-3. Game Over Text 생성
1. GameOverPanel 우클릭 → UI → Text - TextMeshPro
   - 첫 TMP 사용 시 Import TMP Essentials 클릭
2. 이름을 `GameOverText`로 변경
3. Inspector에서 설정:
   - Anchor: Center
   - Pos X: 0, Pos Y: 0
   - Width: 800, Height: 200
   - Text: "YOU DIE" (임시, 스크립트가 변경함)
   - Font Size: 120
   - Alignment: Center (수평/수직)
   - Color: Red (R:255, G:0, B:0, A:255)
   - Font Style: Bold

#### 2-4. 버튼 추가 (선택사항)

##### Restart 버튼
1. GameOverPanel 우클릭 → UI → Button - TextMeshPro
2. 이름을 `RestartButton`으로 변경
3. 위치: Pos Y: -100
4. 버튼 텍스트: "Restart"
5. On Click() 이벤트:
   - GameOverUI (스크립트가 있는 오브젝트)
   - Function: GameOverUI.OnRestartButton

##### Quit 버튼
1. GameOverPanel 우클릭 → UI → Button - TextMeshPro
2. 이름을 `QuitButton`으로 변경
3. 위치: Pos Y: -200
4. 버튼 텍스트: "Quit"
5. On Click() 이벤트:
   - GameOverUI (스크립트가 있는 오브젝트)
   - Function: GameOverUI.OnQuitButton

### 3. GameOverUI 스크립트 설정

1. GameOverPanel에 `GameOverUI.cs` 스크립트 추가
2. Inspector에서 레퍼런스 연결:
   - Game Over Panel: GameOverPanel 드래그
   - Game Over Text: GameOverText 드래그
3. 텍스트 설정:
   - Player Death Text: "YOU DIE"
   - Boss Death Text: "YOU WIN"
   - Player Death Color: Red (255, 0, 0)
   - Boss Death Color: Gold (255, 215, 0)
4. 애니메이션 설정:
   - Fade In Duration: 1.0
   - Use Animation: ✓ (체크)

### 4. 초기 상태 설정

1. GameOverPanel을 비활성화 (Inspector에서 체크박스 해제)
   - 게임 시작 시 숨겨져 있어야 함

## 🎮 테스트 방법

### 플레이어 사망 테스트
1. 게임 실행
2. 플레이어가 적의 공격을 받아 체력이 0이 되면
3. "YOU DIE" (빨간색) 패널이 페이드 인으로 나타남
4. 모든 적 AI가 정지됨

### 보스 사망 테스트
1. 게임 실행
2. 보스를 공격하여 체력을 0으로 만들면
3. "YOU WIN" (금색) 패널이 페이드 인으로 나타남
4. 모든 적 AI가 정지됨

## 📝 코드 구조

### GameManager.cs
- 게임 전체 상태 관리
- 싱글톤 패턴
- 이벤트:
  - OnPlayerDeath: 플레이어 사망 시
  - OnBossDeath: 보스 사망 시
  - OnGameOver: 게임 종료 시
- 모든 적 AI 정지 기능

### GameOverUI.cs
- UI 표시 및 애니메이션 관리
- GameManager 이벤트 구독
- 텍스트 및 색상 변경
- 페이드 인 효과

### PlayerHealth.cs (수정됨)
- HandleDeath()에서 GameManager.Instance.PlayerDied() 호출

### BossSantaAI.cs (수정됨)
- OnBossDeath()에서 GameManager.Instance.BossDied() 호출

## 🎨 커스터마이징

### 텍스트 변경
GameOverUI Inspector에서:
- Player Death Text / Boss Death Text 수정

### 색상 변경
GameOverUI Inspector에서:
- Player Death Color / Boss Death Color 수정
- 금색 기본값: (255, 215, 0)
- 추천 색상:
  - 빨간색: (255, 0, 0)
  - 주황색: (255, 128, 0)
  - 보라색: (128, 0, 255)
  - 청록색: (0, 255, 255)

### 애니메이션 속도 조절
GameOverUI Inspector에서:
- Fade In Duration 값 변경 (초 단위)
- 빠른 효과: 0.5초
- 보통 효과: 1.0초
- 느린 효과: 2.0초

### 배경 투명도 조절
GameOverPanel의 Image 컴포넌트:
- Alpha 값 조절 (0~255)
- 완전 투명: 0
- 반투명: 128~200
- 불투명: 255

## 🐛 문제 해결

### 패널이 나타나지 않음
1. GameManager가 씬에 있는지 확인
2. GameOverUI 스크립트의 레퍼런스가 연결되었는지 확인
3. Console 창에서 에러 메시지 확인

### 텍스트가 표시되지 않음
1. TextMeshPro 패키지가 설치되었는지 확인
2. Font Asset이 있는지 확인
3. Canvas Sorting Layer 확인

### 적 AI가 정지되지 않음
1. 적 오브젝트에 올바른 스크립트가 있는지 확인:
   - SnowmanAI
   - ExplosiveSnowmanAI
   - BossSantaAI
2. Console 로그 확인: "[GameManager] 정지된 AI: ..."

### 버튼이 작동하지 않음
1. EventSystem이 씬에 있는지 확인
2. 버튼의 On Click() 이벤트 설정 확인
3. GameOverUI 스크립트 레퍼런스 확인

## ✅ 완료 체크리스트

- [ ] GameManager GameObject 생성 및 스크립트 추가
- [ ] Canvas 생성 및 설정
- [ ] GameOverPanel 생성 및 Canvas Group 추가
- [ ] GameOverText (TextMeshPro) 생성 및 설정
- [ ] GameOverUI 스크립트 추가 및 레퍼런스 연결
- [ ] 색상 및 텍스트 설정
- [ ] GameOverPanel 비활성화 (초기 상태)
- [ ] (선택) Restart/Quit 버튼 추가
- [ ] 플레이어 사망 테스트
- [ ] 보스 사망 테스트
- [ ] 적 AI 정지 확인

## 🎯 다음 단계

1. 사운드 효과 추가 (게임 오버 사운드)
2. 점수 표시 기능
3. 승리/패배 통계
4. 메인 메뉴로 돌아가기 기능
5. 게임 일시정지 (Time.timeScale = 0)

