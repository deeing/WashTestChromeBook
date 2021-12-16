using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishScrubButton : MonoBehaviour
{
    public void FinishScrub()
    {
        MusicManager.instance.ReturnToNonLinearNeutral();
    }
}
