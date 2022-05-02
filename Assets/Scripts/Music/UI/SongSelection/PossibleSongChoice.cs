using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PossibleSongChoice : MonoBehaviour
{
    [SerializeField]
    TMP_Text songText;
    [SerializeField]
    private ToggleColor songColor;
    [SerializeField]
    private Button thisButton;
    [SerializeField]
    private GameObject lockImage;

    private SongData songData;

    public void Setup(SongData song)
    {
        songText.text = song.songAudio.name;
        songData = song;
    }

    public void ChooseThisSong()
    {
        SongSelection.instance.ChooseSong(songData);
        SongSelectionHintManager.instance.RegisterChoseSong();
    }

    public ToggleColor GetColor()
    {
        return songColor;
    }    

    public void SetLocked()
    {
        thisButton.interactable = false;
        lockImage.SetActive(true);
    }
}
