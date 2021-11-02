using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RhythmTool;

public class VerticalParallelDrag : RhythymInput 
{
    [SerializeField]
    private MovingButton leftButton;

    [SerializeField]
    private MovingButton rightButton;

    public override void HandleBeat(Beat currentBeat, Beat nextBeat)
    {
        leftButton.Move(nextBeat.timestamp - currentBeat.timestamp);
        rightButton.Move(nextBeat.timestamp - currentBeat.timestamp);
    }
}
