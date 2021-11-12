using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaligraphyInputManager : SingletonMonoBehaviour<CaligraphyInputManager>
{
    [SerializeField]
    private CaligraphyInput caligraphyInput;

    private Dictionary<int, HashSet<int>> playerSymbolConnections = null;

    protected override void Awake()
    {
        if (!InitializeSingleton(this))
        {
            return;
        }
    }

    public void SubmitCaligraphy(Dictionary<int, HashSet<int>> buttonConnectionsById)
    {
        playerSymbolConnections = buttonConnectionsById;
    }

    public void ClearSymbol()
    {
        playerSymbolConnections = null;
    }

    public bool HasDoneCaligraphy(CaligraphyMove caligraphyMove)
    {
        if (playerSymbolConnections == null)
        {
            return false;
        }

        List<CaligraphyConnection> expectedSymbolConnections = caligraphyMove.symbol.symbolConnections;
        Dictionary<int, int> expectedSymbolMap = new Dictionary<int, int>();
        foreach(CaligraphyConnection expectedConn in expectedSymbolConnections)
        {
            expectedSymbolMap.Add(expectedConn.buttonId1, expectedConn.buttonId2);
        }

        // different number of moves
        if (playerSymbolConnections.Count < expectedSymbolConnections.Count)
        {
            Debug.Log("not the right number of connections! Was " + playerSymbolConnections.Count + " but expected " + expectedSymbolConnections.Count);
            ClearSymbol();
            return false;
        }

        foreach (KeyValuePair<int, HashSet<int>> connSet in playerSymbolConnections)
        {
            int firstButton = connSet.Key;
            foreach (int secondButton in connSet.Value)
            {
                // check if this belongs in the expected symbols
                bool firstButtonCheck = expectedSymbolMap.ContainsKey(firstButton) && expectedSymbolMap[firstButton] == secondButton;
                bool secondButtonCheck = expectedSymbolMap.ContainsKey(secondButton) && expectedSymbolMap[secondButton] == firstButton;

                if (!firstButtonCheck && !secondButtonCheck)
                {
                    Debug.Log("Did not need connection between :" + firstButton + " and " + secondButton);
                    ClearSymbol();
                    return false;
                }
            }
        }
        return true;
    }

    public void ToggleCaligraphy(bool status)
    {
        caligraphyInput.Toggle(status);
    }

    public void SetupGuideLines(CaligraphyMove caligraphyMove)
    {
        caligraphyInput.SetupGuideLines(caligraphyMove);
    }

    public void ClearGuideLines()
    {
        caligraphyInput.ClearGuideLines();
    }
}
