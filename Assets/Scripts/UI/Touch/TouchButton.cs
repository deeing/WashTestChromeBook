using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TouchButton : MonoBehaviour
{
    [SerializeField]
    private LayerMask touchMask;
    [SerializeField]
    private GameObject touchArea;
    [SerializeField]
    [Tooltip("Whether or not we want to touch input to only account for the topmost button only")]
    private bool topOnly = false;

    public UnityEvent onPress;
    public UnityEvent onHover;
    public UnityEvent onExit;
    public UnityEvent onRelease;

    private bool isHovering = false;

    void OnEnable()
    {
        if (onPress.GetPersistentEventCount() > 0)
        {
            Lean.Touch.LeanTouch.OnFingerDown += HandleFingerDown;
        }
        if (onHover.GetPersistentEventCount() > 0 || onExit.GetPersistentEventCount() > 0)
        {
            Lean.Touch.LeanTouch.OnFingerUpdate += HandleFingerHover;
        }
        if (onRelease.GetPersistentEventCount() > 0)
        {
            Lean.Touch.LeanTouch.OnFingerUp += HandleFingerUp;
        }
    }

    void OnDisable()
    {
        if (onPress.GetPersistentEventCount() > 0)
        {
            Lean.Touch.LeanTouch.OnFingerDown -= HandleFingerDown;
        }
        if (onHover.GetPersistentEventCount() > 0 || onExit.GetPersistentEventCount() > 0)
        {
            Lean.Touch.LeanTouch.OnFingerUpdate -= HandleFingerHover;
        }
        if (onRelease.GetPersistentEventCount() > 0)
        {
            Lean.Touch.LeanTouch.OnFingerUp -= HandleFingerUp;
        }
    }

    private bool GetIsOverButton(Lean.Touch.LeanFinger finger)
    {
        // loop through all the fingers and see if any of the fingers are touchnig this button's touch area
        foreach (Lean.Touch.LeanFinger currFinger in Lean.Touch.LeanTouch.Fingers)
        {
            List<RaycastResult> hits = Lean.Touch.LeanTouch.RaycastGui(currFinger.ScreenPosition, touchMask);
            if (topOnly)
            {
                return hits.Count > 0 && hits[0].gameObject == touchArea;
            } else
            {
                // N SQUARED - OPTIMIZE THIS IF IT BECOMES AN ISSUE
                foreach (RaycastResult hit in hits)
                {
                    if (hit.gameObject == touchArea)
                    {
                        return true;
                    }
                }
            }
        }
        
        return false;
    }

    void HandleFingerDown(Lean.Touch.LeanFinger finger)
    {
        if (GetIsOverButton(finger))
        {
            OnPress();
        } 
    }

    void HandleFingerHover(Lean.Touch.LeanFinger finger)
    {
        if (GetIsOverButton(finger))
        {
            OnHover();
            isHovering = true;
        }
        else if (isHovering)
        {
            OnExit();
            isHovering = false;
        }
    }

    void HandleFingerUp(Lean.Touch.LeanFinger finger)
    {
        OnRelease();
    }

    public void OnPress()
    {
        onPress.Invoke();
    }

    public void OnHover()
    {
        onHover.Invoke();
    }

    public void OnExit()
    {
        onExit.Invoke();
    }

    public void OnRelease()
    {
        onRelease.Invoke();
    }
}
