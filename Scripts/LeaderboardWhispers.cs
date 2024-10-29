using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.GlobalIllumination;

public class LeaderboardWhispers : MonoBehaviour
{
    public TMP_Text leaderboardText;
    public List<float> bestTimesWhisper = new();
    private int SavedListCountWhisper;
    private const int maxEntries = 10;
    public static bool ifAddedWhisper = false;
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


        SavedListCountWhisper = PlayerPrefs.GetInt("CountWhisper");
        for (int i = 0; i < SavedListCountWhisper; i++)
        {
            float time = PlayerPrefs.GetFloat("PlayersWhisper" + i);
            bestTimesWhisper.Add(time);
        }


    }


    public void SaveLeaderboard()
    {
        for (int i = 0; i < bestTimesWhisper.Count; i++)
        {
            PlayerPrefs.SetFloat("PlayersWhisper" + i, bestTimesWhisper[i]);

        }

        PlayerPrefs.SetInt("CountWhisper", bestTimesWhisper.Count);
    }

    private void Update()
    {

        if (SudokuGrid.endChecker == true && ifAddedWhisper == false && SudokuGrid.currentSceneName == "whispers")
        {
            LoadData();
            AddTime(Timer.finalTime - 2);
            SaveLeaderboard();
            UpdateLeaderboard();

            ifAddedWhisper = true;
        }


    }

    public void AddTime(float time)
    {
        bestTimesWhisper.Add(time);
        bestTimesWhisper.Sort();

        // Ensure the list does not exceed the maximum number of entries
        if (bestTimesWhisper.Count > maxEntries)
        {
            bestTimesWhisper.RemoveAt(bestTimesWhisper.Count - 1);
        }

        mostRecentTime = time;
    }

    private void UpdateLeaderboard()
    {
        string headerColor = isLight == 0 ? "#EFEFD0" : "#2E3138";
        leaderboardText.text = $"<color={headerColor}><size=120>Best Times German Whispers:</size></color>\n\n";
        for (int i = 0; i < bestTimesWhisper.Count; i++)
        {
            if (isLight == 0)
            {
                if (bestTimesWhisper[i] == mostRecentTime)
                {
                    // Use rich text to color the most recent time differently

                    leaderboardText.text += $"<color=#EFEFD0>{i + 1}. {FormatTime(bestTimesWhisper[i])}</color>\n";
                }
                else
                {
                    leaderboardText.text += $"{i + 1}. {FormatTime(bestTimesWhisper[i])}\n";

                }
            }
            else
            {
                if (bestTimesWhisper[i] == mostRecentTime)
                {
                    // Use rich text to color the most recent time differently
                    leaderboardText.text += $"<color=#2E3138>{i + 1}. {FormatTime(bestTimesWhisper[i])}</color>\n";

                }
                else
                {
                    leaderboardText.text += $"{i + 1}. {FormatTime(bestTimesWhisper[i])}\n";

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
