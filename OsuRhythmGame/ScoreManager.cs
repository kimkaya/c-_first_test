using System;
using System.Collections.Generic;

namespace OsuRhythmGame
{
    /// <summary>
    /// 점수 및 콤보 관리
    /// </summary>
    public class ScoreManager
    {
        public int Score { get; private set; }
        public int Combo { get; private set; }
        public int MaxCombo { get; private set; }
        public float Accuracy { get; private set; }

        private int perfectCount;
        private int greatCount;
        private int goodCount;
        private int missCount;
        private int totalHits;

        private const int PERFECT_SCORE = 300;
        private const int GREAT_SCORE = 100;
        private const int GOOD_SCORE = 50;

        public ScoreManager()
        {
            Reset();
        }

        public void Reset()
        {
            Score = 0;
            Combo = 0;
            MaxCombo = 0;
            Accuracy = 100f;

            perfectCount = 0;
            greatCount = 0;
            goodCount = 0;
            missCount = 0;
            totalHits = 0;
        }

        public void AddHit(HitResult result)
        {
            totalHits++;

            switch (result)
            {
                case HitResult.Perfect:
                    perfectCount++;
                    Combo++;
                    Score += PERFECT_SCORE * GetComboMultiplier();
                    break;

                case HitResult.Great:
                    greatCount++;
                    Combo++;
                    Score += GREAT_SCORE * GetComboMultiplier();
                    break;

                case HitResult.Good:
                    goodCount++;
                    Combo++;
                    Score += GOOD_SCORE * GetComboMultiplier();
                    break;

                case HitResult.Miss:
                    missCount++;
                    Combo = 0;
                    break;
            }

            if (Combo > MaxCombo)
            {
                MaxCombo = Combo;
            }

            CalculateAccuracy();
        }

        private int GetComboMultiplier()
        {
            if (Combo >= 100)
                return 4;
            else if (Combo >= 50)
                return 3;
            else if (Combo >= 20)
                return 2;
            else
                return 1;
        }

        private void CalculateAccuracy()
        {
            if (totalHits == 0)
            {
                Accuracy = 100f;
                return;
            }

            int totalPoints = perfectCount * 300 + greatCount * 100 + goodCount * 50;
            int maxPoints = totalHits * 300;

            Accuracy = (float)totalPoints / maxPoints * 100f;
        }

        public string GetGrade()
        {
            if (Accuracy >= 95f && missCount == 0)
                return "SS";
            else if (Accuracy >= 95f)
                return "S";
            else if (Accuracy >= 90f)
                return "A";
            else if (Accuracy >= 80f)
                return "B";
            else if (Accuracy >= 70f)
                return "C";
            else
                return "D";
        }

        public string GetStatistics()
        {
            return $"Perfect: {perfectCount}\n" +
                   $"Great: {greatCount}\n" +
                   $"Good: {goodCount}\n" +
                   $"Miss: {missCount}\n" +
                   $"Max Combo: {MaxCombo}";
        }
    }
}
