using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public TMP_Text timerText;
    private Leaderboard leaderboard;
    public float startTime = 0f; // Start time in seconds
    private sudokuGrid sudokuGrid;
    private float currentTime;
    public static float finalTime;

    private void Start()
    {
        currentTime = startTime;
    }

    private void Update()
    {
        if (sudokuGrid.endChecker == true) finalTime=currentTime;
        currentTime += Time.deltaTime;
        UpdateTimerText();
    }

    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);
        string timeString = string.Format("{0:00}:{1:00}", minutes, seconds);
        timerText.text = timeString;
    }

  
}
