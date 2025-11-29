using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Diagnostics;
using System.Linq;

namespace OsuRhythmGame
{
    /// <summary>
    /// 메인 게임 윈도우
    /// </summary>
    public partial class GameForm : Form
    {
        // 게임 상태
        private bool isPlaying = false;
        private Stopwatch gameTimer;
        private float songStartTime = 0f;

        // 게임 오브젝트
        private Beatmap? currentBeatmap;
        private List<HitObject> activeHitObjects;
        private List<NoteData> remainingNotes;
        private ScoreManager scoreManager;

        // 렌더링
        private Timer renderTimer;
        private const int FPS = 60;
        private BufferedGraphicsContext? graphicsContext;
        private BufferedGraphics? bufferedGraphics;

        // UI 컨트롤
        private Label scoreLabel;
        private Label comboLabel;
        private Label accuracyLabel;
        private Label songInfoLabel;
        private Button startButton;
        private Panel gamePanel;

        // 게임 설정
        private const float PREEMPT_TIME = 1.5f; // 노트가 미리 나타나는 시간

        public GameForm()
        {
            InitializeComponents();
            InitializeGame();
        }

        private void InitializeComponents()
        {
            // Form 설정
            this.Text = "osu! 스타일 리듬게임";
            this.ClientSize = new Size(1024, 768);
            this.BackColor = Color.Black;
            this.DoubleBuffered = true;
            this.StartPosition = FormStartPosition.CenterScreen;

            // 게임 패널 (그리기 영역)
            gamePanel = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(1024, 768),
                BackColor = Color.FromArgb(20, 20, 40)
            };
            gamePanel.Paint += GamePanel_Paint;
            gamePanel.MouseClick += GamePanel_MouseClick;
            this.Controls.Add(gamePanel);

            // 점수 레이블
            scoreLabel = new Label
            {
                Location = new Point(20, 20),
                Size = new Size(300, 30),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Font = new Font("Arial", 16, FontStyle.Bold),
                Text = "Score: 0"
            };
            gamePanel.Controls.Add(scoreLabel);

            // 콤보 레이블
            comboLabel = new Label
            {
                Location = new Point(450, 650),
                Size = new Size(200, 40),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Font = new Font("Arial", 24, FontStyle.Bold),
                Text = "0x",
                TextAlign = ContentAlignment.MiddleCenter
            };
            gamePanel.Controls.Add(comboLabel);

            // 정확도 레이블
            accuracyLabel = new Label
            {
                Location = new Point(700, 20),
                Size = new Size(300, 30),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Font = new Font("Arial", 16, FontStyle.Bold),
                Text = "Accuracy: 100.00%"
            };
            gamePanel.Controls.Add(accuracyLabel);

            // 곡 정보 레이블
            songInfoLabel = new Label
            {
                Location = new Point(20, 60),
                Size = new Size(400, 25),
                ForeColor = Color.Cyan,
                BackColor = Color.Transparent,
                Font = new Font("Arial", 12),
                Text = ""
            };
            gamePanel.Controls.Add(songInfoLabel);

            // 시작 버튼
            startButton = new Button
            {
                Location = new Point(412, 360),
                Size = new Size(200, 50),
                Font = new Font("Arial", 16, FontStyle.Bold),
                Text = "Start Game",
                BackColor = Color.FromArgb(100, 100, 255),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            startButton.Click += StartButton_Click;
            gamePanel.Controls.Add(startButton);

            // 키 이벤트
            this.KeyPreview = true;
            this.KeyDown += GameForm_KeyDown;

            // 렌더링 타이머
            renderTimer = new Timer
            {
                Interval = 1000 / FPS
            };
            renderTimer.Tick += RenderTimer_Tick;
        }

        private void InitializeGame()
        {
            gameTimer = new Stopwatch();
            activeHitObjects = new List<HitObject>();
            remainingNotes = new List<NoteData>();
            scoreManager = new ScoreManager();

            // 더블 버퍼링 설정
            graphicsContext = BufferedGraphicsManager.Current;
            graphicsContext.MaximumBuffer = new Size(this.Width + 1, this.Height + 1);
            bufferedGraphics = graphicsContext.Allocate(gamePanel.CreateGraphics(), gamePanel.ClientRectangle);

            // 테스트 비트맵 로드
            LoadTestBeatmap();
        }

