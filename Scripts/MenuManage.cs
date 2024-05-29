using UnityEngine;
using UnityEngine.SceneManagement;

public class TouchToChangeScene : MonoBehaviour
{
    // Name of the scene to load
   

    // Update is called once per frame
    void Update()
    {
        // Check if there is any touch input
        if (Input.touchCount > 0)
        {
            // Loop through all the touch inputs
            for (int i = 0; i < Input.touchCount; i++)
            {
                // Check if the touch input is on the "solve" sprite
                if (Input.GetTouch(i).phase == TouchPhase.Began)
                {
                    // Convert touch position to world position
                    Vector3 touchPosWorld = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);
                    // Convert world position to 2D
                    Vector2 touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);

                    // Check if the touch hits the collider of the "solve" sprite
                    RaycastHit2D hit = Physics2D.Raycast(touchPosWorld2D, Camera.main.transform.forward);

                    // If the hit collider is not null and it's the "solve" sprite
                    if (hit.collider != null && hit.collider.gameObject.CompareTag("solve"))
                    {
                        PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
                        // Change the scene to "solve"
                        SceneManager.LoadScene("solve");
                    }
                    else if (hit.collider != null && hit.collider.gameObject.CompareTag("set"))
                    {
                        PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
                        // Change the scene to "solve"
                        SceneManager.LoadScene("set");
                    }
                    else if (hit.collider != null && hit.collider.gameObject.CompareTag("settings"))
                    {
                        PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
                        // Change the scene to "solve"
                        SceneManager.LoadScene("settings");
                    }
                     else if (hit.collider != null && hit.collider.gameObject.CompareTag("backArrowMenu"))
                      {
                          PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
                          // Change the scene to "solve"
                          SceneManager.LoadScene("mainMenu");
                      }
                    else if (hit.collider != null && hit.collider.gameObject.CompareTag("easy"))
                    {
                        PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
                        // Change the scene to "solve"
                        SceneManager.LoadScene("easy");
                    }
                    else if (hit.collider != null && hit.collider.gameObject.CompareTag("medium"))
                    {
                        PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
                        // Change the scene to "solve"
                        SceneManager.LoadScene("medium");
                    }
                    else if (hit.collider != null && hit.collider.gameObject.CompareTag("hard"))
                    {
                        PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
                        // Change the scene to "solve"
                        SceneManager.LoadScene("hard");
                    }
                    else if (hit.collider != null && hit.collider.gameObject.CompareTag("backArrowSolve"))
                    {
                        PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
                        // Change the scene to "solve"
                        SceneManager.LoadScene("solve");
                    }
                }
            }
        }
    }
}
