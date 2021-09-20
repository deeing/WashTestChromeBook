using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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

    private RectTransform rect;
    private WaitForSeconds startWait;

    private void Awake()
    {
        rect = (RectTransform)transform;
        if (showOnStart)
        {
            startWait = new WaitForSeconds(startDelayTime);
            StartCoroutine(DelayedStart());
        }
    }

    private IEnumerator DelayedStart()
    {
        yield return startWait;
        Show();

    }

    public void Show()
    {
        rect.DOAnchorPosY(targetY, transitionTime);
    }

}
