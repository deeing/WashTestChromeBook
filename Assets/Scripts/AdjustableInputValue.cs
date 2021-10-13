using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AdjustableInputValue : MonoBehaviour
{
    [SerializeField]
    private TMP_Text title;

    public void SetTitle(string titleText)
    {
        title.text = titleText;
    }
}
