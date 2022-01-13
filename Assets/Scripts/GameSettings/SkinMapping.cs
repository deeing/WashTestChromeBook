using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkinMapping", menuName = "Wash/SkinMapping")]
public class SkinMapping : ScriptableObject
{
    public string displayName;
    public string id;
    public Material skinMaterial;
}
