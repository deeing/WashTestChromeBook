using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LlockhamIndustries.Decals;
using Wash.Utilities;

public class GermManager : SingletonMonoBehaviour<GermManager>
{
    [SerializeField]
    private PercentageBar germBar;

    [SerializeField]
    private GameObject[] handObjects;
    private List<Material> handMaterials;

    private Dictionary<GermType, List<GameObject>> allGerms;

    private int maxNumGerms = 0;
    private Dictionary<GermType, int> germMaxMap;

    protected override void Awake()
    {
        if (!InitializeSingleton(this))
        {
            return;
        }

        allGerms = new Dictionary<GermType, List<GameObject>>();
        handMaterials = new List<Material>();
        foreach (GameObject hand in handObjects)
        {
           handMaterials.Add(hand.GetComponent<SkinnedMeshRenderer>().material);
        }

        germMaxMap = new Dictionary<GermType, int>();
    }

    public List<GameObject> GetGermListOfType(GermType type)
    {
        if (allGerms.ContainsKey(type))
        {
            return allGerms[type];
        } else
        {
            return new List<GameObject>();
        }
    }

    public void RegisterGerm(GameObject germ, GermType type)
    {
        List<GameObject> germList = GetGermListOfType(type);
        germList.Add(germ);
        allGerms[type] = germList;
        maxNumGerms++;

        if (germMaxMap.ContainsKey(type))
        {
            germMaxMap[type] = germMaxMap[type] + 1;
        } else
        {
            germMaxMap[type] = 1;
        }
    }

    public void KillRandomGermOfType(GermType type)
    {
        List<GameObject> germList = GetGermListOfType(type);
        if (germList.Count <= 0)
        {
            return;
        }

        int randomIndex = Random.Range(0, germList.Count);
        GameObject randomGerm = germList[randomIndex];
        germList.RemoveAt(randomIndex);
        UpdateGermBar();
        UpdateHandTexture();
        UpdateSplotches();


        Destroy(randomGerm);
    }

    private int CountAllGerms()
    {
        int numGerms = 0;
        foreach(KeyValuePair<GermType, List<GameObject>> germPair in allGerms)
        {
            numGerms += germPair.Value.Count;
        }

        return numGerms;
    }

    private int CountGermsOfType(GermType type)
    {
        int numGerms = 0;
        foreach (KeyValuePair<GermType, List<GameObject>> germPair in allGerms)
        {
            if (germPair.Key == type)
            {
                numGerms += germPair.Value.Count;
            }
        }

        return numGerms;
    }

    private void UpdateGermBar()
    {
        germBar.UpdatePercentage((float)CountAllGerms() / (float)maxNumGerms);
    }

    private void UpdateHandTexture()
    {
        foreach (Material handMat in handMaterials)
        {
            handMat.SetFloat("_BlendAlpha", (float)CountAllGerms() / (float)maxNumGerms);
        }
    }

    private void UpdateSplotches()
    {
        //projectionFade.SetAlpha(GermPercentage());
        // projectionScale1.SetSize(GermPercentage());
        // projectionScale2.SetSize(GermPercentage());
    }

    public bool HasGerms()
    {
        return CountAllGerms() > 0;
    }

    public float GermPercentage()
    {
        return (float)CountAllGerms() / (float)maxNumGerms;
    }
    
    public float GermPercentageByType(GermType type)
    {
        return ((float)allGerms[type].Count / (float)germMaxMap[type]);
    }

    public bool HasGermsOfType(GermType type)
    {
        return allGerms[type].Count > 0;
    }

    public string GetGermReport()
    {
        string report = "\nPercentage of Germs Left:\n";
        foreach(KeyValuePair<GermType, int> germMapPair in germMaxMap)
        {
            report += germMapPair.Key.GetDescription() + ": " + (GermPercentageByType(germMapPair.Key) * 100) + "%\n";
        }

        return report;
    }
}