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
        List<RaycastResult> hits = Lean.Touch.LeanTouch.RaycastGui(finger.ScreenPosition, touchMask);
        foreach (RaycastResult hit in hits)
        {
            if (hit.gameObject == touchArea)
            {
                return true;
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
