using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wash.Utilities;

public class DecalSpawnZone : MonoBehaviour
{
    public GermType germType  = GermType.Palm;
   
    [SerializeField]
    private float radius = 1f;
    [SerializeField]
    private float numGerms = 100f;

    // max number of times the zone can fail to generate a germ before we quit
    private int maxFails = 1000;

    // currently unused - generates splotch by mesh triangles
    public Dictionary<Vector3, Vector3> GenerateGermsForZone(Transform meshTransform, Vector3[] meshPoints)
    {
        List<int> triangleStartIndexInZone = new List<int>();
        Dictionary<Vector3, Vector3> germPositions = new Dictionary<Vector3, Vector3>();

        // find triangles that start in the zone
        for (int i = 0; i < meshPoints.Length; i += 3)
        {
            Vector3 triangleStart = meshTransform.TransformPoint(meshPoints[i]);
            Vector3 triangle2 = meshTransform.TransformPoint(meshPoints[i + 1]);
            Vector3 triangle3 = meshTransform.TransformPoint(meshPoints[i + 1]);

            if (ContainsPoint(triangleStart) || ContainsPoint(triangle2) || ContainsPoint(triangle3))
            {
                triangleStartIndexInZone.Add(i);
            }
        }

        int currentTries = 0;
        while (currentTries < numGerms + maxFails && germPositions.Count < numGerms)
        {
            int triStart = triangleStartIndexInZone.RandomElement();

            float a = Random.value;
            float b = Random.value;

            if (a + b >= 1)
            { // reflect back if > 1
                a = 1 - a;
                b = 1 - b;
            }

            Vector3 newPointOnMesh = meshPoints[triStart] + (a * (meshPoints[triStart + 1] - meshPoints[triStart])) + (b * (meshPoints[triStart + 2] - meshPoints[triStart])); // apply formula to get new random point inside triangle

            newPointOnMesh = meshTransform.TransformPoint(newPointOnMesh); // convert back to worldspace

            Vector3 rayOrigin = transform.position; // put the ray randomly around the transform
            Vector3 rayDirection = newPointOnMesh - rayOrigin;
            RaycastHit hitPoint;
            if (Physics.Raycast(rayOrigin, rayDirection, out hitPoint, 100f))
            {
                germPositions.Add(newPointOnMesh, rayDirection);
            }
            currentTries++;
        }

        //Debug.Log("In the end we found " + germPositions.Count + " for " + germType);

        return germPositions;
    }


    // shoots out random rays and makes a projector wherever it hits (requires mesh collider to work well)
    public Dictionary<Vector3, Vector3> GenerateGermsForZone()
    {
        int currentTries = 0;
        Dictionary<Vector3, Vector3> germPositions = new Dictionary<Vector3, Vector3>();
        while (currentTries < numGerms + maxFails && germPositions.Count < numGerms)
        {
            if (Physics.Raycast(transform.position, Random.onUnitSphere, out RaycastHit hitPoint, 100f))
            {
                if (ContainsPoint(hitPoint.point))
                {
                    germPositions.Add(hitPoint.point, hitPoint.normal);
                }
            }
            currentTries++;
        }
        return germPositions;
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
