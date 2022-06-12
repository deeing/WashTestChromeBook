using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Inspect12Steps : MonoBehaviour
{
    [SerializeField]
    private GameObject buttonsContainer;
    [SerializeField]
    private InspectionStepButton[] buttons;

    public InspectionStepButton currentButton { get; private set; }

    public void SetCurrentButton(InspectionStepButton currButton)
    {
        if (currentButton)
        {
            currentButton.ToggleGermMapHighlight(false);
        }

        currentButton = currButton;
    }

    public void Toggle(bool status)
    {
        buttonsContainer.SetActive(status);
    }

    public void DisableButtons()
    {
        foreach(InspectionStepButton button in buttons)
        {
            button.DisableButton();
        }
    }
}
