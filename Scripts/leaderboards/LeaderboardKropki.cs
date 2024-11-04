using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.GlobalIllumination;

public class LeaderboardKropki : MonoBehaviour
{
    public TMP_Text leaderboardText;
    public List<float> bestTimesKropki = new();
    private int SavedListCountKropki;
    private const int maxEntries = 10;
    public static bool ifAddedKropki = false;
    private string currentSceneName;
    private float mostRecentTime; // Store the most recent time
    private int isLight;

    private void Start()
    {
        ifAddedKropki = false;
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


        SavedListCountKropki = PlayerPrefs.GetInt("CountKropki");
        for (int i = 0; i < SavedListCountKropki; i++)
        {
            float time = PlayerPrefs.GetFloat("PlayersKropki" + i);
            bestTimesKropki.Add(time);
        }


    }


    public void SaveLeaderboard()
    {
        for (int i = 0; i < bestTimesKropki.Count; i++)
        {
            PlayerPrefs.SetFloat("PlayersKropki" + i, bestTimesKropki[i]);

        }

        PlayerPrefs.SetInt("CountKropki", bestTimesKropki.Count);
    }

    private void Update()
    {

        if (SudokuGrid.endChecker == true && ifAddedKropki == false && SudokuGrid.currentSceneName == "kropki")
        {
            LoadData();
            AddTime(Timer.finalTime - 2);
            SaveLeaderboard();
            UpdateLeaderboard();

            ifAddedKropki = true;
        }


    }

    public void AddTime(float time)
    {
        bestTimesKropki.Add(time);
        bestTimesKropki.Sort();

        // Ensure the list does not exceed the maximum number of entries
        if (bestTimesKropki.Count > maxEntries)
        {
            bestTimesKropki.RemoveAt(bestTimesKropki.Count - 1);
        }

        mostRecentTime = time;
    }

    private void UpdateLeaderboard()
    {
        string headerColor = isLight == 0 ? "#EFEFD0" : "#2E3138";
        leaderboardText.text = $"<color={headerColor}><size=120>Best Times Kropki:</size></color>\n\n";
        for (int i = 0; i < bestTimesKropki.Count; i++)
        {
            if (isLight == 0)
            {
                if (bestTimesKropki[i] == mostRecentTime)
                {
                    // Use rich text to color the most recent time differently

                    leaderboardText.text += $"<color=#EFEFD0>{i + 1}. {FormatTime(bestTimesKropki[i])}</color>\n";
                }
                else
                {
                    leaderboardText.text += $"{i + 1}. {FormatTime(bestTimesKropki[i])}\n";

                }
            }
            else
            {
                if (bestTimesKropki[i] == mostRecentTime)
                {
                    // Use rich text to color the most recent time differently
                    leaderboardText.text += $"<color=#2E3138>{i + 1}. {FormatTime(bestTimesKropki[i])}</color>\n";

                }
                else
                {
                    leaderboardText.text += $"{i + 1}. {FormatTime(bestTimesKropki[i])}\n";

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
