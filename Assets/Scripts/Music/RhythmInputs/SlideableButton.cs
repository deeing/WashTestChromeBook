using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;
using Wash.Utilities;

public class SlideableButton : MonoBehaviour
{
    [SerializeField]
    private RectTransform startingPoint;
    [SerializeField]
    private RectTransform endingPoint;

    private RectTransform thisTransform;
    private LeanFinger finger;
    private bool isSliding = false;

    private float lowestY = 0f;
    private float highestY = 0f;
    private float totalDistance = 0f;

    private void Awake()
    {
        thisTransform = (RectTransform)transform;
        float startingY = startingPoint.position.y;
        float endingY = endingPoint.position.y;

        lowestY = Mathf.Min(startingY, endingY);
        highestY = Mathf.Max(startingY, endingY);
        totalDistance = highestY - lowestY;
    }

    private void Update()
    {
        if (!isSliding)
        {
            return;
        }

        UpdateSliderPosition();
    }
    
    private void UpdateSliderPosition()
    {
        Vector3 newPosition = thisTransform.position;
        newPosition.y = Mathf.Clamp(finger.ScreenPosition.y, lowestY, highestY);
        thisTransform.position = newPosition;
    }

    public void StartSlide(LeanFinger finger)
    {
        this.finger = finger;
        isSliding = true;
    }

    public void EndSlide()
    {
        finger = null;
        isSliding = false;
    }

    // percentage towards the end the button is slid
    public float GetPercentage()
    {
        if (!isSliding)
        {
            return -1;
        }
        float currentDistance = finger.ScreenPosition.y - lowestY;
        currentDistance.ClampLower(0f);
        float percentage = currentDistance / totalDistance;
        percentage.Clamp(0f, 1f);
        return percentage;
    }
}
