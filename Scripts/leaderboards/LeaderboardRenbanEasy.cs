using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.GlobalIllumination;

public class LeaderboardRenbanEasy : MonoBehaviour
{
    public TMP_Text leaderboardText;
    public List<float> bestTimesRenbanEasy = new();
    private int SavedListCountRenbanEasy;
    private const int maxEntries = 10;
    public static bool ifAddedRenbanEasy = false;
    private string currentSceneName;
    private float mostRecentTime; // Store the most recent time
    private int isLight;

    private void Start()
    {
        ifAddedRenbanEasy = false;
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


        SavedListCountRenbanEasy = PlayerPrefs.GetInt("CountRenbanEasy");
        for (int i = 0; i < SavedListCountRenbanEasy; i++)
        {
            float time = PlayerPrefs.GetFloat("PlayersRenbanEasy" + i);
            bestTimesRenbanEasy.Add(time);
        }


    }


    public void SaveLeaderboard()
    {
        for (int i = 0; i < bestTimesRenbanEasy.Count; i++)
        {
            PlayerPrefs.SetFloat("PlayersRenbanEasy" + i, bestTimesRenbanEasy[i]);

        }

        PlayerPrefs.SetInt("CountRenbanEasy", bestTimesRenbanEasy.Count);
    }

    private void Update()
    {

        if (SudokuGrid.endChecker == true && ifAddedRenbanEasy == false && SudokuGrid.currentSceneName == "renbanEasy")
        {
            LoadData();
            AddTime(Timer.finalTime - 2);
            SaveLeaderboard();
            UpdateLeaderboard();

            ifAddedRenbanEasy = true;
        }


    }

    public void AddTime(float time)
    {
        bestTimesRenbanEasy.Add(time);
        bestTimesRenbanEasy.Sort();

        // Ensure the list does not exceed the maximum number of entries
        if (bestTimesRenbanEasy.Count > maxEntries)
        {
            bestTimesRenbanEasy.RemoveAt(bestTimesRenbanEasy.Count - 1);
        }

        mostRecentTime = time;
    }

    private void UpdateLeaderboard()
    {
        string headerColor = isLight == 0 ? "#EFEFD0" : "#2E3138";
        leaderboardText.text = $"<color={headerColor}><size=120>Best Times Renban Easy:</size></color>\n\n";
        for (int i = 0; i < bestTimesRenbanEasy.Count; i++)
        {
            if (isLight == 0)
            {
                if (bestTimesRenbanEasy[i] == mostRecentTime)
                {
                    // Use rich text to color the most recent time differently

                    leaderboardText.text += $"<color=#EFEFD0>{i + 1}. {FormatTime(bestTimesRenbanEasy[i])}</color>\n";
                }
                else
                {
                    leaderboardText.text += $"{i + 1}. {FormatTime(bestTimesRenbanEasy[i])}\n";

                }
            }
            else
            {
                if (bestTimesRenbanEasy[i] == mostRecentTime)
                {
                    // Use rich text to color the most recent time differently
                    leaderboardText.text += $"<color=#2E3138>{i + 1}. {FormatTime(bestTimesRenbanEasy[i])}</color>\n";

                }
                else
                {
                    leaderboardText.text += $"{i + 1}. {FormatTime(bestTimesRenbanEasy[i])}\n";

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
