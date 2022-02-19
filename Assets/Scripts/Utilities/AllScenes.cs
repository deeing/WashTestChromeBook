using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllScenes : SingletonMonoBehaviour<AllScenes>
{
    public string build;

    protected override void Awake()
    {
        if(!InitializeSingleton(this))
        {
            return;
        }
    }
}
