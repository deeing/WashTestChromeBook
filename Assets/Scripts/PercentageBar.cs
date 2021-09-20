using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PercentageBar : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Object that represents the filled part of the percentage bar.")]
    private Transform barForeground;

    public void UpdatePercentage(float percentage)
    {
        Vector3 scale = barForeground.localScale;
        barForeground.localScale = new Vector3(scale.x, percentage, scale.z);
    }
}
