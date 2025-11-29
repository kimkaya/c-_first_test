# osu! 스타일 리듬게임 (Unity)

C#과 Unity로 제작된 간단한 osu! 스타일 리듬게임입니다.

## 기능

- ✅ 히트 서클 (osu! 기본 노트)
- ✅ 어프로치 서클 애니메이션
- ✅ 타이밍 판정 시스템 (Perfect, Great, Good, Miss)
- ✅ 스코어 및 콤보 시스템
- ✅ 정확도 계산
- ✅ 등급 시스템 (SS, S, A, B, C, D)
- ✅ 비트맵 시스템
- ✅ 마우스 + 키보드 입력 (Z, X 키)

## Unity 프로젝트 설정

### 1. Unity 프로젝트 생성
1. Unity Hub를 열고 **New Project** 클릭
2. **2D (Core)** 템플릿 선택
3. 프로젝트 이름 입력 후 생성

### 2. 스크립트 설치
1. Unity 프로젝트의 `Assets` 폴더에 `Scripts` 폴더 생성
2. 이 저장소의 `Scripts` 폴더 내 모든 `.cs` 파일을 복사

**파일 목록:**
- `GameManager.cs` - 게임 전체 관리
- `HitObject.cs` - 히트 오브젝트 기본 클래스
- `HitCircle.cs` - 히트 서클 구현
- `ScoreManager.cs` - 점수 및 콤보 관리
- `BeatmapManager.cs` - 비트맵 로딩
- `Beatmap.cs` - 비트맵 데이터 구조
- `UIManager.cs` - UI 관리
- `InputManager.cs` - 입력 처리

### 3. TextMeshPro 설치
1. Unity 상단 메뉴에서 `Window > Package Manager`
2. `TextMeshPro` 검색 후 설치
3. 설치 후 `TMP Essentials` 임포트

### 4. 씬 설정

#### 4.1 카메라 설정
1. **Main Camera** 선택
2. Inspector에서 설정:
   - Background: 검정색 또는 원하는 색
   - Size: 5 (Orthographic)

#### 4.2 GameManager 오브젝트 생성
1. Hierarchy에서 우클릭 → `Create Empty`
2. 이름을 `GameManager`로 변경
3. Inspector에서 `Add Component` → `GameManager` 스크립트 추가
4. Inspector에서 `Add Component` → `BeatmapManager` 스크립트 추가
5. Inspector에서 `Add Component` → `ScoreManager` 스크립트 추가
6. Inspector에서 `Add Component` → `InputManager` 스크립트 추가

#### 4.3 UI 설정

**Canvas 생성:**
1. Hierarchy에서 우클릭 → `UI > Canvas`
2. Canvas 설정:
   - Render Mode: Screen Space - Overlay
   - Canvas Scaler에서 UI Scale Mode: Scale With Screen Size
   - Reference Resolution: 1920 x 1080

**UI Manager 추가:**
1. Canvas에 `UIManager` 스크립트 컴포넌트 추가

**점수 표시 UI:**
1. Canvas 우클릭 → `UI > TextMeshPro - Text` (3개 생성)
2. 각각 이름 변경:
   - `ScoreText`
   - `ComboText`
   - `AccuracyText`

3. 위치 조정 (예시):
   - ScoreText: 화면 좌상단 (Anchor: Top Left)
   - ComboText: 화면 중앙 하단
   - AccuracyText: 화면 우상단 (Anchor: Top Right)

**시작 버튼:**
1. Canvas 우클릭 → `UI > Button - TextMeshPro`
2. 이름을 `StartButton`으로 변경
3. Button의 자식 Text 수정: "Start Game"

**재시작 버튼:**
1. StartButton 복제 (Ctrl+D)
2. 이름을 `RestartButton`으로 변경
3. Text 수정: "Restart"

**UIManager 연결:**
1. Canvas의 UIManager 컴포넌트에서:
   - Score Text → ScoreText 드래그
   - Combo Text → ComboText 드래그
   - Accuracy Text → AccuracyText 드래그
   - Start Button → StartButton 드래그
   - Restart Button → RestartButton 드래그

### 5. 히트 오브젝트 태그 설정
1. Unity 상단 메뉴 `Edit > Project Settings > Tags and Layers`
2. Tags 섹션에서 `+` 버튼 클릭
3. `HitObject` 태그 추가

## 사용 방법

### 게임 시작
1. Unity에서 Play 버튼 클릭
2. "Start Game" 버튼 클릭
3. 비트에 맞춰 나타나는 히트 서클을 클릭!

### 조작 방법
- **마우스 좌클릭**: 히트 서클 클릭
- **Z 또는 X 키**: 커서 근처의 히트 서클 치기

### 판정 기준
- **Perfect**: ±50ms 이내 (300점)
- **Great**: ±100ms 이내 (100점)
- **Good**: ±150ms 이내 (50점)
- **Miss**: 그 외 (0점)

### 콤보 시스템
- 연속으로 노트를 성공하면 콤보가 증가
- 콤보에 따라 점수 배율 증가:
  - 20+ 콤보: 2배
  - 50+ 콤보: 3배
  - 100+ 콤보: 4배

## 커스터마이징

### 비트맵 만들기

`BeatmapManager.cs`의 `CreateTestBeatmap()` 함수를 수정하거나, JSON 파일로 비트맵을 만들 수 있습니다.

**JSON 비트맵 예시:**
```json
{
  "songName": "My Song",
  "artist": "Artist Name",
  "bpm": 120,
  "notes": [
    {
      "time": 2.0,
      "position": {"x": 0, "y": 0},
      "type": 0
    },
    {
      "time": 3.0,
      "position": {"x": 2, "y": 1},
      "type": 0
    }
  ]
}
```

### 음악 추가
1. 음악 파일(.mp3, .wav)을 Unity의 `Assets/Audio` 폴더에 추가
2. `GameManager`의 `Music Source`에 AudioClip 연결
3. 또는 `Beatmap.cs`에서 `audioClip` 필드 사용

### 비주얼 커스터마이징
- `HitCircle.cs`의 `CreateFilledCircleSprite()` 수정: 히트 서클 모양
- `HitObject.cs`의 `CreateCircleSprite()` 수정: 어프로치 서클 모양
- 스프라이트 이미지를 사용하려면 직접 Sprite를 할당

## 확장 아이디어

- 🎵 슬라이더 구현
- 🎵 스피너 구현
- 🎵 오디오 비트 감지로 자동 비트맵 생성
- 🎵 리플레이 시스템
- 🎵 랭킹 시스템
- 🎵 스킨 시스템
- 🎵 모드 (Easy, Hard, etc.)

## 기술 스택

- **Unity 2021.3+** (2D)
- **C# 9.0**
- **TextMeshPro**

## 참고

이 프로젝트는 [osu!](https://osu.ppy.sh/)를 참고하여 만든 교육용 프로젝트입니다.

## 문제 해결

### 노트가 클릭되지 않는 경우
1. `HitObject` 태그가 제대로 추가되었는지 확인
2. `InputManager`가 GameManager에 추가되었는지 확인
3. 카메라가 Orthographic인지 확인

### UI가 표시되지 않는 경우
1. TextMeshPro가 설치되었는지 확인
2. UIManager의 참조가 제대로 연결되었는지 확인
3. Canvas의 Render Mode 확인

### 음악이 재생되지 않는 경우
1. AudioSource 컴포넌트 확인
2. AudioClip이 할당되었는지 확인
3. 볼륨 설정 확인

## 라이센스

MIT License - 자유롭게 사용, 수정, 배포 가능합니다.
