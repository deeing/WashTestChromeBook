using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkinChoice : MonoBehaviour
{
    [SerializeField]
    private TMP_Text buttonText;

    private SkinMapping skin;

    public void SetChoiceForButton(SkinMapping skinChoice)
    {
        skin = skinChoice;
        buttonText.text = skin.displayName;
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
