using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.GlobalIllumination;

public class LeaderboardMed : MonoBehaviour
{
    public TMP_Text leaderboardText;
    public List<float> bestTimesMed = new();
    private int SavedListCountMed;
    private const int maxEntries = 10;
    public static bool ifAddedMed = false;
    private string currentSceneName;
    private float mostRecentTime; // Store the most recent time
    private int isLight;

    private void Start()
    {
        ifAddedMed = false;
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


        SavedListCountMed = PlayerPrefs.GetInt("CountMed");
        for (int i = 0; i < SavedListCountMed; i++)
        {
            float time = PlayerPrefs.GetFloat("PlayersMed" + i);
            bestTimesMed.Add(time);
        }


    }


    public void SaveLeaderboard()
    {
        for (int i = 0; i < bestTimesMed.Count; i++)
        {
            PlayerPrefs.SetFloat("PlayersMed" + i, bestTimesMed[i]);

        }

        PlayerPrefs.SetInt("CountMed", bestTimesMed.Count);
    }

    private void Update()
    {

        if (SudokuGrid.endChecker == true && ifAddedMed == false && SudokuGrid.currentSceneName == "medium")
            {
                LoadData();
                AddTime(Timer.finalTime - 2);
                SaveLeaderboard();
                UpdateLeaderboard();

                ifAddedMed = true;
            }

        
    }

    public void AddTime(float time)
    {
        bestTimesMed.Add(time);
        bestTimesMed.Sort();

        // Ensure the list does not exceed the maximum number of entries
        if (bestTimesMed.Count > maxEntries)
        {
            bestTimesMed.RemoveAt(bestTimesMed.Count - 1);
        }

        mostRecentTime = time;
    }

    private void UpdateLeaderboard()
    {
        string headerColor = isLight == 0 ? "#EFEFD0" : "#2E3138";
        leaderboardText.text = $"<color={headerColor}><size=120>Best Times Medium:</size></color>\n\n";
        for (int i = 0; i < bestTimesMed.Count; i++)
        {
            if (isLight == 0)
            {
                if (bestTimesMed[i] == mostRecentTime)
                {
                    // Use rich text to color the most recent time differently

                    leaderboardText.text += $"<color=#EFEFD0>{i + 1}. {FormatTime(bestTimesMed[i])}</color>\n";
                }
                else
                {
                    leaderboardText.text += $"{i + 1}. {FormatTime(bestTimesMed[i])}\n";

                }
            }
            else
            {
                if (bestTimesMed[i] == mostRecentTime)
                {
                    // Use rich text to color the most recent time differently
                    leaderboardText.text += $"<color=#2E3138>{i + 1}. {FormatTime(bestTimesMed[i])}</color>\n";

                }
                else
                {
                    leaderboardText.text += $"{i + 1}. {FormatTime(bestTimesMed[i])}\n";

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
