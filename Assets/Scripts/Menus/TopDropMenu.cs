using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class TopDropMenu : MonoBehaviour
{
    [SerializeField]
    private float targetY = -100f;
    [SerializeField]
    private float transitionTime = 2f;

    [SerializeField]
    [Tooltip("If show on start, how much time should we wait before showing")]
    private float startDelayTime = 1f;
    [SerializeField]
    private bool showOnStart;
    [SerializeField]
    private TMP_Text menuText;

    private RectTransform rect;
    private WaitForSeconds startWait;
    private float originalY;

    private bool isShowing = false;

    private void Awake()
    {
        rect = (RectTransform)transform;
        if (showOnStart)
        {
            startWait = new WaitForSeconds(startDelayTime);
            StartCoroutine(DelayedStart());
            isShowing = true;
        }
        originalY = rect.position.y;
    }

    private IEnumerator DelayedStart()
    {
        yield return startWait;
        Show();

    }

    public void Show()
    {
        rect.DOAnchorPosY(targetY, transitionTime);
        isShowing = true;
    }

    public void Hide()
    {
        rect.DOAnchorPosY(originalY, transitionTime);
        isShowing = false;
    }

    public void Toggle()
    {
        isShowing = !isShowing;
        rect.DOAnchorPosY(isShowing ? targetY : originalY, transitionTime);
    }

    public void SetText(string text)
    {
        menuText.text = text;
    }

}
