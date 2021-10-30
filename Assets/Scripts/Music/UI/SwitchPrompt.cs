using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class SwitchPrompt : MonoBehaviour
{
    [SerializeField]
    private Transform switchTargetPosition;
    [SerializeField]
    private TMP_Text promptText;
    [SerializeField]
    private GameObject promptContainer;

    private Transform thisTransform;
    private Vector3 originalPosition;

    private void Awake()
    {
        thisTransform = transform;
        originalPosition = thisTransform.position;
    }

    public void ShowPrompt(string text, float time)
    {
        thisTransform.position = originalPosition;
        promptContainer.SetActive(true);
        promptText.text = text;
        thisTransform.DOMove(switchTargetPosition.position, time).SetEase(Ease.Linear);
    }

    public void HidePrompt()
    {
        thisTransform.position = originalPosition;
        promptContainer.SetActive(false);
    }

    private void OnDestroy()
    {
        thisTransform.DOKill();
    }
}
