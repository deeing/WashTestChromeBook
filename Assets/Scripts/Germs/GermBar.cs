using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GermBar : MonoBehaviour
{
    [SerializeField]
    private PercentageBar germBarDisplay;

    private LeftSlideMenu leftSlideMenu;

    private void Start()
    {
        leftSlideMenu = GetComponent<LeftSlideMenu>();
    }

    public void Show()
    {
        leftSlideMenu.Show();
    }

    public void Hide()
    {
        leftSlideMenu.Hide();
    }

    public void UpdateGermPercentage(float percentage)
    {
        germBarDisplay.UpdatePercentage(percentage);
    }

    public void SetText(string text)
    {
        leftSlideMenu.SetText(text);
    }
}
