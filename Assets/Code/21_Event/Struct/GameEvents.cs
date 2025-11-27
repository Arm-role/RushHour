
using System;
using UnityEngine;

namespace GameEvents
{
    public struct GameScene
    {
        public readonly EGameScene Scene;

        public GameScene(EGameScene scene)
        {
            Scene = scene;
        }
    }

    public static class GameFlowState
    {
        public static EGameFlow Current { get; private set; }

        public static void Set(EGameFlow flow)
        {
            Current = flow;
            EventManager.Invoke(new GameFlow(flow));
        }
    }

    public struct GameFlow
    {
        public readonly EGameFlow Flow;

        public GameFlow(EGameFlow flow)
        {
            Flow = flow;
        }
    }

    public struct TouchItem
    {
        public readonly bool IsTouched;

        public TouchItem(bool isTouched)
        {
            IsTouched = isTouched;
        }
    }

    public struct TimeSpeed
    {
        public readonly float Speed;

        public TimeSpeed(float speed)
        {
            Speed = speed;
        }
    }

    public struct PlayMusicSound
    {
        public readonly string SoundName;

        public PlayMusicSound(string soundName)
        {
            SoundName = soundName;
        }
    }
    public struct PlaySFXSound
    {
        public readonly string SoundName;
        public readonly Vector2 Position;
        public readonly float? Timer;

        public PlaySFXSound(string soundName, Vector2 position, float? timer = null)
        {
            SoundName = soundName;
            Position = position;
            Timer = timer;
        }
    }

    public struct PlayParticle
    {
        public readonly string ParticleName;
        public readonly Vector2 Position;

        public PlayParticle(string particleName, Vector2 position)
        {
            ParticleName = particleName;
            Position = position;
        }
    }
    public struct OrderFulfilledEvent
    {
        public readonly int Score;

        public OrderFulfilledEvent(int score)
        {
            Score = score;
        }
    }

}
