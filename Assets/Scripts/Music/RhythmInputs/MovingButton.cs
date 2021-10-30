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
        transform.DOKill();
    }

    public void Move(float time)
    {
        if (moveToTarget)
        {
            transform.DOMove(targetPoint.position, time, true);
        } else
        {
            transform.DOMove(startingPoint.position, time, true);
        }
        moveToTarget = !moveToTarget;
    }

    private void OnDestroy()
    {
        transform.DOKill();
    }
}
