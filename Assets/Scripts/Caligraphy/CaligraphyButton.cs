using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CaligraphyButton : MonoBehaviour
{
    [SerializeField]
    private CaligraphyInput caligraphyInput;
    [SerializeField]
    private int _id = 0;
    [SerializeField]
    private Image buttonImage;
    [SerializeField]
    private Color markedColor;

    public int id { get => _id; private set => value = _id; }

    private Transform thisTransform;
    private Color originalColor;

    private void Awake()
    {
        thisTransform = transform;
        originalColor = buttonImage.color;
    }

    public void StartCaligraphy(Lean.Touch.LeanFinger finger)
    {
        caligraphyInput.SetStartingPoint(thisTransform.position, id);
        ToggleAltColor(true);
    }

    public void MarkButton(Lean.Touch.LeanFinger finger)
    {
        if (caligraphyInput.isDrawing)
        {
            caligraphyInput.AddMarkedPoint(thisTransform.position, id);
            ToggleAltColor(true);
        }
    }

    private void ToggleAltColor(bool status)
    {
        if (status)
        {
            buttonImage.DOColor(markedColor, .5f);
        } else
        {
            buttonImage.DOColor(originalColor, .5f);
        }
    }

    public void ResetButton()
    {
        ToggleAltColor(false);
    }
}
