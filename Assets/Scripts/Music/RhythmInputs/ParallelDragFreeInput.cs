using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RhythmTool;
using TMPro;

public class ParallelDragFreeInput : RhythmInput 
{
    [SerializeField]
    private SlideableButton leftButton;
    [SerializeField]
    private SlideableButton rightButton;
    [SerializeField]
    private MovingButton leftGuide;
    [SerializeField]
    private MovingButton rightGuide;
    [SerializeField]
    private TMP_Text debugText;

    // suceeds if one of the fingers is touching
    private bool forgivingInput = true;

    private RhythmInputStatus leftStatus = RhythmInputStatus.Miss;
    private RhythmInputStatus rightStatus = RhythmInputStatus.Miss;

    private bool targetingEnd = true;

    public override void HandleBeat(Beat currentBeat, Beat nextBeat)
    {
        HandleStatus();
        MoveGuides(nextBeat, currentBeat);
    }

    private void HandleStatus()
    {
        float leftPercentage = leftButton.GetPercentage();
        float rightPercentage = rightButton.GetPercentage();

        if (targetingEnd)
        {
            // it means the previous score was at the start so score based on zero
            leftStatus = GetStatusByPercentage(leftPercentage, 0f);
            rightStatus = GetStatusByPercentage(rightPercentage, 0f);
            targetingEnd = false;
            //Debug.Log("Left percentage: " + leftPercentage + " aim was " + 1f + " status is : " + leftStatus);
            debugText.text = "Aim: 0f, status: " + leftStatus + ", percent: " + leftPercentage;
        }
        else
        {
            leftStatus = GetStatusByPercentage(leftPercentage, 1f);
            rightStatus = GetStatusByPercentage(rightPercentage, 1f);
            targetingEnd = true;
            //Debug.Log("Left percentage: " + leftPercentage + " aim was " + 0f + " status is : " + leftStatus);
            debugText.text = "Aim: 1f, status: " + leftStatus + ", percent: " + leftPercentage;
        }
    }

    private void MoveGuides(Beat nextBeat, Beat currentBeat)
    {
        leftGuide.Move(nextBeat.timestamp - currentBeat.timestamp);
        rightGuide.Move(nextBeat.timestamp - currentBeat.timestamp);
    }

    private RhythmInputStatus GetStatusByPercentage(float percentage, float targetPercentage) 
    {
        float difference = Mathf.Abs(percentage - targetPercentage);
        
        if (difference <= .1f)
        {
            return RhythmInputStatus.Perfect;
        }
        else if (difference <= .3f)
        {
            return RhythmInputStatus.Great;
        } 
        else if (difference <= .5f)
        {
            return RhythmInputStatus.Good;
        } 
        else
        {
            return RhythmInputStatus.Miss;
        }
    }

    public override RhythmInputStatus GetCurrentInputStatus()
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

        return chosenStatus;
    }

    public override void SetInput(RhythmInputStatus status, bool isLeftInput)
    {
        if (isLeftInput)
        {
            leftStatus = status;
        } else
        {
            rightStatus = status;
        }
    }
}
