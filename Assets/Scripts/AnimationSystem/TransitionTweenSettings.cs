using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TransitionTweenSettings", menuName = "ScriptableObjects/TransitionTweenSettings", order = 1)]
public class TransitionTweenSettings : ScriptableObject
{
    public TransitionTweenMapping[] transitionTweenMappings;

    public TweenAnimation[] FindTweenAnimations(PlayerEventType start, PlayerEventType target)
    {
        foreach(TransitionTweenMapping tweenMapping in transitionTweenMappings)
        {
            if (tweenMapping.start == start && tweenMapping.target == target)
            {
                return tweenMapping.tweenAnimations;
            }
        }

        return null;
    }
}
