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
    [SerializeField]
    private Transform buttonsContainer;
    [SerializeField]
    private Transform caligraphyHandContainer;
    [SerializeField]
    private Vector3 fullScreenScale;
    [SerializeField]
    private Vector3 nonFullScreenScale;

    [HideInInspector]
    public bool userFinishedSymbol = false;

    public bool userIsDrawing { get; private set; } = false;
    public int userMistakes { get; private set; } = 0;

    private List<Vector2> markedPoints = new List<Vector2>();
    // connections between buttons IDs that were drawn
    private Dictionary<int, HashSet<int>> buttonConnectionsById = new Dictionary<int, HashSet<int>>();

    // maps button ID to the buttons transform (makes it easier to draw)
    public Dictionary<int, RectTransform> buttonMap { get; private set; } = null;

    private int lastButtonId = 0;
    private TouchButton touchButton;

    private bool wasFullScreen = false;
    private CaligraphySymbol lastSymbol;

    private void Awake()
    {
        SetupButtonMap();
        touchButton = GetComponent<TouchButton>();
        SizeToScreen();
    }

    private void OnEnable()
    {
        HandleResize();
    }

    private void Update()
    {
        HandleResize();
    }

    private void HandleResize()
    {
        if (ShouldResize())
        {
            SizeToScreen();
            wasFullScreen = Screen.fullScreen;
        }
    }

    private bool ShouldResize()
    {
        return (wasFullScreen && !Screen.fullScreen) || (!wasFullScreen && Screen.fullScreen);
    }

     private void SizeToScreen()
    {
        if (Screen.fullScreen)
        {
            buttonsContainer.localScale = fullScreenScale;
            caligraphyHandContainer.localScale = fullScreenScale;
        }
        else
        {
            buttonsContainer.localScale = nonFullScreenScale;
            caligraphyHandContainer.localScale = nonFullScreenScale;
        }
        RedrawGuidelines();
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
        //Debug.Log("Adding mark for " + buttonId);

        markedPoints.Add(newPos);
        lineRenderer.AddPosition(newPos);
        if (lastButtonId != 0)
        {
            AddConnectionToSet(lastButtonId, buttonId);
            CaligraphyInputManager.instance.SubmitCaligraphy(buttonConnectionsById);
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

    public void ReRenderLines()
    {
        lineRenderer.RenderLines();
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
        if (userFinishedSymbol || !userIsDrawing)
        {
            return;
        }
        //HandleCompleteCaligraphy();
        ResetLines();
        ToggleDrawing(false);
        CaligraphyInputManager.instance.ClearSymbol();
        ResetAllButtonColors();
        userMistakes++;
    }

    public void HandleCompleteCaligraphy()
    {
        /*foreach(KeyValuePair<int, HashSet<int>> connection in buttonConnectionsById)
        {
            string set = "{";
            foreach(int point in connection.Value)
            {
                set += point + ", ";
            }
            set += "}";
            Debug.Log(connection.Key + "->" + set);
        }*/

        //CaligraphyInputManager.instance.SubmitCaligraphy(buttonConnectionsById);

        ResetLines();
        ToggleDrawing(false);
        CaligraphyInputManager.instance.ClearSymbol();
        ResetAllButtonColors();
    }

    public void ResetAllButtonColors()
    {
        foreach (CaligraphyButton button in buttons)
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
        lineRenderer.gameObject.SetActive(status);
        guideLineRenderer.gameObject.SetActive(status);
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

        lastSymbol = symbol;
    }

    private void RedrawGuidelines()
    {
        if (lastSymbol != null)
        {
            ClearGuideLines();
            SetupGuideLines(lastSymbol);
        }
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

    public UILineRenderer GetLineRenderer()
    {
        return lineRenderer;
    }

    public void StartCheckingForMistakes()
    {
        userMistakes = 0;
    }
}
