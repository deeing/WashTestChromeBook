using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Wash.Utilities;

public class UILineRenderer : MonoBehaviour
{

	[SerializeField]
	private List<Vector2> positions;
	[SerializeField]
	private GameObject linePrefab;
	[SerializeField]
	private Transform lineContainer;
	[SerializeField]
	private float thickness = 20f;
	[SerializeField]
	private Color lineColor = Color.green;

	private List<GameObject> lines = new List<GameObject>();

	public void SetPositions(List<Vector2> pos)
    {
		positions = pos;
    }

	public void AddPosition(Vector2 pos)
    {
		positions.Add(pos);
    }

	public void RemovePosition(Vector2 pos)
    {
		positions.Remove(pos);
    }

	public void RemoveAllPositions()
    {
		positions = new List<Vector2>();
    }

	public void RemoveAllExcept(List<Vector2> exemptList)
    {
		List<Vector2> renwedList = new List<Vector2>();

		foreach (Vector2 pos in positions)
        {
			if (exemptList.Contains(pos) && !renwedList.Contains(pos))
            {
				renwedList.Add(pos);
            }
        }

		positions = renwedList;
    }

	public void RenderLines()
    {
		ClearLines();
		Debug.Log("rendering:" + positions.Count);
		if (positions.Count < 2)
        {
			return;
        }

		for (int i=0; i < positions.Count - 1; i++)
        {
			Vector2 startPos = positions[i];
			Vector2 endPos = positions[i + 1];

			Vector2 linePos = (endPos  + startPos) / 2;
			GameObject lineObj = Instantiate(linePrefab, linePos, Quaternion.identity, lineContainer);
			lines.Add(lineObj);
			UILine line = lineObj.GetComponent<UILine>();
			line.SetupLine(startPos, endPos, thickness, lineColor);
		}
    }

	public void ClearLines()
    {
		lineContainer.DestroyAllChildren();
    }
}