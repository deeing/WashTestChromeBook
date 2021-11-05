using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicResultsMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject washEventResultPrefab;
    [SerializeField]
    private GameObject totalResultPrefab;
    [SerializeField]
    private Transform resultsContainer;

    private TopDropMenu topDropMenu;

    private void Awake()
    {
        topDropMenu = GetComponent<TopDropMenu>();
    }

    public void AddWashEventResults(MusicWashEvent washEvent)
    {
        GameObject resultObj = Instantiate(washEventResultPrefab, resultsContainer);
        resultObj.GetComponent<WashEventResult>().SetEvent(washEvent);
    }

    public void AddTotalScore(float totalScore)
    {
        GameObject resultObj = Instantiate(totalResultPrefab, resultsContainer);
        WashEventResult washEventResult = resultObj.GetComponent<WashEventResult>();
        washEventResult.SetText("Total score", totalScore);
        resultObj.transform.SetSiblingIndex(0);
    }

    public void Show()
    {
        topDropMenu.Show();
    }
}
