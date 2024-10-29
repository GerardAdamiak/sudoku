using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.GlobalIllumination;

public class LeaderboardThermo : MonoBehaviour
{
    public TMP_Text leaderboardText;
    public List<float> bestTimesThermo = new();
    private int SavedListCountThermo;
    private const int maxEntries = 10;
    public static bool ifAddedThermo = false;
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


        SavedListCountThermo = PlayerPrefs.GetInt("CountThermo");
        for (int i = 0; i < SavedListCountThermo; i++)
        {
            float time = PlayerPrefs.GetFloat("PlayersThermo" + i);
            bestTimesThermo.Add(time);
        }


    }


    public void SaveLeaderboard()
    {
        for (int i = 0; i < bestTimesThermo.Count; i++)
        {
            PlayerPrefs.SetFloat("PlayersThermo" + i, bestTimesThermo[i]);

        }

        PlayerPrefs.SetInt("CountThermo", bestTimesThermo.Count);
    }

    private void Update()
    {

        if (SudokuGrid.endChecker == true && ifAddedThermo == false && SudokuGrid.currentSceneName == "thermo")
        {
            LoadData();
            AddTime(Timer.finalTime - 2);
            SaveLeaderboard();
            UpdateLeaderboard();

            ifAddedThermo = true;
        }


    }

    public void AddTime(float time)
    {
        bestTimesThermo.Add(time);
        bestTimesThermo.Sort();

        // Ensure the list does not exceed the maximum number of entries
        if (bestTimesThermo.Count > maxEntries)
        {
            bestTimesThermo.RemoveAt(bestTimesThermo.Count - 1);
        }

        mostRecentTime = time;
    }

    private void UpdateLeaderboard()
    {
        string headerColor = isLight == 0 ? "#EFEFD0" : "#2E3138";
        leaderboardText.text = $"<color={headerColor}><size=120>Best Times Thermo:</size></color>\n\n";
        for (int i = 0; i < bestTimesThermo.Count; i++)
        {
            if (isLight == 0)
            {
                if (bestTimesThermo[i] == mostRecentTime)
                {
                    // Use rich text to color the most recent time differently

                    leaderboardText.text += $"<color=#EFEFD0>{i + 1}. {FormatTime(bestTimesThermo[i])}</color>\n";
                }
                else
                {
                    leaderboardText.text += $"{i + 1}. {FormatTime(bestTimesThermo[i])}\n";

                }
            }
            else
            {
                if (bestTimesThermo[i] == mostRecentTime)
                {
                    // Use rich text to color the most recent time differently
                    leaderboardText.text += $"<color=#2E3138>{i + 1}. {FormatTime(bestTimesThermo[i])}</color>\n";

                }
                else
                {
                    leaderboardText.text += $"{i + 1}. {FormatTime(bestTimesThermo[i])}\n";

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
