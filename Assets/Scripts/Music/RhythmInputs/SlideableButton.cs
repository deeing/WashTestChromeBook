using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;
using TMPro;

public class SlideableButton : MonoBehaviour
{
    [SerializeField]
    private RectTransform startingPoint;
    [SerializeField]
    private RectTransform endingPoint;
    [SerializeField]
    private TMP_Text debugText;

    private RectTransform thisTransform;
    private LeanFinger finger;
    private bool isSliding = false;

    private float startingY = 0f;
    private float endingY = 0f;
    private float lowestY = 0f;
    private float highestY = 0f;
    private float totalDistance = 0f;

    private float sliderPercentage = -1f;
    private bool slidingUp = false;

    private void Awake()
    {
        thisTransform = (RectTransform)transform;
        startingY = startingPoint.position.y;
        endingY = endingPoint.position.y;

        lowestY = Mathf.Min(startingY, endingY);
        highestY = Mathf.Max(startingY, endingY);
        totalDistance = highestY - lowestY;

        if (startingY < endingY)
        {
            slidingUp = true;
        }
    }

    private void Update()
    {
        if (!isSliding)
        {
            sliderPercentage = -1f;
            return;
        }

        UpdateSliderPosition();
    }
    
    private void UpdateSliderPosition()
    {
        Vector3 newPosition = thisTransform.position;
        newPosition.y = Mathf.Clamp(finger.ScreenPosition.y, lowestY, highestY);
        thisTransform.position = newPosition;
        sliderPercentage = CalculateSliderPercentage(newPosition.y);
        debugText.text = sliderPercentage.ToString();
    }

    private float CalculateSliderPercentage(float newPosition)
    {
        if (slidingUp)
        {
            if (newPosition <= startingY)
            {
                return 0f;
            } 
            else if (newPosition >= endingY)
            {
                return 1f;
            }
        } 
        else
        {
            if (newPosition >= startingY)
            {
                return 0f;
            }
            else if (newPosition <= endingY)
            {
                return 1f;
            }
        }

        float distanceFromStart = Mathf.Abs(newPosition - startingY);
        return distanceFromStart / totalDistance;
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
        return sliderPercentage;
    }
}
