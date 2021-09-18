using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HandAnimations : MonoBehaviour
{
    [SerializeField] private Animator anim;

    [SerializeField, Range(0,1)]
    private float animationTime = 0f;

    private string currentAnim = "";

    public static float scrubVelocity = 0f;

    private void Start()
    {
        anim.speed = 0;
    }

    public bool Pray(float animationIncrease)
    {
        return PlayAnimation("Pray", animationIncrease);
    }

    public void Scrub(float animationIncrease)
    {
        scrubVelocity = animationIncrease;
        PlayAnimation("Scrub", animationIncrease);
    }

    // returns whether or not we have "finished" animation
    private bool PlayAnimation(string animationName, float animationIncrease)
    {
        if (currentAnim != animationName)
        {
            currentAnim = animationName;
            animationTime = 0f;
        } else
        {
            animationTime += animationIncrease;
        }

        anim.Play(animationName, 0, animationTime);

        return animationTime < 1f;
    }

    public void CrossFade(string nextAnim,   float fadeTime)
    {
        anim.speed = 1f;
        anim.CrossFade(nextAnim, fadeTime);
    }
}
