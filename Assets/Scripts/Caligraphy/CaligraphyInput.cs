using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaligraphyInput : MonoBehaviour
{
    [SerializeField]
    private UILineRenderer lineRenderer;
    [SerializeField]
    private UILineRenderer guideLineRenderer;
    [SerializeField]
    private RectTransform canvas;
    [SerializeField]
    private List<CaligraphyButton> buttons;
    
    public bool isDrawing { get; private set; }  = false;

    private List<Vector2> markedPoints = new List<Vector2>();
    // connections between buttons IDs that were drawn
    private Dictionary<int, int> buttonConnectionsById = new Dictionary<int, int>();

    // maps button ID to the buttons transform (makes it easier to draw)
    private Dictionary<int, RectTransform> buttonMap = null;

    private int lastButtonId = 0;

    private void Awake()
    {
        SetupButtonMap();
    }

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
        if (isDrawing)
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
        ToggleDrawing(true);
    }

    public void Release(Lean.Touch.LeanFinger finger)
    {
        HandleCompleteCaligraphy();
        ResetLines();
        ToggleDrawing(false);
    }

    public void HandleCompleteCaligraphy()
    {
        foreach(KeyValuePair<int, int> connection in buttonConnectionsById)
        {
            Debug.Log(connection.Key + "->" + connection.Value);
        }

        CaligraphyInputManager.instance.SubmitCaligraphy(buttonConnectionsById);
    }

    public void ToggleDrawing(bool status)
    {
        isDrawing = status;
    }

    public void Toggle(bool status)
    {
        gameObject.SetActive(status);
    }

    public void SetupGuideLines(CaligraphyMove caligraphyMove)
    {
        //ClearGuideLines();
        List<CaligraphyConnection> connections = caligraphyMove.symbol.symbolConnections;
        foreach (CaligraphyConnection conn in connections)
        {
            DrawGuideLine(conn);
        }
        guideLineRenderer.RenderLines();
    }

    public void ClearGuideLines()
    {
        guideLineRenderer.RemoveAllPositions();
        guideLineRenderer.ClearLines();
    }

    private void SetupButtonMap()
    {
        buttonMap = new Dictionary<int, RectTransform>();
        foreach (CaligraphyButton button in buttons)
        {
            buttonMap.Add(button.id, (RectTransform)button.transform);
        }
    }

    private void DrawGuideLine(CaligraphyConnection conn)
    {
        RectTransform button1 = buttonMap[conn.buttonId1];
        RectTransform button2 = buttonMap[conn.buttonId2];

        Vector2 button1Center = GetCenter(button1);
        Vector2 button2Center = GetCenter(button2);

        guideLineRenderer.AddPosition(button1Center);
        guideLineRenderer.AddPosition(button2Center);
    }

    private Vector2 GetCenter(RectTransform button)
    {
        return button.position;
    }
}
