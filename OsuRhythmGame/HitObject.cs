using System;
using System.Drawing;

namespace OsuRhythmGame
{
    /// <summary>
    /// 히트 오브젝트 (노트) 기본 클래스
    /// </summary>
    public class HitObject
    {
        public float HitTime { get; set; }          // 노트를 쳐야 하는 시간 (초)
        public PointF Position { get; set; }        // 노트 위치
        public bool IsHit { get; set; }             // 이미 쳤는지 여부
        public float ApproachRate { get; set; }     // 어프로치 속도
        public float SpawnTime { get; set; }        // 노트가 생성된 시간

        public const float HitCircleRadius = 40f;   // 히트 서클 반경 (더 크게 - 쉬움)

        public HitObject(float hitTime, PointF position, float approachRate = 1.5f)
        {
            HitTime = hitTime;
            Position = position;
            ApproachRate = approachRate;
            IsHit = false;
        }

        /// <summary>
        /// 노트 그리기
        /// </summary>
        public virtual void Draw(Graphics g, float currentTime)
        {
            if (IsHit) return;

            float timeUntilHit = HitTime - currentTime;

            // 어프로치 서클 (시간에 따라 크기 감소) - 더 화려하게
            if (timeUntilHit > 0)
            {
                float approachScale = 1f + (timeUntilHit / ApproachRate) * 2f;
                float approachRadius = HitCircleRadius * approachScale;

                // 그라데이션 색상 효과
                int colorShift = (int)(timeUntilHit * 100) % 360;
                Color approachColor = ColorFromHSV(colorShift, 0.7, 1.0);

                using (Pen approachPen = new Pen(approachColor, 4))
                {
                    g.DrawEllipse(approachPen,
                        Position.X - approachRadius,
                        Position.Y - approachRadius,
                        approachRadius * 2,
                        approachRadius * 2);
                }
            }

            // 히트 서클 - 그라데이션과 글로우 효과
            // 외부 글로우 효과
            for (int i = 4; i > 0; i--)
            {
                int alpha = 30 * i;
                float glowRadius = HitCircleRadius + (i * 3);
                using (SolidBrush glowBrush = new SolidBrush(Color.FromArgb(alpha, 100, 200, 255)))
                {
                    g.FillEllipse(glowBrush,
                        Position.X - glowRadius,
                        Position.Y - glowRadius,
                        glowRadius * 2,
                        glowRadius * 2);
                }
            }

            // 메인 서클 - 그라데이션
            using (System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath())
            {
                path.AddEllipse(
                    Position.X - HitCircleRadius,
                    Position.Y - HitCircleRadius,
                    HitCircleRadius * 2,
                    HitCircleRadius * 2);

                using (System.Drawing.Drawing2D.PathGradientBrush gradBrush = new System.Drawing.Drawing2D.PathGradientBrush(path))
                {
                    gradBrush.CenterColor = Color.FromArgb(220, 150, 220, 255);
                    gradBrush.SurroundColors = new Color[] { Color.FromArgb(200, 100, 150, 255) };
                    g.FillEllipse(gradBrush,
                        Position.X - HitCircleRadius,
                        Position.Y - HitCircleRadius,
                        HitCircleRadius * 2,
                        HitCircleRadius * 2);
                }

                // 외곽선 - 더 선명하게
                using (Pen pen = new Pen(Color.FromArgb(255, 200, 230, 255), 3))
                {
                    g.DrawEllipse(pen,
                        Position.X - HitCircleRadius,
                        Position.Y - HitCircleRadius,
                        HitCircleRadius * 2,
                        HitCircleRadius * 2);
                }
            }

            // 노트 번호 표시 (선택적) - 더 보기 좋은 색상
            using (Font font = new Font("Arial", 14, FontStyle.Bold))
            using (SolidBrush textBrush = new SolidBrush(Color.FromArgb(255, 255, 255, 255)))
            {
                string text = "◯";
                SizeF textSize = g.MeasureString(text, font);
                g.DrawString(text, font, textBrush,
                    Position.X - textSize.Width / 2,
                    Position.Y - textSize.Height / 2);
            }
        }

        /// <summary>
        /// HSV를 RGB로 변환하는 헬퍼 메서드
        /// </summary>
        private static Color ColorFromHSV(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            if (hi == 0)
                return Color.FromArgb(255, v, t, p);
            else if (hi == 1)
                return Color.FromArgb(255, q, v, p);
            else if (hi == 2)
                return Color.FromArgb(255, p, v, t);
            else if (hi == 3)
                return Color.FromArgb(255, p, q, v);
            else if (hi == 4)
                return Color.FromArgb(255, t, p, v);
            else
                return Color.FromArgb(255, v, p, q);
        }

        /// <summary>
        /// 클릭이 이 노트에 히트했는지 확인
        /// </summary>
        public bool IsPointInside(PointF point)
        {
            float dx = point.X - Position.X;
            float dy = point.Y - Position.Y;
            float distance = (float)Math.Sqrt(dx * dx + dy * dy);
            return distance <= HitCircleRadius;
        }

        /// <summary>
        /// 타이밍 판정 (난이도 쉬움)
        /// </summary>
        public HitResult JudgeTiming(float timing)
        {
            float timingAbs = Math.Abs(timing);

            // 타이밍 윈도우를 더 넓게 조정 (더 쉽게)
            if (timingAbs <= 0.12f)  // 기존: 0.05f
                return HitResult.Perfect;
            else if (timingAbs <= 0.25f)  // 기존: 0.1f
                return HitResult.Great;
            else if (timingAbs <= 0.35f)  // 기존: 0.15f
                return HitResult.Good;
            else
                return HitResult.Miss;
        }
    }

    /// <summary>
    /// 판정 결과
    /// </summary>
    public enum HitResult
    {
        Miss,
        Good,
        Great,
        Perfect
    }
}
