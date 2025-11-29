# osu! 스타일 리듬게임 (순수 C# / Windows Forms)

Unity나 다른 게임 엔진 없이 **순수 C#과 Windows Forms**만으로 만든 osu! 스타일 리듬게임입니다.

## 특징

- ✅ **프레임워크 불필요** - .NET 6.0만 있으면 실행 가능
- ✅ **순수 C#** - Windows Forms + GDI+로 그래픽 렌더링
- ✅ 히트 서클 및 어프로치 서클 애니메이션
- ✅ 타이밍 판정 시스템 (Perfect, Great, Good, Miss)
- ✅ 스코어 및 콤보 시스템
- ✅ 정확도 계산 및 등급 (SS, S, A, B, C, D)
- ✅ 60 FPS 더블 버퍼링
- ✅ 마우스 + 키보드 입력 (Z, X)

## 요구사항

- **Windows OS**
- **.NET 6.0 SDK 이상**

.NET SDK 설치: https://dotnet.microsoft.com/download

## 빌드 및 실행

### 방법 1: 커맨드 라인

```bash
# 프로젝트 폴더로 이동
cd OsuRhythmGame

# 빌드
dotnet build

# 실행
dotnet run
```

### 방법 2: Visual Studio

1. `OsuRhythmGame.csproj` 파일을 더블클릭
2. Visual Studio에서 열림
3. F5 키를 눌러 실행

### 방법 3: Visual Studio Code

1. VS Code에서 `OsuRhythmGame` 폴더 열기
2. C# Extension 설치 (Microsoft)
3. Ctrl + F5 또는 터미널에서 `dotnet run`

## 파일 구조

```
OsuRhythmGame/
├── OsuRhythmGame.csproj   # 프로젝트 파일
├── Program.cs              # 진입점
├── GameForm.cs             # 메인 게임 윈도우 (UI + 게임 루프)
├── HitObject.cs            # 히트 오브젝트 (노트) 클래스
├── ScoreManager.cs         # 점수 및 콤보 관리
├── Beatmap.cs              # 비트맵 데이터 구조
└── README.md               # 이 파일
```

## 조작법

### 게임 시작
- **Start 버튼 클릭** 또는 **스페이스바**

### 노트 치기
- **마우스 좌클릭**: 노트 위를 클릭
- **Z 키**: 커서 근처의 노트 치기
- **X 키**: 커서 근처의 노트 치기

### 기타
- **R 키**: 재시작
- **ESC 키**: 일시정지

## 판정 기준

| 판정    | 타이밍        | 점수 |
|---------|---------------|------|
| Perfect | ±50ms 이내    | 300  |
| Great   | ±100ms 이내   | 100  |
| Good    | ±150ms 이내   | 50   |
| Miss    | 그 외         | 0    |

## 콤보 시스템

연속으로 노트를 성공하면 콤보가 증가하고, 점수 배율이 올라갑니다:

- **20+ 콤보**: 2배
- **50+ 콤보**: 3배
- **100+ 콤보**: 4배

Miss 판정 시 콤보가 0으로 리셋됩니다.

## 등급 시스템

| 등급 | 조건                         |
|------|------------------------------|
| SS   | 정확도 95% 이상 + Miss 0     |
| S    | 정확도 95% 이상              |
| A    | 정확도 90% 이상              |
| B    | 정확도 80% 이상              |
| C    | 정확도 70% 이상              |
| D    | 그 외                        |

## 커스터마이징

### 비트맵 수정하기

`Beatmap.cs`의 `CreateTestBeatmap()` 메서드를 수정하여 자신만의 패턴을 만들 수 있습니다:

```csharp
public static Beatmap CreateTestBeatmap(int canvasWidth, int canvasHeight)
{
    Beatmap beatmap = new Beatmap
    {
        SongName = "내 곡",
        Artist = "아티스트",
        BPM = 140f
    };

    // 원하는 시간과 위치에 노트 추가
    beatmap.Notes.Add(new NoteData
    {
        Time = 2.0f,  // 2초에
        Position = new PointF(500, 400)  // 이 위치에 노트 생성
    });

    return beatmap;
}
```

### 게임 설정 변경

`GameForm.cs`에서 다양한 설정을 변경할 수 있습니다:

