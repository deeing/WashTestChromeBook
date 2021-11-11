using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaligraphyInputManager : SingletonMonoBehaviour<CaligraphyInputManager>
{
    [SerializeField]
    private CaligraphyInput caligraphyInput;

    protected override void Awake()
    {
        if (!InitializeSingleton(this))
        {
            return;
        }
    }

}
