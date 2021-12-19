using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GermMap : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup upView;
    [SerializeField]
    private CanvasGroup downView;

    [SerializeField]
    private GameObject palmsMap;
    [SerializeField]
    private GameObject backLMap;
    [SerializeField]
    private GameObject backRMap;
    [SerializeField]
    private GameObject betweenMap;
    [SerializeField]
    private GameObject fingertipsLMap;
    [SerializeField]
    private GameObject fingertipsRMap;
    [SerializeField]
    private GameObject fingertipsLUpMap;
    [SerializeField]
    private GameObject fingertipsRUpMap;
    [SerializeField]
    private GameObject fingernailsLMap;
    [SerializeField]
    private GameObject fingernailsRMap;
    [SerializeField]
    private GameObject thumbsLMap;
    [SerializeField]
    private GameObject thumbsRMap;
    [SerializeField]
    private GameObject thumbsLUpMap;
    [SerializeField]
    private GameObject thumbsRUpMap;
    [SerializeField]
    private GameObject wristLMap;
    [SerializeField]
    private GameObject wristRMap;
    [SerializeField]
    private GameObject wristLUpMap;
    [SerializeField]
    private GameObject wristRUpMap;
    [SerializeField]
    private float flipSpeed = 1f;
    [SerializeField]
    private float upDownTransitionSpeed = 1f;

    private RectTransform thisTransform;
    private bool isFlipped = false;

    public enum HandViewMode
    {
        Up,
        Down
    }

    private void Awake()
    {
        thisTransform = (RectTransform)transform;
    }

    public void ToggleMap(bool status) {
        if (status)
        {
            List<GermType> remainingGermTypes = GermManager.instance.GetRemaningGermTypes();
            foreach (GermType type in remainingGermTypes)
            {
                ActivateGermMap(type);
            }
            downView.alpha = 1f;
        }
        else
        {
            upView.alpha = 0f;
            upView.DOKill();
            downView.alpha = 0f;
            downView.DOKill();
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
                backLMap.SetActive(true);
                break;
            case GermType.BackOfHandR:
                backRMap.SetActive(true);
                break;
            case GermType.BetweenFingers:
                betweenMap.SetActive(true);
                break;
            case GermType.FingertipsL:
                fingertipsLMap.SetActive(true);
                fingertipsLUpMap.SetActive(true);
                break;
            case GermType.FingertipsR:
                fingertipsRMap.SetActive(true);
                fingertipsRUpMap.SetActive(true);
                break;
            case GermType.FingernailsL:
                fingernailsLMap.SetActive(true);
                break;
            case GermType.FingernailsR:
                fingernailsRMap.SetActive(true);
                break;
            case GermType.ThumbL:
                thumbsLMap.SetActive(true);
                thumbsLUpMap.SetActive(true);
                break;
            case GermType.ThumbR:
                thumbsRMap.SetActive(true);
                thumbsRUpMap.SetActive(true);
                break;
            case GermType.WristL:
                wristLMap.SetActive(true);
                wristLUpMap.SetActive(true);
                break;
            case GermType.WristR:
                wristRMap.SetActive(true);
                wristRUpMap.SetActive(true);
                break;
            default:
                break;
        }
    }

    private void DeactivateAllGermMaps()
    {
        palmsMap.SetActive(false);
        backLMap.SetActive(false);
        backRMap.SetActive(false);
        betweenMap.SetActive(false);
        fingertipsLMap.SetActive(false);
        fingertipsRMap.SetActive(false);
        fingernailsLMap.SetActive(false);
        fingernailsRMap.SetActive(false);
        thumbsLMap.SetActive(false);
        thumbsRMap.SetActive(false);
        wristLMap.SetActive(false);
        wristRMap.SetActive(false);
        fingertipsLUpMap.SetActive(false);
        fingertipsRUpMap.SetActive(false);
        thumbsLUpMap.SetActive(false);
        thumbsRUpMap.SetActive(false);
        wristLUpMap.SetActive(false);
        wristRUpMap.SetActive(false);
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

    public void SetViewMode(HandViewMode mode)
    {
        bool isUpMode = mode == HandViewMode.Up;
        upView.DOFade( isUpMode ? 1f : 0f, upDownTransitionSpeed);
        downView.DOFade(isUpMode ? 0f: 1f, upDownTransitionSpeed);
    }
}
