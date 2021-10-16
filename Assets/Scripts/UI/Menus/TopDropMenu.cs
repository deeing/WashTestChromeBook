using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class TopDropMenu : SlideInMenu
{
    [SerializeField]
    private float targetY = -100f;
    
    private float originalY;

    public override void Setup()
    {
        originalY = rect.position.y;
    }

    public override void SetVisible(bool status)
    {
        rect.DOAnchorPosY(status ? targetY : originalY, transitionTime);
    }
}
