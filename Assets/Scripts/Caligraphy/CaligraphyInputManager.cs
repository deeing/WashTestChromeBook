using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaligraphyInputManager : SingletonMonoBehaviour<CaligraphyInputManager>
{
    [SerializeField]
    private CaligraphyInput caligraphyInput;
    [SerializeField]
    private Transform tutorialHand;

    private Dictionary<int, HashSet<int>> playerSymbolConnections = null;

    protected override void Awake()
    {
        if (!InitializeSingleton(this))
        {
            return;
        }
        playerSymbolConnections = new Dictionary<int, HashSet<int>>();
    }

    public void SubmitCaligraphy(Dictionary<int, HashSet<int>> buttonConnectionsById)
    {
        playerSymbolConnections = buttonConnectionsById;
    }

    public void ClearSymbol()
    {
        playerSymbolConnections = null;
    }

    public Dictionary<int, HashSet<int>> GenerateExpectedSymbolMap(List<CaligraphyConnection> expectedSymbolConnections)
    {
        Dictionary<int, HashSet<int>> expectedSymbolMap = new Dictionary<int, HashSet<int>>();
        foreach (CaligraphyConnection expectedConn in expectedSymbolConnections)
        {
            if (expectedSymbolMap.ContainsKey(expectedConn.buttonId1))
            {
                expectedSymbolMap[expectedConn.buttonId1].Add(expectedConn.buttonId2);
            }
            else
            {
                HashSet<int> newSet = new HashSet<int>();
                newSet.Add(expectedConn.buttonId2);
                expectedSymbolMap.Add(expectedConn.buttonId1, newSet);
            }
        }
        return expectedSymbolMap;
    }

    public int GetNumValidConnections(CaligraphySymbol symbol)
    {
        if (playerSymbolConnections == null)
        {
            return 0;
        }
        HashSet<(int, int)> uniqueConnections = new HashSet<(int, int)>();

        Dictionary<int, HashSet<int>> expectedSymbolMap = GenerateExpectedSymbolMap(symbol.symbolConnections);
        foreach (KeyValuePair<int, HashSet<int>> connSet in playerSymbolConnections)
        {
            int firstButton = connSet.Key;
            foreach (int secondButton in connSet.Value)
            {
                // check if this belongs in the expected symbols
                bool firstButtonCheck = expectedSymbolMap.ContainsKey(firstButton) && expectedSymbolMap[firstButton].Contains(secondButton);
                bool secondButtonCheck = expectedSymbolMap.ContainsKey(secondButton) && expectedSymbolMap[secondButton].Contains(firstButton);

                if (firstButtonCheck && !uniqueConnections.Contains((firstButton, secondButton)) && !uniqueConnections.Contains((secondButton, firstButton))) {
                    uniqueConnections.Add((firstButton, secondButton));
                } else if (secondButtonCheck && !uniqueConnections.Contains((firstButton, secondButton)) && !uniqueConnections.Contains((secondButton, firstButton)))
                {
                    uniqueConnections.Add((secondButton, firstButton));
                }
            }
        }

        return uniqueConnections.Count;
    }

    public bool HasOnlyValidMoves(CaligraphySymbol symbol, Dictionary<int, HashSet<int>> expectedSymbolMap)
    {
        foreach (KeyValuePair<int, HashSet<int>> connSet in playerSymbolConnections)
        {
            int firstButton = connSet.Key;
            foreach (int secondButton in connSet.Value)
            {
                // check if this belongs in the expected symbols
                bool firstButtonCheck = expectedSymbolMap.ContainsKey(firstButton) && expectedSymbolMap[firstButton].Contains(secondButton);
                bool secondButtonCheck = expectedSymbolMap.ContainsKey(secondButton) && expectedSymbolMap[secondButton].Contains(firstButton);

                if (!firstButtonCheck && !secondButtonCheck)
                {
                    //Debug.Log("Did not need connection between :" + firstButton + " and " + secondButton);
                    return false;
                }
            }
        }
        return true;
    }

    public bool HasOnlyValidMoves(CaligraphySymbol symbol)
    {
        Dictionary<int, HashSet<int>> expectedSymbolMap = GenerateExpectedSymbolMap(symbol.symbolConnections);
        return HasOnlyValidMoves(symbol, expectedSymbolMap);
    }

    public bool HasAllNeededMoves(CaligraphySymbol symbol)
    {
        foreach (CaligraphyConnection expectedConnection in symbol.symbolConnections)
        {
            int buttonOneId = expectedConnection.buttonId1;
            int buttonTwoId = expectedConnection.buttonId2;

            // check one direction  of the connection
            bool buttonOneFirstValid = playerSymbolConnections.ContainsKey(buttonOneId) && playerSymbolConnections[buttonOneId].Contains(buttonTwoId);

            // check the other way
            bool buttonTwoFirstValid = playerSymbolConnections.ContainsKey(buttonTwoId) && playerSymbolConnections[buttonTwoId].Contains(buttonOneId);

            if (!buttonOneFirstValid && !buttonTwoFirstValid)
            {
                //Debug.Log("Missing connection between " + buttonOneId + " and " + buttonTwoId);
                //ClearSymbol();
                return false;
            }
        }

        return true;
    }

    public bool HasDoneCaligraphy(CaligraphySymbol symbol)
    {
        if (playerSymbolConnections == null)
        {
            return false;
        }

        Dictionary<int, HashSet<int>> expectedSymbolMap = GenerateExpectedSymbolMap(symbol.symbolConnections);

        // check for any invalid moves
        if (!HasOnlyValidMoves(symbol, expectedSymbolMap))
        {
            //ClearSymbol();
            return false;
        }

        // check if all needed moves were met
        if(!HasAllNeededMoves(symbol))
        {
            //ClearSymbol();
            return false;
        }


        return true;
    }
    public bool HasDoneCaligraphy(CaligraphyMove caligraphyMove)
    {
        return HasDoneCaligraphy(caligraphyMove.symbol);
    }

    public void ToggleCaligraphy(bool status)
    {
        caligraphyInput.Toggle(status);
    }

    public void ToggleInteractable(bool status)
    {
        caligraphyInput.ToggleInteractable(status);
    }

    public void SetupGuideLines(CaligraphyMove caligraphyMove)
    {
        caligraphyInput.SetupGuideLines(caligraphyMove);
    }

    public void ClearGuideLines()
    {
        caligraphyInput.ClearGuideLines();
    }

    public bool CurrentEventIsCaligraphy()
    {
        //WashEvent currEvent = WashEventManager.instance.GetCurrentEvent();
        MusicWashEvent currEvent = MusicManager.instance.GetCurrentEvent();
        return currEvent is MusicSwitchEvent;
    }

    public void HandleCompleteCaligraphy()
    {
        caligraphyInput.HandleCompleteCaligraphy();
        ClearGuideLines();
        ClearSymbol();
        ToggleInteractable(false);
        ToggleCaligraphy(false);
        //HandAnimations.instance.Reset();
    }

    public void SetUserFinishedSymbol(bool status)
    {
        caligraphyInput.userFinishedSymbol = status;
    }

    public bool UserIsDrawing()
    {
        return caligraphyInput.userIsDrawing;
    }

    public CaligraphyInput GetCaligraphyInput()
    {
        return caligraphyInput;
    }

    public Transform GetTutorialHand()
    {
        return tutorialHand;
    }

    public void StartCheckingForMistakes()
    {
        caligraphyInput.StartCheckingForMistakes();
    }

    public int GetNumMistakes()
    {
        return caligraphyInput.userMistakes;
    }
}
