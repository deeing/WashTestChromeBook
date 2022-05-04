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
        return "This is a free trial that ends in " + Mathf.CeilToInt((daysUntilLock - ((float)timePassed.TotalDays))).ToString() + " days.";
    }

    private void CheckExpired()
    {
        if (timePassed.TotalDays > daysUntilLock)
        {
            startButton.SetActive(false);
            freeTrialDialog.UpdateText("Trial has expired!");
        }
    }

    public void Refresh()
    {
        if (!gameObject.activeInHierarchy)
        {
            return;
        }
        PlayerPrefs.SetString(DAY_STARTED_KEY, null);
        Setup();
    }
}
