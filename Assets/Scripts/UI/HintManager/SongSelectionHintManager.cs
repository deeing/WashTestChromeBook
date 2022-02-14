using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongSelectionHintManager : SingletonMonoBehaviour<SongSelectionHintManager>
{
    [SerializeField]
    TutorialHintMenu chooseSongHint;
    [SerializeField]
    TutorialHintMenu chooseDifficultyHint;
    [SerializeField]
    TutorialHintMenu pressStartHint;
    [SerializeField]
    private float startDelay = 2f;

    private WaitForSeconds startWait;
    private bool choseSong = false;
    private bool choseDifficulty = false;

    protected override void Awake()
    {
        if (!InitializeSingleton(this))
        {
            return;
        }

        startWait = new WaitForSeconds(startDelay);
        StartCoroutine(ShowChooseSongHint());
    }

    private IEnumerator ShowChooseSongHint()
    {
        yield return startWait;

        if (!choseSong)
        {
            chooseSongHint.ToggleHintMenu(true);
        }
    }

    private void ShowChooseDifficultyHint()
    {
        if (!choseDifficulty)
        {
            chooseDifficultyHint.ToggleHintMenu(true);
        }
        else
        {
            pressStartHint.ToggleHintMenu(true);
        }
    }

    public void RegisterChoseSong()
    {
        choseSong = true;
        chooseSongHint.ToggleHintMenu(false);
        ShowChooseDifficultyHint();
    }

    public void RegisterChoseDifficulty()
    {
        choseDifficulty = true;
        chooseSongHint.ToggleHintMenu(false);
        chooseDifficultyHint.ToggleHintMenu(false);
        pressStartHint.ToggleHintMenu(true);
    }
}
