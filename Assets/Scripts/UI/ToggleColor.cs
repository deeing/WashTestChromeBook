using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ToggleColor : MonoBehaviour
{
    // set by the group it belongs to
    public ToggleColorGroup toggleColorGroup;

    [SerializeField]
    private Color startingColor = Color.white;
    [SerializeField]
    private Color toggledColor = Color.white    ;
    [SerializeField]
    private Image image;
    [SerializeField]
    private float transitionTime = .25f;
    [SerializeField]
    private bool isAuto = false;

    private bool isToggled = false;
    private WaitForSeconds autoToggleWait;

    private void Awake()
    {
        if (isAuto)
        {
            ToggleAuto(true);
        }
    }

    private IEnumerator AutoToggle()
    {
        while(enabled)
        {
            yield return autoToggleWait;
            Toggle();
        }
    }

    public void OnDisable()
    {
        ToggleAuto(false);
        Toggle(false);
        if (image != null)
        {
            image.DOKill();
            image.color = startingColor;
        }
    }

    public void Toggle()
    {
        Toggle(!isToggled);
    }

    public void Toggle(bool status)
    {
        isToggled = status;

        if (status)
        {
            image.DOColor(toggledColor, transitionTime);
        } else
        {
            image.DOColor(startingColor, transitionTime);
        }
    }

    public void ToggleInGroup()
    {
        if (toggleColorGroup)
        {
            toggleColorGroup.ToggleInGroup(this);
        }
    }

    public void ToggleAuto(bool status)
    {
        isAuto = status;
        Toggle(status);
        if (status)
        {
            autoToggleWait = new WaitForSeconds(transitionTime);
            StartCoroutine(AutoToggle());
        }
        else
        {
            StopAllCoroutines();
        }
    }

    public void SetGroup(ToggleColorGroup group)
    {
        toggleColorGroup = group;
    }
}
