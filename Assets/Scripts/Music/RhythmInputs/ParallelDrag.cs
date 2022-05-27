using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RhythmTool;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class ParallelDrag : RhythmInput 
{
    [SerializeField]
    private MovingButton leftButton;
    [SerializeField]
    private MovingButton rightButton;
    [SerializeField]
    private bool debugFreeze = false;
    [SerializeField]
    private Image leftTutorialImage;
    [SerializeField]
    private Image rightTutorialImage;
    [SerializeField]
    private StatusDisplay statusDisplay;

    // suceeds if one of the fingers is touching
    private bool forgivingInput = true;

    private RhythmInputStatus leftStatus = RhythmInputStatus.Miss;
    private RhythmInputStatus rightStatus = RhythmInputStatus.Miss;

   // private Vector3 originalTutorialPosLeft;
    //private Vector3 originalTutorialPosRight;

    protected override void Awake()
    {
        base.Awake();
       // originalTutorialPosLeft = leftTutorialImage.position;
       // originalTutorialPosRight = rightTutorialImage.position;
    }

    private void OnDisable()
    {
        statusDisplay.HideStatusDisplay();
    }

    public override void HandleBeat(Beat currentBeat, Beat nextBeat)
    {
        if (debugFreeze)
        {
            return;
        }

        if (leftButton != null)
        {
            leftButton.Move(nextBeat.timestamp - currentBeat.timestamp);
        }

        if (rightButton != null)
        {
            rightButton.Move(nextBeat.timestamp - currentBeat.timestamp);
        }

    }

    public override RhythmInputStatus GetCurrentInputStatus()
    {
        RhythmInputStatus chosenStatus;

        if (forgivingInput)
        {
            // choose the greater of the two
            chosenStatus = leftStatus > rightStatus ? leftStatus : rightStatus;
        } else
        {
            // choose the weaker of the two
            chosenStatus = leftStatus < rightStatus ? leftStatus : rightStatus;
        }

      /*  Debug.Log("left: " + leftStatus);
        Debug.Log("right: " + rightStatus);
        Debug.Log("Chosen: " + chosenStatus);*/

        return chosenStatus;
    }

    public override void SetInput(RhythmInputStatus status, bool isLeftInput)
    {
        if (isLeftInput)
        {
            leftStatus = status;
        } else
        {
            rightStatus = status;
        }
    }

    protected override void DoMicroTutorial()
    {
        if (leftTutorialImage == null || rightTutorialImage == null)
        {
            Debug.Log("MISSING TUTORIAL HAND INMAGE FOR RHYTHM INPUT " + gameObject.name);
            return;
        }
        Debug.Log("Starting microtutorial");
        leftTutorialImage.DOFade(1f, 1f);
        rightTutorialImage.DOFade(1f, 1f);

        //leftTutorialImage.gameObject.SetActive(true);
        //rightTutorialImage.gameObject.SetActive(true);

        //leftTutorialImage.DOMove(leftButton.transform.position, 1f);
        //rightTutorialImage.DOMove(rightButton.transform.position, 1f);
    }

    protected override void StopMicroTutorial()
    {
        if (leftTutorialImage == null || rightTutorialImage == null)
        {
            Debug.Log("MISSING TUTORIAL HAND INMAGE FOR RHYTHM INPUT " + gameObject.name);
            return;
        }
        //Debug.Log("Ending microtutorial");
        leftTutorialImage.DOFade(0f, .2f);
        rightTutorialImage.DOFade(0f, .2f);
        // leftTutorialImage.gameObject.SetActive(false);
        // rightTutorialImage.gameObject.SetActive(false);

        //leftTutorialImage.position = originalTutorialPosLeft;
        //rightTutorialImage.position = originalTutorialPosRight;

        //leftTutorialImage.DOKill();
        //rightTutorialImage.DOKill();
    }
}
