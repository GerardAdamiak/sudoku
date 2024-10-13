using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class sudokuSaver : MonoBehaviour
{
    private DigitKeyboard digitKeyboard;
    private string saver;
    private string lastScene;
    // Start is called before the first frame update
    void Start()
    {
        saver= PlayerPrefs.GetString("Sudoku");
        lastScene= PlayerPrefs.GetString("PreviousScene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SaveSudoku1()
    {
        // Debug.Log(saver);
        
            PlayerPrefs.SetInt("number", 1);
            SceneManager.LoadScene("Custom");
        
    }
    public void SaveSudoku2()
    {
        // Debug.Log(saver);
        
            PlayerPrefs.SetInt("number", 2);
            SceneManager.LoadScene("Custom");
        
    }
    public void SaveSudoku3()
    {
        // Debug.Log(saver);
        
            PlayerPrefs.SetInt("number", 3);
            SceneManager.LoadScene("Custom");
        
    }
    public void SaveSudoku4()
    {
        //Debug.Log(saver);

        
            PlayerPrefs.SetInt("number", 4);
            SceneManager.LoadScene("Custom");
        
    }
    public void SaveSudoku5()
    {
        
            PlayerPrefs.SetInt("number", 5);
            SceneManager.LoadScene("Custom");
        
    }

}
