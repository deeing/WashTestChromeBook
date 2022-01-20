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
    private List<SkinMapping> unlockableSkins;

    private bool isShowing = false;

    private void Awake()
    {
        allSkins = MusicManager.instance.gameSettings.GetAllSkinMappings();
        unlockableSkins = new List<SkinMapping>();

        foreach (SkinMapping skinMapping in allSkins)
        {
            if (skinMapping.isUnlockable)
            {
                unlockableSkins.Add(skinMapping);
            }

            GameObject skinObj = Instantiate(skinChoicePrefab, skinsContainer);
            SkinChoice skinchoice = skinObj.GetComponent<SkinChoice>();
            skinchoice.SetChoiceForButton(skinMapping);
        }
    }

    public List<SkinMapping> GetUnlockableSkins()
    {
        return unlockableSkins;
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
