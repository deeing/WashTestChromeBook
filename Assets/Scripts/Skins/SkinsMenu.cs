using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wash.Utilities;

public class SkinsMenu : MonoBehaviour
{
    [SerializeField]
    private Transform skinsContainer;
    [SerializeField]
    private GameObject skinChoicePrefab;
    [SerializeField]
    private Transform resetButton;

    private SkinMapping[] allSkins;
    private List<GameObject> skinChoiceObjs;

    private bool isShowing = false;
    private bool hasInit = false;

    private void Awake()
    {
        Init();
        SetupList();
    }

    private void Init()
    {
        allSkins = MusicManager.instance.gameSettings.GetAllSkinMappings();
        skinChoiceObjs = new List<GameObject>();
        hasInit = true;
    }

    private void SetupList()
    {
        CleanOldList();
        foreach (SkinMapping skinMapping in allSkins)
        {
            GameObject skinObj = Instantiate(skinChoicePrefab, skinsContainer);
            skinChoiceObjs.Add(skinObj);
            SkinChoice skinchoice = skinObj.GetComponent<SkinChoice>();
            skinchoice.SetChoiceForButton(skinMapping);
        }

        // always make the reset button the last sibling
        resetButton.SetAsLastSibling();
    }

    private void CleanOldList()
    {
        foreach(GameObject skinChoiceObj in skinChoiceObjs)
        {
            Destroy(skinChoiceObj);
        }

        skinChoiceObjs = new List<GameObject>();
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

    public void ResetAllSkins()
    {
        // sets all skins back to locked state
        List<SkinMapping> unlockableSkins = MusicManager.instance.gameSettings.GetUnlockableSkinMappings();
        foreach (SkinMapping skinMapping in allSkins)
        {
            skinMapping.SetLocked();
        }

        SetupList();
    }

    private void ForceShowHands()
    {
        HandAnimations.instance.PlayAnimation("Idle");
    }

    public void ShowSkinsListAtEnd()
    {
        if (!hasInit)
        {
            Init();
        }

        SetupList();
        ToggleSkinMenu(true);
        ForceShowHands();
    }
}
