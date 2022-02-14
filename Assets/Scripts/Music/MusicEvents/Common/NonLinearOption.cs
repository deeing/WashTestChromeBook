using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NonLinearOption : MonoBehaviour
{
    [SerializeField]
    private MusicSwitchEvent musicSwitchEvent;

    public void ChooseOption()
    {
        MusicManager.instance.SetNonLinearAction(musicSwitchEvent);
    }
}
