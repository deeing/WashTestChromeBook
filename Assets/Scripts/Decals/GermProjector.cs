using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GermProjector : MonoBehaviour
{
    [SerializeField]
    private GermType germType = GermType.Palm;

    private void Start()
    {
        GermManager.instance.RegisterGerm(gameObject, germType);
    }

    public void SetGermType(GermType type)
    {
        germType = type;
    }
}
