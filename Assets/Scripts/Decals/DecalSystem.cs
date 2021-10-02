using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject decalPrefab;
    [SerializeField]
    private SkinnedMeshRenderer skinnedMesh;
    [SerializeField]
    private int numDecals = 1;

    private DecalParent[] decalParents;

    public void BuildAllDecals()
    {
        DestroyAllDecals();
        decalParents = FindObjectsOfType<DecalParent>();
        for(int i = 0; i < numDecals; i++)
        {
            BuildDecal();
        }
    }

    private void DestroyAllDecals()
    {
        foreach(GameObject decal in GameObject.FindGameObjectsWithTag("Decal"))
        {
            DestroyImmediate(decal);
        }
    }

    public void BuildDecal()
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
        if(Physics.Raycast(rayOrigin, rayDirection, out hitPoint, 100f))
        {
            Debug.Log("We hit");
            Transform nearestParent = FindNearestDecalParent(newPointOnMesh);
            GameObject newDecal = Instantiate(decalPrefab, newPointOnMesh, Quaternion.LookRotation(rayDirection, Vector3.up), transform);

            // set parent after so that it doesn't get affected by the weird scale of the eventual parent
            newDecal.transform.parent = nearestParent;
        }
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
}
