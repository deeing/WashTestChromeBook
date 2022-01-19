using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RhythmTool;
using Lean.Touch;

public abstract class RhythmInput : MonoBehaviour
{
    protected float microTurotialWaitTime = 3f;

    private int beatCounter = 0;
    private int beatsPerinputPeriod = 0;
    private MusicPlayerEvent registeredEvent;
    private WaitForSeconds microTutorialWait;
    private Coroutine microTutorialCoroutine;

    protected virtual void Awake()
    {
        beatsPerinputPeriod = MusicManager.instance.GetBeatsPerInputPeriod();
        microTutorialWait = new WaitForSeconds(microTurotialWaitTime);
    }

    public void Update()
    {
        DoInput(GetCurrentInputStatus());
        HandleMicroTutorial();
    }

    public void Toggle(bool status)
    {
        gameObject.SetActive(status);
        beatCounter = 0;
    }

    private bool IsOffBeat()
    {
        return beatCounter != 0;
    }

    public virtual void DoBeat(Beat currentBeat, Beat nextBeat)
    {
        if (!IsOffBeat())
        {
            HandleBeat(currentBeat, nextBeat);
        }
        IncrementBeatCounter();
    }

    private void IncrementBeatCounter()
    {
        beatCounter++;

        if (beatCounter == GetBeatsPerInputPeriod())
        {
            beatCounter = 0;
        }
    }

    public virtual int GetBeatsPerInputPeriod()
    {
        return beatsPerinputPeriod;
    }

    public void RegisterWashEvent(MusicPlayerEvent musicPlayerEvent)
    {
        registeredEvent = musicPlayerEvent;
    }

    public void DoInput(RhythmInputStatus status)
    {
        if (registeredEvent)
        {
            Debug.Log(status);
            registeredEvent.OnInput(status);
        }
    }

    public abstract RhythmInputStatus GetCurrentInputStatus();
    public abstract void HandleBeat(Beat currentBeat, Beat nextBeat);

    public abstract void SetInput(RhythmInputStatus status, bool isLeftInput);

    public void DoInputMiss(bool isLeftInput)
    {
        SetInput(RhythmInputStatus.Miss, isLeftInput);
    }

    public void DoInputGood(bool isLeftInput)
    {
        SetInput(RhythmInputStatus.Good, isLeftInput);
    }

    public void DoInputGreat(bool isLeftInput)
    {
        SetInput(RhythmInputStatus.Great, isLeftInput);
    }

    public void DoInputPerfect(bool isLeftInput)
    {
        SetInput(RhythmInputStatus.Perfect, isLeftInput);
    }
    private void HandleMicroTutorial()
    {
        // only proceed if user is note touching
        List<LeanFinger> fingers = LeanTouch.GetFingers(false, false, 1);
        if (fingers == null || fingers.Count == 0)
        {
            if (microTutorialCoroutine == null)
            {
                microTutorialCoroutine = StartCoroutine(MicroTutorial());
            }
        }
        else
        {
            if (microTutorialCoroutine != null)
            {
                StopCoroutine(microTutorialCoroutine);
                microTutorialCoroutine = null;
            }
            StopMicroTutorial();
        }
    }

    private IEnumerator MicroTutorial()
    {
        yield return microTutorialWait;
        DoMicroTutorial();
    }

    protected virtual void DoMicroTutorial()
    {
        // optionally implemetned
    }

    protected virtual void StopMicroTutorial()
    {
        // optionally implemetned
    }
}
