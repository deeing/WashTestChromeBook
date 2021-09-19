using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrubEvent : PlayerEvent
{
    [SerializeField]
    private float twistSensitivity = .002f;
    [SerializeField]
    private float crossFadetime = .25f;

    public static float scrubVelocity = 0f;

    public override void SetupEvent()
    {
        HandAnimations.instance.CrossFade("Scrub", crossFadetime);
    }
    public override bool CheckEndEvent()
    {
        return false;
    }

    public override void DoEvent()
    {
        float twistAmount = Mathf.Abs(Lean.Touch.LeanGesture.GetTwistDegrees()) * twistSensitivity;
        HandAnimations.instance.PlayAnimation("Scrub", twistAmount);
        scrubVelocity = twistAmount;
    }
}
