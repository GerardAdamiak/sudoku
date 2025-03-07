using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class TextCycler : MonoBehaviour
{
    // Reference to the TMP Text component
    [SerializeField] private TMP_Text tmpText;

    // Array of texts to cycle through
    [SerializeField] private string[] textArray;

    // Time interval between text changes
    [SerializeField] private float cycleInterval = 2f;

    //[SerializeField] private SpriteRenderer targetSpriteRenderer;
    //[SerializeField] private SpriteRenderer targetSpriteRenderer2;
    //[SerializeField] private SpriteRenderer KolkoNotes;
    //[SerializeField] private SpriteRenderer KolkoThermoLewo;
    //[SerializeField] private SpriteRenderer KolkoThermoSrodek;
    //[SerializeField] private SpriteRenderer KolkoThermoPrawo;
    //[SerializeField] private SpriteRenderer KolkoWhisperLewo;
    //[SerializeField] private SpriteRenderer KolkoWhisperGora;
    //[SerializeField] private SpriteRenderer KolkoWhisperPrawo;

    private SudokuGrid sudokuGrid;
    private DigitKeyboard digitKeyboard;
    public Texture tutoSquare;
    public Texture normalSquare;
    public Image image;
    public Image image2;
    private Texture prevText1, prevText2, prevText3, prevText4, prevText5, prevText6, prevText7;

    public EventSystem eventSystem;
    public GraphicRaycaster raycaster;
    private LoadingScene loadingScene;

    // Current index in the array
    public int currentIndex = 0;

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

    private void Increment()
    {
       currentIndex++;
    }

    public void Decrement()
    {
        currentIndex--;
    }

    void Update()
    {

        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                if (Input.GetTouch(i).phase == TouchPhase.Ended)
                {
                    PointerEventData pointerData = new PointerEventData(eventSystem)
                    {
                        position = Input.GetTouch(i).position
                    };

                    List<RaycastResult> results = new List<RaycastResult>();
                    raycaster.Raycast(pointerData, results);

                    foreach (RaycastResult result in results)
                    {
                        if (result.gameObject.CompareTag("tutoPrev"))
                        {
                            Decrement();
                            Debug.Log(currentIndex);
                            break;
                        }
                        else if (result.gameObject.CompareTag("tutoNext"))
                        {
                            Increment();
                            Debug.Log(currentIndex);
                            break;
                        }
                    }
                }
            }
        }
    }

    private IEnumerator CycleText()
    {
        
        
        

        prevText1 = sudokuGrid.grid_squares_[31].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture;
        prevText2 = sudokuGrid.grid_squares_[40].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture;
        prevText3 = sudokuGrid.grid_squares_[41].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture;
        prevText4 = sudokuGrid.grid_squares_[68].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture;
        prevText5 = sudokuGrid.grid_squares_[67].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture;
        prevText6 = sudokuGrid.grid_squares_[66].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture;
        prevText7 = sudokuGrid.grid_squares_[57].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture;


        while (currentIndex != 29)
        {
            if (currentIndex == 3)
            {
                sudokuGrid.grid_squares_[31].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[40].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[41].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[68].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[67].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[66].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[57].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                //targetSpriteRenderer.enabled = true;
                // Wait until the condition is met

            }
            if (currentIndex == 4)
            {
                sudokuGrid.grid_squares_[31].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = prevText1;
                sudokuGrid.grid_squares_[40].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = prevText2;
                sudokuGrid.grid_squares_[41].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = prevText3;
                sudokuGrid.grid_squares_[68].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = prevText4;
                sudokuGrid.grid_squares_[67].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = prevText5;
                sudokuGrid.grid_squares_[66].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = prevText6;
                sudokuGrid.grid_squares_[57].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = prevText7;
                //targetSpriteRenderer.enabled = true;
                // Wait until the condition is met

            }
            if (currentIndex == 5)
            {
                sudokuGrid.grid_squares_[31].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                
            }

            if (currentIndex == 6)
            {
                
               
                //targetSpriteRenderer.enabled = true;
                // Wait until the condition is met
                while (sudokuGrid.grid_squares_[31].GetComponent<GridSquare>().GetComponentInChildren<TextMeshProUGUI>().text != "1")
                {
                    
                    // Pause briefly before checking again
                    yield return new WaitForSeconds(0.1f);
                   
                }
                
            }
            if (currentIndex == 7)
            {
                sudokuGrid.grid_squares_[68].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;

            }

            if (currentIndex == 8)
            {
                //targetSpriteRenderer2.enabled = true;
                // Wait until the condition is met
                while (sudokuGrid.grid_squares_[68].GetComponent<GridSquare>().GetComponentInChildren<TextMeshProUGUI>().text != "6")
                {
                    
                    // Pause briefly before checking again
                    yield return new WaitForSeconds(0.1f);
                }
            }

            if (currentIndex == 9)
            {
                sudokuGrid.grid_squares_[0].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[1].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[2].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[9].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[10].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[11].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[18].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[19].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[27].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[28].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[29].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[30].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[31].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[68].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[69].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
               

            }

            if (currentIndex == 10)
            {
                sudokuGrid.grid_squares_[0].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = normalSquare;
                sudokuGrid.grid_squares_[1].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = normalSquare;
                sudokuGrid.grid_squares_[2].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = normalSquare;
                sudokuGrid.grid_squares_[9].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = normalSquare;
                sudokuGrid.grid_squares_[10].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = normalSquare;
                sudokuGrid.grid_squares_[11].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = normalSquare;
                sudokuGrid.grid_squares_[18].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = normalSquare;
                sudokuGrid.grid_squares_[19].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = normalSquare;
                sudokuGrid.grid_squares_[27].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = normalSquare;
                sudokuGrid.grid_squares_[28].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = normalSquare;
                sudokuGrid.grid_squares_[29].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = normalSquare;
                sudokuGrid.grid_squares_[30].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = normalSquare;
                sudokuGrid.grid_squares_[31].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = normalSquare;
                sudokuGrid.grid_squares_[68].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = normalSquare;
                sudokuGrid.grid_squares_[69].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = normalSquare;


            }

            if (currentIndex == 11)
            {
                sudokuGrid.grid_squares_[0].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[1].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[9].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[11].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[19].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[27].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[30].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[69].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;


            }

            if (currentIndex == 12)
            {
                while (sudokuGrid.grid_squares_[0].GetComponent<GridSquare>().GetComponentInChildren<TextMeshProUGUI>().text != "1" ||
                sudokuGrid.grid_squares_[1].GetComponent<GridSquare>().GetComponentInChildren<TextMeshProUGUI>().text != "6" ||
                sudokuGrid.grid_squares_[9].GetComponent<GridSquare>().GetComponentInChildren<TextMeshProUGUI>().text != "2" ||
                sudokuGrid.grid_squares_[11].GetComponent<GridSquare>().GetComponentInChildren<TextMeshProUGUI>().text != "9" ||
                sudokuGrid.grid_squares_[19].GetComponent<GridSquare>().GetComponentInChildren<TextMeshProUGUI>().text != "4" ||
                sudokuGrid.grid_squares_[27].GetComponent<GridSquare>().GetComponentInChildren<TextMeshProUGUI>().text != "9" ||
                sudokuGrid.grid_squares_[30].GetComponent<GridSquare>().GetComponentInChildren<TextMeshProUGUI>().text != "3" ||
                sudokuGrid.grid_squares_[69].GetComponent<GridSquare>().GetComponentInChildren<TextMeshProUGUI>().text != "9")
                {
                    yield return new WaitForSeconds(0.1f);
                }
               


            }

            if (currentIndex == 13)
            {

                sudokuGrid.grid_squares_[15].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[16].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[17].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[26].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[35].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[34].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[43].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[42].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[27].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[36].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[45].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[54].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[63].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[72].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[78].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[79].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[69].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;



            }
            if (currentIndex == 14)
            {
                sudokuGrid.grid_squares_[15].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = normalSquare;
                sudokuGrid.grid_squares_[16].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = normalSquare;
                sudokuGrid.grid_squares_[17].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = normalSquare;
                sudokuGrid.grid_squares_[26].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = normalSquare;
                sudokuGrid.grid_squares_[35].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = normalSquare;
                sudokuGrid.grid_squares_[34].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = normalSquare;
                sudokuGrid.grid_squares_[43].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = normalSquare;
                sudokuGrid.grid_squares_[42].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = normalSquare;
                sudokuGrid.grid_squares_[27].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = normalSquare;
                sudokuGrid.grid_squares_[36].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = normalSquare;
                sudokuGrid.grid_squares_[45].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = normalSquare;
                sudokuGrid.grid_squares_[54].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = normalSquare;
                sudokuGrid.grid_squares_[63].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = normalSquare;
                sudokuGrid.grid_squares_[72].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = normalSquare;
                sudokuGrid.grid_squares_[69].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = normalSquare;
                sudokuGrid.grid_squares_[78].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = normalSquare;
                sudokuGrid.grid_squares_[79].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = normalSquare;
            }


            if (currentIndex == 15)//z tym podzialac muszeee
            {
                sudokuGrid.grid_squares_[15].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[16].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[17].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[26].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[35].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[34].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[43].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[42].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;

            }

            if (currentIndex == 16)
            {
               
                // Wait until the condition is met
                while (sudokuGrid.grid_squares_[34].GetComponent<GridSquare>().GetComponentInChildren<TextMeshProUGUI>().text != "5")
                {

                    // Pause briefly before checking again
                    yield return new WaitForSeconds(0.1f);
                }
                sudokuGrid.grid_squares_[15].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = normalSquare;
                sudokuGrid.grid_squares_[16].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = normalSquare;
                sudokuGrid.grid_squares_[17].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = normalSquare;
                sudokuGrid.grid_squares_[26].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = normalSquare;
                sudokuGrid.grid_squares_[35].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = normalSquare;
                sudokuGrid.grid_squares_[34].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = normalSquare;
                sudokuGrid.grid_squares_[43].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = normalSquare;
                sudokuGrid.grid_squares_[42].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = normalSquare;

                sudokuGrid.grid_squares_[27].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[36].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[45].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[54].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[63].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[72].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[78].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[69].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[79].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;

            }
            if (currentIndex == 17)
            {

                // Wait until the condition is met
                while (sudokuGrid.grid_squares_[63].GetComponent<GridSquare>().GetComponentInChildren<TextMeshProUGUI>().text != "5" || sudokuGrid.grid_squares_[79].GetComponent<GridSquare>().GetComponentInChildren<TextMeshProUGUI>().text != "8")
                {

                    // Pause briefly before checking again
                    yield return new WaitForSeconds(0.1f);
                }
                sudokuGrid.grid_squares_[27].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = normalSquare;
                sudokuGrid.grid_squares_[36].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = normalSquare;
                sudokuGrid.grid_squares_[45].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = normalSquare;
                sudokuGrid.grid_squares_[54].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = normalSquare;
                sudokuGrid.grid_squares_[63].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = normalSquare;
                sudokuGrid.grid_squares_[72].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = normalSquare;
                sudokuGrid.grid_squares_[78].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = normalSquare;
                sudokuGrid.grid_squares_[69].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = normalSquare;
                sudokuGrid.grid_squares_[79].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = normalSquare;
            }
            if (currentIndex == 18)
            {
                sudokuGrid.grid_squares_[45].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[46].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[34].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[33].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;

            }

            if (currentIndex == 20)
            {
                sudokuGrid.grid_squares_[45].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = normalSquare;
                sudokuGrid.grid_squares_[46].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[33].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[34].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = normalSquare;

            }
            if (currentIndex == 21)
            {
                // Wait until the condition is met
                while (sudokuGrid.grid_squares_[46].GetComponent<GridSquare>().GetComponentInChildren<TextMeshProUGUI>().text != "3" || sudokuGrid.grid_squares_[33].GetComponent<GridSquare>().GetComponentInChildren<TextMeshProUGUI>().text != "6")
                {

                    // Pause briefly before checking again
                    yield return new WaitForSeconds(0.1f);
                }

            }
            if (currentIndex == 22)
            {
                // Wait until the condition is met
                sudokuGrid.grid_squares_[4].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[5].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[11].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[12].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[13].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[14].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[23].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[24].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[72].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[73].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[64].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[55].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[56].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[57].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[79].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[80].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
            }

            if (currentIndex == 24)
            {
                // Wait until the condition is met
                sudokuGrid.grid_squares_[4].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = normalSquare;
                sudokuGrid.grid_squares_[5].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = normalSquare;
                sudokuGrid.grid_squares_[11].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = normalSquare;
                sudokuGrid.grid_squares_[12].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = normalSquare;
                sudokuGrid.grid_squares_[13].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = normalSquare;
                sudokuGrid.grid_squares_[14].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = normalSquare;
                sudokuGrid.grid_squares_[23].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = normalSquare;
                sudokuGrid.grid_squares_[24].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = normalSquare;
                sudokuGrid.grid_squares_[72].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[73].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[64].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[55].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[56].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[57].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = tutoSquare;
                sudokuGrid.grid_squares_[79].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = normalSquare;
                sudokuGrid.grid_squares_[80].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = normalSquare;
            }
            if (currentIndex == 25)
            {
                // Wait until the condition is met
                while (sudokuGrid.grid_squares_[73].GetComponent<GridSquare>().GetComponentInChildren<TextMeshProUGUI>().text != "9" || sudokuGrid.grid_squares_[56].GetComponent<GridSquare>().GetComponentInChildren<TextMeshProUGUI>().text != "1")
                {

                    // Pause briefly before checking again
                    yield return new WaitForSeconds(0.1f);
                }
                sudokuGrid.grid_squares_[72].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = normalSquare;
                sudokuGrid.grid_squares_[73].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = normalSquare;
                sudokuGrid.grid_squares_[64].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = normalSquare;
                sudokuGrid.grid_squares_[55].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = normalSquare;
                sudokuGrid.grid_squares_[56].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = normalSquare;
                sudokuGrid.grid_squares_[57].GetComponent<GridSquare>().GetComponentInChildren<RawImage>().texture = normalSquare;

            }
            if (currentIndex == 26)
            {
                // Wait until the condition is met
                while (sudokuGrid.grid_squares_[12].GetComponent<GridSquare>().GetComponentInChildren<TextMeshProUGUI>().text != "1" || sudokuGrid.grid_squares_[4].GetComponent<GridSquare>().GetComponentInChildren<TextMeshProUGUI>().text != "2" || sudokuGrid.grid_squares_[23].GetComponent<GridSquare>().GetComponentInChildren<TextMeshProUGUI>().text != "9" || sudokuGrid.grid_squares_[24].GetComponent<GridSquare>().GetComponentInChildren<TextMeshProUGUI>().text != "1" || sudokuGrid.grid_squares_[80].GetComponent<GridSquare>().GetComponentInChildren<TextMeshProUGUI>().text != "3")
                {

                    // Pause briefly before checking again
                    yield return new WaitForSeconds(0.1f);
                }

            }

            if (currentIndex == 27)
            {
                // Wait until the condition is met
                while (sudokuGrid.tutorialDone != true)
                {

                    // Pause briefly before checking again
                    yield return new WaitForSeconds(0.1f);
                }

            }

            if (currentIndex == 28)
            {
                // Wait until the condition is met
                SceneManager.LoadScene("mainMenu");

            }

            // Set the text to the current array element
            if(currentIndex >= 0) tmpText.text = textArray[currentIndex];

            

            // Increment the index and loop back if necessary
            //currentIndex = currentIndex + 1;

            // Wait for the specified interval
            yield return new WaitForSeconds(cycleInterval);
        }
    }

}
