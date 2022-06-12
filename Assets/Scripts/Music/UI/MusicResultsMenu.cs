using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicResultsMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject washEventResultPrefab;
    [SerializeField]
    private GameObject totalResultPrefab;
    [SerializeField]
    private Transform resultsContainer;
    [SerializeField]
    private UnlockSkinMenu unlockSkinMenu;
    [SerializeField]
    private Inspect12Steps stepButtons;
    [SerializeField]
    private float startScoreCalcDelay = 2f;
    [SerializeField]
    private float indiviualScoreCalcTime = 1f;
    [SerializeField]
    private ClimbingCounter totalScore;
    [SerializeField]
    private ScrollRect scrollRect;
    [SerializeField]
    private StarSystem starSystem;
    [SerializeField]
    private float maxScore = 300f;

    private TopDropMenu topDropMenu;
    private WaitForSeconds startScoreCalcWait;
    private WaitForSeconds individualScoreCalcWait;

    private void Awake()
    {
        topDropMenu = GetComponent<TopDropMenu>();
        startScoreCalcWait = new WaitForSeconds(startScoreCalcDelay);
        individualScoreCalcWait = new WaitForSeconds(indiviualScoreCalcTime);
        starSystem.Init(maxScore);
        totalScore.SetMax(maxScore);
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
        stepButtons.Toggle(true);
        stepButtons.DisableButtons();

        StartCoroutine(CalculateScore());


        // TODO: EVENTUALLY FLIP THE LOGIC ON THIS
        //if (GermManager.instance.HasGerms())
        //{
        //unlockSkinMenu.UnlockSkin();
        // }

    }

    private IEnumerator CalculateScore()
    {
        yield return startScoreCalcWait;
        List<MusicPlayerEvent> scoreEvents = new List<MusicPlayerEvent>();

        // add scores for non scrub events
        MusicPlayerEvent[] nonScrubEvents = MusicManager.instance.GetNonSrubPointEvents();
        scoreEvents.AddRange(nonScrubEvents);

        // add scores for all the scrub events 
        List<MusicScrubEvent> scrubEvents = MusicManager.instance.GetScrubEvents();
        scoreEvents.AddRange(scrubEvents);

        foreach (MusicPlayerEvent scoreEvent in scoreEvents)
        {
            AddWashEventResults(scoreEvent);
            yield return null;
            scrollRect.verticalNormalizedPosition = 0f;
            totalScore.IncreaseBy(scoreEvent.GetScore(), indiviualScoreCalcTime);
            starSystem.IncreaseBy(scoreEvent.GetScore(), indiviualScoreCalcTime);
            yield return individualScoreCalcWait;
        }
    }



}
