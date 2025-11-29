using System;
using System.Collections.Generic;
using System.Drawing;

namespace OsuRhythmGame
{
    /// <summary>
    /// 비트맵 데이터
    /// </summary>
    public class Beatmap
    {
        public string SongName { get; set; }
        public string Artist { get; set; }
        public float BPM { get; set; }
        public List<NoteData> Notes { get; set; }

        public Beatmap()
        {
            Notes = new List<NoteData>();
        }

        /// <summary>
        /// 테스트용 간단한 비트맵 생성
        /// </summary>
        public static Beatmap CreateTestBeatmap(int canvasWidth, int canvasHeight)
        {
            Beatmap beatmap = new Beatmap
            {
                SongName = "Test Song",
                Artist = "Test Artist",
                BPM = 120f
            };

            Random random = new Random();
            float beatDuration = 60f / beatmap.BPM; // 한 비트의 길이 (초)

            // 16비트 패턴 생성
            for (int i = 0; i < 16; i++)
            {
                float time = i * beatDuration + 2f; // 2초 후부터 시작

                // 화면 내 랜덤 위치 (여백 고려)
                float x = random.Next(100, canvasWidth - 100);
                float y = random.Next(100, canvasHeight - 100);

                beatmap.Notes.Add(new NoteData
                {
                    Time = time,
                    Position = new PointF(x, y)
                });
            }

            return beatmap;
        }
    }

    /// <summary>
    /// 노트 데이터
    /// </summary>
    public class NoteData
    {
        public float Time { get; set; }
        public PointF Position { get; set; }
    }
}
