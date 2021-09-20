using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GermManager : SingletonMonoBehaviour<GermManager>
{
    [SerializeField]
    private PercentageBar germBar;

    private List<GameObject> allGerms;

    private int maxNumGerms = 0;

    protected override void Awake()
    {
        if (!InitializeSingleton(this))
        {
            return;
        }

        allGerms = new List<GameObject>();
    }

    public void RegisterGerm(GameObject germ)
    {
        allGerms.Add(germ);
        maxNumGerms++;
    }

    public void KillRandomGerm()
    {
        if (allGerms.Count <= 0)
        {
            return;
        }

        int randomIndex = Random.Range(0, allGerms.Count);
        GameObject randomGerm = allGerms[randomIndex];
        allGerms.RemoveAt(randomIndex);
        UpdateGermBar();

        Destroy(randomGerm);
    }

    private void UpdateGermBar()
    {
        germBar.UpdatePercentage((float)allGerms.Count / (float)maxNumGerms);
    }

    public bool HasGerms()
    {
        return allGerms.Count > 0;
    }
}