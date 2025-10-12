
using System.Collections.Generic;

namespace PlayerEvents
{
    public struct PlayerReady
    {
        public readonly bool IsReady;

        public PlayerReady(bool isReady)
        {
            IsReady = isReady;
        }
    }
    public struct SentPlayerScore
    {
        public readonly string PlayerName;
        public readonly float Score;

        public SentPlayerScore(string playerName, float score)
        {
            PlayerName = playerName;
            Score = score;
        }
    }
    public struct KeepPlayerScore
    {
        public readonly Dictionary<string, float> Scores;

        public KeepPlayerScore(Dictionary<string, float> scores)
        {
            Scores = scores;
        }
    }
    public struct SentScoreEvent
    {
        public readonly float Score;

        public SentScoreEvent(float score)
        {
            Score = score;
        }
    }
    public struct SetScoreEvent
    {
        public readonly float Score;

        public SetScoreEvent(float score)
        {
            Score = score;
        }
    }
    public struct TotalScoreEvent
    {
        public readonly float TotalScore;

        public TotalScoreEvent(float totalScore)
        {
            TotalScore = totalScore;
        }
    }
}