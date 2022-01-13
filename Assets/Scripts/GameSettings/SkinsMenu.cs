using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinsMenu : MonoBehaviour
{
    [SerializeField]
    private Transform skinsContainer;
    [SerializeField]
    private GameObject skinChoicePrefab;

    private SkinMapping[] allSkins;

    private bool isShowing = false;

    private void Awake()
    {
        allSkins = MusicManager.instance.gameSettings.GetAllSkinMappings();
        foreach (SkinMapping skinMapping in allSkins)
        {
            GameObject skinObj = Instantiate(skinChoicePrefab, skinsContainer);
            SkinChoice skinchoice = skinObj.GetComponent<SkinChoice>();
            skinchoice.SetChoiceForButton(skinMapping);
        }
    }

    public void ToggleSkinMenu(bool status)
    {
        isShowing = status;
        gameObject.SetActive(status);
    }

    public void ToggleSkinMenu()
    {
        ToggleSkinMenu(!isShowing);
    }
}
