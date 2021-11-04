using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Wash/GameSettings")]

public class GameSettings : ScriptableObject
{
    [SerializeField]
    [Tooltip("How many points you get when you miss an input")]
    private int _missPoints = 0;
    public int missPoints { get => _missPoints; private set => _missPoints = value; }
    [SerializeField]
    [Tooltip("How many points you get when you hit an input at level 'good'")]
    private int _goodPoints = 1;
    public int goodPoints { get => _goodPoints; private set => _goodPoints = value; }
    [SerializeField]
    [Tooltip("How many points you get when you hit an input at level 'great'")]
    private int _greatPoints = 2;
    public int greatPoints { get => _greatPoints; private set => _greatPoints = value; }
    [SerializeField]
    [Tooltip("How many points you get when you hit an input at level 'perfect'")]
    private int _perfectPoints = 5;
    public int perfectPoints { get => _perfectPoints; private set => _perfectPoints = value; }

    private Dictionary<RhythmInputStatus, int> inputStatusToPoints;

    private void OnEnable()
    {
#if UNITY_EDITOR
        // use platform dependent compilation so it only exists in editor, otherwise it'll break the build
        if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
        {
            Init();
        }
#endif
    }

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        inputStatusToPoints = new Dictionary<RhythmInputStatus, int>();
        inputStatusToPoints[RhythmInputStatus.Miss] = missPoints;
        inputStatusToPoints[RhythmInputStatus.Good] = goodPoints;
        inputStatusToPoints[RhythmInputStatus.Great] = greatPoints;
        inputStatusToPoints[RhythmInputStatus.Perfect] = perfectPoints;
    }

    public int GetPointsForInputStatus(RhythmInputStatus inputStatus)
    {
        return inputStatusToPoints[inputStatus];
    }
}
