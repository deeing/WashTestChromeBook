using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerEvent : WashEvent
{
    public override void SetupEvent()
    {
        // make start optional for player events to reduce dead code
    }

    public override void EndEvent()
    {
        // make end optional for player events to reduce dead code
    }
}
