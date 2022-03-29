using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchHandsButton : MonoBehaviour
{
    public void SwitchHands()
    {
        MusicScrubEvent current = (MusicScrubEvent)MusicManager.instance.GetCurrentEvent();

        current.SwitchToOtherHand();
    }
}
