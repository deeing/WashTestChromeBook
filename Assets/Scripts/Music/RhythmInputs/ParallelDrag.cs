using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RhythmTool;

public class ParallelDrag : RhythmInput 
{
    [SerializeField]
    private MovingButton leftButton;
    [SerializeField]
    private MovingButton rightButton;
    [SerializeField]
    private bool debugFreeze = false;

    // suceeds if one of the fingers is touching
    private bool forgivingInput = true;

    private RhythmInputStatus leftStatus = RhythmInputStatus.Miss;
    private RhythmInputStatus rightStatus = RhythmInputStatus.Miss;

    public void Update()
    {
        DoInput(GetCurrentInputStatus());
    }

    public override void HandleBeat(Beat currentBeat, Beat nextBeat)
    {
        if (debugFreeze)
        {
            return;
        }
        leftButton.Move(nextBeat.timestamp - currentBeat.timestamp);
        rightButton.Move(nextBeat.timestamp - currentBeat.timestamp);
    }

    public RhythmInputStatus GetCurrentInputStatus()
    {
        RhythmInputStatus chosenStatus;

        if (forgivingInput)
        {
            // choose the greater of the two
            chosenStatus = leftStatus > rightStatus ? leftStatus : rightStatus;
        } else
        {
            // choose the weaker of the two
            chosenStatus = leftStatus < rightStatus ? leftStatus : rightStatus;
        }

      /*  Debug.Log("left: " + leftStatus);
        Debug.Log("right: " + rightStatus);
        Debug.Log("Chosen: " + chosenStatus);*/

        return chosenStatus;
    }

    public void SetInput(RhythmInputStatus status, bool isLeftInput)
    {
        if (isLeftInput)
        {
            leftStatus = status;
        } else
        {
            rightStatus = status;
        }
    }

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

}
