using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkinMapping", menuName = "Wash/SkinMapping")]
public class SkinMapping : ScriptableObject
{
    public static string PLAYER_PREF_PREFIX = "WashUnlockedSkin";
    public static int PLAYER_PREF_LOCKED = 0;
    public static int PLAYER_PREF_UNLOCKED = 1;
    public string displayName;
    public string id;
    public Material skinMaterial;
    public bool isUnlockable = true;

    public bool GetIsUnlocked()
    {
        return PlayerPrefs.GetInt(PLAYER_PREF_PREFIX + id) == PLAYER_PREF_UNLOCKED;
    }

    public void SetUnlocked()
    {
        PlayerPrefs.SetInt(PLAYER_PREF_PREFIX + id, PLAYER_PREF_UNLOCKED);
    }

    public void SetLocked()
    {
        PlayerPrefs.SetInt(PLAYER_PREF_PREFIX + id, PLAYER_PREF_LOCKED);
    }
}
