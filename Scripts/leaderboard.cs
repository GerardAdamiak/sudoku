using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Leaderboard : MonoBehaviour
{
    public TMP_Text leaderboardText;
    public List<float> bestTimes = new List<float>();
    private int SavedListCount;
    private const int maxEntries = 10;
    private Timer timer;
    public static bool ifAdded = false;
    private string currentSceneName;
    private float mostRecentTime; // Store the most recent time
    private int isLight;

    private void Start()
    {
        isLight = PlayerPrefs.GetInt("IsLight");
        currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName == "Custom")
        {
            gameObject.SetActive(false); // Hide the GameObject
            return; // Exit Start() early
        }
    }

    public void LoadData()
    {
        SavedListCount = PlayerPrefs.GetInt("Count");
        for (int i = 0; i < SavedListCount; i++)
        {
            float time = PlayerPrefs.GetFloat("Players" + i);
            bestTimes.Add(time);
        }
    }

    public void SaveLeaderboard()
    {
        for (int i = 0; i < bestTimes.Count; i++)
        {
            PlayerPrefs.SetFloat("Players" + i, bestTimes[i]);
        }

        PlayerPrefs.SetInt("Count", bestTimes.Count);
    }

    private void Update()
    {
        if (sudokuGrid.endChecker == true && ifAdded == false && sudokuGrid.currentSceneName == "easy")
        {
            LoadData();
            AddTime(Timer.finalTime - 2);
            SaveLeaderboard();
            UpdateLeaderboard();

            ifAdded = true;
        }
    }

    public void AddTime(float time)
    {
        bestTimes.Add(time);
        bestTimes.Sort();

        // Ensure the list does not exceed the maximum number of entries
        if (bestTimes.Count > maxEntries)
        {
            bestTimes.RemoveAt(bestTimes.Count - 1);
        }

        mostRecentTime = time; // Update the most recent time
    }

    private void UpdateLeaderboard()
    {
        string headerColor = isLight == 0 ? "#EFEFD0" : "#2E3138";
        leaderboardText.text = $"<color={headerColor}>Best Times Easy:</color>\n";
        for (int i = 0; i < bestTimes.Count; i++)
        {
            if (isLight == 0)
            {
                if (bestTimes[i] == mostRecentTime)
                {
                    // Use rich text to color the most recent time differently
                    
                    leaderboardText.text += $"<color=#EFEFD0>{i + 1}. {FormatTime(bestTimes[i])}</color>\n";
                }
                else
                {
                    leaderboardText.text += $"{i + 1}. {FormatTime(bestTimes[i])}\n";

                }
            }
            else
            {
                if (bestTimes[i] == mostRecentTime)
                {
                    // Use rich text to color the most recent time differently
                    leaderboardText.text += $"<color=#2E3138>{i + 1}. {FormatTime(bestTimes[i])}</color>\n";

                }
                else
                {
                    leaderboardText.text += $"{i + 1}. {FormatTime(bestTimes[i])}\n";
                    
                }
            }
        }
    }

    private string FormatTime(float time)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(time);
        return string.Format("{0:00}:{1:00}", timeSpan.Minutes, timeSpan.Seconds);
    }
}
