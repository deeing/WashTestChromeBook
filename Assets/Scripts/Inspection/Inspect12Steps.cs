using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inspect12Steps : MonoBehaviour
{
    public InspectionStepButton currentButton { get; private set; }

    public void SetCurrentButton(InspectionStepButton currButton)
    {
        if (currentButton)
        {
            currentButton.ToggleGermMapHighlight(false);
        }

        currentButton = currButton;
    }
}
