using Lean.Touch;
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
    private GameObject crosshairsLocked;
    [SerializeField]
    private Vector3 hidePosition = new Vector3(-100f, -100f);
    [SerializeField]
    private LayerMask touchMask;

    private Transform thisTransform;
    private bool active = false;
    private LeanFinger touchFinger;

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

    private GameObject GetIsOverButton()
    {
        if (touchFinger == null)
        {
            return null;
        }

        List<RaycastResult> hits = LeanTouch.RaycastGui(touchFinger.ScreenPosition, touchMask);

        if (hits.Count > 0)
        {
            RaycastResult firstHit = hits[0];
            return firstHit.gameObject;
        }

        return null;
    }

    private void NoButtonPosition()
    {
        crosshairsBase.SetActive(true);
        crosshairsLocked.SetActive(false);
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
        crosshairsLocked.SetActive(true);
        thisTransform.position = button.transform.position;
    }
}
