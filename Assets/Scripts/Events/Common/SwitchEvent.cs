using System.Collections;
using UnityEngine;

public abstract class SwitchEvent : PlayerEvent
{
    [SerializeField]
    private float sensitivity = .005f;
    public float touchInputwithSensitivity { get; private set; } = 0f;

    public abstract float DoTouchInput();

    public abstract void DoSwitch();
    public override void DoEvent()
    {
        touchInputwithSensitivity = DoTouchInput() * sensitivity * GetSensitivityAdjustment();

        if (touchInputwithSensitivity != 0)
        {
            DoSwitch();
        }
    }

    public override bool CheckEndEvent()
    {
        return HandAnimations.instance.IsAnimationFinished();
    }
}
