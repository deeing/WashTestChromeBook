using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoCutscene : CutsceneEvent
{
    [SerializeField]
    private VideoClip videoClip;

    private VideoManager videoManager;

    public override void ChangeEvent()
    {
        videoManager.HideVideoCanvas(1f);
    }

    public override bool CheckEndEvent()
    {
        return videoManager.isFinished;
    }

    public override void DoEvent()
    {
        // playing video
    }

    public override void EndEvent()
    {
        videoManager.HideVideoCanvas(1f);
    }

    public override void SetupEvent()
    {
        videoManager = WashEventManager.instance.videoManager;
        videoManager.SetVideo(videoClip);
    }

    public override void StartEvent()
    {
        videoManager.FadePlay(3f);
        //videoManager.Play();
    }
}
