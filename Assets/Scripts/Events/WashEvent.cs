using System;
using UnityEngine;

public abstract class WashEvent : MonoBehaviour
{
    public abstract void SetupEvent();
    public abstract void StartEvent();
    public abstract void DoEvent();
    public abstract bool CheckEndEvent();

    public abstract void EndEvent();
}
