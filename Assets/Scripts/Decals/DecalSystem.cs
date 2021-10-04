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
    [Tooltip("Min multiplier for germ decal projectors to add some randomness")]
    private float minScaleMultiplier = 1f;
    [SerializeField]
    [Tooltip("Max multiplier for germ decal projectors to add some randomness")]
    private float maxScaleMultiplier = 1f;
    [SerializeField]
    private DecalSpawnZone[] decalSpawnZones;


    private DecalParent[] decalParents;

    private Mesh bakedMesh;
    private MeshCollider meshCollider;


    public void BuildAllDecals()
    {
        meshCollider = GetComponent<MeshCollider>();
        DestroyAllDecals();
        decalParents = FindObjectsOfType<DecalParent>();
        bakedMesh = new Mesh();
        skinnedMesh.BakeMesh(bakedMesh);
        meshCollider.sharedMesh = null;
        meshCollider.sharedMesh = bakedMesh;


        foreach(DecalSpawnZone zone in decalSpawnZones)
        {
            //Dictionary<Vector3, Vector3> germPositions = zone.GenerateGermsForZone(transform, bakedMesh.vertices);
            Dictionary<Vector3, Vector3> germPositions = zone.GenerateGermsForZone();
            foreach (KeyValuePair<Vector3, Vector3> germPair in germPositions)
            {
                GameObject newGerm = CreateDecal(germPair.Key, germPair.Value);
                newGerm.GetComponent<GermProjector>().SetGermType(zone.germType);
            }
        }
    }

    private void DestroyAllDecals()
    {
        foreach(GameObject decal in GameObject.FindGameObjectsWithTag("Decal"))
        {
            DestroyImmediate(decal);
        }
    }

    public GameObject CreateDecal(Vector3 pointOnMesh, Vector3 normal)
    {
        Transform nearestParent = FindNearestDecalParent(pointOnMesh);
        GameObject newDecal = Instantiate(decalPrefab, pointOnMesh, Quaternion.identity, transform);

        float randomScaleMultiplier = Random.Range(minScaleMultiplier, maxScaleMultiplier);
        newDecal.transform.localScale = newDecal.transform.localScale * randomScaleMultiplier;

        // set parent after so that it doesn't get affected by the weird scale of the eventual parent
        newDecal.transform.parent = nearestParent;
        newDecal.transform.LookAt(pointOnMesh-normal);

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
}
