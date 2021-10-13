using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wash.Utilities;
using TMPro;
using System.Text;

public class SensitivityAdjustment : MonoBehaviour
{
    [SerializeField]
    private Transform eventsContainer;
    [SerializeField]
    private Transform sensitivityAdjustmentContainer;
    [SerializeField]
    private GameObject exportArea;
    [SerializeField]
    private TMP_InputField exportText;
    [SerializeField]
    private GameObject sensitivityInputValuePrefab;

    // map of the event name to the sensitivity value
    private Dictionary<string, float> savedAdjustmentValues = new Dictionary<string, float>();

    private const string savedAdjustementsKey = "Sensitivity Adjustment Values";
    private const string savedAdjustmentFile = "sensitivityAdjustments.json";

    private void Start()
    {
        Dictionary<string, float> previousSave;
        
        try
        {
            previousSave = (Dictionary<string, float>)ES3.Load(savedAdjustementsKey, savedAdjustmentFile);
        } catch (System.IO.FileNotFoundException e)
        {
            previousSave = null;
        }

        if (previousSave != null)
        {
            Debug.Log("previous save detected");
            savedAdjustmentValues = previousSave;
            SetupUI(true);
        }
        else
        {
            Debug.Log("no previous save detected");
            SetupUI(false);
        }
    }

    public void SetupUI(bool loadFromPrevious)
    {
        foreach (Transform evt in eventsContainer)
        {
            AdjustableSensitivity adjust = evt.GetComponent<AdjustableSensitivity>();

            GameObject sensInputVal = Instantiate(sensitivityInputValuePrefab, sensitivityAdjustmentContainer);

            if (loadFromPrevious)
            {
                adjust.SetSensitivityAdjustment(savedAdjustmentValues[adjust.GetEventName()]);
            } else
            {
                savedAdjustmentValues.Add(adjust.GetEventName(), adjust.GetSensitivityAdjustment());
            }

            sensInputVal.GetComponent<AdjustableInputValue>().SetAdjustableSensitivity(adjust);
            sensInputVal.GetComponent<AdjustableInputValue>().RegisterParent(this);
        }
    }

    public void SaveAllValues()
    {
        ES3.Save(savedAdjustementsKey, savedAdjustmentValues, savedAdjustmentFile);
        MenuManager.instance.ShowAlert("Saved sensitivity values", 1f);
    }

    public void DeleteAllValues()
    {
        MenuManager.instance.ShowAlert("Deleted sensitivity values", 1f);
        ES3.DeleteFile(savedAdjustmentFile);
        savedAdjustmentValues = new Dictionary<string, float>();
        sensitivityAdjustmentContainer.DestroyAllChildren();
        foreach (Transform evt in eventsContainer)
        {
            AdjustableSensitivity adjust = evt.GetComponent<AdjustableSensitivity>();

            GameObject sensInputVal = Instantiate(sensitivityInputValuePrefab, sensitivityAdjustmentContainer);

            savedAdjustmentValues.Add(adjust.GetEventName(), 1f);
            adjust.SetSensitivityAdjustment(1f);

            sensInputVal.GetComponent<AdjustableInputValue>().SetAdjustableSensitivity(adjust);
            sensInputVal.GetComponent<AdjustableInputValue>().RegisterParent(this);
        }
    }

    public void UpdateSavedSensitivity(string eventName, float val)
    {
        savedAdjustmentValues[eventName] = val;
    }

    public void ExportValues()
    {
        exportArea.SetActive(true);
        exportText.text = PrintSavedValues();
    }

    private string PrintSavedValues()
    {
        StringBuilder retVal = new StringBuilder();
        foreach (KeyValuePair<string, float> keypair in savedAdjustmentValues)
        {
            retVal.AppendLine(keypair.Key + ":" + keypair.Value);
        }

        return retVal.ToString();
    }

    public void CopyToClipboard()
    {
        GUIUtility.systemCopyBuffer = PrintSavedValues();
        MenuManager.instance.ShowAlert("Copied to clipboard", 1f);
    }
}
