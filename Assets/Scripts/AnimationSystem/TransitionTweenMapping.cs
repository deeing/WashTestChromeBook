using System;

[Serializable]
public class TransitionTweenMapping 
{
    public PlayerEventType start;
    public PlayerEventType target;
    public TweenAnimation[] tweenAnimations;
}
