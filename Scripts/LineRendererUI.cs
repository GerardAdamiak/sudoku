using System.Collections.Generic;
using TMPro;
using UnityEngine;



public class LineRendererUi : MonoBehaviour
{

    public int columns = 9;
    public int rows = 9;
    public float every_square_offset = 0.0f;
    public GameObject grid_square;
    public Vector2 start_position = new(0.0f, 0.0f);
    public float square_scale = 1.0f;
    public List<GridSquare> selectedCells = new();
    private List<GameObject> grid_squares_ = new();
    public Canvas canvas;
    
   
    void Start()
    {

        CreateGrid();
       

    }

    void Update()
    {
       
        SetGridNumbers();
        
    }


   

    private void CreateGrid()
    {
        SpawnGridSquares();
        SetSquaresPosition();
    }

    private void SpawnGridSquares()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                grid_squares_.Add(Instantiate(grid_square) as GameObject);
                grid_squares_[^1].transform.SetParent(this.transform);
                grid_squares_[^1].transform.localScale = new Vector3(
                    square_scale,
                    square_scale,
                    1
                );
            }
        }
    }

    // Reference to the black panel


    private void SetSquaresPosition()
    {
        // Set up the black panel size and position
        // RectTransform panelRect = blackPanel.GetComponent<RectTransform>();

        // Calculate the diagonal size to ensure it covers from top-left to bottom-right
        var square_rect = grid_squares_[0].GetComponent<RectTransform>();

        Vector2 offset =
            new()
            {
                x =
                    square_rect.rect.width * square_rect.transform.localScale.x
                    + every_square_offset,
                y =
                    square_rect.rect.height * square_rect.transform.localScale.y
                    + every_square_offset
            };

        int column_number = 0;
        int row_number = 0;
        int column_offset = 5;
        int row_offset = 5;
        int counterX = 0;
        int counterY = 0;
        foreach (GameObject square in grid_squares_)
        {
            if (column_number + 1 > columns)
            {
                row_number++;
                if (row_number % 3 == 0)
                    counterY++;
                column_number = 0;
                counterX = 0;
            }
            var pos_x_offset = offset.x * column_number + row_offset * counterX;
            var pos_y_offset = offset.y * row_number + column_offset * counterY;
            square.GetComponent<RectTransform>().anchoredPosition = new Vector2(
                start_position.x + pos_x_offset + 10,
                start_position.y - pos_y_offset
            );

            // Store the current X and Y values
            float x = square.transform.position.x;
            float y = square.transform.position.y;

            // Update the position, keeping X and Y the same, and setting Z to 90
            square.transform.position = new Vector3(x, y, 90);


            column_number++;
            if (column_number % 3 == 0)
                counterX++;
        }
    }


    


    private void SetGridNumbers()
    {
        SudokuGrid sudokugrid = FindObjectOfType<SudokuGrid>();
        int x = 0;

        foreach (GameObject square in grid_squares_)
        {

            



            // Get the GridSquare components
           
            var gridSquare2 = sudokugrid.grid_squares_[x].GetComponent<GridSquare>();

            // Find the TMP_Text components within each square
            TMP_Text textComponent = square.GetComponentInChildren<TMP_Text>();
            TMP_Text textComponent2 = sudokugrid.grid_squares_[x].GetComponentInChildren<TMP_Text>();

            // If both TMP_Text components are found, copy the properties
            if (textComponent != null && textComponent2 != null)
            {
                textComponent.text = textComponent2.text;           // Copy text
                textComponent.fontSize = textComponent2.fontSize;   // Copy font size
                textComponent.color = textComponent2.color;         // Copy color
            }
            x++;
        }
    }




   
  
   
   
}
