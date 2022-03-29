using Lean.Touch;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject crosshairPrefab;
    [SerializeField]
    List<Crosshair> crosshairPool;
    [SerializeField]
    private int maxCrosshairNum = 2;

    private Transform thisTransform;

    public RhythmInput rhythmInput { get; private set; }

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        crosshairPool = new List<Crosshair>();
        thisTransform = transform;

        PrepopulateCrosshairPool();
    }

    private void PrepopulateCrosshairPool()
    {
        for (int i = 0; i < maxCrosshairNum; i++)
        {
            GameObject crossObj = Instantiate(crosshairPrefab, thisTransform);
            Crosshair crosshair = crossObj.GetComponent<Crosshair>();
            crosshair.RegisterSystem(this);
            crosshairPool.Add(crosshair);
        }
    }

    private void Update()
    {
        HandleCrosshair();
    }

    private void HandleCrosshair()
    {
        List<LeanFinger> fingers = LeanTouch.GetFingers(false, false);
        int numFingers = Mathf.Min(fingers.Count, maxCrosshairNum);

        // set position of used crosshairs
        for (int i=0; i < numFingers; i++)
        {
            SetupCrosshair(fingers[i], i);
        }

        // hide unused ones
        for (int i=numFingers; i < crosshairPool.Count; i++)
        {
            crosshairPool[i].HideCrosshair();
        }
    }

    private void SetupCrosshair(LeanFinger finger, int index)
    {
        crosshairPool[index].ShowCrosshair(finger);
    }

    private void OnDisable()
    {
        HideAllCrosshairs();
    }

    private void HideAllCrosshairs()
    {
        for (int i = 0; i < crosshairPool.Count; i++)
        {
            crosshairPool[i].HideCrosshair();
        }
    }

    public void SetRhythmInput(RhythmInput rhythmInput)
    {
        this.rhythmInput = rhythmInput;
    }
}
