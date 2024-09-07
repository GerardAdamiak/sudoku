using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Windows;

public class GridSquare : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public GameObject number_text;
    private int number_ = 0;
    private SudokuGrid grid;
    public RawImage squareRawImage; // Reference to the RawImage component
    public TextMeshProUGUI textMeshProComponent;
    // Define variables to hold the textures
    public Texture selectedTexture;
    public Texture originalTexture;
    public Texture secondTexture;
    private int index;
    public int gridRow;
    public int gridColumn;
    private bool ifAble=true;
    public static List<GridSquare> selectedCells = new(); // List to hold selected cells
   // private bool isSelecting = false;
    private DigitKeyboard digitKeyboard;


        
    void Start()
    {
        digitKeyboard = FindObjectOfType<DigitKeyboard>();
        grid = FindObjectOfType<SudokuGrid>();
        squareRawImage = GetComponentInChildren<RawImage>(); // Get the RawImage component from children

        // Store the original texture
        originalTexture = squareRawImage.texture;

    }
    


   
    public void ChangeTextColor(UnityEngine.Color color)
    {

        textMeshProComponent.color = color; // Assuming textMeshProComponent is the reference to your TextMeshPro component
    }
    

    public void OnPointerDown(PointerEventData eventData)
    {
        // Start selecting
        ClearSelectedCells();
        grid.DeselectAllGridSquares();
        SelectCell();
        //isSelecting = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Finish selecting
       // isSelecting = false;
        //PrintSelectedCells();
    }

    public void OnDrag(PointerEventData eventData)
    {

        SelectCell();
    }

    private void SelectCell()
    {
        
        GridSquare gridSquare = this;

        
        RaycastHit2D hit = Physics2D.Raycast(
            Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition),
            Vector2.zero
        );

        if (hit.collider != null && hit.collider.CompareTag("GridSquare"))
        {
           
            gridSquare = hit.collider.GetComponent<GridSquare>();

        }


        if (!selectedCells.Contains(gridSquare))
        {
           
            grid.SelectGridSquare(gridSquare);
            selectedCells.Add(gridSquare);
            Select();
        }

    }

    private void ClearSelectedCells()
    {
        
        foreach (var cell in selectedCells)
        {
            cell.Deselect();
        }
        selectedCells.Clear();
    }

    public void DisplayText()
    {
        if (number_ <= 0)
        {
            number_text.GetComponent<TextMeshProUGUI>().text = "";
        }
        else
        {
            if (grid.isFinished == true)
            {
                if (digitKeyboard.ifNote == true)
                {
                    
                        number_text.GetComponent<TextMeshProUGUI>().text = number_.ToString();
                    if(ifAble==true) number_text.GetComponent<TextMeshProUGUI>().fontSize = 35;
                      

                }
                else
                {
                    
                    number_text.GetComponent<TextMeshProUGUI>().text = number_.ToString();
                    number_text.GetComponent<TextMeshProUGUI>().fontSize = 60;
                }
            }
            else number_text.GetComponent<TextMeshProUGUI>().text = number_.ToString();
        }
    }


    public void Select()
    {
        squareRawImage = GetComponentInChildren<RawImage>();
        
            squareRawImage.texture = selectedTexture;
       
    }
   

    public void Check()
    {
        ifAble = true;
        grid = FindObjectOfType<SudokuGrid>();
        // Change the texture of the square when selected

        foreach (int digit in grid.lockedDigits)
        {
            if (digit == (gridRow * 9 * gridColumn)) ifAble = false;
        }
    }


    public void Deselect()
    {
        // Restore the original texture when deselected
        squareRawImage.texture = originalTexture;
    }

    public void SetNumber(int number)
    {
        
        grid = FindObjectOfType<SudokuGrid>();
        if (grid.isFinished == true)
        {
            
            ifAble = true;
            grid = FindObjectOfType<SudokuGrid>();
            // Change the texture of the square when selected

            foreach (int digit in grid.lockedDigits)
            {
                
                if (digit == (gridRow * 9 + gridColumn)) ifAble = false;
            }
        }
       // if (SceneManager.GetActiveScene().name == "whispers") ifAble = true;
         if (ifAble == true || grid.isFinished == false || (ifAble==false && number==0))
        {
           
            if (grid.isFinished == true && number != 0)
            {
                grid.moveStack.Push((gridRow, gridColumn, number_, digitKeyboard.ifNote));
                grid.PrintStackContents();
            }
            
            if (ifAble == true && (number != 0 || grid.isFinished==false))
            {
                if (number_ != number)
                {
                    number_ = number;
                }
                else
                {
                    number_ = 0;
                }
            }
            if (grid.isFinished == true && number == 0 && grid.moveStack.Count > 0)
            {
                var (row, column, previousNumber, ifNote) = grid.moveStack.Pop();
                grid.SetNumberAt(row, column, previousNumber, ifNote);
            }
        }
        DisplayText();
    }

    public void SetNumberNote(int number)
    {
        if (number_text.GetComponent<TextMeshProUGUI>().fontSize != 60 || number_ == 0)
        {
            grid = FindObjectOfType<SudokuGrid>();
            if (grid.isFinished == true)
            {
                ifAble = true;
                grid = FindObjectOfType<SudokuGrid>();
                // Change the texture of the square when selected

                foreach (int digit in grid.lockedDigits)
                {
                    if (digit == (gridRow * 9 + gridColumn)) ifAble = false;
                }
            }
            // if (SceneManager.GetActiveScene().name == "whispers") ifAble = true;
            if (ifAble == true || grid.isFinished == false || (ifAble == false && number == 0))
            {
                if (grid.isFinished == true && number != 0)
                {
                    grid.moveStack.Push((gridRow, gridColumn, number_, digitKeyboard.ifNote));
                    grid.PrintStackContents();
                }

                if (ifAble == true)
                {
                    string number_string = number_.ToString();
                    string numberStr = number.ToString();
                    char numberChar = numberStr[0];
                    bool ifContain = number_string.Contains(numberStr);

                    if (ifContain == false)
                    {
                        number_string += numberStr;
                        number_string.OrderBy(c => c).ToArray();
                        char[] characters = number_string.ToArray();
                        Array.Sort(characters);
                        number_string = new string(characters);

                    }
                    else
                    {

                        number_string = number_string.Replace(numberStr, "");

                    }
                    if (number_string != "") number_ = int.Parse(number_string);
                    else number_ = 0;
                }
                if (grid.isFinished == true && number == 0 && grid.moveStack.Count > 0)
                {
                    var (row, column, previousNumber, ifNote) = grid.moveStack.Pop();
                    grid.SetNumberAt(row, column, previousNumber, ifNote);
                }
            }
            DisplayText();
        }
        
    }


    public void SetNumber2(int number, bool ifNote2)
    {
        if (ifNote2 == false)
        {
            number_text.GetComponent<TextMeshProUGUI>().fontSize = 60;

        }
        else
        {
            number_text.GetComponent<TextMeshProUGUI>().fontSize = 35;
        }

        if (number_ != number)
        {
            number_ = number;
        }
        else
        {
            number_ = 0;
        }

        if (number_ <= 0)
        {
            number_text.GetComponent<TextMeshProUGUI>().text = "";
        }
        
        else number_text.GetComponent<TextMeshProUGUI>().text = number_.ToString();

        
    }

    public void SetGrid(int row, int column)
    {

        gridRow = row;
        gridColumn = column;

    }
    public string GetNumber()
    {
        return number_text.GetComponent<TextMeshProUGUI>().text;
    }
    public void SetIndex(int idx)
    {
        index = idx;
    }

    public int GetIndex()
    {
        return index;
    }

    
}