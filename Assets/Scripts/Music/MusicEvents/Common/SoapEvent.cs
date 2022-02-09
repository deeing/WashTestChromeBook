using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoapEvent : MusicScrubEvent
{
    [SerializeField]
    private SyncedAnimation syncedAnimation;

    private float currentSoapScore = 0f;
    private float maxSoapScore;

    public override void SetupEvent()
    {
        base.SetupEvent();

        if (syncedAnimation)
        {
            syncedAnimation.enabled = true;
        }

        maxSoapScore = MusicManager.instance.gameSettings.maxSoapPoints;
    }

    public override void EndAnimation()
    {
        if (syncedAnimation)
        {
            syncedAnimation.enabled = false;
        }
        base.HardSwitchEnd();
    }

    public override void HardSwitchEnd()
    {
        if (syncedAnimation)
        {
            syncedAnimation.enabled = false;
        }
        base.HardSwitchEnd();
    }

    protected override void HandleScore()
    {
        if (currentSoapScore >= maxSoapScore)
        {
            return;
        }

        float scoreAmount = MusicManager.instance.gameSettings.GetPointsForInputStatus(latestRhythmInputStatus);
        currentSoapScore += scoreAmount;
        IncreaseEventScore(scoreAmount);
        MenuManager.instance.IncreaseTotalScore(scoreAmount);
        MenuManager.instance.ShowRhythmStatus(latestRhythmInputStatus);
    }

}
