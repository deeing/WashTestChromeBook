using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public abstract class SlideInMenu : MonoBehaviour
{
    [SerializeField]
    protected float transitionTime = 2f;

    [SerializeField]
    [Tooltip("If show on start, how much time should we wait before showing")]
    private float startDelayTime = 1f;
    [SerializeField]
    private bool showOnStart;
    [SerializeField]
    private TMP_Text menuText;

    protected RectTransform rect;
    private WaitForSeconds startWait;

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
        Setup();
    }

    private IEnumerator DelayedStart()
    {
        yield return startWait;
        Show();

    }

    public abstract void Setup();

    public abstract void SetVisible(bool status);

    public void Show()
    {
        isShowing = true;
        SetVisible(true);
    }

    public void Hide()
    {
        isShowing = false;
        SetVisible(false);
    }

    public void Toggle()
    {
        isShowing = !isShowing;
        SetVisible(isShowing);
    }

    public void SetText(string text)
    {
        menuText.text = text;
    }

    private void OnDestroy()
    {
        rect.DOKill();
    }
}
