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

    private void Awake()
    {
        thisTransform = (RectTransform)transform;
    }

    public void Move(float time)
    {
        if (moveToTarget)
        {
            thisTransform.DOMove(targetPoint.position, time, true);
        } else
        {
            thisTransform.DOMove(startingPoint.position, time, true);
        }
        moveToTarget = !moveToTarget;
    }
}
