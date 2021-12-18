using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleImage : MonoBehaviour
{
    [SerializeField]
    private Image imageToSwitch;
    [SerializeField]
    private Sprite startSprite;
    [SerializeField]
    private Sprite toggleSprite;

    public void ShowToggleSprite(bool status)
    {
        imageToSwitch.sprite = status ? toggleSprite : startSprite;
    }
}
