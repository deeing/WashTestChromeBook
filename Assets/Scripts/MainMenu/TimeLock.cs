using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TimeLock : MonoBehaviour
{
    [SerializeField]
    private int daysUntilLock;
    [SerializeField]
    private GameObject startButton;
    [SerializeField]
    private FreeTrialDialog freeTrialDialog;

    private DateTime dayStarted;
    private TimeSpan timePassed;

    private const string DAY_STARTED_KEY = "WASH_DAY_STARTED";

    private void Awake()
    {
        Setup();
    }

    private void Setup()
    {
        dayStarted = GetDayStarted();
        timePassed = DateTime.Now - dayStarted;
        freeTrialDialog.UpdateText(GetTitleText());
        freeTrialDialog.Toggle(true);

        CheckExpired();
    }

    private DateTime GetDayStarted()
    {
        string dayStartedString = PlayerPrefs.GetString(DAY_STARTED_KEY, null);

        if (string.IsNullOrEmpty(dayStartedString))
        {
            dayStartedString = DateTime.Now.ToString();
            PlayerPrefs.SetString(DAY_STARTED_KEY, dayStartedString);
        }

        return DateTime.Parse(dayStartedString);
    }

    private string GetTitleText()
    {
        return "This is a free trial that ends in " + (daysUntilLock - timePassed.TotalSeconds).ToString("F2") + " seconds.";
    }

    private void CheckExpired()
    {
        if (timePassed.TotalSeconds > daysUntilLock)
        {
            Debug.Log("Locking");
            startButton.SetActive(false);
            freeTrialDialog.UpdateText("Trial has expired!");
        }
    }

    public void Refresh()
    {
        PlayerPrefs.SetString(DAY_STARTED_KEY, null);
        Setup();
    }
}