        private void LoadTestBeatmap()
        {
            currentBeatmap = Beatmap.CreateTestBeatmap(gamePanel.Width, gamePanel.Height);
            songInfoLabel.Text = $"{currentBeatmap.SongName} - {currentBeatmap.Artist} ({currentBeatmap.BPM} BPM)";
        }

        private void StartButton_Click(object? sender, EventArgs e)
        {
            StartGame();
        }

        private void StartGame()
        {
            if (currentBeatmap == null || currentBeatmap.Notes.Count == 0)
            {
                MessageBox.Show("비트맵이 로드되지 않았습니다!", "오류");
                return;
            }

            isPlaying = true;
            startButton.Visible = false;

            // 게임 초기화
            activeHitObjects.Clear();
            remainingNotes = new List<NoteData>(currentBeatmap.Notes);
            remainingNotes = remainingNotes.OrderBy(n => n.Time).ToList();
            scoreManager.Reset();

            // 타이머 시작
            gameTimer.Restart();
            renderTimer.Start();

            UpdateScoreUI();
        }

        private void RenderTimer_Tick(object? sender, EventArgs e)
        {
            if (!isPlaying) return;

            float currentTime = (float)gameTimer.Elapsed.TotalSeconds;

            // 노트 스폰
            SpawnNotes(currentTime);

            // 노트 업데이트 (미스 체크)
            UpdateNotes(currentTime);

            // 게임 종료 체크
            if (remainingNotes.Count == 0 && activeHitObjects.Count == 0)
            {
                EndGame();
            }

            // 화면 갱신
            gamePanel.Invalidate();
            UpdateScoreUI();
        }

        private void SpawnNotes(float currentTime)
        {
            List<NoteData> notesToSpawn = new List<NoteData>();

            foreach (var note in remainingNotes)
            {
                if (currentTime + PREEMPT_TIME >= note.Time)
                {
                    notesToSpawn.Add(note);
                }
                else
                {
                    break;
                }
            }

            foreach (var note in notesToSpawn)
            {
                var hitObject = new HitObject(note.Time, note.Position, PREEMPT_TIME)
                {
                    SpawnTime = currentTime
                };
                activeHitObjects.Add(hitObject);
                remainingNotes.Remove(note);
            }
        }

        private void UpdateNotes(float currentTime)
        {
            List<HitObject> notesToRemove = new List<HitObject>();

            foreach (var hitObject in activeHitObjects)
            {
                float timeUntilHit = hitObject.HitTime - currentTime;

                // 미스 판정 (히트 시간을 너무 지나침)
                if (!hitObject.IsHit && timeUntilHit < -0.15f)
                {
                    scoreManager.AddHit(HitResult.Miss);
                    notesToRemove.Add(hitObject);
                }
            }

            foreach (var note in notesToRemove)
            {
                activeHitObjects.Remove(note);
            }
        }

        private void GamePanel_Paint(object? sender, PaintEventArgs e)
        {
            if (bufferedGraphics == null) return;

            Graphics g = bufferedGraphics.Graphics;
            g.Clear(gamePanel.BackColor);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            if (isPlaying)
            {
                float currentTime = (float)gameTimer.Elapsed.TotalSeconds;

                // 모든 히트 오브젝트 그리기
                foreach (var hitObject in activeHitObjects)
                {
                    hitObject.Draw(g, currentTime);
                }
            }
            else
            {
                // 시작 전 안내 메시지
                using (Font font = new Font("Arial", 20, FontStyle.Bold))
                using (SolidBrush brush = new SolidBrush(Color.White))
                {
                    string message = "Start 버튼을 누르거나 스페이스바를 눌러 시작하세요!\n\n" +
                                   "조작법:\n" +
                                   "- 마우스 클릭으로 노트를 치세요\n" +
                                   "- Z 또는 X 키로도 칠 수 있습니다";

                    SizeF messageSize = g.MeasureString(message, font);
                    g.DrawString(message, font, brush,
                        (gamePanel.Width - messageSize.Width) / 2,
                        150);
                }
            }

            // 버퍼를 화면에 그리기
            bufferedGraphics.Render(e.Graphics);
        }

