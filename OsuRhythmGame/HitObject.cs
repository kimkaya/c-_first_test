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

        public const float HitCircleRadius = 30f;   // 히트 서클 반경

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

            // 어프로치 서클 (시간에 따라 크기 감소)
            if (timeUntilHit > 0)
            {
                float approachScale = 1f + (timeUntilHit / ApproachRate) * 2f;
                float approachRadius = HitCircleRadius * approachScale;

                using (Pen approachPen = new Pen(Color.White, 3))
                {
                    g.DrawEllipse(approachPen,
                        Position.X - approachRadius,
                        Position.Y - approachRadius,
                        approachRadius * 2,
                        approachRadius * 2);
                }
            }

            // 히트 서클
            using (SolidBrush brush = new SolidBrush(Color.FromArgb(200, 255, 255, 255)))
            using (Pen pen = new Pen(Color.White, 2))
            {
                g.FillEllipse(brush,
                    Position.X - HitCircleRadius,
                    Position.Y - HitCircleRadius,
                    HitCircleRadius * 2,
                    HitCircleRadius * 2);

                g.DrawEllipse(pen,
                    Position.X - HitCircleRadius,
                    Position.Y - HitCircleRadius,
                    HitCircleRadius * 2,
                    HitCircleRadius * 2);
            }

            // 노트 번호 표시 (선택적)
            using (Font font = new Font("Arial", 14, FontStyle.Bold))
            using (SolidBrush textBrush = new SolidBrush(Color.Black))
            {
                string text = "◯";
                SizeF textSize = g.MeasureString(text, font);
                g.DrawString(text, font, textBrush,
                    Position.X - textSize.Width / 2,
                    Position.Y - textSize.Height / 2);
            }
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
        /// 타이밍 판정
        /// </summary>
        public HitResult JudgeTiming(float timing)
        {
            float timingAbs = Math.Abs(timing);

            if (timingAbs <= 0.05f)
                return HitResult.Perfect;
            else if (timingAbs <= 0.1f)
                return HitResult.Great;
            else if (timingAbs <= 0.15f)
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
