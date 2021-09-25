using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkinnedDecals;
public class DecalProjector : MonoBehaviour
{
	[SerializeField]
	private SkinnedDecal decal;
	[SerializeField]
	private SkinnedDecalSystem skinnedDecalSystems;
	[SerializeField]
	[Tooltip("Whether or not there should be a slight randomization in the projection direction")]
	private bool randomizeDirection = true;
	[SerializeField]
	[Tooltip("How much in world coordinates (x,y) from the forward transform should the projection deviate from the center")]
	private float deviationRange = .5f;
	[SerializeField]
	private int numProjections = 1;
	[SerializeField]
	[Tooltip("Delay between the projections (this helps with randomization angles)")]
	private float projectionDelay = .05f;

	private Transform thisTransform;
	private Vector3 originalRotation;
	private WaitForSeconds projectionWait;

    private void Start()
    {
		thisTransform = transform;
		originalRotation = thisTransform.eulerAngles;
		projectionWait = new WaitForSeconds(projectionDelay);

		StartCoroutine(ProjectDecalCo());
	}

	private IEnumerator ProjectDecalCo()
    {
		for (int i =0; i < numProjections; i++)
        {
			yield return projectionWait;
			ProjectDecal();
		}
    }

    public void ProjectDecal()
	{
		Vector3 projectionAngle = thisTransform.forward;

		if (randomizeDirection)
		{
			projectionAngle += new Vector3(RandomDeviation(), RandomDeviation(), 0f);
		}
		skinnedDecalSystems.CreateDecal(decal, thisTransform.position, projectionAngle, thisTransform.up);

		// set back
		if (randomizeDirection)
        {
			thisTransform.eulerAngles = originalRotation;
        }
	}

	private float RandomDeviation()
    {
		return Random.Range(-deviationRange, deviationRange);
    }
}
