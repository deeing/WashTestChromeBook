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
    [SerializeField]
    private int originalMaterialIndex = 1;
    
    private Material originalMaterial;
    private bool isOn;

    private void Awake()
    {
        originalMaterial = skinnedMesh.materials[originalMaterialIndex];
    }

    public void OnDisable()
    {
        SetUvMode(false);
    }

    public void ToggleUvMode()
    {
        SetUvMode(!isOn);
    }

    public void SetUvMode(bool status)
    {
        if (originalMaterial == null)
        {
            originalMaterial = skinnedMesh.materials[originalMaterialIndex];
        }
        isOn = status;
        SetGermsVisible(status);
        SetHandMaterial(status);
        EffectsManager.instance.ToggleSuds(!status);
        EffectsManager.instance.ToggleUvLights(status);
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
        mats[originalMaterialIndex] = status ? UVSkinMaterial : originalMaterial;
        skinnedMesh.materials = mats;
    }
}
