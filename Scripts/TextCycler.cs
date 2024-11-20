using System.Collections;
using TMPro;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class TextCycler : MonoBehaviour
{
    // Reference to the TMP Text component
    [SerializeField] private TMP_Text tmpText;

    // Array of texts to cycle through
    [SerializeField] private string[] textArray;

    // Time interval between text changes
    [SerializeField] private float cycleInterval = 2f;

    [SerializeField] private SpriteRenderer targetSpriteRenderer;
    [SerializeField] private SpriteRenderer targetSpriteRenderer2;
    [SerializeField] private SpriteRenderer KolkoNotes;
    [SerializeField] private SpriteRenderer KolkoThermoLewo;
    [SerializeField] private SpriteRenderer KolkoThermoSrodek;
    [SerializeField] private SpriteRenderer KolkoThermoPrawo;

    private SudokuGrid sudokuGrid;
    private DigitKeyboard digitKeyboard;
    // Current index in the array
    private int currentIndex = 0;

    private void Start()
    {
        // Validate the TMP reference
        if (tmpText == null)
        {
            Debug.LogError("TMP_Text component is not assigned.");
            return;
        }

        // Validate the text array
        if (textArray == null || textArray.Length == 0)
        {
            Debug.LogError("Text array is empty. Add some text to cycle through.");
            return;
        }

        // Start the text cycling coroutine
        StartCoroutine(CycleText());
    }

    private IEnumerator CycleText()
    {
        sudokuGrid = FindObjectOfType<SudokuGrid>();
        digitKeyboard = FindObjectOfType<DigitKeyboard>();

        while (true)
        {
            if (currentIndex == 6)
            {
                // Wait until the condition is met
                while (digitKeyboard.ifNote != true)
                {
                    // Pause briefly before checking again
                    yield return new WaitForSeconds(0.1f);
                }
            }


            if (currentIndex == 7)
            {
                targetSpriteRenderer.enabled = true;
                // Wait until the condition is met
                while (sudokuGrid.grid_squares_[31].GetComponent<GridSquare>().GetComponentInChildren<TextMeshProUGUI>().text != "124" || sudokuGrid.grid_squares_[40].GetComponent<GridSquare>().GetComponentInChildren<TextMeshProUGUI>().text != "124" || sudokuGrid.grid_squares_[41].GetComponent<GridSquare>().GetComponentInChildren<TextMeshProUGUI>().text != "124")
                {
                    
                    // Pause briefly before checking again
                    yield return new WaitForSeconds(0.1f);
                   
                }
            }

            if (currentIndex == 8)
            {
                targetSpriteRenderer2.enabled = true;
                // Wait until the condition is met
                while (sudokuGrid.grid_squares_[57].GetComponent<GridSquare>().GetComponentInChildren<TextMeshProUGUI>().text != "6789" || sudokuGrid.grid_squares_[66].GetComponent<GridSquare>().GetComponentInChildren<TextMeshProUGUI>().text != "6789" || sudokuGrid.grid_squares_[67].GetComponent<GridSquare>().GetComponentInChildren<TextMeshProUGUI>().text != "6789" || sudokuGrid.grid_squares_[68].GetComponent<GridSquare>().GetComponentInChildren<TextMeshProUGUI>().text != "6789")
                {
                    
                    // Pause briefly before checking again
                    yield return new WaitForSeconds(0.1f);
                }
            }

            if (currentIndex == 10)
            {
                sudokuGrid.grid_squares_[31].GetComponent<GridSquare>().SetNumberNote(35);
                sudokuGrid.grid_squares_[30].GetComponent<GridSquare>().SetNumberNote(23456);
                sudokuGrid.grid_squares_[29].GetComponent<GridSquare>().SetNumberNote(34567);
                sudokuGrid.grid_squares_[28].GetComponent<GridSquare>().SetNumberNote(45678);
                sudokuGrid.grid_squares_[27].GetComponent<GridSquare>().SetNumberNote(56789);
                sudokuGrid.grid_squares_[69].GetComponent<GridSquare>().SetNumberNote(789);

            }

            if (currentIndex == 13)
            {
                
                sudokuGrid.grid_squares_[0].GetComponent<GridSquare>().SetNumber(3);
                sudokuGrid.grid_squares_[0].GetComponent<GridSquare>().GetComponentInChildren<TextMeshProUGUI>().fontSize = 60;


            }
            if (currentIndex == 14)
            {
                
                sudokuGrid.grid_squares_[9].GetComponent<GridSquare>().SetNumber(4);
                sudokuGrid.grid_squares_[9].GetComponent<GridSquare>().GetComponentInChildren<TextMeshProUGUI>().fontSize = 60;
                
                sudokuGrid.grid_squares_[18].GetComponent<GridSquare>().SetNumber(5);
                sudokuGrid.grid_squares_[18].GetComponent<GridSquare>().GetComponentInChildren<TextMeshProUGUI>().fontSize = 60;

                sudokuGrid.grid_squares_[19].GetComponent<GridSquare>().SetNumber(6);
                sudokuGrid.grid_squares_[19].GetComponent<GridSquare>().GetComponentInChildren<TextMeshProUGUI>().fontSize = 60;
                
                sudokuGrid.grid_squares_[10].GetComponent<GridSquare>().SetNumber(7);
                sudokuGrid.grid_squares_[10].GetComponent<GridSquare>().GetComponentInChildren<TextMeshProUGUI>().fontSize = 60;
                
                sudokuGrid.grid_squares_[1].GetComponent<GridSquare>().SetNumber(8);
                sudokuGrid.grid_squares_[1].GetComponent<GridSquare>().GetComponentInChildren<TextMeshProUGUI>().fontSize = 60;
                
                sudokuGrid.grid_squares_[2].GetComponent<GridSquare>().SetNumber(9);
                sudokuGrid.grid_squares_[2].GetComponent<GridSquare>().GetComponentInChildren<TextMeshProUGUI>().fontSize = 60;



            }


            if (currentIndex == 15)//z tym podzialac muszeee
            {
                sudokuGrid.grid_squares_[0].GetComponent<GridSquare>().GetComponentInChildren<TextMeshProUGUI>().text = "";
                sudokuGrid.grid_squares_[0].GetComponent<GridSquare>().GetComponentInChildren<TextMeshProUGUI>().fontSize = 35;

                sudokuGrid.grid_squares_[9].GetComponent<GridSquare>().GetComponentInChildren<TextMeshProUGUI>().text = "";
                sudokuGrid.grid_squares_[9].GetComponent<GridSquare>().GetComponentInChildren<TextMeshProUGUI>().fontSize = 35;

                sudokuGrid.grid_squares_[18].GetComponent<GridSquare>().GetComponentInChildren<TextMeshProUGUI>().text = "";
                sudokuGrid.grid_squares_[18].GetComponent<GridSquare>().GetComponentInChildren<TextMeshProUGUI>().fontSize = 35;

                sudokuGrid.grid_squares_[19].GetComponent<GridSquare>().GetComponentInChildren<TextMeshProUGUI>().text = "";
                sudokuGrid.grid_squares_[19].GetComponent<GridSquare>().GetComponentInChildren<TextMeshProUGUI>().fontSize = 35;

                sudokuGrid.grid_squares_[10].GetComponent<GridSquare>().GetComponentInChildren<TextMeshProUGUI>().text = "";
                sudokuGrid.grid_squares_[10].GetComponent<GridSquare>().GetComponentInChildren<TextMeshProUGUI>().fontSize = 35;

                sudokuGrid.grid_squares_[1].GetComponent<GridSquare>().GetComponentInChildren<TextMeshProUGUI>().text = "";
                sudokuGrid.grid_squares_[1].GetComponent<GridSquare>().GetComponentInChildren<TextMeshProUGUI>().fontSize = 35;

                sudokuGrid.grid_squares_[2].GetComponent<GridSquare>().GetComponentInChildren<TextMeshProUGUI>().text = "";
                sudokuGrid.grid_squares_[2].GetComponent<GridSquare>().GetComponentInChildren<TextMeshProUGUI>().fontSize = 35;

                // Wait until the condition is met

            }

            if (currentIndex == 16)
            {
               
                // Wait until the condition is met
                while (sudokuGrid.grid_squares_[0].GetComponent<GridSquare>().GetComponentInChildren<TextMeshProUGUI>().text != "12")
                {

                    // Pause briefly before checking again
                    yield return new WaitForSeconds(0.1f);
                }
            }
            // Set the text to the current array element
            tmpText.text = textArray[currentIndex];

            // Activate or deactivate components as needed
            targetSpriteRenderer.enabled = (currentIndex == 3);
            targetSpriteRenderer2.enabled = (currentIndex == 3);
            KolkoNotes.enabled = (currentIndex == 5);
            KolkoThermoLewo.enabled = (currentIndex == 9 || currentIndex == 11);
            KolkoThermoSrodek.enabled = (currentIndex == 9 || currentIndex == 10);
            KolkoThermoPrawo.enabled = (currentIndex == 9 || currentIndex == 10);

            // Increment the index and loop back if necessary
            currentIndex = (currentIndex + 1) % textArray.Length;

            // Wait for the specified interval
            yield return new WaitForSeconds(cycleInterval);
        }
    }

}
