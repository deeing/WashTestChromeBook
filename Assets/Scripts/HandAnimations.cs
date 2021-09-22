using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HandAnimations : SingletonMonoBehaviour<HandAnimations>
{
    [SerializeField] private Animator anim;

    [Range(0,1)]
    private float animationTime = 0f;
    [Range(0, 1)]
    private float crossFadeTime = 0f;
    private float crossFadeLimit = 1f; // WHY ISNT THIS ALWAYS ONE?

    protected override void Awake()
    {
        if (!InitializeSingleton(this))
        {
            return;
        }
        Reset();
        anim.speed = 0;
    }

    public void Reset()
    {
        animationTime = 0f;
        crossFadeTime = 0f;
        anim.speed = 0;
    }

    public void Stop()
    {
        anim.enabled = false;
    }

    public void PlayAnimationStep(string animationName, float animationIncrease)
    {
        //anim.speed = 0;
        animationTime += animationIncrease;
        anim.Play(animationName, 0, animationTime);
    }

    public void CrossFade(string nextAnim, float fadeTime)
    {
        anim.speed = 1f;
        anim.CrossFade(nextAnim, fadeTime);
    }

    public void CrossFadeStep(string nextAnim, float fadeTime, float timeOffsetIncrease, float crossFadeLimit)
    {
        this.crossFadeLimit = crossFadeLimit;
        anim.speed = 0f;
        crossFadeTime += timeOffsetIncrease;
        anim.CrossFade(nextAnim, fadeTime, -1, float.NegativeInfinity, crossFadeTime);
    }

    public bool IsAnimationFinished()
    {
        return animationTime >= 1f;
    }

    public bool IsCrossFadeFinished()
    {
        return crossFadeTime >= crossFadeLimit;
    }

}
