using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;

public class LeaderboardMed : MonoBehaviour
{
    public TMP_Text leaderboardText;
    public List<float> bestTimesMed = new List<float>();
    private int SavedListCountMed;
    private const int maxEntries = 10;
    private Timer timer;
    public static bool ifAddedMed = false;

    private void Start()
    {
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

        if (sudokuGrid.endChecker == true && ifAddedMed == false && sudokuGrid.currentSceneName == "medium")
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


    }

    private void UpdateLeaderboard()//git (raczej)
    {
        leaderboardText.text = ":Best Times Medium:\n";
        for (int i = 0; i < bestTimesMed.Count; i++)
        {
            leaderboardText.text += (i + 1) + ". " + FormatTime(bestTimesMed[i]) + "\n";
        }

    }

    private string FormatTime(float time)//git
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(time);
        return string.Format("{0:00}:{1:00}", timeSpan.Minutes, timeSpan.Seconds);
    }

    private void OnDestroy()//git
    {
        //  SaveLeaderboard();
    }
}
