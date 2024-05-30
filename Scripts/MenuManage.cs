using UnityEngine;
using UnityEngine.SceneManagement;

public class TouchToChangeScene : MonoBehaviour
{
    // Name of the scene to load

    private sudokuSaver sudokuSaver;
    private string saver;
    private string lastScene;
    private string whichSet;
    private string sudoku1;
    private string sudoku2;
    private string sudoku3;
    private string sudoku4;
    private string sudoku5;
    // Start is called before t
    // Update is called once per frame


    void Start()
    {
        saver = PlayerPrefs.GetString("Sudoku");
        lastScene = PlayerPrefs.GetString("PreviousScene");
        whichSet = PlayerPrefs.GetString("whichSet");
        sudoku1 = PlayerPrefs.GetString("Sudoku1");
        sudoku2 = PlayerPrefs.GetString("Sudoku2");
        sudoku3 = PlayerPrefs.GetString("Sudoku3");
        sudoku4 = PlayerPrefs.GetString("Sudoku4");
        sudoku5 = PlayerPrefs.GetString("Sudoku5");
        if(sudoku1 == "") PlayerPrefs.SetString("Sudoku1", "100000000000000000000000000000000000000000000000000000000000000000000000000000001");
        if (sudoku2 == "") PlayerPrefs.SetString("Sudoku2", "200000000000000000000000000000000000000000000000000000000000000000000000000000002");
        if (sudoku3 == "") PlayerPrefs.SetString("Sudoku3", "300000000000000000000000000000000000000000000000000000000000000000000000000000003");
        if (sudoku4 == "") PlayerPrefs.SetString("Sudoku4", "400000000000000000000000000000000000000000000000000000000000000000000000000000004");
        if (sudoku5 == "") PlayerPrefs.SetString("Sudoku5", "500000000000000000000000000000000000000000000000000000000000000000000000000000005");
    }

    void Update()

    {
        // Check if there is any touch input
        if (Input.touchCount > 0)
        {
            // Loop through all the touch inputs
            for (int i = 0; i < Input.touchCount; i++)
            {
                // Check if the touch input is on the "solve" sprite
                if (Input.GetTouch(i).phase == TouchPhase.Ended)
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
                        PlayerPrefs.SetString("whichSet", "set");
                        PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
                        // Change the scene to "solve"
                        SceneManager.LoadScene("savedList");
                        

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
                    else if (hit.collider != null && hit.collider.gameObject.CompareTag("custom"))
                    {
                        PlayerPrefs.SetString("whichSet", "custom");
                        PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
                        // Change the scene to "solve"
                        SceneManager.LoadScene("savedList");
                    }
                    else if (hit.collider != null && hit.collider.gameObject.CompareTag("backArrowSolve"))
                    {
                        PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
                        // Change the scene to "solve"
                        if(SceneManager.GetActiveScene().name == "Custom")
                        {

                            if (whichSet == "set")
                            {
                                SceneManager.LoadScene("mainMenu");
                                break;
                            }
                        }
                        SceneManager.LoadScene("solve");

                    }
                    else if (hit.collider != null && hit.collider.gameObject.CompareTag("sudoku1"))
                    {
                        PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
                        // Change the scene to "solve"
                        
                            PlayerPrefs.SetInt("number", 1);
                            SceneManager.LoadScene("Custom");
                        
                    }
                    else if (hit.collider != null && hit.collider.gameObject.CompareTag("sudoku2"))
                    {
                        PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
                        // Change the scene to "solve"
                       
                            PlayerPrefs.SetInt("number", 2);
                            SceneManager.LoadScene("Custom");
                        
                    }
                    else if (hit.collider != null && hit.collider.gameObject.CompareTag("sudoku3"))
                    {
                        PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
                        // Change the scene to "solve"
                        
                            PlayerPrefs.SetInt("number", 3);
                            SceneManager.LoadScene("Custom");
                        
                    }
                    else if (hit.collider != null && hit.collider.gameObject.CompareTag("sudoku4"))
                    {
                        PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
                        // Change the scene to "solve"
                        
                            PlayerPrefs.SetInt("number", 4);
                            SceneManager.LoadScene("Custom");
                        
                    }
                    else if (hit.collider != null && hit.collider.gameObject.CompareTag("sudoku5"))
                    {
                        PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
                        // Change the scene to "solve"
                        
                            PlayerPrefs.SetInt("number", 5);
                            SceneManager.LoadScene("Custom");

                    }
                }
            }
        }
    }
}
