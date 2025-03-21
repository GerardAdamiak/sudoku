using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.GlobalIllumination;

public class LeaderboardWhispersEasy : MonoBehaviour
{
    public TMP_Text leaderboardText;
    public List<float> bestTimesWhisperEasy = new();
    private int SavedListCountWhisperEasy;
    private const int maxEntries = 10;
    public static bool ifAddedWhisperEasy = false;
    private string currentSceneName;
    private float mostRecentTime; // Store the most recent time
    private int isLight;

    private void Start()
    {
        ifAddedWhisperEasy = false;
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


        SavedListCountWhisperEasy = PlayerPrefs.GetInt("CountWhisperEasy");
        for (int i = 0; i < SavedListCountWhisperEasy; i++)
        {
            float time = PlayerPrefs.GetFloat("PlayersWhisperEasy" + i);
            bestTimesWhisperEasy.Add(time);
        }


    }


    public void SaveLeaderboard()
    {
        for (int i = 0; i < bestTimesWhisperEasy.Count; i++)
        {
            PlayerPrefs.SetFloat("PlayersWhisperEasy" + i, bestTimesWhisperEasy[i]);

        }

        PlayerPrefs.SetInt("CountWhisperEasy", bestTimesWhisperEasy.Count);
    }

    private void Update()
    {

        if (SudokuGrid.endChecker == true && ifAddedWhisperEasy == false && SudokuGrid.currentSceneName == "whispersEasy")
        {
            LoadData();
            AddTime(Timer.finalTime - 2);
            SaveLeaderboard();
            UpdateLeaderboard();

            ifAddedWhisperEasy = true;
        }


    }

    public void AddTime(float time)
    {
        bestTimesWhisperEasy.Add(time);
        bestTimesWhisperEasy.Sort();

        // Ensure the list does not exceed the maximum number of entries
        if (bestTimesWhisperEasy.Count > maxEntries)
        {
            bestTimesWhisperEasy.RemoveAt(bestTimesWhisperEasy.Count - 1);
        }

        mostRecentTime = time;
    }

    private void UpdateLeaderboard()
    {
        string headerColor = isLight == 0 ? "#EFEFD0" : "#2E3138";
        leaderboardText.text = $"<color={headerColor}><size=120>Best Times German Whispers Easy:</size></color>\n\n";
        for (int i = 0; i < bestTimesWhisperEasy.Count; i++)
        {
            if (isLight == 0)
            {
                if (bestTimesWhisperEasy[i] == mostRecentTime)
                {
                    // Use rich text to color the most recent time differently

                    leaderboardText.text += $"<color=#EFEFD0>{i + 1}. {FormatTime(bestTimesWhisperEasy[i])}</color>\n";
                }
                else
                {
                    leaderboardText.text += $"{i + 1}. {FormatTime(bestTimesWhisperEasy[i])}\n";

                }
            }
            else
            {
                if (bestTimesWhisperEasy[i] == mostRecentTime)
                {
                    // Use rich text to color the most recent time differently
                    leaderboardText.text += $"<color=#2E3138>{i + 1}. {FormatTime(bestTimesWhisperEasy[i])}</color>\n";

                }
                else
                {
                    leaderboardText.text += $"{i + 1}. {FormatTime(bestTimesWhisperEasy[i])}\n";

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
