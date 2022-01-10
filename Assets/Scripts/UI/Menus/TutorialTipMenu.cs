using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTipMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject[] tipMenus;

    public void ToggleTipMenus(bool status)
    {
        foreach(GameObject tipMenu in tipMenus)
        {
            tipMenu.SetActive(status);
        }
    }
}
