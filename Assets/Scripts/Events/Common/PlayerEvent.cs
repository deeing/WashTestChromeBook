using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wash.Utilities;

public abstract class PlayerEvent : WashEvent, AdjustableSensitivity
{
    [SerializeField]
    [Tooltip("Tutorial that should show when this event starts")]
    private GameObject tutorialObject;
    [SerializeField]
    [Tooltip("Whether or not we should time the completion of the current event")]
    private bool shouldTimeEvent = false;
    [SerializeField]
    [Tooltip("How much the player wants to adjust the sensitivity of this event's input")]
    private float sensitivityAdjustment = 1f;

    private float eventStart = 0;

    public override void SetupEvent()
    {
        // make it so not all of the children need an empty function
    }

    public override void StartEvent()
    {
        if (tutorialObject)
        {
            tutorialObject.SetActive(true);
        }

        if (shouldTimeEvent)
        {
            eventStart = Time.time;
        }
    }

    public override void EndEvent()
    {
        EffectsManager.instance.Celebrate();
        if (tutorialObject)
        {
            tutorialObject.SetActive(false);
        }

        if (shouldTimeEvent)
        {
            float eventEnd = Time.time;
            WashEventManager.instance.AddTimeRecording(GetEventType().GetDescription(), eventEnd - eventStart);
        }
    }

    public abstract PlayerEventType GetEventType();

    public void SetSensitivityAdjustment(float sensitivityAdjustment)
    {
        this.sensitivityAdjustment = sensitivityAdjustment;
    }

    public float GetSensitivityAdjustment()
    {
        return sensitivityAdjustment;
    }

    public string GetEventName()
    {
        return GetEventType().GetDescription();
    }
}
