using Fusion;

namespace NetworkEvents
{
    public readonly struct PlayerViewSpawned
    {
        public readonly PlayerNetwork Player;

        public PlayerViewSpawned(PlayerNetwork player)
        {
            Player = player;
        }
    }
    public readonly struct PlayerViewDespawned
    {
        public readonly PlayerNetwork Player;

        public PlayerViewDespawned(PlayerNetwork player)
        {
            Player = player;
        }
    }

    public readonly struct PlayerScoreUpdate
    {
        public readonly PlayerRef PlayerRef;
        public readonly float Score;

        public PlayerScoreUpdate(PlayerRef playerRef, float newScore)
        {
            PlayerRef = playerRef;
            Score = newScore;
        }
    }
}
