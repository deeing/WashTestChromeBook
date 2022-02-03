using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncedAnimation : MonoBehaviour
{
    [SerializeField]
    private string animationName;
    [SerializeField]
    Animator anim;

    private void Update()
    {
        PlaySyncedAnimation(animationName);
    }

    // plays animation synced to the time of Hand animations
    public void PlaySyncedAnimation(string animationName)
    {
        float animTime = HandAnimations.instance.GetAnimationTime();

        anim.speed = 0;
        anim.Play(animationName, 0, animTime);
    }
}
