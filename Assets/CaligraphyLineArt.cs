using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CaligraphyLineArt : MonoBehaviour
{
    [SerializeField]
    private LineArtMapping[] lineArtPairs;
    [SerializeField]
    private GameObject lineArtBackground;

    public void ShowLineArt(PlayerEventType eventType)
    {
        foreach (LineArtMapping mapping in lineArtPairs)
        {
            if (mapping.playerEvent == eventType)
            {
                TurnOnLineArt(mapping);
            }
        }
    }

    private void TurnOnLineArt(LineArtMapping mapping)
    {
        mapping.lineArt.SetActive(true);
        lineArtBackground.SetActive(true);
    }

    public void HideLineArt()
    {
        lineArtBackground.SetActive(false);
        foreach (LineArtMapping mapping in lineArtPairs)
        {
            if (mapping.lineArt.activeSelf)
            {
                mapping.lineArt.SetActive(false);
            }
        }
    }

    // should this be in its own file?
    [Serializable]
    private class LineArtMapping
    {
        public PlayerEventType playerEvent;
        public GameObject lineArt;
    }
}


