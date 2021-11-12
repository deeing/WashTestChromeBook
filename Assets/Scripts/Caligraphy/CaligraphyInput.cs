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
    
    public bool userIsDrawing { get; private set; }  = false;

    private List<Vector2> markedPoints = new List<Vector2>();
    // connections between buttons IDs that were drawn
    private Dictionary<int, HashSet<int>> buttonConnectionsById = new Dictionary<int, HashSet<int>>();

    // maps button ID to the buttons transform (makes it easier to draw)
    public Dictionary<int, RectTransform> buttonMap { get; private set; } = null;

    private int lastButtonId = 0;
    private TouchButton touchButton;

    private void Awake()
    {
        SetupButtonMap();
        touchButton = GetComponent<TouchButton>();
    }

    public void ResetLines()
    {
        lastButtonId = 0;
        lineRenderer.RemoveAllPositions();
        lineRenderer.ClearLines();
        markedPoints = new List<Vector2>();
        buttonConnectionsById = new Dictionary<int, HashSet<int>>();
    }

    public void RemoveUnmarkedPoints()
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
            AddConnectionToSet(lastButtonId, buttonId);
        }
        lastButtonId = buttonId;
    }

    private void AddConnectionToSet(int lastId, int newId)
    {
        if (buttonConnectionsById.ContainsKey(lastId))
        {
            buttonConnectionsById[lastId].Add(newId);
        }
        else
        {
            HashSet<int> newSet = new HashSet<int>();
            newSet.Add(newId);
            // create new set
            buttonConnectionsById.Add(lastId, newSet);
        }
    }

    public void HandleHover(Lean.Touch.LeanFinger finger)
    {
        if (userIsDrawing)
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
        foreach(KeyValuePair<int, HashSet<int>> connection in buttonConnectionsById)
        {
            string set = "{";
            foreach(int point in connection.Value)
            {
                set += point + ", ";
            }
            set += "}";
            Debug.Log(connection.Key + "->" + set);
        }

        CaligraphyInputManager.instance.SubmitCaligraphy(buttonConnectionsById);

        foreach(CaligraphyButton button in buttons)
        {
            button.ResetButton();
        }
    }

    public void ToggleDrawing(bool status)
    {
        userIsDrawing = status;
    }

    public void Toggle(bool status)
    {
        gameObject.SetActive(status);
    }

    public void SetupGuideLines(CaligraphyMove caligraphyMove)
    {
        //ClearGuideLines();
        CaligraphySymbol symbol = caligraphyMove.symbol;
        SetupGuideLines(symbol);
    }

    public void SetupGuideLines(CaligraphySymbol symbol)
    {
        List<CaligraphyConnection> connections = symbol.symbolConnections;
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

    public void ToggleInteractable(bool status)
    {
        touchButton.enabled = status;
        ToggleButtonsInteractable(status);
    }

    public void ToggleButtonsInteractable(bool status)
    {
        foreach(CaligraphyButton button in buttons)
        {
            button.ToggleInteractable(status);
        }
    }
}
