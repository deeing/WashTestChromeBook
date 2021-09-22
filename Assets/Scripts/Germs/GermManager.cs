using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GermManager : SingletonMonoBehaviour<GermManager>
{
    [SerializeField]
    private PercentageBar germBar;

    private Dictionary<GermType, List<GameObject>> allGerms;

    private int maxNumGerms = 0;

    protected override void Awake()
    {
        if (!InitializeSingleton(this))
        {
            return;
        }

        allGerms = new Dictionary<GermType, List<GameObject>>();
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

    private void UpdateGermBar()
    {
        germBar.UpdatePercentage((float)CountAllGerms() / (float)maxNumGerms);
    }

    public bool HasGerms()
    {
        return CountAllGerms() > 0;
    }

    public bool HasGermsOfType(GermType type)
    {
        return allGerms[type].Count > 0;
    }
}