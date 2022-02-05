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
    private bool autoToggle = false;

    private bool isToggled = false;
    private WaitForSeconds autoToggleWait;

    private void OnEnable()
    {
        if (autoToggle)
        {
            autoToggleWait = new WaitForSeconds(transitionTime);
            StartCoroutine(AutoToggle());
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
        Toggle(false);
        image.DOKill();
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
}
