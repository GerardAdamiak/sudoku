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


public class SudokuGrid : MonoBehaviour
{
    //dodac:
    //-warianty trzeba bedzie zaczac, mozliwe ze zaczne od killera, ale bedzie duzo z tym roboty XD
    //-naprawa tego bledu, ze pierwsze klikniecie po rozpoczeciu nowej gry nie dziala i z wielkoscia notatek przy cofaniu


    private int[,] grid = new int[9, 9];
    private string[,] currentGrid = new string[9, 9];
    public char[,] currentGridInt = new char[9, 9];
    private char[,] temp = new char[9, 9];
    public int columns = 9;
    public int rows = 9;
    public float every_square_offset = 0.0f;
    public GameObject grid_square;
    public Vector2 start_position = new(0.0f, 0.0f);
    public float square_scale = 1.0f;
    public int squaresToDelete;
    public List<GridSquare> selectedCells = new();
    private List<GameObject> grid_squares_ = new();
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
    void Start()
    {
        customNumber = PlayerPrefs.GetInt("number");
        whichSet = PlayerPrefs.GetString("whichSet");
        isFinished = false;
        switch (customNumber)
        {
            case 1:
                sudokuLog = PlayerPrefs.GetString("Sudoku1");

                break;
            case 2:
                sudokuLog = PlayerPrefs.GetString("Sudoku2");
                break;
            case 3:
                sudokuLog = PlayerPrefs.GetString("Sudoku3");
                break;
            case 4:
                sudokuLog = PlayerPrefs.GetString("Sudoku4");
                break;
            case 5:
                sudokuLog = PlayerPrefs.GetString("Sudoku5");
                break;
        }
        ResolveLog();
        currentSceneName = SceneManager.GetActiveScene().name;
        if (grid_square.GetComponent<GridSquare>() == null)
            UnityEngine.Debug.LogError("grid_square object needs to have GridSquare script attached");
        CreateGrid();
        if (currentSceneName != "set" && currentSceneName != "Custom")
        {
            do
            {
                GenerateSudoku();
                SetGridNumbers();
                DeleteSquaresFromEachSubgrid(squaresToDelete);
                if (currentSceneName == "whispers" || currentSceneName == "kropki" || currentSceneName == "renban" || currentSceneName == "killer") ifOk = true;
            } while (ifOk == false);
            if (currentSceneName == "whispers" || currentSceneName =="kropki" || currentSceneName == "renban" || currentSceneName == "killer") GetCurrentGridState();
            UnclickableDigits();
        }

        if (currentSceneName == "Custom")
        {
            ConvertTables();
            // PrintGrid2(grid);
            SetGridNumbers();
            // UnclickableDigits();
        }

        GetCurrentGridState();
        //ConvertTables();
        if (currentSceneName == "whispers")
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {

                    if (i != 8)
                    {
                        if (grid[i, j] != 0 && grid[i + 1, j] != 0)
                        {
                            if (((grid[i, j] - grid[i + 1, j]) >= 5))
                            {
                                var square1 = grid_squares_[(i * 9) + j].GetComponent<GridSquare>();
                                var square2 = grid_squares_[(i * 9) + j + 9].GetComponent<GridSquare>();

                                direction = "down";

                                DrawLineBetweenSquares(square1, square2);
                            }

                        }
                    }
                    if (i != 0)
                    {
                        if (grid[i, j] != 0 && grid[i - 1, j] != 0)
                        {
                            if (((grid[i, j] - grid[i - 1, j]) >= 5))
                            {
                                var square1 = grid_squares_[(i * 9) + j].GetComponent<GridSquare>();
                                var square2 = grid_squares_[(i * 9) + j - 9].GetComponent<GridSquare>();

                                direction = "up";

                                DrawLineBetweenSquares(square1, square2);
                            }
                        }
                    }
                    if (j != 8)
                    {
                        if (grid[i, j] != 0 && grid[i, j + 1] != 0)
                        {
                            if (((grid[i, j] - grid[i, j + 1]) >= 5))
                            {
                                var square1 = grid_squares_[(i * 9) + j].GetComponent<GridSquare>();
                                var square2 = grid_squares_[(i * 9) + j + 1].GetComponent<GridSquare>();

                                direction = "left";

                                DrawLineBetweenSquares(square1, square2);
                            }
                        }
                    }
                    if (j != 0)
                    {
                        if (grid[i, j] != 0 && grid[i, j - 1] != 0)
                        {
                            if (((grid[i, j] - grid[i, j - 1]) >= 5))
                            {
                                var square1 = grid_squares_[(i * 9) + j].GetComponent<GridSquare>();
                                var square2 = grid_squares_[(i * 9) + j - 1].GetComponent<GridSquare>();

                                direction = "right";
                               

                                DrawLineBetweenSquares(square1, square2);
                            }
                        }
                    }


                }
            }
        }
        else if (currentSceneName == "renban")
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {

                    if (i != 8)
                    {
                        if (grid[i, j] != 0 && grid[i + 1, j] != 0)
                        {
                            if (((grid[i, j] - grid[i + 1, j]) >= 5))
                            {
                                var square1 = grid_squares_[(i * 9) + j].GetComponent<GridSquare>();
                                var square2 = grid_squares_[(i * 9) + j + 9].GetComponent<GridSquare>();

                                direction = "down";

                                DrawLineBetweenSquares(square1, square2);
                            }

                        }
                    }
                    if (i != 0)
                    {
                        if (grid[i, j] != 0 && grid[i - 1, j] != 0)
                        {
                            if (((grid[i, j] - grid[i - 1, j]) >= 5))
                            {
                                var square1 = grid_squares_[(i * 9) + j].GetComponent<GridSquare>();
                                var square2 = grid_squares_[(i * 9) + j - 9].GetComponent<GridSquare>();

                                direction = "up";

                                DrawLineBetweenSquares(square1, square2);
                            }
                        }
                    }
                    if (j != 8)
                    {
                        if (grid[i, j] != 0 && grid[i, j + 1] != 0)
                        {
                            if (((grid[i, j] - grid[i, j + 1]) >= 5))
                            {
                                var square1 = grid_squares_[(i * 9) + j].GetComponent<GridSquare>();
                                var square2 = grid_squares_[(i * 9) + j + 1].GetComponent<GridSquare>();

                                direction = "left";

                                DrawLineBetweenSquares(square1, square2);
                            }
                        }
                    }
                    if (j != 0)
                    {
                        if (grid[i, j] != 0 && grid[i, j - 1] != 0)
                        {
                            if (((grid[i, j] - grid[i, j - 1]) >= 5))
                            {
                                var square1 = grid_squares_[(i * 9) + j].GetComponent<GridSquare>();
                                var square2 = grid_squares_[(i * 9) + j - 1].GetComponent<GridSquare>();

                                direction = "right";


                                DrawLineBetweenSquares(square1, square2);
                            }
                        }
                    }


                }
            }
        }
        else if (currentSceneName == "kropki")
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {

                    if (i != 8)
                    {
                        if (grid[i, j] != 0 && grid[i + 1, j] != 0)
                        {
                            if (((grid[i, j] - grid[i + 1, j]) == grid[i + 1, j]))
                            {
                                var square1 = grid_squares_[(i * 9) + j].GetComponent<GridSquare>();
                                var square2 = grid_squares_[(i * 9) + j + 9].GetComponent<GridSquare>();

                                direction = "down";
                                ifDot = false;
                                DrawBlackDotBetweenSquares(square1, square2);
                            }
                            else if (((grid[i, j] - grid[i + 1, j]) == 1))
                            {
                                var square1 = grid_squares_[(i * 9) + j].GetComponent<GridSquare>();
                                var square2 = grid_squares_[(i * 9) + j + 9].GetComponent<GridSquare>();

                                direction = "down";
                                ifDot = true;
                                DrawBlackDotBetweenSquares(square1, square2);
                            }

                        }
                    }
                    if (i != 0)
                    {
                        if (grid[i, j] != 0 && grid[i - 1, j] != 0)
                        {
                            if (((grid[i, j] - grid[i - 1, j]) == grid[i - 1, j]))
                            {
                                var square1 = grid_squares_[(i * 9) + j].GetComponent<GridSquare>();
                                var square2 = grid_squares_[(i * 9) + j - 9].GetComponent<GridSquare>();
                                ifDot = false;
                                direction = "up";

                                DrawBlackDotBetweenSquares(square1, square2);
                            }
                            else if (((grid[i, j] - grid[i - 1, j]) == 1))
                            {
                                var square1 = grid_squares_[(i * 9) + j].GetComponent<GridSquare>();
                                var square2 = grid_squares_[(i * 9) + j - 9].GetComponent<GridSquare>();
                                ifDot = true;
                                direction = "up";

                                DrawBlackDotBetweenSquares(square1, square2);
                            }
                        }
                    }
                    if (j != 8)
                    {
                        if (grid[i, j] != 0 && grid[i, j + 1] != 0)
                        {
                            if (((grid[i, j] - grid[i, j + 1]) == grid[i, j + 1]))
                            {
                                var square1 = grid_squares_[(i * 9) + j].GetComponent<GridSquare>();
                                var square2 = grid_squares_[(i * 9) + j + 1].GetComponent<GridSquare>();
                                ifDot = false;
                                direction = "left";

                                DrawBlackDotBetweenSquares(square1, square2);
                            }
                            else if (((grid[i, j] - grid[i, j + 1]) == 1))
                            {
                                var square1 = grid_squares_[(i * 9) + j].GetComponent<GridSquare>();
                                var square2 = grid_squares_[(i * 9) + j + 1].GetComponent<GridSquare>();
                                ifDot = true;
                                direction = "left";

                                DrawBlackDotBetweenSquares(square1, square2);
                            }
                        }
                    }
                    if (j != 0)
                    {
                        if (grid[i, j] != 0 && grid[i, j - 1] != 0)
                        {
                            if (((grid[i, j] - grid[i, j - 1]) == grid[i, j - 1]))
                            {
                                var square1 = grid_squares_[(i * 9) + j].GetComponent<GridSquare>();
                                var square2 = grid_squares_[(i * 9) + j - 1].GetComponent<GridSquare>();
                                ifDot = false;
                                direction = "right";


                                DrawBlackDotBetweenSquares(square1, square2);
                            }
                            else if (((grid[i, j] - grid[i, j - 1]) == 1))
                            {
                                var square1 = grid_squares_[(i * 9) + j].GetComponent<GridSquare>();
                                var square2 = grid_squares_[(i * 9) + j - 1].GetComponent<GridSquare>();
                                ifDot = true;
                                direction = "right";


                                DrawBlackDotBetweenSquares(square1, square2);
                            }
                        }
                    }


                }
            }
        }
        else if (currentSceneName == "killer")
        {
            // Number of killer cages to generate
            int numberOfCages = 5;
            int cageSum = 0;
            // Random object to generate numbers
            System.Random rand = new System.Random();

            // Define a 9x9 grid as a 1D array (tracking visited cells for all cages)
            bool[] visited = new bool[81];

            // Possible directions: [left, right, up, down] in 1D grid terms
            int[] directions = { -1, 1, -9, 9 };  // left (-1), right (+1), up (-9), down (+9)

            // Function to check if a move is valid (inside grid boundaries)
            bool IsValidMove(int index, int direction)
            {
                // Ensure index stays in the grid
                if (index + direction < 0 || index + direction >= 81)
                    return false;

                // Ensure left-right wrapping isn't violated
                if (direction == -1 && index % 9 == 0) // Going left from the leftmost column
                    return false;
                if (direction == 1 && (index + 1) % 9 == 0) // Going right from the rightmost column
                    return false;

                return true;
            }

            // Generate multiple killer cages
            for (int cageCount = 0; cageCount < numberOfCages; cageCount++)
            {
                // Retry generating a cage if there is an overlap
                bool cageGenerated = false;
                while (!cageGenerated)
                {
                    // Generate random root cell index (0 to 80 for 9x9 grid)
                    int rootCell = rand.Next(0, 81);

                    // If the root cell is already visited, retry
                    if (visited[rootCell])
                        continue;

                    // Generate random size for killer cage (2 to 7 cells)
                    int cageSize = rand.Next(2, 8);

                    // Start building the cage from the root cell
                    List<int> cageCells = new List<int> { rootCell };
                    visited[rootCell] = true;

                    // Grow the cage starting from the rootCell
                    while (cageCells.Count < cageSize)
                    {
                        // Get a random direction and the last cell in the cage
                        int currentCell = cageCells[rand.Next(cageCells.Count)];
                        cageSum = cageSum + grid[currentCell / 9,currentCell % 9];
                        var square1 = grid_squares_[currentCell].GetComponent<GridSquare>();
                        square1.SelectCage();
                        int direction = directions[rand.Next(4)];

                        // Check if moving in this direction is valid
                        if (IsValidMove(currentCell, direction))
                        {
                            newCell = currentCell + direction;

                            // If the new cell is unvisited, add it to the cage
                            if (!visited[newCell])
                            {
                                cageCells.Add(newCell);
                                
                                visited[newCell] = true;
                            }
                        }
                        if(cageCells.Count == cageSize)
                        {
                            var square2 = grid_squares_[newCell].GetComponent<GridSquare>();
                            square2.SelectCage();
                            cageSum = cageSum + grid[newCell / 9, newCell % 9];
                        }
                    }

                    // Cage successfully generated, print the cells
                    UnityEngine.Debug.Log("Cage " + (cageCount + 1) + ": " + string.Join(", ", cageCells));
                    UnityEngine.Debug.Log(cageSum);
                    cageSum = 0;
                    cageGenerated = true;
                }
            }
        }


        isFinished = true;




    }

    void Update()
    {
        
        GetCurrentGridState();

        FixZPosition();

        ChangeColor();

        //PrintGrid2(currentGridInt);
        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                RaycastHit2D hit = Physics2D.Raycast(
                    Camera.main.ScreenToWorldPoint(Input.mousePosition),
                    Vector2.zero
                );
                if (hit.collider != null && hit.collider.CompareTag("GridSquare"))
                {
                    if (hit.collider.TryGetComponent<GridSquare>(out var gridSquare))
                    {
                        SelectGridSquare(gridSquare);
                    }
                }
            }
        }
        if (currentSceneName != "Custom" || whichSet != "set") EndCheck();
    }

    public void UpdateSelectedCell(int number)
    {
        keyboard = FindObjectOfType<DigitKeyboard>();
        x = 0;
        if (keyboard.selectedCount > 1 && number == 0)
        {
            foreach (var square in selectedCells)
            {
                if (x == 0) square.SetNumber(number);
                x++;
            }

        }
        else
        {
            foreach (var square in selectedCells)
            {
                square.SetNumber(number);
            }
        }

    }

    private void DrawLineBetweenSquares(GridSquare square1, GridSquare square2)
    {
        // Instantiate a new line
        GameObject lineObject = Instantiate(linePrefab);
        LineRenderer lineRenderer = lineObject.GetComponent<LineRenderer>();

        // Define the pixel offset (in world units)
        

        // Get the start and end positions
        Vector3 startPosition = square1.transform.position;
        Vector3 endPosition = square2.transform.position;

        float offset = 0.02f;

        startPosition.x -= offset;
        endPosition.x -= offset;
        if (direction == "up")
        {
            startPosition.y -= 0.1f;
            endPosition.y += 0.1f;
        }
        else if (direction == "down")
        {
            startPosition.y += 0.1f;
            endPosition.y -= 0.1f;
        }
        else if (direction == "left")
        {
            startPosition.x -= 0.1f;
            endPosition.x += 0.1f;
        }
        else if (direction == "right")
        {
            startPosition.x += 0.1f;
            endPosition.x -= 0.1f;
        }
        // Move both ends of the line to the left (along the x-axis)


        // Set the start and end positions of the line
        lineRenderer.SetPosition(0, startPosition);
        lineRenderer.SetPosition(1, endPosition);
    }

    private void DrawBlackDotBetweenSquares(GridSquare square1, GridSquare square2)
    {
        // Instantiate a new line
        GameObject lineObject = Instantiate(linePrefab);
        GameObject dotObject = Instantiate(dotPrefab);
        LineRenderer lineRendererDot = dotObject.GetComponent<LineRenderer>();
        LineRenderer lineRenderer = lineObject.GetComponent<LineRenderer>();

        // Define the pixel offset (in world units)


        // Get the start and end positions
        Vector3 startPosition = square1.transform.position;
        Vector3 endPosition = square2.transform.position;

        float offset = 0.03f;

        startPosition.x -= offset;
        endPosition.x -= offset;
        if (direction == "up")
        {
            startPosition.y += 0.2f;
            endPosition.y -= 0.12f;
        }
        else if (direction == "down")
        {
            startPosition.y -= 0.12f;
            endPosition.y += 0.2f;
        }
        else if (direction == "left")
        {
            startPosition.x += 0.16f;
            endPosition.x -= 0.16f;
        }
        else if (direction == "right")
        {
            startPosition.x -= 0.16f;
            endPosition.x += 0.16f;
        }
        // Move both ends of the line to the left (along the x-axis)


        // Set the start and end positions of the line
        Random random = new Random();
        double randomNumber = random.NextDouble();
        if (randomNumber > 0.3)
        {
            if (ifDot == false)
            {
                lineRenderer.SetPosition(0, startPosition);
                lineRenderer.SetPosition(1, endPosition);
            }
            else
            {
                lineRendererDot.SetPosition(0, startPosition);
                lineRendererDot.SetPosition(1, endPosition);
            }
        }
    }


    public void UpdateSelectedCellNote(int number)
    {
        keyboard = FindObjectOfType<DigitKeyboard>();
        x = 0;
        if (keyboard.selectedCount > 1 && number == 0)
        {
            foreach (var square in selectedCells)
            {
                if(x==0)square.SetNumberNote(number);
                x++;
            }

        }
        else
        {
            foreach (var square in selectedCells)
            {
                square.SetNumberNote(number);
            }
        }
        
    }

    public void DeselectAllGridSquares()
    {
        foreach (var square in selectedCells)
        {
            square.Deselect();
        }
        selectedCells.Clear();
    }

    public List<GridSquare> GetSelectedSquares()
    {
        return selectedCells;
    }

    public void ResolveLog()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                currentGridInt[i, j] = sudokuLog[i * 9 + j];
            }
        }
    }


    public void FixZPosition()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                Vector3 newPosition = grid_squares_[(i * 9) + j].transform.position;
                newPosition.z = 1f; // Set the z position to 1
                grid_squares_[(i * 9) + j].transform.position = newPosition;
            }
        }
    }


    

    public void SelectGridSquare(GridSquare gridSquare)
    {
        if (!selectedCells.Contains(gridSquare))
        {
            selectedCells.Add(gridSquare);
            gridSquare.Select();
        }
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
                    square_scale
                );
                
            }
        }
    }

    private void SetSquaresPosition()
    {
        var square_rect = grid_squares_[0].GetComponent<RectTransform>();
        Vector2 offset = new()
        {
            x = square_rect.rect.width * square_rect.transform.localScale.x + every_square_offset,
            y = square_rect.rect.height * square_rect.transform.localScale.y + every_square_offset
        };

        int column_number = 0;
        int row_number = 0;
        int column_offset = 20;
        int row_offset = 20;
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
            square.GetComponent<RectTransform>().anchoredPosition = new Vector3(
                start_position.x + pos_x_offset,
                start_position.y - pos_y_offset
            );
            column_number++;
            if (column_number % 3 == 0)
                counterX++;
        }
    }

    private void GenerateSudoku()
    {
        ClearGrid();

        if (GenerateRandomSudoku(0, 0))
        {
            UnityEngine.Debug.Log("Sudoku generated successfully!");
        }
        else
        {
            UnityEngine.Debug.LogError("Failed to generate Sudoku.");
        }
    }

    private bool GenerateRandomSudoku(int row, int col)
    {
        if (row == 9)
            return true;

        if (col == 9)
            return GenerateRandomSudoku(row + 1, 0);

        int[] numbers = GenerateRandomOrder();

        foreach (int num in numbers)
        {
            if (IsValidPlacement(row, col, num))
            {
                grid[row, col] = num;

                if (GenerateRandomSudoku(row, col + 1))
                    return true;

                grid[row, col] = 0;
            }
        }

        return false;
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



    private void EndCheck()
    {

        endChecker = true;
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (currentGridInt[i, j] == '0')
                    endChecker = false;
                GameObject square = grid_squares_[i * 9 + j];
                TextMeshProUGUI textMeshPro = square.GetComponentInChildren<TextMeshProUGUI>();

                if (textMeshPro.color == redHexColor)
                    endChecker = false;
            }
        }

        if (endChecker)
        {
            if (done == false)
            {
                int countEasy = PlayerPrefs.GetInt("medalEasy");
                int countMed = PlayerPrefs.GetInt("medalMed");
                int countHard = PlayerPrefs.GetInt("medalHard");
                if (currentSceneName == "easy")
                {
                    countEasy++;
                    PlayerPrefs.SetInt("medalEasy", countEasy);
                }
                else if (currentSceneName == "medium")
                {
                    countMed++;
                    PlayerPrefs.SetInt("medalMed", countMed);
                }
                else if (currentSceneName == "hard")
                {
                    countHard++;
                    PlayerPrefs.SetInt("medalHard", countHard);
                }
                done = true;
            }
            GameObject panel = new("DimPanel");
            panel.transform.SetParent(canvas.transform);
            RectTransform panelRect = panel.AddComponent<RectTransform>();
            panelRect.anchorMin = Vector2.zero;
            panelRect.anchorMax = Vector2.one;
            panelRect.sizeDelta = Vector2.zero;

            Image image = panel.AddComponent<Image>();
            image.color = new Color(0, 0, 0, 0.1f);

            SpriteRenderer previousSpriteRenderer = previous.GetComponent<SpriteRenderer>();
            SpriteRenderer diffSpriteRenderer = diff.GetComponent<SpriteRenderer>();

            if (previousSpriteRenderer != null)
                previousSpriteRenderer.color = new Color(
                    previousSpriteRenderer.color.r,
                    previousSpriteRenderer.color.g,
                    previousSpriteRenderer.color.b,
                    0f
                );
            if (diffSpriteRenderer != null)
                diffSpriteRenderer.color = new Color(
                    diffSpriteRenderer.color.r,
                    diffSpriteRenderer.color.g,
                    diffSpriteRenderer.color.b,
                    0f
                );

            Leaderboard.ifAdded = false;
            LeaderboardMed.ifAddedMed = false;
            LeaderboardHard.ifAddedHard = false;

            StartCoroutine(LoadMainMenuAfterDelay());
        }
    }

    private IEnumerator LoadMainMenuAfterDelay()
    {
        PlayerPrefs.SetString("PreviousScene", currentSceneName);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("end");
    }


    private void SetGridNumbers()
    {
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                int index = row * 9 + col;
                var square = grid_squares_[index];
                var gridSquare = square.GetComponent<GridSquare>();
                gridSquare.SetNumber(grid[row, col]);
                gridSquare.SetGrid(row, col);

                if (!IsValidPlacement(row, col, currentGridInt[row, col]))
                {
                    Color redHexColor = new Color32(180, 44, 15, 255);
                    gridSquare.ChangeTextColor(redHexColor);
                }
            }
        }
    }

   
    private void DeleteSquaresFromEachSubgrid(int totalSquaresToDelete)
    {
        if (totalSquaresToDelete <= 0)
        {
            UnityEngine.Debug.LogError("Invalid number of squares to delete.");
            return;
        }

        int squaresPerSubgrid = totalSquaresToDelete / 9;

        for (int i = 0; i < 9; i++)
        {
            HashSet<int> generatedNumbers = new();

            for (int b = 0; b < squaresPerSubgrid; b++)
            {
                do
                {
                    Random rnd = new();
                    int c = rnd.Next(0, 3);
                    int d = rnd.Next(0, 3);
                    switch (i)
                    {
                        case 0:
                            e = c * 9 + d;
                            break;
                        case 1:
                            e = c * 9 + d + 3;
                            break;
                        case 2:
                            e = c * 9 + d + 6;
                            break;
                        case 3:
                            e = 27 + c * 9 + d;
                            break;
                        case 4:
                            e = 27 + c * 9 + d + 3;
                            break;
                        case 5:
                            e = 27 + c * 9 + d + 6;
                            break;
                        case 6:
                            e = 54 + c * 9 + d;
                            break;
                        case 7:
                            e = 54 + c * 9 + d + 3;
                            break;
                        case 8:
                            e = 54 + c * 9 + d + 6;
                            break;
                        default:
                            e = -1;
                            break;
                    }
                } while (!generatedNumbers.Add(e));
                grid_squares_[e].GetComponent<GridSquare>().SetNumber(0);
                GetCurrentGridState();
            }
        }
        RunSolver();
        if (g == 1) ifOk = true;

        

    }



    private bool IsValidPlacement2(int row, int col)
    {
        int index = row * 9 + col;
        var square = grid_squares_[index];
        var gridSquare = square.GetComponent<GridSquare>();
        char num = currentGridInt[row, col];
        if (gridSquare.number_text.GetComponent<TextMeshProUGUI>().fontSize == 35) return true;
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
                    GameObject square = grid_squares_[i * 9 + j];
                    TextMeshProUGUI textMeshPro = square.GetComponentInChildren<TextMeshProUGUI>();
                    textMeshPro.color = Color.black;
                }
                else
                {
                    GameObject square = grid_squares_[i * 9 + j];
                    TextMeshProUGUI textMeshPro = square.GetComponentInChildren<TextMeshProUGUI>();
                    Color redHexColor = new Color32(180, 44, 15, 255);
                    textMeshPro.color = redHexColor;
                }
            }
        }
    }

   


    private void ArrayCopy()
    {
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                temp[row, col] = currentGridInt[row, col];
            }
        }
    }
    public void Reverse()
    {
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                currentGridInt[row, col] = temp[row, col];
            }
        }
    }

    private char[,] GetCurrentGridState()
    {
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                currentGrid[row, col] = grid_squares_[row * 9 + col]
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


    private void ClearGrid()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                grid[i, j] = 0;
            }
        }
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

    public void RunSolver()
    {
        set = new HashSet<Grid>();
        Stopwatch stopwatch = Stopwatch.StartNew();
        if (CountSolutions(set, stopwatch) == 0)
        {
            g = 0;
        }
        else
        {
            g = set.Count;
        }
        Console.WriteLine("Number of unique grids: " + set.Count);
    }

    public int CountSolutions(HashSet<Grid> uniqueGrids, Stopwatch stopwatch)
    {
        ArrayCopy();
        return SolveAndCount(uniqueGrids, 0, 0, stopwatch);
    }

    private int SolveAndCount(HashSet<Grid> uniqueGrids, int row, int col, Stopwatch stopwatch)
    {
        // Check for timeout
        if (stopwatch.Elapsed.TotalSeconds > 1 && uniqueGrids.Count == 0)
        {
            return 0;
        }

        // Find the next empty cell
        while (row < 9 && currentGridInt[row, col] != '0')
        {
            col++;
            if (col == 9)
            {
                col = 0;
                row++;
            }
        }

        // If no empty cell is found, we have a complete grid
        if (row == 9)
        {
            var solvedGrid = new Grid(currentGridInt);
            uniqueGrids.Add(solvedGrid);
            return uniqueGrids.Count;
        }

        // Try each number from 1 to 9
        for (char k = '1'; k <= '9'; k++)
        {
            if (IsValid(currentGridInt, row, col, k))
            {
                currentGridInt[row, col] = k;

                // Recursive call
                int result = SolveAndCount(uniqueGrids, row, col, stopwatch);
                if (result > 1) return result; // Early exit if more than one solution is found

                // Backtrack
                currentGridInt[row, col] = '0';
            }
        }

        return uniqueGrids.Count;
    }


   
    private void UnclickableDigits()

    {
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                if (currentGridInt[row, col] != '0')
                {
                    lockedDigits.Add((row * 9) + col);
                }
            }
        }
        

        
    }

    public void PrintStackContents()
    {
        // Creating a string to hold the stack contents
        string stackContents = "Stack Contents:\n";
        int counter = 0;
        // Iterating through the stack without modifying it
        foreach (var move in moveStack)
        {
            counter++;
            stackContents += $"(Row: {move.row}, Column: {move.column}, Previous Number: {move.previousNumber}, If Note: {move.ifNote})\n";
        }

        // Logging the stack contents to the console
        //UnityEngine.Debug.Log(stackContents);
        //UnityEngine.Debug.Log(counter);
    }

}