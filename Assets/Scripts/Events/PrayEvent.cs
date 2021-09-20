using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrayEvent : PlayerEvent
{
    [SerializeField]
    private float pinchSensitivity = .002f;
    [SerializeField]
    ParticleSystem horayParticles;

    public override void DoEvent()
    {
        if (Lean.Touch.LeanGesture.GetPinchScale() < 1f)
        {
            float currFingerDistance = Lean.Touch.LeanGesture.GetScaledDistance();
            float lastFingerDistance = Lean.Touch.LeanGesture.GetLastScaledDistance();

            float pinchAmounnt = (lastFingerDistance - currFingerDistance) * pinchSensitivity;
            HandAnimations.instance.PlayAnimation("Pray", pinchAmounnt);
        }
    }
    public override bool CheckEndEvent()
    {
        return HandAnimations.instance.IsAnimationFinished();
    }

    public override void EndEvent()
    {
        base.EndEvent();
        horayParticles.Play();
    }

}
