using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingManager : MonoBehaviour
{
    public string sceneToLoad;
    private bool loadScene = false;
    private bool isLoaded = false;

    private void Start()
    {
        StartLoading();
    }
    public void StartLoading()
    {
        Debug.Log("started loading");
        if (!loadScene)
        {
            loadScene = true;
            StartCoroutine(LoadSceneAsync());
        }
        
    }

    IEnumerator LoadSceneAsync()
    {
        Debug.Log("started async");
        yield return new WaitForSeconds(1); // Ensure at least 1 second has passed

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneToLoad);
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress >= 0.9f && SceneLoaded.sceneReady)
            {
                asyncOperation.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
