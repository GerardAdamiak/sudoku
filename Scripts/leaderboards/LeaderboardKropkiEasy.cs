using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.GlobalIllumination;

public class LeaderboardKropkiEasy : MonoBehaviour
{
    public TMP_Text leaderboardText;
    public List<float> bestTimesKropkiEasy = new();
    private int SavedListCountKropkiEasy;
    private const int maxEntries = 10;
    public static bool ifAddedKropkiEasy = false;
    private string currentSceneName;
    private float mostRecentTime; // Store the most recent time
    private int isLight;

    private void Start()
    {
        ifAddedKropkiEasy = false;
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


        SavedListCountKropkiEasy = PlayerPrefs.GetInt("CountKropkiEasy");
        for (int i = 0; i < SavedListCountKropkiEasy; i++)
        {
            float time = PlayerPrefs.GetFloat("PlayersKropkiEasy" + i);
            bestTimesKropkiEasy.Add(time);
        }


    }


    public void SaveLeaderboard()
    {
        for (int i = 0; i < bestTimesKropkiEasy.Count; i++)
        {
            PlayerPrefs.SetFloat("PlayersKropkiEasy" + i, bestTimesKropkiEasy[i]);

        }

        PlayerPrefs.SetInt("CountKropkiEasy", bestTimesKropkiEasy.Count);
    }

    private void Update()
    {

        if (SudokuGrid.endChecker == true && ifAddedKropkiEasy == false && SudokuGrid.currentSceneName == "kropkiEasy")
        {
            LoadData();
            AddTime(Timer.finalTime - 2);
            SaveLeaderboard();
            UpdateLeaderboard();

            ifAddedKropkiEasy = true;
        }


    }

    public void AddTime(float time)
    {
        bestTimesKropkiEasy.Add(time);
        bestTimesKropkiEasy.Sort();

        // Ensure the list does not exceed the maximum number of entries
        if (bestTimesKropkiEasy.Count > maxEntries)
        {
            bestTimesKropkiEasy.RemoveAt(bestTimesKropkiEasy.Count - 1);
        }

        mostRecentTime = time;
    }

    private void UpdateLeaderboard()
    {
        string headerColor = isLight == 0 ? "#EFEFD0" : "#2E3138";
        leaderboardText.text = $"<color={headerColor}><size=120>Best Times Kropki Easy:</size></color>\n\n";
        for (int i = 0; i < bestTimesKropkiEasy.Count; i++)
        {
            if (isLight == 0)
            {
                if (bestTimesKropkiEasy[i] == mostRecentTime)
                {
                    // Use rich text to color the most recent time differently

                    leaderboardText.text += $"<color=#EFEFD0>{i + 1}. {FormatTime(bestTimesKropkiEasy[i])}</color>\n";
                }
                else
                {
                    leaderboardText.text += $"{i + 1}. {FormatTime(bestTimesKropkiEasy[i])}\n";

                }
            }
            else
            {
                if (bestTimesKropkiEasy[i] == mostRecentTime)
                {
                    // Use rich text to color the most recent time differently
                    leaderboardText.text += $"<color=#2E3138>{i + 1}. {FormatTime(bestTimesKropkiEasy[i])}</color>\n";

                }
                else
                {
                    leaderboardText.text += $"{i + 1}. {FormatTime(bestTimesKropkiEasy[i])}\n";

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
