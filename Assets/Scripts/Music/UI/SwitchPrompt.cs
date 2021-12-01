using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class SwitchPrompt : MonoBehaviour
{
    [SerializeField]
    private Transform targetPosition;
    [SerializeField]
    private TMP_Text promptText;
    [SerializeField]
    private GameObject promptContainer;
    [SerializeField]
    private Transform goodDistancePos;
    [SerializeField]
    private Transform greatDistancePos;
    [SerializeField]
    private Transform perfectDistancePos;

    private Transform thisTransform;
    private Vector3 originalPosition;

    private float goodDistance;
    private float greatDistance;
    private float perfectDistance;

    private void Awake()
    {
        thisTransform = transform;
        originalPosition = thisTransform.position;

        CalculateDistances();
    }

    private void CalculateDistances()
    {
        goodDistance = Vector3.Distance(originalPosition, goodDistancePos.position);
        greatDistance = Vector3.Distance(originalPosition, greatDistancePos.position);
        perfectDistance = Vector3.Distance(originalPosition, perfectDistancePos.position);
    }

    public void ShowPrompt(string text, float time)
    {
        thisTransform.position = originalPosition;
        promptContainer.SetActive(true);
        promptText.text = text;
        thisTransform.DOMove(targetPosition.position, time).SetEase(Ease.Linear);
    }

    public void HidePrompt()
    {
        thisTransform.position = originalPosition;
        promptContainer.SetActive(false);
    }

    public RhythmInputStatus GetPromptDistanceStatus()
    {
        float distanceToTarget = Vector3.Distance(thisTransform.position, targetPosition.position);

        if (distanceToTarget <= perfectDistance)
        {
            return RhythmInputStatus.Perfect;
        } else if (distanceToTarget <= greatDistance)
        {
            return RhythmInputStatus.Great;
        } else if (distanceToTarget <= goodDistance)
        {
            return RhythmInputStatus.Good;
        } else
        {
            return RhythmInputStatus.Miss;
        }
    }

    private void OnDestroy()
    {
        thisTransform.DOKill();
    }
}
