using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wash.Utilities;
using Random = UnityEngine.Random;

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

    public void DisplayPoseOptions(List<MusicSwitchEvent> starterEvents, MusicSwitchEvent currentEvent, int numPoseOptions)
    {
        List<MusicSwitchEvent> randomEvents = starterEvents.Shuffle();
        randomEvents.Remove(currentEvent);

        // insert the correct option in one of the first four slots
        randomEvents.Insert(Random.Range(0, numPoseOptions), currentEvent);

        for (int i=0; i < numPoseOptions; i++)
        {
            GameObject optionObj = Instantiate(poseOptionPrefab, poseOptionContainer);
            PoseOption option = optionObj.GetComponent<PoseOption>();
            option.SetupPoseOption(randomEvents[i], currentEvent);
        }

        topDropMenu.Show();
    }

    public void DisplayPoseOptions(MusicSwitchEvent currentEvent)
    {
        GameObject optionObj = Instantiate(poseOptionPrefab, poseOptionContainer);
        PoseOption option = optionObj.GetComponent<PoseOption>();
        option.SetupPoseOption(currentEvent, currentEvent);

        topDropMenu.Show();
    }

    public void Hide()
    {
        poseOptionContainer.DestroyAllChildren();
        topDropMenu.Hide();
    }
}
