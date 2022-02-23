using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeviceIdButton : MonoBehaviour
{
    [SerializeField]
    private int numPressThreshold = 10;
    [SerializeField]
    private TMP_Text deviceIdDisplay;
    [SerializeField]
    private GameObject deviceIdEditMenu;
    [SerializeField]
    private GameObject passwordMenu;
    [SerializeField]
    private TMP_InputField passwordInput;
    [SerializeField]
    private TMP_InputField deviceIdInput;

    private int numPressed = 0;
    private string password = "sleep";

    private void Start()
    {
        deviceIdDisplay.text = AllScenes.instance.deviceId;
    }

    public void PressDeviceIdButton()
    {
        numPressed++;

        if (numPressed >= numPressThreshold)
        {
            TogglePasswordInput(true);
            numPressed = 0;
            passwordInput.text = "";
        }
    }

    private void TogglePasswordInput(bool status)
    {
        passwordMenu.SetActive(status);
    }

    public void CheckPassword()
    {
        if (passwordInput.text == password)
        {
            ToggleDeviceIdEdit(true);
        }
        TogglePasswordInput(false);
    }

    private void ToggleDeviceIdEdit(bool status)
    {
        deviceIdEditMenu.SetActive(status);
    }

    public void SubmitDeviceId()
    {
        ToggleDeviceIdEdit(false);

        deviceIdDisplay.text = deviceIdInput.text;

        AllScenes.instance.SaveDeviceId(deviceIdInput.text);
    }
}
