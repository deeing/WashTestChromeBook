using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wash.Utilities;

public class DecalSpawnZone : MonoBehaviour
{
    [SerializeField]
    float radius = 1f;
    public GermType germType  = GermType.Palm;

    public Dictionary<Vector3, Vector3> GetPointsInZone(Dictionary<Vector3, Vector3> decalPositionPool)
    {
        Transform thisTransform = transform;
        Dictionary<Vector3, Vector3> pointsInZone = new Dictionary<Vector3, Vector3>();

        foreach (KeyValuePair<Vector3, Vector3> pointPair in decalPositionPool)
        {
            if (Vector3.Distance(thisTransform.position, pointPair.Key) < radius)
            {
                pointsInZone.Add(pointPair.Key, pointPair.Value);
            }
        }

        return pointsInZone;
    }

    public bool ContainsPoint(Vector3 point)
    {
        return Vector3.Distance(transform.position, point) < radius;
    }

    void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        Gizmos.color = new Color(1, 1, 0, 1);
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
