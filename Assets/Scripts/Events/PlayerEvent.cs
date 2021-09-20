using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerEvent : WashEvent
{
    [SerializeField]
    [Tooltip("Tutorial that should show when this event starts")]
    private GameObject tutorialObject;

    public override void SetupEvent()
    {
        // make start optional for player events to reduce dead code
    }

    public override void StartEvent()
    {
        if (tutorialObject)
        {
            tutorialObject.SetActive(true);
        }
    }

    public override void EndEvent()
    {
        Debug.Log("TESTING");
        if (tutorialObject)
        {
            tutorialObject.SetActive(false);
        }
    }
}
