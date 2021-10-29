using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SwitchPrompt : MonoBehaviour
{
    [SerializeField]
    private Transform switchTargetPosition;

    private Transform thisTransform;
    private Vector3 originalPosition;

    private void Awake()
    {
        thisTransform = transform;
        originalPosition = thisTransform.position;
    }

    public void ShowPrompt(string Text, float time)
    {
        thisTransform.DOMove(switchTargetPosition.position, time).SetEase(Ease.Linear);
    }

    public void HidePrompt()
    {
        thisTransform.position = originalPosition;
    }
}
