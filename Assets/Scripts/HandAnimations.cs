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

    private string crossFadeAnimation = "";
    private bool finishedTransition = false;
    private Coroutine transitionCoroutine = null;

    protected override void Awake()
    {
        if (!InitializeSingleton(this))
        {
            return;
        }
        Reset();
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

    // cross fades to an animation and then starts to play it
    public void TransitionPlay(string animationName, float fadeTime, float animationIncrease)
    {
        if (IsCrossFading(animationName))
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

    // plays a transition animation and then starts to play animation
    public void TransitionPlay(string animationName, string transitionAnimation, float fadeTime, float animationIncrease)
    {
        if (finishedTransition)
        {
            PlayAnimationStep(animationName, animationIncrease);
            transitionCoroutine = null;
        }
        else
        {
            if (!IsPlayingAnimation(transitionAnimation))
            {
                PlayAnimation(transitionAnimation, fadeTime);
                finishedTransition = false;
                transitionCoroutine = StartCoroutine(FinishTransition(fadeTime));
            }
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
        anim.Play(animationName, 0, animationTime);
    }

    public void PlayAnimation(string animationName, float animTime)
    {
        anim.speed = 1;
        anim.Play(animationName, 0, animTime);
    }

    public void CrossFade(string nextAnim, float fadeTime)
    {
        // block if already fading
        if (IsCrossFading(nextAnim))
        {
            return;
        }
        crossFadeAnimation = nextAnim;
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

    public bool IsCrossFading(string animationName)
    {
        return crossFadeAnimation == animationName;
    }

    public bool IsPlayingAnimation(string animationName)
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName(animationName);
    }

}
