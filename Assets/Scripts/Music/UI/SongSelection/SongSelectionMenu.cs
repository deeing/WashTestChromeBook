using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongSelectionMenu : SingletonMonoBehaviour<SongSelectionMenu>
{
    [SerializeField]
    private Transform _songSelectionContainer;
    public Transform songSelectionContainer { get => _songSelectionContainer; set => _songSelectionContainer = value; }

    protected override void Awake()
    {
        if(!InitializeSingleton(this))
        {
            return;
        }
    }
}
