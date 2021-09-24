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

    private string transitionAnimation = "";
    private bool finishedTransition = false;
    private Coroutine transitionCoroutine = null;

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
    }

    public void Stop()
    {
        anim.enabled = false;
    }

    public void TransitionPlay(string animationName, float fadeTime, float animationIncrease)
    {
        if (transitionAnimation == animationName)
        {
            if (finishedTransition)
            {
                PlayAnimationStep(animationName, animationIncrease);
                transitionCoroutine = null;
            }
        } else
        {
            CrossFade(animationName, fadeTime);
            crossFadeTime = 0;
            finishedTransition = false;
            if (transitionCoroutine != null)
            {
                StopCoroutine(transitionCoroutine);
            }
            transitionCoroutine = StartCoroutine(FinishTransition(fadeTime));
        }
    }

    private IEnumerator FinishTransition(float transitionTime)
    {
        yield return new WaitForSeconds(transitionTime);
        finishedTransition = true;
    }

    public void PlayAnimationStep(string animationName, float animationIncrease)
    {
        anim.speed = 0;
        animationTime += animationIncrease;
        animationTime = Mathf.Clamp(animationTime, 0f, animationTime);
        anim.Play(animationName, 0, animationTime);
    }

    public void CrossFade(string nextAnim, float fadeTime)
    {
        transitionAnimation = nextAnim;
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

    public bool IsPlayingAnimation(string animationName)
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName(animationName);
    }

}
