using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrubEvent : PlayerEvent
{
    [SerializeField]
    private float twistSensitivity = .002f;
    [SerializeField]
    private float crossFadetime = .25f;


    public override void SetupEvent()
    {
        HandAnimations.instance.CrossFade("Scrub", crossFadetime);
    }
    public override bool CheckEndEvent()
    {
        return !GermManager.instance.HasGerms();
    }

    public override void DoEvent()
    {
        float twistAmount = Mathf.Abs(Lean.Touch.LeanGesture.GetTwistDegrees()) * twistSensitivity;
        HandAnimations.instance.PlayAnimation("Scrub", twistAmount);

        if (twistAmount > 0)
        {
            GermManager.instance.KillRandomGerm();
            EffectsManager.instance.Bubbles();
        }
    }
}
