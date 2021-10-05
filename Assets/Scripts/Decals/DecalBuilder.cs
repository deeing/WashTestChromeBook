using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
#if UNITY_EDITOR
[CustomEditor(typeof(DecalSystem))]
public class DecalBuilder : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DecalSystem decalSystem = (DecalSystem)target;

        if (GUILayout.Button("Generate Decals"))
        {
            decalSystem.BuildAllDecals();
        }
    }
}
#endif