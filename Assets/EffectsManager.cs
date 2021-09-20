using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsManager : SingletonMonoBehaviour<EffectsManager>
{
    [SerializeField]
    private ParticleSystem celebrationParticles;

    protected override void Awake()
    {
        if(!InitializeSingleton(this))
        {
            return;
        }
    }

    public void Celebrate()
    {
        celebrationParticles.Play();
    }
}
