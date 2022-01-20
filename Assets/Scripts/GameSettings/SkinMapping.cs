using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkinMapping", menuName = "Wash/SkinMapping")]
public class SkinMapping : ScriptableObject
{
    public static int PLAYER_PREF_LOCKED = 0;
    public static int PLAYER_PREF_UNLOCKED = 1;
    public string displayName;
    public string id;
    public Material skinMaterial;
    public bool isUnlockable = true;
}
