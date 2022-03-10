using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Wash.Utilities;

public class RadialProgress : MonoBehaviour
{
    [SerializeField]
    private Image loading;

    public void SetPercent(float percent)
    {
        percent.Clamp(0f, 1f);
        loading.fillAmount = percent;
    }
}
