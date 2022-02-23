using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllScenes : SingletonMonoBehaviour<AllScenes>
{
    public string build;

    public string deviceId = "";

    private const string DEVICE_ID_KEY = "WASH_DEVICE_ID";

    protected override void Awake()
    {
        if(!InitializeSingleton(this))
        {
            return;
        }

        GetSavedDeviceId();
    }

    private void GetSavedDeviceId()
    {
        string savedDeviceId = PlayerPrefs.GetString(DEVICE_ID_KEY);

        if (savedDeviceId != null)
        {
            deviceId = savedDeviceId;
        }
    }

    public void SaveDeviceId(string newDeviceId)
    {
        PlayerPrefs.SetString(DEVICE_ID_KEY, newDeviceId);
        deviceId = newDeviceId;
    }
}
