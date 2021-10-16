using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using DG.Tweening;

public class VideoManager : MonoBehaviour
{
    [SerializeField]
    private VideoPlayer videoPlayer;
    [SerializeField]
    private RawImage videoRender;
    [SerializeField]
    private Image fadeBlackImage;

    public bool isFinished { get; private set; } = false;

    private void Awake()
    {
        videoPlayer.loopPointReached += FadeToBlack;
    }

    public void SetVideo(VideoClip clip)
    {
        videoRender.gameObject.SetActive(true);
        videoPlayer.clip = clip;
        isFinished = false;
    }

    public void FadePlay(float duration)
    {
        //videoRender.DOColor(Color.white, duration).OnComplete(Play);
        fadeBlackImage.DOFade(0f, duration);
        Play();
    }

    public void Play()
    {
        videoPlayer.Play();
        isFinished = false;
    }

    public void FadeToBlack(VideoPlayer vp)
    {
        //fadeBlackImage.DOFade(255f, 3f).OnComplete(FinishedPlaying);
        videoRender.DOColor(Color.black, 1f).OnComplete(FinishedPlaying);

    }

    public void FinishedPlaying()
    {
        isFinished = true;
    }

    public void HideVideoCanvas(float duration)
    {
        videoRender.DOFade(0f, duration);
        //fadeBlackImage.DOFade(0f, duration);
    }
}
