using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderwaterScene : MonoBehaviour
{
    [SerializeField]
    private Color fogColor;
    [SerializeField]
    private float fogDensity;

    private void Start()
    {
        RenderSettings.fog = true;
        SetFog();
    }

    private void SetFog()
    {
        RenderSettings.fogColor = fogColor;
        RenderSettings.fogDensity = fogDensity;
    }


}
