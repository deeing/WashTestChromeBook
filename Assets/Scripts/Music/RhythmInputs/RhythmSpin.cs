using RhythmTool;
using UnityEngine;
using DG.Tweening;

public class RhythmSpin : RhythmInput
{
    [SerializeField]
    private RectTransform button;
    [SerializeField]
    private RectTransform centerPoint;
    [SerializeField]
    private Direction direction = Direction.Clockwise;
    [SerializeField]
    private int beatMultiplier = 2;
    [SerializeField]
    private Ease easingPattern = Ease.OutBack;
    [SerializeField]
    private StatusDisplay statusDisplay;

    [SerializeField]
    private bool longBeat = false;

    private RhythmInputStatus rhythmStatus = RhythmInputStatus.Miss;

    protected enum Direction
    {
        Clockwise,
        CounterClockwise
    }

    private void OnDisable()
    {
        statusDisplay.HideStatusDisplay();
    }

    public override void HandleBeat(Beat currentBeat, Beat nextBeat)
    {
        // TODO: Clean this up if we actually use this
        MusicManager.instance.GetNextBeat(nextBeat);
        Beat trueNextBeat = nextBeat;

        if (longBeat)
        {
            trueNextBeat = MusicManager.instance.GetNextBeat(nextBeat);
        }


        float duration = trueNextBeat.timestamp - currentBeat.timestamp;
        button.DOShapeCircle(centerPoint.anchoredPosition, GetDirection(), duration).SetEase(easingPattern);
        statusDisplay.ShowStatusDisplay(GetCurrentInputStatus());
    }

    private float GetDirection()
    {
        return direction == Direction.Clockwise ? 360f : -360f;
    }

    public override void SetInput(RhythmInputStatus status, bool isLeftInput)
    {
        rhythmStatus = status;
    }

    public override RhythmInputStatus GetCurrentInputStatus()
    {
        return rhythmStatus;
    }

    public override int GetBeatsPerInputPeriod()
    {
        return base.GetBeatsPerInputPeriod() * beatMultiplier;
    }
}
