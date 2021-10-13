using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AlertMenu : MonoBehaviour
{
    [SerializeField]
    private TMP_Text alertText;

    private TopDropMenu topDrop;

    private void Start()
    {
        topDrop = GetComponent<TopDropMenu>();
    }

    public void Alert(string message, float duration)
    {
        alertText.text = message;
        topDrop.Show();
        StartCoroutine(HideAfterTime(duration));
    }

    private IEnumerator HideAfterTime(float duration)
    {
        yield return new WaitForSeconds(duration);
        topDrop.Hide();
    }
}
