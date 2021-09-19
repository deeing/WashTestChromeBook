using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T instance;

    protected abstract void Awake();

    protected bool InitializeSingleton(T singleton)
    {
        if(!instance)
        {
            instance = singleton;
            return true;
        }
        Destroy(this);
        return false;
    }
}
