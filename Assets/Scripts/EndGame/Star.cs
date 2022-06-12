using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Star : MonoBehaviour
{
    [SerializeField]
    private RectTransform starFill;

    public float starPercentage { get; private set; } = 0f;

    private void Awake()
    {
        SetStarFill(0f, 0f);
    }

    public DG.Tweening.Core.TweenerCore<Vector3, Vector3, DG.Tweening.Plugins.Options.VectorOptions> SetStarFill(float percentage, float fillTime)
    {
        starPercentage = percentage;
        return starFill.DOScaleX(percentage, fillTime);
    }
}