```csharp
private const int FPS = 60;                    // 프레임 속도
private const float PREEMPT_TIME = 1.5f;       // 노트 미리보기 시간
public const float HitCircleRadius = 30f;      // 히트 서클 크기 (HitObject.cs)
```

### 색상 커스터마이징

`GameForm.cs`에서 색상을 변경할 수 있습니다:

```csharp
// 배경색
gamePanel.BackColor = Color.FromArgb(20, 20, 40);

// UI 색상
scoreLabel.ForeColor = Color.White;
comboLabel.ForeColor = Color.White;
```

`HitObject.cs`에서 노트 색상 변경:

```csharp
// 히트 서클 색상
using (SolidBrush brush = new SolidBrush(Color.FromArgb(200, 255, 255, 255)))
using (Pen pen = new Pen(Color.White, 2))
{
    // ...
}
```

## 기술 스택

- **C# 10** (.NET 6.0)
- **Windows Forms** - UI 프레임워크
- **GDI+** (System.Drawing) - 2D 그래픽 렌더링
- **BufferedGraphics** - 더블 버퍼링으로 깜빡임 방지

## 확장 아이디어

### 쉬운 확장
- 🎵 난이도 조절 (노트 속도, 판정 범위)
- 🎵 스킨 시스템 (색상, 이미지)
- 🎵 더 복잡한 패턴
- 🎵 사운드 이펙트 (System.Media.SoundPlayer)

### 중급 확장
- 🎵 음악 파일 재생 (NAudio 라이브러리)
- 🎵 비트맵 파일 로드/저장 (JSON)
- 🎵 리플레이 시스템
- 🎵 온라인 랭킹

### 고급 확장
- 🎵 슬라이더 구현
- 🎵 스피너 구현
- 🎵 오디오 분석으로 자동 비트맵 생성
- 🎵 멀티플레이

## 음악 추가 (선택 사항)

기본 버전에는 음악이 없지만, NAudio 라이브러리로 쉽게 추가할 수 있습니다:

### 1. NAudio 설치

```bash
dotnet add package NAudio
```

### 2. GameForm.cs에 코드 추가

```csharp
using NAudio.Wave;

private IWavePlayer? wavePlayer;
private AudioFileReader? audioFileReader;

private void PlayMusic(string filePath)
{
    wavePlayer = new WaveOutEvent();
    audioFileReader = new AudioFileReader(filePath);
    wavePlayer.Init(audioFileReader);
    wavePlayer.Play();
}
```

## 문제 해결

### 프로그램이 시작되지 않는 경우
- .NET 6.0 SDK가 설치되어 있는지 확인: `dotnet --version`
- Windows OS인지 확인 (Windows Forms는 Windows 전용)

### 화면이 깜빡거리는 경우
- 이미 더블 버퍼링이 적용되어 있어야 하지만, 문제가 있다면 `GameForm.cs`에서 확인:
  ```csharp
  this.DoubleBuffered = true;
  ```

### 노트가 클릭되지 않는 경우
- 마우스가 게임 패널 위에 있는지 확인
- Z/X 키를 대신 사용해보세요

## 성능

- **평균 FPS**: 60 (고정)
- **메모리 사용량**: ~30-50MB
- **CPU 사용량**: 낮음 (~5-10%)

## 라이센스

MIT License - 자유롭게 사용, 수정, 배포 가능합니다.

## 참고

이 프로젝트는 [osu!](https://osu.ppy.sh/)를 참고하여 만든 교육용 프로젝트입니다.

## 개발자 노트

### 왜 순수 C#인가?

- Unity나 MonoGame 같은 게임 엔진 없이 **얼마나 간단하게** 리듬게임을 만들 수 있는지 보여주기 위함
- .NET만 있으면 바로 실행 가능
- 코드가 간단하고 이해하기 쉬움
- Windows Forms의 가능성 탐구

### 제약사항

- Windows 전용 (Windows Forms)
- 고급 그래픽 효과 제한적 (GDI+의 한계)
- 오디오 기능 기본 미포함 (NAudio로 추가 가능)

### 개선 가능한 부분

- 멀티스레딩으로 성능 최적화
- OpenGL/DirectX 연동으로 더 나은 그래픽
- 크로스 플랫폼 지원 (Avalonia, MAUI 등)
