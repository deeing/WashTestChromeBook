using UnityEngine;
using DG.Tweening;

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
        if (rect != null)
        {
            rect.DOAnchorPosY(status ? targetY : originalY, transitionTime);
        }
    }
}
