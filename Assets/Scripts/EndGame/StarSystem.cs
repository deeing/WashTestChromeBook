using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarSystem : MonoBehaviour
{
    [SerializeField]
    private Star[] stars;

    private float maxPoints;
    private int currentStarIndex;
    private float pointsPerStar;
    private float currentPoints = 0f;

    public void Init(float max)
    {
        maxPoints = max;
        currentStarIndex = 0;
        pointsPerStar = maxPoints / stars.Length;
    }

    // have this called from total score increase
    public void IncreaseBy(float amount, float time)
    {
        Star currentStar = stars[currentStarIndex];
       
        // catch the overflow star case
        if (currentPoints + amount > pointsPerStar)
        {
            float firstStarTime = time / 2;
            float secondStarTime = time / 2;

            if (currentStarIndex + 1 < stars.Length)
            {
                currentPoints = currentPoints + amount - pointsPerStar;

                currentStar.SetStarFill(1f, firstStarTime).OnComplete(()=>
                {
                    currentStarIndex++;
                    currentStar = stars[currentStarIndex];
                    currentStar.SetStarFill(currentPoints / pointsPerStar, secondStarTime);
                });
            }
            else
            {
                currentStar.SetStarFill(1f, firstStarTime);
            }
        }
        else
        {
            currentPoints += amount;
            currentStar.SetStarFill(currentPoints / pointsPerStar, time);

        }
       
    }
}
