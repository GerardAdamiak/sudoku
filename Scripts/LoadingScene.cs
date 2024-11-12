using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    public GameObject LoadingScreen;
    public Image imageComponent;       // Reference to the UI Image component
    public Sprite[] frames;            // Array of sprites for animation
    public float frameRate = 0.05f;    // Time between frames in seconds

    private Coroutine animationCoroutine;

    void Start()
    {
        LoadingScreen.SetActive(false);
    }

    public void LoadScene(int SceneId)
    {
        StartCoroutine(LoadSceneAsync(SceneId));
    }

    IEnumerator LoadSceneAsync(int SceneId)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneId);
        operation.allowSceneActivation = false;  // Prevents scene from activating immediately

        LoadingScreen.SetActive(true);

        // Start the image animation
        animationCoroutine = StartCoroutine(AnimateImage());

        // Wait for 2 seconds to ensure the animation has played
        yield return new WaitForSeconds(0.9f);

        // Continue waiting until the loading is complete
        while (!operation.isDone)
        {
            if (operation.progress >= 0.9f)
            {
                // Stop the animation coroutine before scene activation
                StopCoroutine(animationCoroutine);
                operation.allowSceneActivation = true;  // Activates the scene after loading
            }
            yield return null;
        }
    }

    IEnumerator AnimateImage()
    {
        int index = 0;
        while (true)
        {
            imageComponent.sprite = frames[index];
            index = (index + 1) % frames.Length;
            yield return new WaitForSeconds(frameRate);
        }
    }
}
