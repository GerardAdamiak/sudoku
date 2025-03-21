using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.GlobalIllumination;

public class LeaderboardKillerEasy : MonoBehaviour
{
    public TMP_Text leaderboardText;
    public List<float> bestTimesKillerEasy = new();
    private int SavedListCountKillerEasy;
    private const int maxEntries = 10;
    public static bool ifAddedKillerEasy = false;
    private string currentSceneName;
    private float mostRecentTime; // Store the most recent time
    private int isLight;

    private void Start()
    {
        ifAddedKillerEasy = false;
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


        SavedListCountKillerEasy = PlayerPrefs.GetInt("CountKillerEasy");
        for (int i = 0; i < SavedListCountKillerEasy; i++)
        {
            float time = PlayerPrefs.GetFloat("PlayersKillerEasy" + i);
            bestTimesKillerEasy.Add(time);
        }


    }


    public void SaveLeaderboard()
    {
        for (int i = 0; i < bestTimesKillerEasy.Count; i++)
        {
            PlayerPrefs.SetFloat("PlayersKillerEasy" + i, bestTimesKillerEasy[i]);

        }

        PlayerPrefs.SetInt("CountKillerEasy", bestTimesKillerEasy.Count);
    }

    private void Update()
    {

        if (SudokuGrid.endChecker == true && ifAddedKillerEasy == false && SudokuGrid.currentSceneName == "killerEasy")
        {
            LoadData();
            AddTime(Timer.finalTime - 2);
            SaveLeaderboard();
            UpdateLeaderboard();

            ifAddedKillerEasy = true;
        }


    }

    public void AddTime(float time)
    {
        bestTimesKillerEasy.Add(time);
        bestTimesKillerEasy.Sort();

        // Ensure the list does not exceed the maximum number of entries
        if (bestTimesKillerEasy.Count > maxEntries)
        {
            bestTimesKillerEasy.RemoveAt(bestTimesKillerEasy.Count - 1);
        }

        mostRecentTime = time;
    }

    private void UpdateLeaderboard()
    {
        string headerColor = isLight == 0 ? "#EFEFD0" : "#2E3138";
        leaderboardText.text = $"<color={headerColor}><size=120>Best Times Killer Easy:</size></color>\n\n";
        for (int i = 0; i < bestTimesKillerEasy.Count; i++)
        {
            if (isLight == 0)
            {
                if (bestTimesKillerEasy[i] == mostRecentTime)
                {
                    // Use rich text to color the most recent time differently

                    leaderboardText.text += $"<color=#EFEFD0>{i + 1}. {FormatTime(bestTimesKillerEasy[i])}</color>\n";
                }
                else
                {
                    leaderboardText.text += $"{i + 1}. {FormatTime(bestTimesKillerEasy[i])}\n";

                }
            }
            else
            {
                if (bestTimesKillerEasy[i] == mostRecentTime)
                {
                    // Use rich text to color the most recent time differently
                    leaderboardText.text += $"<color=#2E3138>{i + 1}. {FormatTime(bestTimesKillerEasy[i])}</color>\n";

                }
                else
                {
                    leaderboardText.text += $"{i + 1}. {FormatTime(bestTimesKillerEasy[i])}\n";

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
