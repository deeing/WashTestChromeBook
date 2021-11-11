using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaligraphyInput : MonoBehaviour
{
    [SerializeField]
    private UILineRenderer lineRenderer;
    
    public bool isActive { get; private set; }  = false;

    private List<Vector2> markedPoints = new List<Vector2>();
    // connections between buttons IDs that were drawn
    private Dictionary<int, int> buttonConnectionsById = new Dictionary<int, int>();

    private int lastButtonId = 0;

    private void ResetLines()
    {
        lineRenderer.RemoveAllPositions();
        lineRenderer.ClearLines();
        markedPoints = new List<Vector2>();
        buttonConnectionsById = new Dictionary<int, int>();
    }

    private void RemoveUnmarkedPoints()
    {
        lineRenderer.RemoveAllExcept(markedPoints);
    }

    public void AddMarkedPoint(Vector2 newPos, int buttonId)
    {
        if (buttonId == lastButtonId)
        {
            return;
        }

        markedPoints.Add(newPos);
        lineRenderer.AddPosition(newPos);
        if (lastButtonId != 0)
        {
            buttonConnectionsById.Add(lastButtonId, buttonId);
        }
        lastButtonId = buttonId;
    }

    public void HandleHover(Lean.Touch.LeanFinger finger)
    {
        if (isActive)
        {
            RemoveUnmarkedPoints();
            lineRenderer.AddPosition(finger.ScreenPosition);
            lineRenderer.RenderLines();
        }
    }

    public void SetStartingPoint(Vector2 startingPoint, int buttonId)
    {
        ResetLines();
        lineRenderer.AddPosition(startingPoint);
        markedPoints.Add(startingPoint);
        lastButtonId = buttonId;
        SetActive(true);
    }

    public void Release(Lean.Touch.LeanFinger finger)
    {
        HandleCompleteCaligraphy();
        ResetLines();
        SetActive(false);
    }

    public void HandleCompleteCaligraphy()
    {
        foreach(KeyValuePair<int, int> connection in buttonConnectionsById)
        {
            Debug.Log(connection.Key + "->" + connection.Value);
        }

        CaligraphyInputManager.instance.SubmitCaligraphy(buttonConnectionsById);
    }

    public void SetActive(bool status)
    {
        isActive = status;
    }

}
