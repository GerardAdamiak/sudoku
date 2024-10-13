using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string sceneName; // Name of the scene to transition to
   

    public void LoadSceneMenu()
    {
        SceneManager.LoadScene("mainMenu");
        
    }
    public void LoadSceneBack()
    {
        if(SudokuGrid.currentSceneName=="easy")SceneManager.LoadScene("easy");
        else if (SudokuGrid.currentSceneName == "medium") SceneManager.LoadScene("medium");
        else if (SudokuGrid.currentSceneName == "hard") SceneManager.LoadScene("hard");
        
    }
    
}
