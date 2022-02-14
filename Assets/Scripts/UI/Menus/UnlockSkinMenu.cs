using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Wash.Utilities;
using DG.Tweening;

public class UnlockSkinMenu : MonoBehaviour
{
    [SerializeField]
    private TMP_Text unlockSkinText;
    [SerializeField]
    private float entranceDelay = 1f;

    private string unlockFormatText = "You Unlocked the <b>{0} skin</b>! Go to the Settings menu (gear icon) to use your new skin!";
    private WaitForSeconds entranceWait;
    private Transform thisTransform;

    private void Awake()
    {
        entranceWait = new WaitForSeconds(entranceDelay);
        thisTransform = transform;
    }
    public void UnlockSkin()
    {

        // check if there are any skins to unlock
        List<SkinMapping> stillLockedSkins = GetLockedSkins();

        Debug.Log(stillLockedSkins.Count);
        if (stillLockedSkins.Count > 0)
        {
            SkinMapping newlyUnlocked = stillLockedSkins.RandomElement<SkinMapping>();
            newlyUnlocked.SetUnlocked();
            SetText(newlyUnlocked);
            gameObject.SetActive(true);

            StartCoroutine(DelayedReveal());
        }

    }

    private IEnumerator DelayedReveal()
    {
        yield return entranceWait;
        thisTransform.DOScale(1, 1f);
    }

    private void SetText(SkinMapping unlockedSkin)
    {
        unlockSkinText.text = String.Format(unlockFormatText, unlockedSkin.displayName);
    }

    private List<SkinMapping> GetLockedSkins()
    {
        List<SkinMapping> stillLockedSkins = new List<SkinMapping>();

        List<SkinMapping> unlockableSkins = MusicManager.instance.gameSettings.GetUnlockableSkinMappings();
        foreach (SkinMapping unlockableSkin in unlockableSkins)
        {
            if (!unlockableSkin.GetIsUnlocked())
            {
                stillLockedSkins.Add(unlockableSkin);
            }
        }

        return stillLockedSkins;
    }
}
