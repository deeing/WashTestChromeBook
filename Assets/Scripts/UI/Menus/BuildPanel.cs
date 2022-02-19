using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildPanel : MonoBehaviour
{
    [SerializeField]
    private TMP_Text buildText;

    private void Start()
    {
        buildText.text = AllScenes.instance.build;
    }
}
