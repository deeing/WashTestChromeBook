using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Wash.Utilities;

public class StatusDisplay : MonoBehaviour
{
    [SerializeField]
    private TMP_Text statusText;

    private float timeUntilFade = .75f;
    private WaitForSeconds fadeWait;
    private Coroutine willFade;

    private void Awake()
    {
        fadeWait = new WaitForSeconds(timeUntilFade);
    }

    public void ShowStatusDisplay(RhythmInputStatus inputStatus)
    {
        // doesn't show if it's a miss?
        if (inputStatus == RhythmInputStatus.Miss)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);
        statusText.text = inputStatus.GetDescription();

        if (willFade != null)
        {
            StopCoroutine(willFade);
            willFade = null;
        }
        willFade = StartCoroutine(HideCoroutine());
    }

    public void HideStatusDisplay()
    {
        gameObject.SetActive(false);
    }

    private IEnumerator HideCoroutine()
    {
        yield return fadeWait;
        HideStatusDisplay();
        willFade = null;
    }
}
