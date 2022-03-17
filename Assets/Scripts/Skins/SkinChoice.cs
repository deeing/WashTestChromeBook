using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SkinChoice : MonoBehaviour
{
    [SerializeField]
    private TMP_Text buttonText;
    [SerializeField]
    private GameObject lockIcon;
    [SerializeField]
    private GameObject unlockIcon;
    [SerializeField]
    private Button thisButton;

    private SkinMapping skin;

    public void SetChoiceForButton(SkinMapping skinChoice)
    {
        skin = skinChoice;
        buttonText.text = skin.displayName;

        if (skinChoice.isUnlockable)
        {
            SetLockedState(skinChoice);
        }
    }

    private void SetLockedState(SkinMapping skinChoice)
    {
        if (skinChoice.GetIsUnlocked())
        {
            unlockIcon.SetActive(true);
        } 
        else
        {
            lockIcon.SetActive(true);
            thisButton.interactable = false;
        }
    }

    public void ChooseSkin()
    {
        MusicManager.instance.SetSkin(skin);
    }

    public void ToggleSkinChoiceMenu(bool status)
    {
        gameObject.SetActive(status);
    }
}
