using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface AdjustableSensitivity 
{
    public void SetSensitivityAdjustment(float sensitivityAdjustment);
    public float GetSensitivityAdjustment();

    public float GetBaseSensitivity();

    public string GetEventName();
}
