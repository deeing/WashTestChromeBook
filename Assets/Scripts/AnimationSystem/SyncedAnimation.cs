using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SyncedAnimation : MonoBehaviour
{
    [SerializeField]
    private string animationName;
    [SerializeField]
    Animator anim;
    [SerializeField]
    SyncedAnimationEvent syncedEvent;

    private void Update()
    {
        float animTime = HandAnimations.instance.GetAnimationTime();
        PlaySyncedAnimation(animationName, animTime);
        syncedEvent.Invoke(animTime);
    }

    // plays animation synced to the time of Hand animations
    public void PlaySyncedAnimation(string animationName, float animTime)
    {
        anim.speed = 0;
        anim.Play(animationName, 0, animTime);
    }
}
