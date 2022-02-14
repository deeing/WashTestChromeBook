using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextScene : MonoBehaviour
{
    [SerializeField]
    private string sceneToLoad;
    [SerializeField]
    private PercentageBar loadingBar;
    [SerializeField]
    private float loadDelay = 1f;
    [SerializeField]
    private bool loadOnAwake = true;

    private AsyncOperation loadingOperation;
    private WaitForSeconds delayWait;

    private void Awake()
    {
        if (loadOnAwake)
        {
            StartCoroutine(LoadOnDelay());
        }
    }

    private IEnumerator LoadOnDelay()
    {
        yield return delayWait;
        LoadScene();
    }

    public void LoadScene()
    {
        loadingOperation = SceneManager.LoadSceneAsync(sceneToLoad);
    }

    void Update()
    {
        if (loadingBar && loadingOperation != null)
        {
            float progressValue = Mathf.Clamp01(loadingOperation.progress / 0.9f);
            loadingBar.UpdatePercentage(progressValue);
        }
    }
}
