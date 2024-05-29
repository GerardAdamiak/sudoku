using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;

public class LeaderboardHard : MonoBehaviour
{
    public TMP_Text leaderboardText;
    public List<float> bestTimesHard = new List<float>();
    private int SavedListCountHard;
    private const int maxEntries = 10;
    private Timer timer;
    public static bool ifAddedHard = false;

    private void Start()
    {
    }

    public void LoadData()
    {


        SavedListCountHard = PlayerPrefs.GetInt("CountHard");
        for (int i = 0; i < SavedListCountHard; i++)
        {
            float time = PlayerPrefs.GetFloat("PlayersHard" + i);
            bestTimesHard.Add(time);
        }


    }


    public void SaveLeaderboard()
    {
        for (int i = 0; i < bestTimesHard.Count; i++)
        {
            PlayerPrefs.SetFloat("PlayersHard" + i, bestTimesHard[i]);

        }

        PlayerPrefs.SetInt("CountHard", bestTimesHard.Count);
    }

    private void Update()
    {

        if (sudokuGrid.endChecker == true && ifAddedHard == false && sudokuGrid.currentSceneName == "hard")
        {
            LoadData();
            AddTime(Timer.finalTime - 2);
            SaveLeaderboard();
            UpdateLeaderboard();

            ifAddedHard = true;
        }


    }

    public void AddTime(float time)
    {
        bestTimesHard.Add(time);
        bestTimesHard.Sort();

        // Ensure the list does not exceed the maximum number of entries
        if (bestTimesHard.Count > maxEntries)
        {
            bestTimesHard.RemoveAt(bestTimesHard.Count - 1);
        }


    }

    private void UpdateLeaderboard()//git (raczej)
    {
        leaderboardText.text = "Best Times Hard:\n";
        for (int i = 0; i < bestTimesHard.Count; i++)
        {
            leaderboardText.text += (i + 1) + ". " + FormatTime(bestTimesHard[i]) + "\n";
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
