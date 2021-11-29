using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wash.Utilities;

public abstract class PlayerEvent : WashEvent
{
    [SerializeField]
    protected float sensitivity = .005f;
    [SerializeField]
    [Tooltip("Tutorial that should show when this event starts")]
    private GameObject tutorialObject;
    [SerializeField]
    [Tooltip("Whether or not we should time the completion of the current event")]
    private bool shouldTimeEvent = false;
    [SerializeField]
    [Tooltip("Whether or not we should have this as a checklist item")]
    private bool shouldBeChecklistEvent = false;
    [SerializeField]
    [Tooltip("Name of sound clip that plays at the start of the event")]
    private string eventAudio;

    // How long in seconcds it takes for player to become impatient
    protected float impatienceThreshold = 8f;
    // Duration in seconds for the impatience animation to finish and return to idle (not used for normalized animation time)
    protected float impatienceAnimDuration = 3f;
    protected WaitForSeconds impatienceWait;
    // how long user has been idle
    protected float impatienceTimer = 0f;
    protected bool isImpatient = false;

    private float eventStart = 0;

    public override void SetupEvent()
    {
        // make it so not all of the children need an empty function
        impatienceWait = new WaitForSeconds(impatienceAnimDuration);

        if (!string.IsNullOrWhiteSpace(eventAudio))
        {
            AudioManager.instance.PlayOneShot(eventAudio);
        }
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


    // handles when changing an event without "ending"
    public override void ChangeEvent()
    {
        if (tutorialObject)
        {
            tutorialObject.SetActive(false);
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

        if (shouldBeChecklistEvent)
        {
            MenuManager.instance.CheckListCheckOffItem();
        }
    }



    public abstract PlayerEventType GetEventType();

    public string GetEventName()
    {
        return GetEventType().GetDescription();
    }

    public bool GetShouldBeChecklistEvent()
    {
        return shouldBeChecklistEvent;
    }

    protected void ResetImpatienceTimer()
    {
        impatienceTimer = 0f;
    }

    protected void IncrementImpatienceTimer(float time)
    {
        impatienceTimer += time;
    }

    protected virtual string GetImpatienceAnimationName()
    {
        return null;
    }

    protected void HandleImpatience()
    {
        HandAnimations.instance.TransitionPlay(GetImpatienceAnimationName(), .2f);
        isImpatient = true;
        StartCoroutine(ImpatienceEnd());
    }

    public IEnumerator ImpatienceEnd()
    {
        yield return impatienceWait;
        isImpatient = false;
        impatienceTimer = 0f;
    }

    protected bool HasImpatienceAnim()
    {
        return GetImpatienceAnimationName() != null;
    }

    public abstract void ReturnFromInspect();
}
