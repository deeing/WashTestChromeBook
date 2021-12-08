using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonLinearOption : MonoBehaviour
{
    [SerializeField]
    MusicSwitchEvent musicSwitchEvent;

    public void ChooseOption()
    {
        MusicManager.instance.SetNonLinearAction(musicSwitchEvent);
    }
}
