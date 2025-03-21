using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.GlobalIllumination;

public class LeaderboardWhispersMed : MonoBehaviour
{
    public TMP_Text leaderboardText;
    public List<float> bestTimesWhisperMed = new();
    private int SavedListCountWhisperMed;
    private const int maxEntries = 10;
    public static bool ifAddedWhisperMed = false;
    private string currentSceneName;
    private float mostRecentTime; // Store the most recent time
    private int isLight;

    private void Start()
    {
        ifAddedWhisperMed = false;
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


        SavedListCountWhisperMed = PlayerPrefs.GetInt("CountWhisperMed");
        for (int i = 0; i < SavedListCountWhisperMed; i++)
        {
            float time = PlayerPrefs.GetFloat("PlayersWhisperMed" + i);
            bestTimesWhisperMed.Add(time);
        }


    }


    public void SaveLeaderboard()
    {
        for (int i = 0; i < bestTimesWhisperMed.Count; i++)
        {
            PlayerPrefs.SetFloat("PlayersWhisperMed" + i, bestTimesWhisperMed[i]);

        }

        PlayerPrefs.SetInt("CountWhisperMed", bestTimesWhisperMed.Count);
    }

    private void Update()
    {

        if (SudokuGrid.endChecker == true && ifAddedWhisperMed == false && SudokuGrid.currentSceneName == "whispersMedium")
        {
            LoadData();
            AddTime(Timer.finalTime - 2);
            SaveLeaderboard();
            UpdateLeaderboard();

            ifAddedWhisperMed = true;
        }


    }

    public void AddTime(float time)
    {
        bestTimesWhisperMed.Add(time);
        bestTimesWhisperMed.Sort();

        // Ensure the list does not exceed the maximum number of entries
        if (bestTimesWhisperMed.Count > maxEntries)
        {
            bestTimesWhisperMed.RemoveAt(bestTimesWhisperMed.Count - 1);
        }

        mostRecentTime = time;
    }

    private void UpdateLeaderboard()
    {
        string headerColor = isLight == 0 ? "#EFEFD0" : "#2E3138";
        leaderboardText.text = $"<color={headerColor}><size=120>Best Times German Whispers Medium:</size></color>\n\n";
        for (int i = 0; i < bestTimesWhisperMed.Count; i++)
        {
            if (isLight == 0)
            {
                if (bestTimesWhisperMed[i] == mostRecentTime)
                {
                    // Use rich text to color the most recent time differently

                    leaderboardText.text += $"<color=#EFEFD0>{i + 1}. {FormatTime(bestTimesWhisperMed[i])}</color>\n";
                }
                else
                {
                    leaderboardText.text += $"{i + 1}. {FormatTime(bestTimesWhisperMed[i])}\n";

                }
            }
            else
            {
                if (bestTimesWhisperMed[i] == mostRecentTime)
                {
                    // Use rich text to color the most recent time differently
                    leaderboardText.text += $"<color=#2E3138>{i + 1}. {FormatTime(bestTimesWhisperMed[i])}</color>\n";

                }
                else
                {
                    leaderboardText.text += $"{i + 1}. {FormatTime(bestTimesWhisperMed[i])}\n";

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
