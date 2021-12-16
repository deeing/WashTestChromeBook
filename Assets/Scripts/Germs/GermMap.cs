using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GermMap : MonoBehaviour
{
    [SerializeField]
    private GameObject palmsMap;
    [SerializeField]
    private GameObject backMap;
    [SerializeField]
    private GameObject betweenMap;
    [SerializeField]
    private GameObject fingertipsMap;
    [SerializeField]
    private GameObject fingernailsMap;
    [SerializeField]
    private GameObject thumbsMap;
    [SerializeField]
    private GameObject wristMap;
    [SerializeField]
    private float flipSpeed = 1f;

    private RectTransform thisTransform;
    private bool isFlipped = false;

    private void Awake()
    {
        thisTransform = (RectTransform)transform;
    }

    public void ToggleMap(bool status) {
        gameObject.SetActive(status);
        if (status)
        {
            List<GermType> remainingGermTypes = GermManager.instance.GetRemaningGermTypes();
            foreach (GermType type in remainingGermTypes)
            {
                ActivateGermMap(type);
            }
        } else
        {
            DeactivateAllGermMaps();
        }
    }

    private void ActivateGermMap(GermType type)
    {
        switch(type)
        {
            case GermType.Palm:
                palmsMap.SetActive(true);
                break;
            case GermType.BackOfHandL:
            case GermType.BackOfHandR:
                backMap.SetActive(true);
                break;
            case GermType.BetweenFingers:
                betweenMap.SetActive(true);
                break;
            case GermType.FingertipsL:
            case GermType.FingertipsR:
                fingertipsMap.SetActive(true);
                break;
            case GermType.FingernailsL:
            case GermType.FingernailsR:
                fingernailsMap.SetActive(true);
                break;
            case GermType.ThumbL:
            case GermType.ThumbR:
                thumbsMap.SetActive(true);
                break;
            case GermType.WristL:
            case GermType.WristR:
                wristMap.SetActive(true);
                break;
            default:
                break;
        }
    }

    private void DeactivateAllGermMaps()
    {
        palmsMap.SetActive(false);
        backMap.SetActive(false);
        betweenMap.SetActive(false);
        fingertipsMap.SetActive(false);
        fingernailsMap.SetActive(false);
        thumbsMap.SetActive(false);
        wristMap.SetActive(false);
    }

    // flips germ map upside down for better orientation from front view
    public void ToggleFlipped(bool status)
    {
        isFlipped = status;
        Vector3 rotationVector = status ? new Vector3(0f, 0f, 180f) : Vector3.zero;
        thisTransform.DORotate(rotationVector, flipSpeed);
    }

    public void ToggleFlipped()
    {
        ToggleFlipped(!isFlipped);
    }
}
