using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LeftSlideMenu : SlideInMenu
{
    [SerializeField]
    private float targetX = -100f;

    private float originalX;

    public override void Setup()
    {
        originalX = rect.position.x;
    }

    public override void SetVisible(bool status)
    {
        rect.DOAnchorPosX(status ? targetX : originalX, transitionTime);
    }
}
