using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.GlobalIllumination;

public class LeaderboardKropkiMed : MonoBehaviour
{
    public TMP_Text leaderboardText;
    public List<float> bestTimesKropkiMed = new();
    private int SavedListCountKropkiMed;
    private const int maxEntries = 10;
    public static bool ifAddedKropkiMed = false;
    private string currentSceneName;
    private float mostRecentTime; // Store the most recent time
    private int isLight;

    private void Start()
    {
        ifAddedKropkiMed = false;
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


        SavedListCountKropkiMed = PlayerPrefs.GetInt("CountKropkiMed");
        for (int i = 0; i < SavedListCountKropkiMed; i++)
        {
            float time = PlayerPrefs.GetFloat("PlayersKropkiMed" + i);
            bestTimesKropkiMed.Add(time);
        }


    }


    public void SaveLeaderboard()
    {
        for (int i = 0; i < bestTimesKropkiMed.Count; i++)
        {
            PlayerPrefs.SetFloat("PlayersKropkiMed" + i, bestTimesKropkiMed[i]);

        }

        PlayerPrefs.SetInt("CountKropkiMed", bestTimesKropkiMed.Count);
    }

    private void Update()
    {

        if (SudokuGrid.endChecker == true && ifAddedKropkiMed == false && SudokuGrid.currentSceneName == "kropkiMedium")
        {
            LoadData();
            AddTime(Timer.finalTime - 2);
            SaveLeaderboard();
            UpdateLeaderboard();

            ifAddedKropkiMed = true;
        }


    }

    public void AddTime(float time)
    {
        bestTimesKropkiMed.Add(time);
        bestTimesKropkiMed.Sort();

        // Ensure the list does not exceed the maximum number of entries
        if (bestTimesKropkiMed.Count > maxEntries)
        {
            bestTimesKropkiMed.RemoveAt(bestTimesKropkiMed.Count - 1);
        }

        mostRecentTime = time;
    }

    private void UpdateLeaderboard()
    {
        string headerColor = isLight == 0 ? "#EFEFD0" : "#2E3138";
        leaderboardText.text = $"<color={headerColor}><size=120>Best Times Kropki Medium:</size></color>\n\n";
        for (int i = 0; i < bestTimesKropkiMed.Count; i++)
        {
            if (isLight == 0)
            {
                if (bestTimesKropkiMed[i] == mostRecentTime)
                {
                    // Use rich text to color the most recent time differently

                    leaderboardText.text += $"<color=#EFEFD0>{i + 1}. {FormatTime(bestTimesKropkiMed[i])}</color>\n";
                }
                else
                {
                    leaderboardText.text += $"{i + 1}. {FormatTime(bestTimesKropkiMed[i])}\n";

                }
            }
            else
            {
                if (bestTimesKropkiMed[i] == mostRecentTime)
                {
                    // Use rich text to color the most recent time differently
                    leaderboardText.text += $"<color=#2E3138>{i + 1}. {FormatTime(bestTimesKropkiMed[i])}</color>\n";

                }
                else
                {
                    leaderboardText.text += $"{i + 1}. {FormatTime(bestTimesKropkiMed[i])}\n";

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
