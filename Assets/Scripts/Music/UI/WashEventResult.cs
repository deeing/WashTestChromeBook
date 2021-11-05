using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Wash.Utilities;
using UnityEngine.UI;

public class WashEventResult : MonoBehaviour
{
    [SerializeField]
    private TMP_Text eventName;
    [SerializeField]
    private TMP_Text score;

    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void SetEvent(MusicWashEvent musicWashEvent)
    {
        string resultName = musicWashEvent.GetEventType().GetDescription();
        float eventScore = musicWashEvent.GetScore();
        SetText(resultName, eventScore);
    }

    public void SetText(string resultName, float score)
    {
        eventName.text = resultName;
        this.score.text = score.ToString();
    }

    public void SetColor(Color color)
    {
        image.color = color;
    }
}
