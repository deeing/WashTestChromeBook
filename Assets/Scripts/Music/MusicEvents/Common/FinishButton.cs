using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishButton : MonoBehaviour
{
    [SerializeField]
    private GameObject confirmationMenu;

    private bool isShowingConfirmation = false;

    private void Start()
    {

    }

    public void ToggleConfirmation(bool status)
    {
        isShowingConfirmation = status;
        confirmationMenu.SetActive(status);
    }

    public void ToggleConfirmation()
    {
        isShowingConfirmation = !isShowingConfirmation;
        ToggleConfirmation(isShowingConfirmation);
    }
}
