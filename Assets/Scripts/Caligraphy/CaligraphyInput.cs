using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaligraphyInput : MonoBehaviour
{
    [SerializeField]
    private UILineRenderer lineRenderer;
    [SerializeField]
    private bool isActive = false;

    private List<Vector2> markedPoints = new List<Vector2>();

    private void ResetLines()
    {
        lineRenderer.RemoveAllPositions();
        lineRenderer.ClearLines();
        markedPoints = new List<Vector2>();
    }

    private void RemoveUnmarkedPoints()
    {
        lineRenderer.RemoveAllExcept(markedPoints);
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

    public void SetStartingPoint(Lean.Touch.LeanFinger finger)
    {
        ResetLines();
        Vector2 startingPoint = finger.ScreenPosition;
        lineRenderer.AddPosition(startingPoint);
        markedPoints.Add(startingPoint);
        SetActive(true);
    }

    public void Release(Lean.Touch.LeanFinger finger)
    {
        ResetLines();
        SetActive(false);
    }

    public void SetActive(bool status)
    {
        isActive = status;
    }

}
