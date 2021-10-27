using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wash.Utilities;

public class PoseOptionsMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject poseOptionPrefab;
    [SerializeField]
    private Transform poseOptionContainer;

    private TopDropMenu topDropMenu;

    private void Awake()
    {
        topDropMenu = GetComponent<TopDropMenu>();
    }

    public void DisplayPoseOptions(List<MusicSwitchEvent> starterEvents, MusicSwitchEvent currentEvent)
    {
        foreach(MusicSwitchEvent starterEvent in starterEvents)
        {
            GameObject optionObj = Instantiate(poseOptionPrefab, poseOptionContainer);
            PoseOption option = optionObj.GetComponent<PoseOption>();
            option.SetupPoseOption(starterEvent, currentEvent);

        }
        
        topDropMenu.Show();
    }

    public void Hide()
    {
        poseOptionContainer.DestroyAllChildren();
        topDropMenu.Hide();
    }
}
