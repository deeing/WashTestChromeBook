using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintManagerBase : SingletonMonoBehaviour<HintManagerBase>
{
    private HashSet<string> seenHintSet;
    public bool hintsEnabled = false;

    protected override void Awake()
    {
        if(!InitializeSingleton(this))
        {
            return;
        }

        seenHintSet = new HashSet<string>();
    }

    public void SeenHint(string hintId)
    {
        seenHintSet.Add(hintId);
    }

    public bool HasSeenHint(string hintId)
    {
        return seenHintSet.Contains(hintId);
    }
}
