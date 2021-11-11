using System.Collections;
using UnityEngine;

public abstract class SwitchEvent : PlayerEvent
{

    public float touchInputwithSensitivity { get; private set; } = 0f;

    private bool isIdle = false;
    private bool unMoved = true;

    public abstract void DoSwitch();
    public virtual float DoTouchInput()
    {
        return 0f;
    }

    private void NeutralIdle()
    {
        HandAnimations.instance.TransitionPlay("Idle", 1f, .2f);
    }

    protected override string GetImpatienceAnimationName()
    {
        return "Neutral Impatience";
    }
}