        private void GamePanel_MouseClick(object? sender, MouseEventArgs e)
        {
            if (!isPlaying) return;

            HandleClick(e.Location);
        }

        private void GameForm_KeyDown(object? sender, KeyEventArgs e)
        {
            if (!isPlaying)
            {
                if (e.KeyCode == Keys.Space)
                {
                    StartGame();
                }
                return;
            }

            // Z, X 키로 클릭
            if (e.KeyCode == Keys.Z || e.KeyCode == Keys.X)
            {
                Point mousePos = gamePanel.PointToClient(Cursor.Position);
                HandleClick(mousePos);
            }

            // R 키로 재시작
            if (e.KeyCode == Keys.R)
            {
                StartGame();
            }

            // ESC 키로 일시정지/메뉴
            if (e.KeyCode == Keys.Escape)
            {
                PauseGame();
            }
        }

        private void HandleClick(Point clickPoint)
        {
            float currentTime = (float)gameTimer.Elapsed.TotalSeconds;
            PointF clickPointF = new PointF(clickPoint.X, clickPoint.Y);

            // 클릭 위치에 가장 가까운 히트 오브젝트 찾기
            HitObject? closestHit = null;
            float closestDistance = float.MaxValue;

            foreach (var hitObject in activeHitObjects)
            {
                if (hitObject.IsHit) continue;

                if (hitObject.IsPointInside(clickPointF))
                {
                    float distance = GetDistance(clickPointF, hitObject.Position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestHit = hitObject;
                    }
                }
            }

            // 히트 판정
            if (closestHit != null)
            {
                float timing = currentTime - closestHit.HitTime;
                HitResult result = closestHit.JudgeTiming(timing);

                closestHit.IsHit = true;
                scoreManager.AddHit(result);

                // 히트 오브젝트 제거
                activeHitObjects.Remove(closestHit);
            }
        }

        private float GetDistance(PointF p1, PointF p2)
        {
            float dx = p1.X - p2.X;
            float dy = p1.Y - p2.Y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

        private void UpdateScoreUI()
        {
            scoreLabel.Text = $"Score: {scoreManager.Score:N0}";
            comboLabel.Text = $"{scoreManager.Combo}x";
            accuracyLabel.Text = $"Accuracy: {scoreManager.Accuracy:F2}%";

            // 콤보에 따라 색상 변경
            if (scoreManager.Combo >= 100)
                comboLabel.ForeColor = Color.Yellow;
            else if (scoreManager.Combo >= 50)
                comboLabel.ForeColor = Color.Cyan;
            else if (scoreManager.Combo >= 20)
                comboLabel.ForeColor = Color.Lime;
            else
                comboLabel.ForeColor = Color.White;
        }

        private void PauseGame()
        {
            isPlaying = false;
            renderTimer.Stop();
            gameTimer.Stop();

            MessageBox.Show("일시정지\n\nR 키를 눌러 재시작하세요.", "일시정지");
        }

        private void EndGame()
        {
            isPlaying = false;
            renderTimer.Stop();
            gameTimer.Stop();

            string result = $"게임 종료!\n\n" +
                          $"최종 점수: {scoreManager.Score:N0}\n" +
                          $"최대 콤보: {scoreManager.MaxCombo}\n" +
                          $"정확도: {scoreManager.Accuracy:F2}%\n" +
                          $"등급: {scoreManager.GetGrade()}\n\n" +
                          scoreManager.GetStatistics();

            MessageBox.Show(result, "게임 결과");

            startButton.Visible = true;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            renderTimer.Stop();
            bufferedGraphics?.Dispose();
            graphicsContext?.Dispose();
            base.OnFormClosing(e);
        }
    }
}
