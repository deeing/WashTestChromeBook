using UnityEngine;
using UnityEngine.UI;

public class UILine : MonoBehaviour
{
    [SerializeField]
    private Vector2 startPoint;
    [SerializeField]
    private Vector2 endPoint;
    [SerializeField]
    private RawImage lineImage;
    [SerializeField]
    private BoxCollider2D lineCollider;
    [SerializeField]
    private bool useCollider;

    private RectTransform thisTransform;
    private float offsetPercent = 1f;

    private void Awake()
    {
        thisTransform = (RectTransform)transform;
    }

    public void SetupLine(Vector2 start, Vector2 end, float thickness, Color lineColor)
    {
        startPoint = start;
        endPoint = end;

        float lineHeight = Vector2.Distance(startPoint, endPoint) * offsetPercent;

        thisTransform.sizeDelta = new Vector2(thickness, lineHeight);
        thisTransform.up = endPoint - startPoint;
        lineImage.color = lineColor;

        if (useCollider)
        {
            //lineCollider.enabled = true;
            lineCollider.size = thisTransform.sizeDelta;
        }
    }
}
