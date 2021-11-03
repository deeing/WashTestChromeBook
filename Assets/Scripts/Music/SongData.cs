using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RhythmTool;

[CreateAssetMenu(fileName = "SongData", menuName = "Wash/SongData")]
public class SongData : ScriptableObject
{
    [SerializeField]
    private AudioClip _songAudio;
    public AudioClip songAudio { get => _songAudio; private set => _songAudio = value; }
    [SerializeField]
    private RhythmData _songRhythmData;
    public RhythmData songRhythmData { get => _songRhythmData; private set => _songRhythmData = value; }
    [SerializeField]
    private int _beatsPerMeasure = 4;
    public int beatsPerMeasure { get => _beatsPerMeasure; private set => _beatsPerMeasure = value; }
    [SerializeField]
    [Tooltip("How often in beats should the inputs switch (makes the game easier or harder)")]
    private int _beatsPerInputPeriod = 2;
    public int beatsPerInputPeriod { get => _beatsPerInputPeriod; private set => _beatsPerInputPeriod = value; }
    [SerializeField]
    [Tooltip("How many beats into the song should we begin?")]
    private int _startingBeatOffset = 1;
    public int startingBeatOffset { get => _startingBeatOffset; private set => _startingBeatOffset = value; }
}
