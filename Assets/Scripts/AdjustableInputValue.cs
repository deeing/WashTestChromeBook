using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AdjustableInputValue : MonoBehaviour
{
    [SerializeField]
    private TMP_Text title;
    [SerializeField]
    private TMP_Text baseSensitivity;
    [SerializeField]
    private TMP_InputField input;

    private AdjustableSensitivity adjustable;
    private SensitivityAdjustment parent;

    public void SetAdjustableSensitivity(AdjustableSensitivity adjustable)
    {
        this.adjustable = adjustable;
        SetTitle(this.adjustable.GetEventName());
        SetBaseSensitivitty(this.adjustable.GetBaseSensitivity().ToString());
        SetInputText(this.adjustable.GetSensitivityAdjustment());
    }

    public void SetTitle(string titleText)
    {
        title.text = titleText;
    }

    public void SetBaseSensitivitty(string sensitivity)
    {
        baseSensitivity.text = sensitivity;
    }

    public void SetInputText(float val)
    {
        input.text = val.ToString();
    }

    public void SetInputValue(string val)
    {
        if (float.TryParse(val, out float parsedVal))
        {
            adjustable.SetSensitivityAdjustment(parsedVal);
            if (parent != null)
            {
                parent.UpdateSavedSensitivity(adjustable.GetEventName(), parsedVal);
            }
        }
        else
        {
            Debug.LogWarning(val + " is not a valid float");
        }
    }

    public void RegisterParent(SensitivityAdjustment sens)
    {
        parent = sens;
    }
}
