using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingertipsScrubEvent : PlayerEvent
{
    [SerializeField]
    private float twistSensitivity = .002f;
    [SerializeField]
    private float crossFadetime = .25f;


    public override bool CheckEndEvent()
    {
        return !GermManager.instance.HasGermsOfType(GermType.FINGERTIPS);
    }

    public override void DoEvent()
    {
        float twistAmount = Mathf.Abs(Lean.Touch.LeanGesture.GetTwistDegrees()) * twistSensitivity;
        HandAnimations.instance.PlayAnimationStep("FingertipsScrub", twistAmount);

        if (twistAmount > 0)
        {
            GermManager.instance.KillRandomGermOfType(GermType.FINGERTIPS);
            EffectsManager.instance.Bubbles();
        }
    }
}
