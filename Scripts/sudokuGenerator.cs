using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GridSquare : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public GameObject number_text;
    private int number_ = 0;
    private sudokuGrid grid;
    private RawImage squareRawImage; // Reference to the RawImage component
    public TextMeshProUGUI textMeshProComponent;
    // Define variables to hold the textures
    public Texture selectedTexture;
    private Texture originalTexture;
    private int index;
    public int gridRow;
    public int gridColumn;
    private bool ifAble;

    private static List<GridSquare> selectedCells = new List<GridSquare>(); // List to hold selected cells
    private bool isSelecting = false;



    void Start()
    {
        grid = FindObjectOfType<sudokuGrid>();
        squareRawImage = GetComponentInChildren<RawImage>(); // Get the RawImage component from children

        // Store the original texture
        originalTexture = squareRawImage.texture;

    }

    void Update()
    {

    }
    public void ChangeTextColor(Color color)
    {
        textMeshProComponent.color = color; // Assuming textMeshProComponent is the reference to your TextMeshPro component
    }
    //public void OnPointerClick(PointerEventData eventData)
    //{
    //    // Handle grid square click
    //    grid.SelectGridSquare(this);
    //}

    public void OnPointerDown(PointerEventData eventData)
    {
        // Start selecting
        ClearSelectedCells();
        SelectCell();
        isSelecting = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Finish selecting
        isSelecting = false;
        PrintSelectedCells();
    }

    public void OnDrag(PointerEventData eventData)
    {

        SelectCell();
    }

    private void SelectCell()
    {
        Debug.Log("SelectCell");
        GridSquare gridSquare = this;
        
            Debug.Log("SelectCell.EventSystem");
            RaycastHit2D hit = Physics2D.Raycast(
                Camera.main.ScreenToWorldPoint(Input.mousePosition),
                Vector2.zero
            );
            
            if (hit.collider != null && hit.collider.CompareTag("GridSquare"))
            {
                Debug.Log("SelectCell.Hit");
                gridSquare = hit.collider.GetComponent<GridSquare>();
                
            }
        

        if (!selectedCells.Contains(gridSquare))
        {
            Debug.Log("SelectCell.Contain");
            grid.SelectGridSquare(gridSquare);
            selectedCells.Add(gridSquare);
            Select();
        }
        
    }

    private void ClearSelectedCells()
    {
        Debug.Log("ClearSeletedCells");
        foreach (var cell in selectedCells)
        {
            cell.Deselect();
        }
        selectedCells.Clear();
    }

    public void DisplayText()
    {
        if (number_ <= 0)
            number_text.GetComponent<TextMeshProUGUI>().text = "";
        else
            number_text.GetComponent<TextMeshProUGUI>().text = number_.ToString();
    }

    public void Select()
    {
        squareRawImage.texture = selectedTexture;
    }

    public void Check()
    {
        ifAble = true;
        grid = FindObjectOfType<sudokuGrid>();
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
        grid = FindObjectOfType<sudokuGrid>();
        if (grid.isFinished == true)
        {
            ifAble = true;
            grid = FindObjectOfType<sudokuGrid>();
            // Change the texture of the square when selected

            foreach (int digit in grid.lockedDigits)
            {
                if (digit == (gridRow * 9 + gridColumn)) ifAble = false;
            }
        }

        if (ifAble == true || grid.isFinished == false)
        {
            if (grid.isFinished == true && number != 0)
            {
                grid.moveStack.Push((gridRow, gridColumn, number_));
            }

            if (number_ != number)
            {
                number_ = number;
            }
            else
            {
                number_ = 0;
            }

            if (grid.isFinished == true && number == 0 && grid.moveStack.Count > 0)
            {
                var lastMove = grid.moveStack.Pop();
                grid.SetNumberAt(lastMove.row, lastMove.column, lastMove.previousNumber);
            }
        }
        DisplayText();
    }


    public void SetNumber2(int number)
    {
     

            if (number_ != number)
            {
                number_ = number;
            }
            else
            {
                number_ = 0;
            }

        
        DisplayText();
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

    private void PrintSelectedCells()
    {
        Debug.Log("Selected Cells:");
        foreach (var cell in selectedCells)
        {
            Debug.Log($"Row: {cell.gridRow}, Column: {cell.gridColumn}, Number: {cell.number_}");
        }
    }
}
