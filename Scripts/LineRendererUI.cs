using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Random = System.Random;
using System.Diagnostics;
using System.Linq;
using Unity.VisualScripting;


public class LineRendererUi : MonoBehaviour
{



    private int[,] grid = new int[9, 9];
    private string[,] currentGrid = new string[9, 9];
    public char[,] currentGridInt = new char[9, 9];
    private int[,] temp = new int[9, 9];
    public int columns = 9;
    public int rows = 9;
    public float every_square_offset = 0.0f;
    public GameObject grid_square;
    public Vector2 start_position = new(0.0f, 0.0f);
    public float square_scale = 1.0f;
    public int squaresToDelete;
    public List<GridSquare> selectedCells = new();
    public List<GameObject> grid_squares_ = new();
    public Canvas canvas;
    public GameObject previous;
    public GameObject diff;
    public static string currentSceneName;
    public static bool endChecker;
    private int e;
    private HashSet<Grid> set;
    public int g;
    private bool ifOk = false;
    private Color redHexColor = new Color32(180, 44, 15, 255);
    private int customNumber;
    private string sudokuLog;
    public HashSet<int> lockedDigits = new();
    public bool isFinished;
    public Stack<(int row, int column, int previousNumber, bool ifNote)> moveStack = new();
    public GameObject leaderboardText;
    private string whichSet;
    public GameObject linePrefab;
    public GameObject dotPrefab;
    private bool done = false;
    private string direction;
    private DigitKeyboard keyboard;
    public bool ifMore;
    private int x;
    private bool ifDot;
    private int newCell;
    private int loopCounter = 0;
    private bool ifSingleCage = false;
    public Image blackSquare;
    private int randomDigit;
    private LineRendererUi lineRend;
    void Start()
    {

        CreateGrid();
       

    }

    void Update()
    {
        GetCurrentGridState();
        ChangeColor();
        ChangeKillerColor();
        
        SetGridNumbers();
       
    }


