using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TutorialText : MonoBehaviour
{
    [SerializeField]
    private float targetY = -100f;
    [SerializeField]
    private float transitionTime = 2f;

    private RectTransform rect;
    void Start()
    {
        rect = (RectTransform)transform;
        StartCoroutine(SlideIn());
    }

    private IEnumerator SlideIn()
    {
        yield return new WaitForSeconds(1f);
        rect.DOAnchorPosY(targetY, transitionTime);
    }

}
