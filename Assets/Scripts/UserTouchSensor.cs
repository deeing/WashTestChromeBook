using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserTouchSensor : MonoBehaviour
{
	[SerializeField]
	private HandAnimations handAnimations;
	[SerializeField]
	private float pinchSensitivity = .01f;
	[SerializeField]
	private float twistSensitivity = .01f;

	[SerializeField]
	ParticleSystem horayParticles;

	private bool prayMode = true;

	private bool transitioningModes = false;

    private void Update()
    {
		if (transitioningModes)
        {
			return;
        }

		if (prayMode)
        {
			if (Lean.Touch.LeanGesture.GetPinchScale() < 1f)
			{
				float currFingerDistance = Lean.Touch.LeanGesture.GetScaledDistance();
				float lastFingerDistance = Lean.Touch.LeanGesture.GetLastScaledDistance();

				float pinchAmounnt = (lastFingerDistance - currFingerDistance) * pinchSensitivity;
				prayMode = handAnimations.Pray(pinchAmounnt);

				if (!prayMode)
                {
					StartCoroutine(TransitionModes("Scrub"));
                }
			}
		}
		else
        {
			float twistAmount = Mathf.Abs(Lean.Touch.LeanGesture.GetTwistDegrees()) * twistSensitivity;
			handAnimations.Scrub(twistAmount);
		}

    }

	private IEnumerator TransitionModes(string nextAnim)
    {
		horayParticles.Play();
		handAnimations.CrossFade(nextAnim, 1f);
		transitioningModes = true;
		yield return new WaitForSeconds(1f);
		transitioningModes = false;
    }
}
