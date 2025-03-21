using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.GlobalIllumination;

public class LeaderboardThermoMed : MonoBehaviour
{
    public TMP_Text leaderboardText;
    public List<float> bestTimesThermoMed = new();
    private int SavedListCountThermoMed;
    private const int maxEntries = 10;
    public static bool ifAddedThermoMed = false;
    private string currentSceneName;
    private float mostRecentTime; // Store the most recent time
    private int isLight;

    private void Start()
    {
        ifAddedThermoMed = false;
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


        SavedListCountThermoMed = PlayerPrefs.GetInt("CountThermoMed");
        for (int i = 0; i < SavedListCountThermoMed; i++)
        {
            float time = PlayerPrefs.GetFloat("PlayersThermoMed" + i);
            bestTimesThermoMed.Add(time);
        }


    }


    public void SaveLeaderboard()
    {
        for (int i = 0; i < bestTimesThermoMed.Count; i++)
        {
            PlayerPrefs.SetFloat("PlayersThermoMed" + i, bestTimesThermoMed[i]);

        }

        PlayerPrefs.SetInt("CountThermoMed", bestTimesThermoMed.Count);
    }

    private void Update()
    {

        if (SudokuGrid.endChecker == true && ifAddedThermoMed == false && SudokuGrid.currentSceneName == "thermoMedium")
        {
            LoadData();
            AddTime(Timer.finalTime - 2);
            SaveLeaderboard();
            UpdateLeaderboard();

            ifAddedThermoMed = true;
        }


    }

    public void AddTime(float time)
    {
        bestTimesThermoMed.Add(time);
        bestTimesThermoMed.Sort();

        // Ensure the list does not exceed the maximum number of entries
        if (bestTimesThermoMed.Count > maxEntries)
        {
            bestTimesThermoMed.RemoveAt(bestTimesThermoMed.Count - 1);
        }

        mostRecentTime = time;
    }

    private void UpdateLeaderboard()
    {
        string headerColor = isLight == 0 ? "#EFEFD0" : "#2E3138";
        leaderboardText.text = $"<color={headerColor}><size=120>Best Times Thermo Medium:</size></color>\n\n";
        for (int i = 0; i < bestTimesThermoMed.Count; i++)
        {
            if (isLight == 0)
            {
                if (bestTimesThermoMed[i] == mostRecentTime)
                {
                    // Use rich text to color the most recent time differently

                    leaderboardText.text += $"<color=#EFEFD0>{i + 1}. {FormatTime(bestTimesThermoMed[i])}</color>\n";
                }
                else
                {
                    leaderboardText.text += $"{i + 1}. {FormatTime(bestTimesThermoMed[i])}\n";

                }
            }
            else
            {
                if (bestTimesThermoMed[i] == mostRecentTime)
                {
                    // Use rich text to color the most recent time differently
                    leaderboardText.text += $"<color=#2E3138>{i + 1}. {FormatTime(bestTimesThermoMed[i])}</color>\n";

                }
                else
                {
                    leaderboardText.text += $"{i + 1}. {FormatTime(bestTimesThermoMed[i])}\n";

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
