using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.GlobalIllumination;

public class LeaderboardRenban : MonoBehaviour
{
    public TMP_Text leaderboardText;
    public List<float> bestTimesRenban = new();
    private int SavedListCountRenban;
    private const int maxEntries = 10;
    public static bool ifAddedRenban = false;
    private string currentSceneName;
    private float mostRecentTime; // Store the most recent time
    private int isLight;

    private void Start()
    {
        ifAddedRenban = false;
        isLight = PlayerPrefs.GetInt("IsLight");
        currentSceneName = SceneManager.GetActiveScene().name;
        if ((currentSceneName == "Custom"))
        {
            // If whichSet is "custom", hide the sprite and make it unclickable
            gameObject.SetActive(false); // Hide the GameObject
            return; // Exit Start() early
        }
    }

    public void LoadData()
    {


        SavedListCountRenban = PlayerPrefs.GetInt("CountRenban");
        for (int i = 0; i < SavedListCountRenban; i++)
        {
            float time = PlayerPrefs.GetFloat("PlayersRenban" + i);
            bestTimesRenban.Add(time);
        }


    }


    public void SaveLeaderboard()
    {
        for (int i = 0; i < bestTimesRenban.Count; i++)
        {
            PlayerPrefs.SetFloat("PlayersRenban" + i, bestTimesRenban[i]);

        }

        PlayerPrefs.SetInt("CountRenban", bestTimesRenban.Count);
    }

    private void Update()
    {

        if (SudokuGrid.endChecker == true && ifAddedRenban == false && SudokuGrid.currentSceneName == "renban")
        {
            LoadData();
            AddTime(Timer.finalTime - 2);
            SaveLeaderboard();
            UpdateLeaderboard();

            ifAddedRenban = true;
        }


    }

    public void AddTime(float time)
    {
        bestTimesRenban.Add(time);
        bestTimesRenban.Sort();

        // Ensure the list does not exceed the maximum number of entries
        if (bestTimesRenban.Count > maxEntries)
        {
            bestTimesRenban.RemoveAt(bestTimesRenban.Count - 1);
        }

        mostRecentTime = time;
    }

    private void UpdateLeaderboard()
    {
        string headerColor = isLight == 0 ? "#EFEFD0" : "#2E3138";
        leaderboardText.text = $"<color={headerColor}><size=120>Best Times Renban:</size></color>\n\n";
        for (int i = 0; i < bestTimesRenban.Count; i++)
        {
            if (isLight == 0)
            {
                if (bestTimesRenban[i] == mostRecentTime)
                {
                    // Use rich text to color the most recent time differently

                    leaderboardText.text += $"<color=#EFEFD0>{i + 1}. {FormatTime(bestTimesRenban[i])}</color>\n";
                }
                else
                {
                    leaderboardText.text += $"{i + 1}. {FormatTime(bestTimesRenban[i])}\n";

                }
            }
            else
            {
                if (bestTimesRenban[i] == mostRecentTime)
                {
                    // Use rich text to color the most recent time differently
                    leaderboardText.text += $"<color=#2E3138>{i + 1}. {FormatTime(bestTimesRenban[i])}</color>\n";

                }
                else
                {
                    leaderboardText.text += $"{i + 1}. {FormatTime(bestTimesRenban[i])}\n";

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
