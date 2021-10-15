using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using DG.Tweening;

public class VideoManager : SingletonMonoBehaviour<VideoManager>
{
    [SerializeField]
    private VideoPlayer videoPlayer;
    [SerializeField]
    private RawImage videoRender;

    public bool isFinished { get; private set; } = false;

    protected override void Awake()
    {
        if(!InitializeSingleton(this))
        {
            return;
        }
        videoPlayer.loopPointReached += FadeToBlack;
    }

    public void SetVideo(VideoClip clip)
    {
        videoPlayer.clip = clip;
        isFinished = false;
    }

    public void FadePlay(float duration)
    {
        videoRender.DOColor(Color.white, duration).OnComplete(Play);
    }

    public void Play()
    {
        videoPlayer.Play();
        isFinished = false;
    }

    public void FadeToBlack(VideoPlayer vp)
    {
        videoRender.DOColor(Color.black, 1f).OnComplete(FinishedPlaying);
    }

    public void FinishedPlaying()
    {
        isFinished = true;
    }

    public void HideVideoCanvas(float duration)
    {
        videoRender.DOFade(0f, duration);
    }
}
