using RhythmTool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonLinearAnimationSwitch : MusicSwitchEvent
{
    [SerializeField]
    private float animationTime = 1f;
    [SerializeField]
    private float pointsForEvent = 0f;

    public float pointsEarned { get; private set; } = 0f;
    private WaitForSeconds animationWait;

    public override void SetupEvent()
    {
        animationWait = new WaitForSeconds(animationTime);
        HandAnimations.instance.Reset();
        hasFinished = false;
        StartEvent();
    }

    private void StartEvent()
    {
        //  HandAnimations.instance.CameraAnimation(cameraAnimationName, animationTime);
        StartCoroutine(FinishAnimation());
        HandAnimations.instance.PlayAnimation(animationName);
        HandleScore();
    }

    private void HandleScore()
    {
        if (pointsEarned >= pointsForEvent)
        {
            return;
        }
        pointsEarned += pointsForEvent;
        IncreaseEventScore(pointsForEvent);
        MenuManager.instance.IncreaseTotalScore(pointsForEvent);
    }

    private  IEnumerator FinishAnimation()
    {
        yield return animationWait;
        EndAnimationEvent();
    }

    protected virtual void EndAnimationEvent()
    {
        hasFinished = true;
    }

    public override void DoEvent(Beat beat)
    {
        // nothing
    }

}
