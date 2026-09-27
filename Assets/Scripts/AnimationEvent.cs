using UnityEngine;
using UnityEngine.Events;

public class AnimationEvent : MonoBehaviour
{

    public UnityEvent animationStartEvent;
    public UnityEvent animationMidpointEvent;
    public UnityEvent animationFinishEvent;

    public void AnimationStarted()
    {
        animationStartEvent.Invoke();
    }

    public void AnimationMidpoint()
    {
        animationMidpointEvent.Invoke();
    }
    public void AnimationFinished()
    {
        animationFinishEvent.Invoke();
    }
}
