using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TouchToChangeScene : MonoBehaviour
{
    public AudioSource audioSource; // Reference to the AudioSource component
    public AudioClip clickSound; // Reference to the click sound clip

   

    private string whichSet;
    private string sudoku1;
    private string sudoku2;
    private string sudoku3;
    private string sudoku4;
    private string sudoku5;

    void Start()
    {
       
       
        whichSet = PlayerPrefs.GetString("whichSet");
        sudoku1 = PlayerPrefs.GetString("Sudoku1");
        sudoku2 = PlayerPrefs.GetString("Sudoku2");
        sudoku3 = PlayerPrefs.GetString("Sudoku3");
        sudoku4 = PlayerPrefs.GetString("Sudoku4");
        sudoku5 = PlayerPrefs.GetString("Sudoku5");

        if (sudoku1 == "") PlayerPrefs.SetString("Sudoku1", "100000000000000000000000000000000000000000000000000000000000000000000000000000001");
        if (sudoku2 == "") PlayerPrefs.SetString("Sudoku2", "200000000000000000000000000000000000000000000000000000000000000000000000000000002");
        if (sudoku3 == "") PlayerPrefs.SetString("Sudoku3", "300000000000000000000000000000000000000000000000000000000000000000000000000000003");
        if (sudoku4 == "") PlayerPrefs.SetString("Sudoku4", "400000000000000000000000000000000000000000000000000000000000000000000000000000004");
        if (sudoku5 == "") PlayerPrefs.SetString("Sudoku5", "500000000000000000000000000000000000000000000000000000000000000000000000000000005");
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                if (Input.GetTouch(i).phase == TouchPhase.Ended)
                {
                    Vector3 touchPosWorld = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);
                    Vector2 touchPosWorld2D = (Vector2)touchPosWorld;

                    RaycastHit2D hit = Physics2D.Raycast(touchPosWorld2D, Camera.main.transform.forward);

                    if (hit.collider != null)
                    {
                        string sceneToLoad = null;

                        if (hit.collider.gameObject.CompareTag("solve"))
                        {
                            PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
                            sceneToLoad = "solve";
                        }
                        if (hit.collider.gameObject.CompareTag("backArrowVariant"))
                        {
                            PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
                            sceneToLoad = "variants";
                        }
                        else if (hit.collider.gameObject.CompareTag("set"))
                        {
                            PlayerPrefs.SetString("whichSet", "set");
                            PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
                            sceneToLoad = "savedList";
                        }

                        else if (hit.collider.gameObject.CompareTag("settings"))
                        {
                            PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
                            sceneToLoad = "settings";
                        }
                        else if (hit.collider.gameObject.CompareTag("podium"))
                        {
                            PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
                            sceneToLoad = "podium";
                        }
                        else if (hit.collider.gameObject.CompareTag("whispers"))
                        {
                            PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
                            sceneToLoad = "whispers";
                        }
                        else if (hit.collider.gameObject.CompareTag("backArrowMenu"))
                        {
                            PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
                            sceneToLoad = "mainMenu";
                        }
                        else if (hit.collider.gameObject.CompareTag("thermo"))
                        {
                            PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
                            sceneToLoad = "thermo";
                        }
                        else if (hit.collider.gameObject.CompareTag("kropki"))
                        {
                            PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
                            sceneToLoad = "kropki";
                        }
                        else if (hit.collider.gameObject.CompareTag("renban"))
                        {
                            PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
                            sceneToLoad = "renban";
                        }
                        else if (hit.collider.gameObject.CompareTag("killer"))
                        {
                            PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
                            sceneToLoad = "killer";
                        }
                        else if (hit.collider.gameObject.CompareTag("easy"))
                        {
                            PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
                            sceneToLoad = "LoadEasy";
                        }
                        else if (hit.collider.gameObject.CompareTag("medium"))
                        {
                            PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
                            sceneToLoad = "LoadMed";
                        }
                        else if (hit.collider.gameObject.CompareTag("hard"))
                        {
                            PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
                            sceneToLoad = "LoadingScene";
                        }
                        else if (hit.collider.gameObject.CompareTag("variants"))
                        {
                            PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
                            sceneToLoad = "variants";
                        }
                        else if (hit.collider.gameObject.CompareTag("custom"))
                        {
                            PlayerPrefs.SetString("whichSet", "custom");
                            PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
                            sceneToLoad = "savedList";
                        }
                        else if (hit.collider.gameObject.CompareTag("backArrowSolve"))
                        {
                            PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
                            if (SceneManager.GetActiveScene().name == "Custom")
                            {
                                if (whichSet == "set")
                                {
                                    sceneToLoad = "mainMenu";
                                }
                            }
                            sceneToLoad ??= "solve";
                        }

                        else if (hit.collider.gameObject.CompareTag("sudoku1"))
                        {
                            PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
                            PlayerPrefs.SetInt("number", 1);
                            sceneToLoad = "Custom";
                        }
                        else if (hit.collider.gameObject.CompareTag("sudoku2"))
                        {
                            PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
                            PlayerPrefs.SetInt("number", 2);
                            sceneToLoad = "Custom";
                        }
                        else if (hit.collider.gameObject.CompareTag("sudoku3"))
                        {
                            PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
                            PlayerPrefs.SetInt("number", 3);
                            sceneToLoad = "Custom";
                        }
                        else if (hit.collider.gameObject.CompareTag("sudoku4"))
                        {
                            PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
                            PlayerPrefs.SetInt("number", 4);
                            sceneToLoad = "Custom";
                        }
                        else if (hit.collider.gameObject.CompareTag("sudoku5"))
                        {
                            PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
                            PlayerPrefs.SetInt("number", 5);
                            sceneToLoad = "Custom";
                        }
                        else if (hit.collider.gameObject.CompareTag("restart"))
                        {
                            string test = PlayerPrefs.GetString("PreviousScene");
                            sceneToLoad = test;
                        }

                        if (sceneToLoad != null)
                        {
                            StartCoroutine(PlaySoundAndChangeScene(sceneToLoad));
                        }
                    }
                }
            }
        }
    }

    private IEnumerator PlaySoundAndChangeScene(string sceneName)
    {
        if (clickSound != null)
        {
            // Play the sound or do whatever is needed with it
            audioSource.PlayOneShot(clickSound);
            yield return new WaitForSeconds(clickSound.length);
            SceneManager.LoadScene(sceneName);
        }
        
    
    
    }
}
