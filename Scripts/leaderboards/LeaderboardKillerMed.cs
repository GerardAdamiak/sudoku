using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.GlobalIllumination;

public class LeaderboardKillerMedium : MonoBehaviour
{
    public TMP_Text leaderboardText;
    public List<float> bestTimesKillerMedium = new();
    private int SavedListCountKillerMedium;
    private const int maxEntries = 10;
    public static bool ifAddedKillerMedium = false;
    private string currentSceneName;
    private float mostRecentTime; // Store the most recent time
    private int isLight;

    private void Start()
    {
        ifAddedKillerMedium = false;
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


        SavedListCountKillerMedium = PlayerPrefs.GetInt("CountKillerMedium");
        for (int i = 0; i < SavedListCountKillerMedium; i++)
        {
            float time = PlayerPrefs.GetFloat("PlayersKillerMedium" + i);
            bestTimesKillerMedium.Add(time);
        }


    }


    public void SaveLeaderboard()
    {
        for (int i = 0; i < bestTimesKillerMedium.Count; i++)
        {
            PlayerPrefs.SetFloat("PlayersKillerMedium" + i, bestTimesKillerMedium[i]);

        }

        PlayerPrefs.SetInt("CountKillerMedium", bestTimesKillerMedium.Count);
    }

    private void Update()
    {

        if (SudokuGrid.endChecker == true && ifAddedKillerMedium == false && SudokuGrid.currentSceneName == "killerMedium")
        {
            LoadData();
            AddTime(Timer.finalTime - 2);
            SaveLeaderboard();
            UpdateLeaderboard();

            ifAddedKillerMedium = true;
        }


    }

    public void AddTime(float time)
    {
        bestTimesKillerMedium.Add(time);
        bestTimesKillerMedium.Sort();

        // Ensure the list does not exceed the maximum number of entries
        if (bestTimesKillerMedium.Count > maxEntries)
        {
            bestTimesKillerMedium.RemoveAt(bestTimesKillerMedium.Count - 1);
        }

        mostRecentTime = time;
    }

    private void UpdateLeaderboard()
    {
        string headerColor = isLight == 0 ? "#EFEFD0" : "#2E3138";
        leaderboardText.text = $"<color={headerColor}><size=120>Best Times Killer Medium:</size></color>\n\n";
        for (int i = 0; i < bestTimesKillerMedium.Count; i++)
        {
            if (isLight == 0)
            {
                if (bestTimesKillerMedium[i] == mostRecentTime)
                {
                    // Use rich text to color the most recent time differently

                    leaderboardText.text += $"<color=#EFEFD0>{i + 1}. {FormatTime(bestTimesKillerMedium[i])}</color>\n";
                }
                else
                {
                    leaderboardText.text += $"{i + 1}. {FormatTime(bestTimesKillerMedium[i])}\n";

                }
            }
            else
            {
                if (bestTimesKillerMedium[i] == mostRecentTime)
                {
                    // Use rich text to color the most recent time differently
                    leaderboardText.text += $"<color=#2E3138>{i + 1}. {FormatTime(bestTimesKillerMedium[i])}</color>\n";

                }
                else
                {
                    leaderboardText.text += $"{i + 1}. {FormatTime(bestTimesKillerMedium[i])}\n";

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
