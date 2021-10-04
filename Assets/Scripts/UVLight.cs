using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UVLight : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private SkinnedMeshRenderer skinnedMesh;
    [SerializeField]
    private Material UVSkinMaterial;

    private bool isOn;
    private Material originalMaterial;

    private void Awake()
    {
        originalMaterial = skinnedMesh.materials[1];
    }

    public void ToggleUvMode()
    {
        isOn = !isOn;

        SetGermsVisible(isOn);
        SetHandMaterial(isOn);
    }

    private void SetGermsVisible(bool status)
    {
        if (status)
        {
            mainCamera.cullingMask |= 1 << LayerMask.NameToLayer("Germ");
        } else
        {
            mainCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("Germ"));
        }
    }

    private void SetHandMaterial(bool status)
    {
        Material[] mats = skinnedMesh.materials;
        mats[1] = status ? UVSkinMaterial : originalMaterial;
        skinnedMesh.materials = mats;
    }
}
