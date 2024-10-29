using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.GlobalIllumination;

public class LeaderboardKiller : MonoBehaviour
{
    public TMP_Text leaderboardText;
    public List<float> bestTimesKiller = new();
    private int SavedListCountKiller;
    private const int maxEntries = 10;
    public static bool ifAddedKiller = false;
    private string currentSceneName;
    private float mostRecentTime; // Store the most recent time
    private int isLight;

    private void Start()
    {
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


        SavedListCountKiller = PlayerPrefs.GetInt("CountKiller");
        for (int i = 0; i < SavedListCountKiller; i++)
        {
            float time = PlayerPrefs.GetFloat("PlayersKiller" + i);
            bestTimesKiller.Add(time);
        }


    }


    public void SaveLeaderboard()
    {
        for (int i = 0; i < bestTimesKiller.Count; i++)
        {
            PlayerPrefs.SetFloat("PlayersKiller" + i, bestTimesKiller[i]);

        }

        PlayerPrefs.SetInt("CountKiller", bestTimesKiller.Count);
    }

    private void Update()
    {

        if (SudokuGrid.endChecker == true && ifAddedKiller == false && SudokuGrid.currentSceneName == "killer")
        {
            LoadData();
            AddTime(Timer.finalTime - 2);
            SaveLeaderboard();
            UpdateLeaderboard();

            ifAddedKiller = true;
        }


    }

    public void AddTime(float time)
    {
        bestTimesKiller.Add(time);
        bestTimesKiller.Sort();

        // Ensure the list does not exceed the maximum number of entries
        if (bestTimesKiller.Count > maxEntries)
        {
            bestTimesKiller.RemoveAt(bestTimesKiller.Count - 1);
        }

        mostRecentTime = time;
    }

    private void UpdateLeaderboard()
    {
        string headerColor = isLight == 0 ? "#EFEFD0" : "#2E3138";
        leaderboardText.text = $"<color={headerColor}><size=120>Best Times Killer:</size></color>\n\n";
        for (int i = 0; i < bestTimesKiller.Count; i++)
        {
            if (isLight == 0)
            {
                if (bestTimesKiller[i] == mostRecentTime)
                {
                    // Use rich text to color the most recent time differently

                    leaderboardText.text += $"<color=#EFEFD0>{i + 1}. {FormatTime(bestTimesKiller[i])}</color>\n";
                }
                else
                {
                    leaderboardText.text += $"{i + 1}. {FormatTime(bestTimesKiller[i])}\n";

                }
            }
            else
            {
                if (bestTimesKiller[i] == mostRecentTime)
                {
                    // Use rich text to color the most recent time differently
                    leaderboardText.text += $"<color=#2E3138>{i + 1}. {FormatTime(bestTimesKiller[i])}</color>\n";

                }
                else
                {
                    leaderboardText.text += $"{i + 1}. {FormatTime(bestTimesKiller[i])}\n";

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
