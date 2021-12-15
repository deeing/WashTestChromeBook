using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LlockhamIndustries.Decals;
using Wash.Utilities;

public class GermManager : SingletonMonoBehaviour<GermManager>
{
    [SerializeField]
    private GermBar germBar;

    [SerializeField]
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
        UpdateGermBarWithType(type);
        //UpdateHandTexture();

        Destroy(randomGerm);
    }

    public void KillRandomGermsOfType(GermType type, int numGermsToKill)
    {
        List<GameObject> germList = GetGermListOfType(type);
        if (germList.Count <= 0)
        {
            return;
        }

        int numGerms = Mathf.Min(numGermsToKill, germList.Count);
        //  Debug.Log("Killng " + numGerms);
        for (int i=0; i<numGerms; i++)
        {
            int randomIndex = Random.Range(0, germList.Count);
            GameObject randomGerm = germList[randomIndex];
            germList.RemoveAt(randomIndex);

            Destroy(randomGerm);
        }
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
    
    public void ShowGermBar(GermType type)
    {
        UpdateGermBarWithType(type);
        germBar.Show();
    }

    public void HideGermBar()
    {
        germBar.Hide();
    }

    private void UpdateGermBar()
    {
        germBar.UpdateGermPercentage((float)CountAllGerms() / (float)maxNumGerms);
    }

    private void UpdateGermBarWithType(GermType type)
    {
        germBar.UpdateGermPercentage(GermPercentageByType(type));
    }

    private void UpdateHandTexture()
    {
        foreach (Material handMat in handMaterials)
        {
            handMat.SetFloat("_BlendAlpha", (float)CountAllGerms() / (float)maxNumGerms);
        }
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

    public List<GermType> GetRemaningGermTypes()
    {
        List<GermType> remainingGerms = new List<GermType>();
        foreach (KeyValuePair<GermType, List<GameObject>> germMapPair in allGerms)
        {
            int germsLeft = germMapPair.Value.Count;

            if (germsLeft > 0)
            {
                remainingGerms.Add(germMapPair.Key);
            }
        }

        return remainingGerms;
    }
}