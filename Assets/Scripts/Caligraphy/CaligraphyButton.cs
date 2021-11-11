using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaligraphyButton : MonoBehaviour
{
    [SerializeField]
    private CaligraphyInput caligraphyInput;
    [SerializeField]
    private int _id = 0;
    public int id { get => _id; private set => value = _id; }

    private Transform thisTransform;

    private void Awake()
    {
        thisTransform = transform;
    }

    public void StartCaligraphy(Lean.Touch.LeanFinger finger)
    {
        caligraphyInput.SetStartingPoint(thisTransform.position, id);
    }

    public void MarkButton(Lean.Touch.LeanFinger finger)
    {
        if (caligraphyInput.isDrawing)
        {
            caligraphyInput.AddMarkedPoint(thisTransform.position, id);
        }
    }
}
