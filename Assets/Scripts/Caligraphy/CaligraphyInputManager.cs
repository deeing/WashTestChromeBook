using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaligraphyInputManager : SingletonMonoBehaviour<CaligraphyInputManager>
{
    [SerializeField]
    private CaligraphyInput caligraphyInput;

    private Dictionary<int, int> playerSymbolConnections = null;

    protected override void Awake()
    {
        if (!InitializeSingleton(this))
        {
            return;
        }
    }

    public void SubmitCaligraphy(Dictionary<int, int> buttonConnectionsById)
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

        // different number of moves
        if (playerSymbolConnections.Count != expectedSymbolConnections.Count)
        {
            Debug.Log("not the right number of connections! Was " + playerSymbolConnections.Count + " but expected " + expectedSymbolConnections.Count);
            ClearSymbol();
            return false;
        }

        foreach (CaligraphyConnection expectedConnection in expectedSymbolConnections)
        {
            int buttonOneId = expectedConnection.buttonId1;
            int buttonTwoId = expectedConnection.buttonId2;

            // check one direction  of the connection
            bool buttonOneFirstValid = playerSymbolConnections.ContainsKey(buttonOneId) && playerSymbolConnections[buttonOneId] == buttonTwoId;

            // check the other way
            bool buttonTwoFirstValid = playerSymbolConnections.ContainsKey(buttonTwoId) && playerSymbolConnections[buttonTwoId] == buttonOneId;

            if (!buttonOneFirstValid && !buttonTwoFirstValid)
            {
                Debug.Log("Missing connection between " + buttonOneId + " and " + buttonTwoId);
                ClearSymbol();
                return false;
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
