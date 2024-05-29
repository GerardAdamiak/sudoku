using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour
{
    private string previousSceneName;

    void Start()
    {
        // Get the name of the previously loaded scene
        previousSceneName = PlayerPrefs.GetString("PreviousScene", "MainMenu");
    }

    void Update()
    {
        // Check for touch input
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            // Get the touch position
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

            // Check if the touch is over this sprite
            Collider2D hitCollider = Physics2D.OverlapPoint(touchPos);
            if (hitCollider != null && hitCollider.gameObject == gameObject)
            {
                // Check if the touched sprite has a specific tag
                if (hitCollider.CompareTag("backArrow"))
                {
                    // Load the previous scene
                    SceneManager.LoadScene(previousSceneName);
                }
            }
        }
    }
}
