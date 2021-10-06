using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScrubEvent : PlayerEvent
{
    [SerializeField]
    protected float sensitivity = .002f;
    [SerializeField]
    protected float crossFadetime = .25f;
    [SerializeField]
    [Tooltip("If idle after this time, we transition to the idle animation")]
    private float idleTime = .5f;
    [SerializeField]
    [Tooltip("Time for return to neutral animation after scrubbing finishes")]
    protected float returnNeutralTime = .5f;

    public float touchInput { get; private set; } = 0f;
    private WaitForSeconds idleWait;
    private Coroutine idleCoroutine;
    private bool isIdle = false;
    private GermType germType;

    public override void SetupEvent()
    {
        idleWait = new WaitForSeconds(idleTime);
        germType = GetGermType();
    }

    public override bool CheckEndEvent()
    {
        return !GermManager.instance.HasGermsOfType(germType);
    }

    public abstract GermType GetGermType();
    public abstract void DoScrub();

    public abstract void DoIdle();

    public abstract float DoTouchInput();

    public abstract void ReturnToNeutral();

    public override void DoEvent()
    {
        touchInput = DoTouchInput() * sensitivity;

        if (touchInput > 0)
        {
            GermManager.instance.KillRandomGermOfType(germType);
            EffectsManager.instance.Bubbles();
            DoScrub();
            if (idleCoroutine != null)
            {
                StopCoroutine(idleCoroutine);
                idleCoroutine = null;
                isIdle = false;
            }
        } else
        {
            if (idleCoroutine == null)
            {
                idleCoroutine = StartCoroutine(SetIdle());
            } else if (isIdle)
            {
                DoIdle();
            }
        }
    }

    private IEnumerator SetIdle()
    {
        yield return idleWait;
        isIdle = true;
    }

    public override void EndEvent()
    {
        base.EndEvent();

        ReturnToNeutral();
    }
}
