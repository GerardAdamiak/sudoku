using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;

public class Leaderboard : MonoBehaviour
{
    public TMP_Text leaderboardText;
    public List<float> bestTimes = new List<float>();
    private int SavedListCount;
    private const int maxEntries = 10;
    private Timer timer;
    public static bool ifAdded = false;
    private const string LeaderboardKey = "LeaderboardTimes";

    private void Start()
    {
    }

    public void LoadData()
    {

        
        SavedListCount = PlayerPrefs.GetInt("Count");
        for (int i = 0; i < SavedListCount; i++)
        {
            float time = PlayerPrefs.GetFloat("Players" + i);
            bestTimes.Add(time);
        }

       
    }


    public void SaveLeaderboard()
    {
        for (int i = 0; i < bestTimes.Count; i++)
        {
            PlayerPrefs.SetFloat("Players" + i, bestTimes[i]);

        }
        
        PlayerPrefs.SetInt("Count", bestTimes.Count);
    }

    private void Update()
    {

        if (sudokuGrid.endChecker == true && ifAdded == false && sudokuGrid.currentSceneName == "easy")
        {
            LoadData();
            AddTime(Timer.finalTime - 2);
            SaveLeaderboard();
            UpdateLeaderboard();

            ifAdded = true;
        }

        
    }

    public void AddTime(float time)
    {
        bestTimes.Add(time);
        bestTimes.Sort();

        // Ensure the list does not exceed the maximum number of entries
        if (bestTimes.Count > maxEntries)
        {
            bestTimes.RemoveAt(bestTimes.Count - 1);
        }

  
    }

    private void UpdateLeaderboard()//git (raczej)
    {
        leaderboardText.text = "Best Times Easy:\n";
        for (int i = 0; i < bestTimes.Count; i++)
        {
            leaderboardText.text += (i + 1) + ". " + FormatTime(bestTimes[i]) + "\n";
        }

    }

    private string FormatTime(float time)//git
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(time);
        return string.Format("{0:00}:{1:00}", timeSpan.Minutes, timeSpan.Seconds);
    }

    
}
