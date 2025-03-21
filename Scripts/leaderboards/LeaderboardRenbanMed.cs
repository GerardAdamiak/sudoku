using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.GlobalIllumination;

public class LeaderboardRenbanMedium : MonoBehaviour
{
    public TMP_Text leaderboardText;
    public List<float> bestTimesRenbanMedium = new();
    private int SavedListCountRenbanMedium;
    private const int maxEntries = 10;
    public static bool ifAddedRenbanMedium = false;
    private string currentSceneName;
    private float mostRecentTime; // Store the most recent time
    private int isLight;

    private void Start()
    {
        ifAddedRenbanMedium = false;
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


        SavedListCountRenbanMedium = PlayerPrefs.GetInt("CountRenbanMedium");
        for (int i = 0; i < SavedListCountRenbanMedium; i++)
        {
            float time = PlayerPrefs.GetFloat("PlayersRenbanMedium" + i);
            bestTimesRenbanMedium.Add(time);
        }


    }


    public void SaveLeaderboard()
    {
        for (int i = 0; i < bestTimesRenbanMedium.Count; i++)
        {
            PlayerPrefs.SetFloat("PlayersRenbanMedium" + i, bestTimesRenbanMedium[i]);

        }

        PlayerPrefs.SetInt("CountRenbanMedium", bestTimesRenbanMedium.Count);
    }

    private void Update()
    {

        if (SudokuGrid.endChecker == true && ifAddedRenbanMedium == false && SudokuGrid.currentSceneName == "renbanMedium")
        {
            LoadData();
            AddTime(Timer.finalTime - 2);
            SaveLeaderboard();
            UpdateLeaderboard();

            ifAddedRenbanMedium = true;
        }


    }

    public void AddTime(float time)
    {
        bestTimesRenbanMedium.Add(time);
        bestTimesRenbanMedium.Sort();

        // Ensure the list does not exceed the maximum number of entries
        if (bestTimesRenbanMedium.Count > maxEntries)
        {
            bestTimesRenbanMedium.RemoveAt(bestTimesRenbanMedium.Count - 1);
        }

        mostRecentTime = time;
    }

    private void UpdateLeaderboard()
    {
        string headerColor = isLight == 0 ? "#EFEFD0" : "#2E3138";
        leaderboardText.text = $"<color={headerColor}><size=120>Best Times Renban Medium:</size></color>\n\n";
        for (int i = 0; i < bestTimesRenbanMedium.Count; i++)
        {
            if (isLight == 0)
            {
                if (bestTimesRenbanMedium[i] == mostRecentTime)
                {
                    // Use rich text to color the most recent time differently

                    leaderboardText.text += $"<color=#EFEFD0>{i + 1}. {FormatTime(bestTimesRenbanMedium[i])}</color>\n";
                }
                else
                {
                    leaderboardText.text += $"{i + 1}. {FormatTime(bestTimesRenbanMedium[i])}\n";

                }
            }
            else
            {
                if (bestTimesRenbanMedium[i] == mostRecentTime)
                {
                    // Use rich text to color the most recent time differently
                    leaderboardText.text += $"<color=#2E3138>{i + 1}. {FormatTime(bestTimesRenbanMedium[i])}</color>\n";

                }
                else
                {
                    leaderboardText.text += $"{i + 1}. {FormatTime(bestTimesRenbanMedium[i])}\n";

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
