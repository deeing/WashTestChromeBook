using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectionScaleModifier : MonoBehaviour
{
    private Transform thisTransform;
    private Vector3 originalScale;

    private void Start()
    {
        thisTransform = transform;
        originalScale = thisTransform.localScale;
    }

    public void SetSize(float sizePercentage)
    {
        thisTransform.localScale = Vector3.Lerp(Vector3.zero, originalScale, sizePercentage);
    }
}
