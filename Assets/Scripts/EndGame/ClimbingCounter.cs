using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClimbingCounter : MonoBehaviour
{
    [SerializeField]
    private TMP_Text counterText;
    [SerializeField]
    private float max = 100f;

    public float currentValue { get; private set; }

    private float targetValue = 0f;
    private Coroutine climbingCo = null;

    private float climbInterval = .02f;
    private WaitForSeconds climbIntervalWait;

    private void Awake()
    {
        climbIntervalWait = new WaitForSeconds(climbInterval);
        SetCounterText(0f);
    }

    public void IncreaseBy(float amount, float time)
    {
        if (climbingCo != null)
        {
            StopCoroutine(climbingCo);
            climbingCo = null;
            currentValue = targetValue;
        }

        climbingCo = StartCoroutine(ClimbToTarget(currentValue + amount, time));
    }

    private IEnumerator ClimbToTarget(float newTarget, float time)
    {
        targetValue = newTarget;

        float climbRate = (targetValue - currentValue) / time * climbInterval;
        while (currentValue <= targetValue)
        {
            yield return climbIntervalWait;
            currentValue = Mathf.Min(currentValue + climbRate, targetValue);
            SetCounterText(currentValue);
        }
    }

    private void SetCounterText(float value)
    {
        counterText.text = Mathf.FloorToInt(value) + "/" + max;
    }

    public void SetMax(float max)
    {
        this.max = max;
        SetCounterText(0f);
    }
}
