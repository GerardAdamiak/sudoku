using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public TMP_Text timerText;
    public float startTime = 0f; // Start time in seconds
    private float currentTime;
    public static float finalTime;
    private string currentSceneName;

    private void Start()
    {
        currentTime = startTime;
        currentSceneName = SceneManager.GetActiveScene().name;
    }

    private void Update()
    {
        if (SudokuGrid.endChecker == true) finalTime=currentTime;
        currentTime += Time.deltaTime;
        UpdateTimerText();


        string whichSet = PlayerPrefs.GetString("whichSet", "");

        if ((whichSet != "custom") && (currentSceneName!="easy") && (currentSceneName != "medium") && (currentSceneName != "hard"))
        {
            // If whichSet is "custom", hide the sprite and make it unclickable
            gameObject.SetActive(false); // Hide the GameObject
            return; // Exit Start() early
        }
    }

    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);
        string timeString = string.Format("{0:00}:{1:00}", minutes, seconds);
        timerText.text = timeString;
    }

  
}
