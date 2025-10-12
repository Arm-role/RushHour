
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

    public struct PlaySound
    {
        public readonly string SoundName;

        public PlaySound(string soundName)
        {
            SoundName = soundName;
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
