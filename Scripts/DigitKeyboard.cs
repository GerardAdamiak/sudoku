using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class DigitKeyboard : MonoBehaviour
{
    public GameObject buttonPrefab;
    public Transform buttonsParent;
    private sudokuGrid grid;
    private string sudokuLog="";
    private string whichSet;
    private int number;
    private string sceneName;
    
    private void Start()
    {
        whichSet = PlayerPrefs.GetString("whichSet", "");
        grid = FindObjectOfType<sudokuGrid>();
        sceneName=SceneManager.GetActiveScene().name;
        CreateButtons();
        PositionKeyboard();
    }

    private void CreateButtons()
    {
        float buttonWidth = 150f; // Adjust as needed
        float buttonHeight = 150f; // Adjust as needed
        float padding = 10f; // Adjust as needed

        int rows = 3;
        int columns = 4;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                int digit = i * columns + j + 1;
                digit = digit - i;
                if ((i == 0) && (j == 3)) digit = 0;
                if ((i == 1) && (j == 3)) digit = 10;
                if (digit == 10 && whichSet == "custom")
                {
                    continue; // Skip creating this button
                }
                if ((i != 1) || (j != 3) || (sceneName == "Custom")) { 
                if ((i == 0) || (i == 1) || (j != 3))
                {

                    GameObject buttonGO = Instantiate(buttonPrefab, buttonsParent);
                    Button button = buttonGO.GetComponent<Button>();
                    button.onClick.AddListener(() => OnDigitButtonClick(digit));
                    button.GetComponentInChildren<TextMeshProUGUI>().text = digit.ToString();
                    if (digit == 0) button.GetComponentInChildren<TextMeshProUGUI>().text = "back";
                    if (digit == 10)
                    {
                        button.GetComponentInChildren<TextMeshProUGUI>().text = "save";
                        buttonHeight = 310f;
                    }
                    RectTransform rectTransform = button.GetComponent<RectTransform>();
                    rectTransform.sizeDelta = new Vector2(buttonWidth, buttonHeight);

                    if (digit == 10)
                    {
                        rectTransform.anchoredPosition = new Vector2((buttonWidth + padding) * j, -((buttonHeight + padding) * i - 80f));
                    }
                    else rectTransform.anchoredPosition = new Vector2((buttonWidth + padding) * j, -((buttonHeight + padding) * i));
                }

                buttonHeight = 150f; // Adjust as needed
            }
            }
        }
    }


    private void PositionKeyboard()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.5f, 0f);
        rectTransform.anchorMax = new Vector2(0.5f, 0f);
        rectTransform.pivot = new Vector2(0.5f, 0f);
        rectTransform.anchoredPosition = new Vector2(-160f, 470f); // Adjust Y position as needed
    }

    private void OnDigitButtonClick(int digit)
    {
        // Debug.Log("Clicked digit: " + digit);
        if (digit != 10) grid.UpdateSelectedCell(digit);

        else {
            whichSet = PlayerPrefs.GetString("whichSet");
            number = PlayerPrefs.GetInt("number");

            for (int a=0; a < 9; a++)
            {
                for(int b=0; b < 9; b++) {
                    sudokuLog = sudokuLog + grid.currentGridInt[a, b];
                
                }
            }
            PlayerPrefs.SetString("Sudoku", sudokuLog);
            if (whichSet == "set")
            {
                switch (number)
                {
                    case 1:
                        PlayerPrefs.SetString("Sudoku1", sudokuLog);
                        break;
                    case 2:
                        PlayerPrefs.SetString("Sudoku2", sudokuLog);
                        break;
                    case 3:
                        PlayerPrefs.SetString("Sudoku3", sudokuLog);
                        break;
                    case 4:
                        PlayerPrefs.SetString("Sudoku4", sudokuLog);
                        break;
                    case 5:
                        PlayerPrefs.SetString("Sudoku5", sudokuLog);
                        break;

                }
                sudokuLog = "";
                SceneManager.LoadScene("mainMenu");
            }
        } 

        // Handle the digit click event here, you can use it to input the digit into your application
    }
}
