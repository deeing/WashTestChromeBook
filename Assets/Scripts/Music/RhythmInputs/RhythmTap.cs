using RhythmTool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RhythmTap : RhythmInput
{
    [SerializeField]
    private RectTransform buttonImage;
    [SerializeField]
    private RectTransform enclosingCircle;
    [SerializeField]
    private Vector2 targetSize = new Vector2(180f, 180f);

    private Vector2 originalCircleSize;

    protected override void Awake()
    {
        base.Awake();
        //buttonSize = new Vector2(buttonImage.rect.width, buttonImage.rect.height);
        originalCircleSize = new Vector2(enclosingCircle.rect.width, enclosingCircle.rect.height);
    }

    public override int GetBeatsPerInputPeriod()
    {
        return base.GetBeatsPerInputPeriod() * 2;
    }

    public override void HandleBeat(Beat currentBeat, Beat nextBeat)
    {
        Beat trueNextBeat = MusicManager.instance.GetNextBeat(GetBeatsPerInputPeriod());
        float timeUntilBeat = trueNextBeat.timestamp - currentBeat.timestamp;

        Sequence mySequence = DOTween.Sequence();
        mySequence.AppendInterval(timeUntilBeat*.2f);
        mySequence.Append(enclosingCircle.DOSizeDelta(originalCircleSize, 0f, true));
        mySequence.Append(enclosingCircle.DOSizeDelta(targetSize, timeUntilBeat * .7f, true));
    }

    public override void SetInput(RhythmInputStatus status, bool isLeftInput)
    {
        throw new System.NotImplementedException();
    }

    public override RhythmInputStatus GetCurrentInputStatus()
    {
        throw new System.NotImplementedException();
    }
}