    public void SetNumberAt(int row, int column, int number, bool ifNote)
    {
        grid[row, column] = number;
        grid_squares_[(row * 9) + column].GetComponent<GridSquare>().SetNumber2(number, ifNote);

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


    
   
    private int[] GenerateRandomOrder()
    {
        int[] order = new int[9];
        for (int i = 0; i < 9; i++)
        {
            order[i] = i + 1;
        }

        System.Random rnd = new();
        for (int i = 0; i < 9; i++)
        {
            int temp = order[i];
            int randomIndex = rnd.Next(i, 9);
            order[i] = order[randomIndex];
            order[randomIndex] = temp;
        }

        return order;
    }

    private bool IsValidPlacement(int row, int col, int num)
    {
        for (int i = 0; i < 9; i++)
        {
            if (grid[row, i] == num || grid[i, col] == num)
                return false;
        }

        int subgridRow = row - row % 3;
        int subgridCol = col - col % 3;
        for (int i = subgridRow; i < subgridRow + 3; i++)
        {
            for (int j = subgridCol; j < subgridCol + 3; j++)
            {
                if (grid[i, j] == num)
                    return false;
            }
        }

        return true;
    }



    private void SetGridNumbers()
    {
        SudokuGrid sudokugrid = FindObjectOfType<SudokuGrid>();

        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                int index = row * 9 + col;

                var square = sudokugrid.grid_squares_[0];
                // Get references to the current grid squares from both arrays
               if (grid_squares_.Count != 0) square = grid_squares_[index];
                    var square2 = sudokugrid.grid_squares_[index];

                
                // Get the GridSquare components
                var gridSquare = square.GetComponent<GridSquare>();
                var gridSquare2 = square2.GetComponent<GridSquare>();

                // Find the TMP_Text components within each square
                TMP_Text textComponent = square.GetComponentInChildren<TMP_Text>();
                TMP_Text textComponent2 = square2.GetComponentInChildren<TMP_Text>();

                // If both TMP_Text components are found, copy the properties
                if (textComponent != null && textComponent2 != null)
                {
                    textComponent.text = textComponent2.text;           // Copy text
                    textComponent.fontSize = textComponent2.fontSize;   // Copy font size
                    textComponent.color = textComponent2.color;         // Copy color
                }
            }
        }
    }




    private bool IsValidPlacement2(int row, int col)
    {
        SudokuGrid grid = FindObjectOfType<SudokuGrid>();
        int index = row * 9 + col;
        var square = grid.grid_squares_[index];
        var gridSquare = square.GetComponent<GridSquare>();
        char num = currentGridInt[row, col];
        if (gridSquare.number_text.GetComponent<TextMeshProUGUI>().fontSize == 35)
            return true;
        for (int i = 0; i < 9; i++)
        {
            if (i != col && currentGridInt[row, i] == num)
                return false;
            if (i != row && currentGridInt[i, col] == num)
                return false;
        }

        int subgridStartRow = 3 * (row / 3);
        int subgridStartCol = 3 * (col / 3);
        for (int i = subgridStartRow; i < subgridStartRow + 3; i++)
        {
            for (int j = subgridStartCol; j < subgridStartCol + 3; j++)
            {
                if ((i != row || j != col) && currentGridInt[i, j] == num)
                    return false;
            }
        }

        return true;
    }

    private void ChangeColor()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                char currentDigit = currentGridInt[i, j];
                if (IsValidPlacement2(i, j))
                {
                    SudokuGrid grid = FindObjectOfType<SudokuGrid>();
                    GameObject square = grid.grid_squares_[i * 9 + j];
                    TextMeshProUGUI[] textComponents =
                        square.GetComponentsInChildren<TextMeshProUGUI>();

                    // Find the specific TextMeshPro component with the GameObject name "killerSum"
                    TextMeshProUGUI textMeshPro = textComponents.FirstOrDefault(
                        tmp => tmp.gameObject.name != "killerSum"
                    );
                    textMeshPro.color = Color.black;
                }
                else
                {
                    SudokuGrid grid = FindObjectOfType<SudokuGrid>();
                    GameObject square = grid.grid_squares_[i * 9 + j];
                    TextMeshProUGUI[] textComponents =
                        square.GetComponentsInChildren<TextMeshProUGUI>();

                    // Find the specific TextMeshPro component with the GameObject name "killerSum"
                    TextMeshProUGUI textMeshPro = textComponents.FirstOrDefault(
                        tmp => tmp.gameObject.name != "killerSum"
                    );
                    Color redHexColor = new Color32(180, 44, 15, 255);
                    textMeshPro.color = redHexColor;
                }
            }
        }
    }

    private void ChangeKillerColor()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                SudokuGrid grid = FindObjectOfType<SudokuGrid>();
                var killerSquare = grid.grid_squares_[i * 9 + j].GetComponent<GridSquare>();
                TextMeshProUGUI[] textComponents =
                    killerSquare.GetComponentsInChildren<TextMeshProUGUI>();

                // Find the specific TextMeshPro component with the GameObject name "killerSum"
                TextMeshProUGUI killerText = textComponents.FirstOrDefault(
                    tmp => tmp.gameObject.name == "killerSum"
                );

                killerText.color = Color.black;
            }
        }
    }

    private void ArrayCopy()
    {
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                temp[row, col] = grid[row, col];
            }
        }
    }
    public void Reverse()
    {
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                grid[row, col] = temp[row, col];
            }
        }
    }

    private char[,] GetCurrentGridState()
    {
        SudokuGrid grid = FindObjectOfType<SudokuGrid>();
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                currentGrid[row, col] = grid.grid_squares_[row * 9 + col]
                    .GetComponent<GridSquare>()
                    .GetNumber();
                switch (currentGrid[row, col])
                {
                    default:
                        currentGridInt[row, col] = '0';
                        break;
                    case "1":
                        currentGridInt[row, col] = '1';
                        break;
                    case "2":
                        currentGridInt[row, col] = '2';
                        break;
                    case "3":
                        currentGridInt[row, col] = '3';
                        break;
                    case "4":
                        currentGridInt[row, col] = '4';
                        break;
                    case "5":
                        currentGridInt[row, col] = '5';
                        break;
                    case "6":
                        currentGridInt[row, col] = '6';
                        break;
                    case "7":
                        currentGridInt[row, col] = '7';
                        break;
                    case "8":
                        currentGridInt[row, col] = '8';
                        break;
                    case "9":
                        currentGridInt[row, col] = '9';
                        break;
                }
            }
        }

        return currentGridInt;
    }

    private int[,] ConvertTables()
    {
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                switch (currentGridInt[row, col])
                {
                    default:
                        grid[row, col] = 0;
                        break;
                    case '1':
                        grid[row, col] = 1;
                        break;
                    case '2':
                        grid[row, col] = 2;
                        break;
                    case '3':
                        grid[row, col] = 3;
                        break;
                    case '4':
                        grid[row, col] = 4;
                        break;
                    case '5':
                        grid[row, col] = 5;
                        break;
                    case '6':
                        grid[row, col] = 6;
                        break;
                    case '7':
                        grid[row, col] = 7;
                        break;
                    case '8':
                        grid[row, col] = 8;
                        break;
                    case '9':
                        grid[row, col] = 9;
                        break;
                }
            }
        }

        return grid;
    }

    private static bool Solve(char[,] board)
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (board[i, j] == '0')
                {
                    for (char c = '1'; c <= '9'; c++)
                    {
                        if (IsValid(board, i, j, c))
                        {
                            board[i, j] = c;

                            if (Solve(board))
                                return true;
                            else
                                board[i, j] = '0';
                        }
                    }
                    return false;
                }
            }
        }
        return true;
    }
    private static bool IsValid(char[,] board, int row, int col, char c)
    {
        for (int i = 0; i < 9; i++)
        {
            if (board[i, col] != '0' && board[i, col] == c)
                return false;

            if (board[row, i] != '0' && board[row, i] == c)
                return false;

            if (
                board[3 * (row / 3) + i / 3, 3 * (col / 3) + i % 3] != '0'
                && board[3 * (row / 3) + i / 3, 3 * (col / 3) + i % 3] == c
            )
                return false;
        }
        return true;
    }

   
   
}
