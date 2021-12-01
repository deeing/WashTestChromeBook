using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wash.Utilities;
public class HandAnimations : SingletonMonoBehaviour<HandAnimations>
{
    [SerializeField] 
    private Animator anim;

    [Range(0,1)]
    private float animationTime = 0f;
    [Range(0, 1)]
    private float crossFadeTime = 0f;
    private float crossFadeLimit = 1f; // WHY ISNT THIS ALWAYS ONE?
    [SerializeField]
    private TransitionTweenSettings transitionTweenSettings;
    [SerializeField]
    private Animator cinemachine;

    private string crossFadeAnimation = "";
    private bool finishedTransition = false;
    private Coroutine transitionCoroutine = null;

    private int tweenCounter = 0;
    private bool isTweening = false;

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
        tweenCounter = 0;
        isTweening = false;
        crossFadeAnimation = "";
    }

    public void Resume()
    {
        anim.speed = 1f;
    }

    public void Pause()
    {
        anim.speed = 0f;
    }

    public void Stop()
    {
        anim.enabled = false;
    }

    // cross fades to an animation and starts to play it with a standard time
    public void TransitionPlay(string animationName)
    {
        TransitionPlay(animationName, .2f);
    }
    // cross fades to an animation and then starts to play it
    public void TransitionPlay(string animationName, float fadeTime)
    {
        if (IsCrossFading(animationName))
        {
            if (finishedTransition)
            {
                PlayAnimation(animationName);
                transitionCoroutine = null;
                crossFadeTime = 0;
                finishedTransition = false;
            }
        }
        else
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

    // cross fades to an animation and then starts to play it in steps
    public void TransitionPlayStep(string animationName, float fadeTime, float animationIncrease)
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
    public void TransitionPlayStep(string animationName, string transitionAnimation, float fadeTime, float animationIncrease)
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
                PlayAnimation(transitionAnimation);
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

    public void PlayAnimation(string animationName)
    {
        anim.speed = 1;
        //anim.Play(animationName, 0, animTime);
        anim.Play(animationName, 0);
    }

    public void PlayAnimationStep(string animationName, float animEnd, float animationIncrease)
    {
        anim.speed = 0;
        animationTime += animationIncrease;
        animationTime.ClampUpper(animEnd);
        anim.Play(animationName, 0, animationTime);
    }

    public bool HasAnimationReachedTime(float animStart, float animEnd)
    {
        return animStart + animationTime >= animEnd;
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

    // goes through tween motions first before finishing with final crossfade
    public void CrossFadeTweenStep(string nextAnim, TweenAnimation[] tweenAnimations, float fadeTime, float timeOffsetIncrease, float crossFadeLimit)
    {
        anim.speed = 0f;
        crossFadeTime += timeOffsetIncrease;

        if (tweenCounter < tweenAnimations.Length)
        {
            isTweening = true;
            TweenAnimation tween = tweenAnimations[tweenCounter];
            this.crossFadeLimit = tween.crossFadeLimit;
            anim.CrossFade(tween.animName, tween.duration, -1, float.NegativeInfinity, crossFadeTime);

            if (IsCrossFadeFinished())
            {
                tweenCounter++;
                crossFadeTime = 0f;
            }
        }
        else
        {
            this.crossFadeLimit = crossFadeLimit;
            isTweening = false;
            anim.CrossFade(nextAnim, fadeTime, -1, float.NegativeInfinity, crossFadeTime);
        }
    }

    public bool IsAnimationFinished()
    {
        return animationTime >= 1f;
    }

    public bool HasAnimationReached(float time)
    {
        return animationTime >= time;
    }

    public bool IsCrossFadeFinished()
    {
        return crossFadeTime >= crossFadeLimit;
    }

    public bool IsCrossFadeWithTweensFinished()
    {
        return !isTweening && IsCrossFadeFinished();
    }

    public bool IsCrossFading(string animationName)
    {
        return crossFadeAnimation == animationName;
    }

    public bool IsPlayingAnimation(string animationName)
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName(animationName);
    }

    public TweenAnimation[] FindTweenAnimations(PlayerEventType target)
    {
        PlayerEvent prevWashEvent = WashEventManager.instance.GetPrevEvent() as PlayerEvent;
        if (prevWashEvent != null)
        {
            PlayerEventType prevPlayerEventType = prevWashEvent.GetEventType();
            return transitionTweenSettings.FindTweenAnimations(prevPlayerEventType, target);
        }

        return null;
    }

    public void CameraAnimation(string animationName, float animTime)
    {
        cinemachine.Play(animationName, 0, animTime);
    }
}
