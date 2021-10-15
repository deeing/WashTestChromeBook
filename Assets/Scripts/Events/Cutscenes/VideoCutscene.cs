using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoCutscene : CutsceneEvent
{
    [SerializeField]
    private VideoClip videoClip;

    public override bool CheckEndEvent()
    {
        return VideoManager.instance.isFinished;
    }

    public override void DoEvent()
    {
        // playing video
    }

    public override void EndEvent()
    {
        VideoManager.instance.HideVideoCanvas(1f);
    }

    public override void SetupEvent()
    {
        VideoManager.instance.SetVideo(videoClip);
    }

    public override void StartEvent()
    {
        //VideoManager.instance.FadePlay(5f);
        VideoManager.instance.Play();
    }
}
