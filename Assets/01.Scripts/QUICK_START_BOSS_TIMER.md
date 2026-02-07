# 보스 스폰 타이머 빠른 시작 가이드

## 5분 안에 설정하기

### 1단계: UI 설정 (2분)

1. **Canvas 생성** (없다면)
   - Hierarchy 우클릭 > UI > Canvas

2. **타이머 텍스트 생성**
   - Canvas 우클릭 > UI > Text - TextMeshPro
   - 이름: "BossTimerText"
   - 위치: 화면 상단 중앙 (Anchor: Top Center)
   - Font Size: 48
   - Alignment: Center
   - Text: "30" (기본값, 자동으로 업데이트됨)

3. **타이머 패널 생성** (옵션)
   - Canvas 우클릭 > UI > Panel
   - 이름: "BossTimerPanel"
   - BossTimerText를 이 Panel의 자식으로 이동

### 2단계: 스폰 위치 설정 (1분)

1. **스폰 포인트 생성**
   - Hierarchy 우클릭 > Create Empty
   - 이름: "BossSpawnPoint"
   - Scene 뷰에서 보스가 등장할 위치로 이동
   - 예: (0, 0, 50) - 플레이어에서 앞쪽 50미터

### 3단계: 타이머 시스템 설정 (2분)

1. **BossSpawnTimer GameObject 생성**
   - Hierarchy 우클릭 > Create Empty
   - 이름: "BossSpawnTimer"

2. **BossSpawnTimer 컴포넌트 추가**
   - Inspector > Add Component > BossSpawnTimer

3. **필수 설정 연결**
   - **Spawn Time**: 30 (기본값)
   - **Timer Text**: BossTimerText 드래그 앤 드롭
   - **Start On Awake**: ✓ 체크 (게임 시작 시 자동 실행)
   - **Boss Prefab**: 보스 프리팹 드래그 앤 드롭 (예: BossSanta)
   - **Spawn Point**: BossSpawnPoint 드래그 앤 드롭
   - **Timer Panel**: BossTimerPanel 드래그 앤 드롭 (옵션)

4. **UI 텍스트 형식 설정** (옵션)
   - **Timer Prefix Text**: "보스 등장까지: "
   - **Timer Suffix Text**: "초"
   - **Timer Format**: "{0:00}" (예: 05, 04, 03...)

### 완료!

이제 플레이 버튼을 누르면:
1. 게임 시작 시 타이머가 30초부터 카운트다운 시작
2. UI에 "보스 등장까지: 30초", "29초", "28초"... 표시
3. 0초가 되면 BossSpawnPoint 위치에 보스 스폰

---

## 추가 기능 (선택사항)

### 트리거 기반 스폰 (플레이어가 특정 영역 진입 시)

1. **트리거 영역 생성**
   - Hierarchy 우클릭 > 3D Object > Cube
   - 이름: "BossSpawnTrigger"
   - Transform > Scale: (10, 5, 10) - 넓은 영역으로 설정
   - Mesh Renderer 체크 해제 (보이지 않게)

2. **트리거 스크립트 추가**
   - Inspector > Add Component > BossSpawnTrigger
   - **Boss Spawn Timer**: BossSpawnTimer GameObject 드래그
   - **Trigger Once**: ✓ 체크

3. **BossSpawnTimer 수정**
   - **Start On Awake**: ✗ 체크 해제 (트리거로만 시작)

### 고급 UI 효과 추가

1. **Canvas Group 추가**
   - BossTimerPanel 선택
   - Add Component > Canvas Group

2. **BossSpawnTimerUI 컴포넌트 추가**
   - BossSpawnTimer GameObject 선택
   - Add Component > BossSpawnTimerUI
   - **Timer Canvas Group**: BossTimerPanel의 Canvas Group 드래그

3. **효과 활성화**
   - **Use Fade Effect**: ✓ (시간 임박 시 깜빡임)
   - **Change Color By Time**: ✓ (흰색 → 노란색 → 빨간색)
   - **Use Scale Effect**: ✓ (펄스 효과)

---

## 문제 해결 체크리스트

- [ ] TextMeshPro가 임포트되었나? (Window > TextMeshPro > Import TMP Essential Resources)
- [ ] Timer Text가 연결되었나?
- [ ] Boss Prefab이 연결되었나?
- [ ] Spawn Point가 연결되었나?
- [ ] Start On Awake가 체크되어 있나? (또는 다른 방법으로 StartTimer() 호출?)
- [ ] Console에 에러가 있나?

---

## 테스트 방법

### 방법 1: 게임 실행
- Start On Awake 체크 후 Play 버튼

### 방법 2: 버튼으로 테스트
1. Canvas에 Button 생성
2. Button의 OnClick 이벤트에 BossSpawnTimer.StartTimer() 연결
3. Start On Awake 체크 해제

### 방법 3: 즉시 스폰 테스트
1. Canvas에 Button 생성
2. Button의 OnClick 이벤트에 BossSpawnTimer.SpawnBossImmediately() 연결
3. 타이머 없이 즉시 보스 스폰 확인

---

## 다음 단계

보스 스폰 타이머가 작동하면:

1. **보스 AI 연결**: BossSantaAI 컴포넌트가 보스 프리팹에 있는지 확인
2. **체력 바 연결**: HealthBarUI를 보스에 연결
3. **사운드 추가**: Tick Sound, Spawn Sound 설정
4. **파티클 이펙트**: 스폰 시 이펙트 추가 (OnBossSpawned 이벤트 활용)
5. **카메라 연출**: 스폰 시 카메라 포커스 이동
6. **배경 음악 변경**: 보스전 음악으로 전환

상세 가이드는 `README_BOSS_SPAWN_TIMER.md`를 참조하세요!

