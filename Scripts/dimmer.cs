using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string sceneName; // Name of the scene to transition to
    private Leaderboard leaderboard;
    private LeaderboardMed leaderboardMed;
    private LeaderboardHard leaderboardHard;

    public void LoadSceneMenu()
    {
        SceneManager.LoadScene("mainMenu");
        
    }
    public void LoadSceneBack()
    {
        if(sudokuGrid.currentSceneName=="easy")SceneManager.LoadScene("easy");
        else if (sudokuGrid.currentSceneName == "medium") SceneManager.LoadScene("medium");
        else if (sudokuGrid.currentSceneName == "hard") SceneManager.LoadScene("hard");
        
    }
    
}
