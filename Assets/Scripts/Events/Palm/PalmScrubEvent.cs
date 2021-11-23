using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalmScrubEvent : ScrubEvent
{
    [SerializeField]
    [Tooltip("Max input amount that this event can take before triggering the fire event")]
    private float fireInputLimit = 15f;

    // whether or not we have caught on fire
    private bool isOnFire = false;
    private bool isPlayingFireanimation = false;
    // time for animation to play
    private float animationFireTime = 2f;
    private WaitForSeconds animationFireWait;
    // time for fire particles to play
    private float particleFireTime = 1.3f;
    private WaitForSeconds particleFireWait;

    public override void SetupEvent()
    {
        base.SetupEvent();
        animationFireWait = new WaitForSeconds(animationFireTime);
        particleFireWait = new WaitForSeconds(particleFireTime);
    }

    public override GermType GetGermType()
    {
        return GermType.Palm;
    }

    public override void DoScrub()
    {
        //HandAnimations.instance.PlayAnimationStep("Palm Scrub", touchInput);
        //HandAnimations.instance.TransitionPlay("Palm Scrub", "Palm Together", crossFadetime, touchInput);
        HandAnimations.instance.TransitionPlayStep("Palm Scrub", idleTransitionTime, touchInputWithSensitivity);

    }

    public override void DoEvent()
    {
        if (!isOnFire)
        {
            base.DoEvent();
        } else
        {
            DoFire();
        }
    }

    private void DoFire()
    {
        if (!isPlayingFireanimation)
        {
            HandAnimations.instance.TransitionPlay("Fire Hands");
            EffectsManager.instance.ToggleFire(true);
            isPlayingFireanimation = true;
            StartCoroutine(TurnOffFire());
            StartCoroutine(TurnOffParticles());
        }
    }

    private IEnumerator TurnOffFire()
    {
        yield return animationFireWait;
        isPlayingFireanimation = false;
        isOnFire = false;
    }

    private IEnumerator TurnOffParticles()
    {
        yield return particleFireWait;
        EffectsManager.instance.ToggleFire(false);
    }

    public override void DoIdle()
    {
        HandAnimations.instance.CrossFade("Palm Idle", idleTransitionTime);
    }

    public override float DoTouchInput()
    {
        float input = Mathf.Abs(Lean.Touch.LeanGesture.GetTwistDegrees());
        if (input > fireInputLimit)
        {
            isOnFire = true;
        }
        return input;
    }


    public override PlayerEventType GetEventType()
    {
        return PlayerEventType.PalmScrub;
    }

    public override void ReturnToNeutral()
    {
        HandAnimations.instance.PlayAnimation("Palm Return");
    }
}
