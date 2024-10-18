using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingScreenController : MonoBehaviour
{
    // Name of the game scene to load
    public string gameSceneName = "easy";

    // Reference to the Animator for the loading screen animation
    public Animator loadingAnimator;

    // The asynchronous operation for loading the scene
    private AsyncOperation asyncLoad;

    // Start is called before the first frame update
    void Start()
    {
        // Start loading the game scene asynchronously
        StartCoroutine(LoadSceneAsync());
    }

    // Coroutine to load the game scene asynchronously
    IEnumerator LoadSceneAsync()
    {
        // Begin loading the scene in the background
        asyncLoad = SceneManager.LoadSceneAsync(gameSceneName);
        asyncLoad.allowSceneActivation = false; // Prevent the scene from auto-switching when loaded

        // Keep looping the animation while loading
        while (!asyncLoad.isDone)
        {
            // If the load has reached 90% (it can't complete until we set allowSceneActivation)
            if (asyncLoad.progress >= 0.9f)
            {
                // Check for game readiness (via PlayerPrefs or Game Manager)
                if (IsGameReady())
                {
                    // Activate the scene when the game is ready
                    asyncLoad.allowSceneActivation = true;
                }
            }
            yield return null;
        }
    }

    // A mockup function that checks whether the game scene is ready
    bool IsGameReady()
    {
        // This can be changed to your actual game logic for readiness.
        // For example, you might check a PlayerPrefs key or a GameManager variable.
        return PlayerPrefs.GetInt("GameReady", 0) == 1;
    }
}
