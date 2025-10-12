using System;
using UnityEngine;

public class UIAnimation : MonoBehaviour
{
    [SerializeField] private AnimationData[] animationData;

    public void TriggerAnimation(string triggerName)
    {
        foreach (var animation in animationData)
        {
            if(animation.TriggerName == triggerName)
            {
                animation.Animator.SetTrigger(triggerName);
            }
        }
    }
}

[Serializable]
public class AnimationData
{
    public Animator Animator;
    public string TriggerName;
}