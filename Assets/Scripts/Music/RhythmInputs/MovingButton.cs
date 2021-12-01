using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MovingButton : MonoBehaviour
{
    [SerializeField]
    private RectTransform startingPoint;
    [SerializeField]
    private RectTransform targetPoint;

    private RectTransform thisTransform;
    private bool moveToTarget = true;

    private float speedFactor = 3; // make this a game setting?

    private void Awake()
    {
        thisTransform = (RectTransform)transform;
        transform.DOKill();
    }

    public void Move(float time)
    {
        if (moveToTarget)
        {
            Sequence moveButton = DOTween.Sequence();
            moveButton.Append(transform.DOMove(targetPoint.position, time / speedFactor, true).SetEase(Ease.OutBack));
            //transform.DOMove(targetPoint.position, time, true).SetEase(Ease.Linear);
        }
        else
        {
            Sequence moveButton = DOTween.Sequence();
            moveButton.Append(transform.DOMove(startingPoint.position, time / speedFactor, true).SetEase(Ease.OutBack));
            //transform.DOMove(startingPoint.position, time, true).SetEase(Ease.Linear);
        }
        moveToTarget = !moveToTarget;
    }

    private void OnDestroy()
    {
        transform.DOKill();
    }
}
