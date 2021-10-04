using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wash.Utilities;

public class DecalSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject decalPrefab;
    [SerializeField]
    private SkinnedMeshRenderer skinnedMesh;
    [SerializeField]
    private int decalPoolSize = 100;
    [SerializeField]
    private DecalSpawnZone[] decalSpawnZones;

    private DecalParent[] decalParents;

    // Maps a vector3 along a mesh that is a possible decal spot to its normal
    private Dictionary<Vector3, Vector3> decalPositionPool;

    Mesh bakedMesh;

    public void BuildAllDecals()
    {
        DestroyAllDecals();
        decalParents = FindObjectsOfType<DecalParent>();
        bakedMesh = new Mesh();
        skinnedMesh.BakeMesh(bakedMesh);

        BuildDecalPositionPool(decalPoolSize);

        // loop through each point in the pool and create it if it is in any of the zones
        foreach (KeyValuePair<Vector3, Vector3> pointPair in decalPositionPool)
        {
            DecalSpawnZone decalZone = FindDecalZoneForPoint(pointPair.Key);
            if (decalZone != null)
            {
                GameObject newGerm = CreateDecal(pointPair.Key, pointPair.Value);
                newGerm.GetComponent<GermProjector>().SetGermType(decalZone.germType);
            }
        }
    }

    private DecalSpawnZone FindDecalZoneForPoint(Vector3 point)
    {
        foreach (DecalSpawnZone zone in decalSpawnZones)
        {
            if (zone.ContainsPoint(point))
            {
                return zone;
            }
        }

        return null;
    }

    private void DestroyAllDecals()
    {
        foreach(GameObject decal in GameObject.FindGameObjectsWithTag("Decal"))
        {
            DestroyImmediate(decal);
        }
    }

   

    private void BuildDecalPositionPool(int numPointsToFind)
    {
        decalPositionPool = new Dictionary<Vector3, Vector3>();
        for (int i =0; i < numPointsToFind; i++)
        {
            Vector3[] meshPoints = bakedMesh.vertices;
            int[] tris = bakedMesh.triangles;
            int triStart = Random.Range(0, meshPoints.Length / 3) * 3; // get first index of each triangle

            float a = Random.value;
            float b = Random.value;

            if (a + b >= 1)
            { // reflect back if > 1
                a = 1 - a;
                b = 1 - b;
            }

            Vector3 newPointOnMesh = meshPoints[triStart] + (a * (meshPoints[triStart + 1] - meshPoints[triStart])) + (b * (meshPoints[triStart + 2] - meshPoints[triStart])); // apply formula to get new random point inside triangle

            newPointOnMesh = transform.TransformPoint(newPointOnMesh); // convert back to worldspace

            Vector3 rayOrigin = ((Random.onUnitSphere * 1f) + transform.position); // put the ray randomly around the transform
            Vector3 rayDirection = newPointOnMesh - rayOrigin;
            RaycastHit hitPoint;
            if (Physics.Raycast(rayOrigin, rayDirection, out hitPoint, 100f))
            {
                decalPositionPool.Add(newPointOnMesh, rayDirection);
            }
        }
    }

    public GameObject CreateDecal(Vector3 pointOnMesh, Vector3 normal)
    {
        Transform nearestParent = FindNearestDecalParent(pointOnMesh);
        GameObject newDecal = Instantiate(decalPrefab, pointOnMesh, Quaternion.LookRotation(normal, Vector3.up), transform);

        // set parent after so that it doesn't get affected by the weird scale of the eventual parent
        newDecal.transform.parent = nearestParent;

        return newDecal;
    }

    private Transform FindNearestDecalParent(Vector3 decalPoint)
    {
        Transform closest = decalParents[0].transform;
        float closestDistance = Vector3.Distance(decalPoint, closest.position);

        for(int i = 0; i < decalParents.Length; i++)
        {
            Transform currentParent = decalParents[i].transform;
            float currDistance = Vector3.Distance(decalPoint, currentParent.position);
            if (currDistance < closestDistance)
            {
                closestDistance = currDistance;
                closest = currentParent;
            }
        }

        return closest;
    }

    /*public void BuildDecal()
   {
       Mesh bakedMesh = new Mesh();
       skinnedMesh.BakeMesh(bakedMesh);
       Debug.Log("Baked");

       Vector3[] meshPoints = bakedMesh.vertices;
       int[] tris = bakedMesh.triangles;
       int triStart = Random.Range(0, meshPoints.Length / 3) * 3; // get first index of each triangle

       float a = Random.value;
       float b = Random.value;

       if (a + b >= 1)
       { // reflect back if > 1
           a = 1 - a;
           b = 1 - b;
       }

       Vector3 newPointOnMesh = meshPoints[triStart] + (a * (meshPoints[triStart + 1] - meshPoints[triStart])) + (b * (meshPoints[triStart + 2] - meshPoints[triStart])); // apply formula to get new random point inside triangle

       newPointOnMesh = transform.TransformPoint(newPointOnMesh); // convert back to worldspace

       Vector3 rayOrigin = ((Random.onUnitSphere * 1f) + transform.position); // put the ray randomly around the transform
       Vector3 rayDirection = newPointOnMesh - rayOrigin;
       RaycastHit hitPoint;
      // Debug.DrawRay(rayOrigin, rayDirection, Color.green, 100f, true);
       if (Physics.Raycast(rayOrigin, rayDirection, out hitPoint, 100f))
       {
           Debug.Log("We hit");
           Transform nearestParent = FindNearestDecalParent(newPointOnMesh);
           GameObject newDecal = Instantiate(decalPrefab, newPointOnMesh, Quaternion.LookRotation(rayDirection, Vector3.up), transform);

           // set parent after so that it doesn't get affected by the weird scale of the eventual parent
           newDecal.transform.parent = nearestParent;
       }
   }*/
}
