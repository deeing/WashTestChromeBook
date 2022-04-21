using Lean.Touch;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    [SerializeField]
    private GameObject crosshairsBase;
    [SerializeField]
    private GameObject crosshairsGood;
    [SerializeField]
    private GameObject crosshairsGreat;
    [SerializeField]
    private GameObject crosshairsPerfect;
    [SerializeField]
    private Vector3 hidePosition = new Vector3(-100f, -100f);
    [SerializeField]
    private LayerMask touchMask;

    private Transform thisTransform;
    private LeanFinger touchFinger;
    private CrosshairSystem crosshairSystem;

    private void Awake()
    {
        thisTransform = transform;
    }

    private void Update()
    {
        HandleCrosshairPosition();
    }

    public void ShowCrosshair(LeanFinger finger)
    {
        enabled = true;
        touchFinger = finger;
    }

    public void HideCrosshair()
    {
        enabled = false;
        touchFinger = null;
        thisTransform.position = hidePosition;
    }

    private void HandleCrosshairPosition()
    {
        GameObject button = GetIsOverButton();

        if (button == null)
        {
            NoButtonPosition();
        }
        else
        {
            LockCrosshair(button);
        }
    }

    public void RegisterSystem(CrosshairSystem crosshairSystem)
    {
        this.crosshairSystem = crosshairSystem;
    }

    private GameObject GetIsOverButton()
    {
        if (touchFinger == null)
        {
            return null;
        }

        List<RaycastResult> hits = LeanTouch.RaycastGui(touchFinger.ScreenPosition, touchMask);

        if (hits.Count > 0)
        {

            GameObject firstHit = hits[0].gameObject;
            return firstHit;

            // revisit this if it's a performance issue
            foreach(RaycastResult hit in hits)
            {
                if (hit.gameObject.CompareTag("TouchBase"))
                {
                    return hit.gameObject;
                }
            }
        }


        return null;
    }

    private void NoButtonPosition()
    {
        crosshairsBase.SetActive(true);
        crosshairsGood.SetActive(false);
        crosshairsGreat.SetActive(false);
        crosshairsPerfect.SetActive(false);
        if (touchFinger == null)
        {
            thisTransform.position = hidePosition;
        }
        else
        {
            thisTransform.position = touchFinger.ScreenPosition;
        }
    }

    private void LockCrosshair(GameObject button)
    {
        crosshairsBase.SetActive(false);
        crosshairsGood.SetActive(false);
        crosshairsGreat.SetActive(false);
        crosshairsPerfect.SetActive(false);
        thisTransform.position = button.transform.position;

        switch (crosshairSystem.rhythmInput.GetCurrentInputStatus())
        {
            case RhythmInputStatus.Good:
                crosshairsGood.SetActive(true);
                break;
            case RhythmInputStatus.Great:
                crosshairsGreat.SetActive(true);
                break;
            case RhythmInputStatus.Perfect:
                crosshairsPerfect.SetActive(true);
                break;
            default:
                crosshairsGood.SetActive(true);
                break;
        }
    }
}
