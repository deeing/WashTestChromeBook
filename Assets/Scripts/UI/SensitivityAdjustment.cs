using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensitivityAdjustment : MonoBehaviour
{
    [SerializeField]
    private Transform eventsContainer;
    [SerializeField]
    private Transform sensitivityAdjustmentContainer;
    [SerializeField]
    private GameObject sensitivityInputValuePrefab;

    private void Start()
    {
        foreach(Transform evt in eventsContainer)
        {
            AdjustableSensitivity adjust = evt.GetComponent<AdjustableSensitivity>();

            GameObject sensInputVal = Instantiate(sensitivityInputValuePrefab, sensitivityAdjustmentContainer);
            sensInputVal.GetComponent<AdjustableInputValue>().SetTitle(adjust.GetEventName());
        }
    }
}
