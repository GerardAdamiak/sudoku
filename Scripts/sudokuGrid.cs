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


public class sudokuGrid : MonoBehaviour
{
    //dodac:
    //-ilosc rozwiazanych dla kazdego trybu (?)
    //najlepszy wynik z kazdej trudnosci w menu
    //-warianty trzeba bedzie zaczac, mozliwe ze zaczne od killera, ale bedzie duzo z tym roboty XD
    //-wiadomo jakies ogolne poprawki zeby wszystko smigalo, przede wszystkim to ze animacja sie laduje dlugo przy pierwszym wlaczeniu
    //-a no i ogarniecie zeby na pewno dzialalo na wszystkich urzadzeniach, bo igor mial jakies problemy :cc


    private int[,] grid = new int[9, 9];
    private string[,] currentGrid = new string[9, 9];
    public char[,] currentGridInt = new char[9, 9];
    private char[,] temp = new char[9, 9];
    public int columns = 9;
    public int rows = 9;
    public float every_square_offset = 0.0f;
    public GameObject grid_square;
    public Vector2 start_position = new Vector2(0.0f, 0.0f);
    public float square_scale = 1.0f;
    public int squaresToDelete;
    private GridSquare selectedSquare;
    private static List<GridSquare> selectedCells = new List<GridSquare>();
    private List<GameObject> grid_squares_ = new List<GameObject>();
    private int counter = 0;
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
    public HashSet<int> lockedDigits = new HashSet<int>();
    public bool isFinished;
    public Stack<(int row, int column, int previousNumber, bool ifNote)> moveStack = new Stack<(int, int, int, bool)>();
    public GameObject leaderboardText;
    private string whichSet;

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
        resolveLog();
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
            } while (ifOk == false);
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


        isFinished = true;




    }

    void Update()
    {
        counter++;
        GetCurrentGridState();



        ChangeColor(currentGridInt);

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
                    GridSquare gridSquare = hit.collider.GetComponent<GridSquare>();
                    if (gridSquare != null)
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
        foreach (var square in selectedCells)
        {
            square.SetNumber(number);
        }
    }

    public void UpdateSelectedCellNote(int number)
    {
        foreach (var square in selectedCells)
        {
            square.SetNumberNote(number);
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

    public void resolveLog()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                currentGridInt[i, j] = sudokuLog[i * 9 + j];
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
                grid_squares_[grid_squares_.Count - 1].transform.SetParent(this.transform);
                grid_squares_[grid_squares_.Count - 1].transform.localScale = new Vector3(
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
        Vector2 offset = new Vector2();
        offset.x = square_rect.rect.width * square_rect.transform.localScale.x + every_square_offset;
        offset.y = square_rect.rect.height * square_rect.transform.localScale.y + every_square_offset;

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

        System.Random rnd = new System.Random();
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
            GameObject panel = new GameObject("DimPanel");
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

    private void SetGridNumbers2()
    {
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                grid_squares_[(row * 9) + col].GetComponent<GridSquare>().SetNumber(grid[row, col]);


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
            HashSet<int> generatedNumbers = new HashSet<int>();

            for (int b = 0; b < squaresPerSubgrid; b++)
            {
                do
                {
                    Random rnd = new Random();
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

    private void ChangeColor(char[,] grid)
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

    public static void solveSudoku(char[,] board)
    {
        if (board == null || board.Length == 0)
            return;
        solve(board);
    }
    private static bool solve(char[,] board)
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (board[i, j] == '0')
                {
                    for (char c = '1'; c <= '9'; c++)
                    {
                        if (isValid(board, i, j, c))
                        {
                            board[i, j] = c;

                            if (solve(board))
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
    private static bool isValid(char[,] board, int row, int col, char c)
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
            if (isValid(currentGridInt, row, col, k))
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


    private void RestoreDigits()
    {
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                int index = row * 9 + col;
                var square = grid_squares_[index];
                var gridSquare = square.GetComponent<GridSquare>();
                gridSquare.SetNumber(grid[row, col]);


            }
        }
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

        // Logging the stack contents to the conso
        UnityEngine.Debug.Log(stackContents);
        UnityEngine.Debug.Log(counter);
    }

}