using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LeaderboardHard : MonoBehaviour
{
    public TMP_Text leaderboardText;
    public List<float> bestTimesHard = new();
    private int SavedListCountHard;
    private const int maxEntries = 10;

    public static bool ifAddedHard = false;
    private string previousScene;
    private float mostRecentTime; // Store the most recent time
    private int isLight;

    private void Start()
    {
        isLight = PlayerPrefs.GetInt("IsLight");
        previousScene = PlayerPrefs.GetString("PreviousScene");
        if ((previousScene == "Custom"))
        {
            // If whichSet is "custom", hide the sprite and make it unclickable
            gameObject.SetActive(false); // Hide the GameObject
            return; // Exit Start() early
        }
    }

    public void LoadData()
    {


        SavedListCountHard = PlayerPrefs.GetInt("CountHard");
        for (int i = 0; i < SavedListCountHard; i++)
        {
            float time = PlayerPrefs.GetFloat("PlayersHard" + i);
            bestTimesHard.Add(time);
        }


    }


    public void SaveLeaderboard()
    {
        for (int i = 0; i < bestTimesHard.Count; i++)
        {
            PlayerPrefs.SetFloat("PlayersHard" + i, bestTimesHard[i]);

        }

        PlayerPrefs.SetInt("CountHard", bestTimesHard.Count);
    }

    private void Update()
    {

        if (SudokuGrid.endChecker == true && ifAddedHard == false && SudokuGrid.currentSceneName == "hard")
        {
            LoadData();
            AddTime(Timer.finalTime - 2);
            SaveLeaderboard();
            UpdateLeaderboard();

            ifAddedHard = true;
        }


    }

    public void AddTime(float time)
    {
        bestTimesHard.Add(time);
        bestTimesHard.Sort();

        // Ensure the list does not exceed the maximum number of entries
        if (bestTimesHard.Count > maxEntries)
        {
            bestTimesHard.RemoveAt(bestTimesHard.Count - 1);
        }

        mostRecentTime = time;
    }

    private void UpdateLeaderboard()
    {
        string headerColor = isLight == 0 ? "#EFEFD0" : "#2E3138";
        leaderboardText.text = $"<color={headerColor}><size=120>Best Times Hard:</size></color>\n\n";
        for (int i = 0; i < bestTimesHard.Count; i++)
        {
            if (isLight == 0)
            {
                if (bestTimesHard[i] == mostRecentTime)
                {
                    // Use rich text to color the most recent time differently

                    leaderboardText.text += $"<color=#EFEFD0>{i + 1}. {FormatTime(bestTimesHard[i])}</color>\n";
                }
                else
                {
                    leaderboardText.text += $"{i + 1}. {FormatTime(bestTimesHard[i])}\n";

                }
            }
            else
            {
                if (bestTimesHard[i] == mostRecentTime)
                {
                    // Use rich text to color the most recent time differently
                    leaderboardText.text += $"<color=#2E3138>{i + 1}. {FormatTime(bestTimesHard[i])}</color>\n";

                }
                else
                {
                    leaderboardText.text += $"{i + 1}. {FormatTime(bestTimesHard[i])}\n";

                }
            }
        }
    }

    private string FormatTime(float time)//git
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(time);
        return string.Format("{0:00}:{1:00}", timeSpan.Minutes, timeSpan.Seconds);
    }

   
}
