using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HandAnimations : SingletonMonoBehaviour<HandAnimations>
{
    [SerializeField] private Animator anim;

    [SerializeField, Range(0,1)]
    private float animationTime = 0f;

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
    }

    public void PlayAnimation(string animationName, float animationIncrease)
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

    public bool IsAnimationFinished()
    {
        return animationTime >= 1f;
    }

}
