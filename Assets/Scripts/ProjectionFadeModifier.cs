using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LlockhamIndustries.Decals;

public class ProjectionFadeModifier : Fade
{
    [SerializeField]
    private bool playOnAwake = false;

    protected override void Begin()
    {
        if (playOnAwake)
        {
            base.Begin();
        }
    }

    public void SetAlpha(float alpha)
    {
        SetAlpha(projection, alpha);
    }
}
