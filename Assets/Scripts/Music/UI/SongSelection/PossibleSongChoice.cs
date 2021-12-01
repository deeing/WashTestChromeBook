using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PossibleSongChoice : MonoBehaviour
{
    [SerializeField]
    TMP_Text songText;

    private SongData songData;

    public void Setup(SongData song)
    {
        songText.text = song.songAudio.name;
        songData = song;
    }

    public void ChooseThisSong()
    {
        SongSelection.instance.ChooseSong(songData);
    }
}
