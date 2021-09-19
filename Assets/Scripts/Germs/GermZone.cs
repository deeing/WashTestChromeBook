using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GermZone : MonoBehaviour
{
    [SerializeField]
    private int numGerms = 5;
    [SerializeField]
    [Tooltip("Position is a randomized value at most maxDistanceFromOrigin units from the GermZone")]
    private float maxDistanceFromOrigin = 1f;
    [SerializeField]
    private GameObject germPrefab;

    private Transform thisTransform;

    private void Awake()
    {
        thisTransform = transform;

        SpawnGerms();
    }

    private void SpawnGerms()
    {
        for (int i=0; i < numGerms; i++)
        {
            Vector3 pos = thisTransform.position + Random.insideUnitSphere.normalized * maxDistanceFromOrigin;

            Instantiate(germPrefab, pos, Quaternion.identity, thisTransform);
        }
    }
}
