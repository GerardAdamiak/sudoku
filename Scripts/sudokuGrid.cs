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
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using Debug = UnityEngine.Debug;


public class SudokuGrid : MonoBehaviour
{
    //dodac:
    //-warianty trzeba bedzie dopracowac balans dobry
    // optymalizacja, bo zaczyna troche zacinac
    // jakis problem z lewym gornym kwadracikiem w whispers


    private int[,] grid = new int[9, 9];
    private string[,] currentGrid = new string[9, 9];
    public char[,] currentGridInt = new char[9, 9];
    private int[,] temp = new int[9, 9];
    public int columns = 4;
    public int rows = 4;
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
    public GameObject linePrefabRenban;
    public GameObject linePrefabThermo;
    public GameObject linePrefabDot;
    public GameObject dotPrefab;
    private bool done = false;
    private string directionLine;
    private DigitKeyboard keyboard;
    public bool ifMore;
    private int x;
    private bool ifDot;
    private int newCell;
    private int loopCounter = 0;
    private bool ifSingleCage = false;
    public Image blackSquare;
    private int randomDigit;
    public bool tutorialDone = false;
    private bool ifFirstCage = true;
    public bool ifAddedDot = false;

    [System.Serializable]
    public class GermanConnection
    {
        public int fromIndex;
        public int toIndex;
        public string direction;
    }


    List<GermanConnection> GermanConnections = new List<GermanConnection>();

    [System.Serializable]
    public class DotConnection
    {
        public int fromIndex;
        public int toIndex;
        public string direction;
        public bool isDot; // true for black dot, false for white dot
    }
    List<DotConnection> DotConnections = new List<DotConnection>();

    [System.Serializable]
    public class ThermoConnection
    {
        public int fromIndex;
        public int toIndex;
        public string direction;
        public bool isDot; // for kropki dots
        public bool isThermo; // true if this is a thermo connection
        public bool isThermoStart; // true if this cell has the thermo bulb/circle
    }
    List<ThermoConnection> ThermoConnections = new List<ThermoConnection>();

    [System.Serializable]
    public class KillerConnection
    {
        public int fromIndex;
        public int toIndex;
        public string direction;
        public bool isDot; // for kropki dots
        public bool isThermo; // true if this is a thermo connection
        public bool isThermoStart; // true if this cell has the thermo bulb/circle
        public bool isKiller; // true if this is a killer cage connection
        public int textureID; // for killer cage textures
        public string killerSum; // for killer cage sum text
    }
    List<KillerConnection> KillerConnections = new List<KillerConnection>();

    [System.Serializable]
    public class RenbanConnection
    {
        public int fromIndex;
        public int toIndex;
        public string direction;
        public bool isDot; // for kropki dots
        public bool isThermo; // true if this is a thermo connection
        public bool isThermoStart; // true if this cell has the thermo bulb/circle
        public bool isKiller; // true if this is a killer cage connection
        public int textureID; // for killer cage textures
        public string killerSum; // for killer cage sum text
        public bool isRenban; // true if this is a renban line connection
    }

    List<RenbanConnection> RenbanConnections = new List<RenbanConnection>();
    public void DrawGerman()
    {
        var squareIndices = new int[]
            {
        11, 12, 13, 4, 5, 14, 23, 24, 15, // First group
        72, 73, 64, 55, 56, 57, 79, 80, 0, 9, 18, 19, 10, 1, 2, 11, 31, 30, 29, 28, 27, 68, 69, 27, 36, 45, 54, 63, 72, 15, 16, 17, 26, 35, 34, 43, 42, 69, 78, 79    // Second group
            };

        var directions = new string[]
        {
        "left", "left", "up", "left", "down", "down", "left", "up", // Directions for the first group
        "left", "left", "up", "up", "left", "left", "left", "left"                 // Directions for the second group
        };

        // Get all the squares
        var squares = squareIndices.Select(index => grid_squares_[index].GetComponent<GridSquare>()).ToArray();

        // Draw lines between squares
        for (int i = 0; i < 16; i++)
        {
            if (i != 8 && i != 14)
            {
                directionLine = directions[i];
                DrawLineBetweenSquares(squares[i], squares[i + 1]);
            }
        }
    }
    public void DrawThermo()
    {
        var squareIndices = new int[]
           {
        11, 12, 13, 4, 5, 14, 23, 24, 15, // First group
        72, 73, 64, 55, 56, 57, 79, 80, 0, 9, 18, 19, 10, 1, 2, 11, 31, 30, 29, 28, 27, 68, 69, 27, 36, 45, 54, 63, 72, 15, 16, 17, 26, 35, 34, 43, 42, 69, 78, 79    // Second group
           };

        var directions = new string[]
        {
        "left", "left", "up", "left", "down", "down", "left", "up", // Directions for the first group
        "left", "left", "up", "up", "left", "left", "left", "left"                 // Directions for the second group
        };

        // Get all the squares
        var squares = squareIndices.Select(index => grid_squares_[index].GetComponent<GridSquare>()).ToArray();

        for (int i = 17; i < 31; i++)
        {
            if (i == 24 || i == 29) continue;
            int rowDiff = (squareIndices[i] / 9) - (squareIndices[i + 1] / 9);
            int colDiff = (squareIndices[i] % 9) - (squareIndices[i + 1] % 9);
            directionLine = "";

            if (rowDiff == -1 && colDiff == 0) directionLine = "down";

            else if (rowDiff == 0 && colDiff == 1) directionLine = "right";

            else if (rowDiff == 1 && colDiff == 0) directionLine = "up";

            else if (rowDiff == 0 && colDiff == -1) directionLine = "left";

            DrawThermoLineBetweenSquares(squares[i], squares[i + 1]);
        }

        var firstSquare = grid_squares_[0].GetComponent<GridSquare>();
        var firstSquare2 = grid_squares_[31].GetComponent<GridSquare>();
        var firstSquare3 = grid_squares_[68].GetComponent<GridSquare>();

        Image spriteRenderer = firstSquare.GetComponentInChildren<Image>();
        Image spriteRenderer2 = firstSquare2.GetComponentInChildren<Image>();
        Image spriteRenderer3 = firstSquare3.GetComponentInChildren<Image>();


        // Deactivate the SpriteRenderer
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
            spriteRenderer2.enabled = true;
            spriteRenderer3.enabled = true;
        }
    }

    public void DrawRenban()
    {
        var squareIndices = new int[]
           {
        11, 12, 13, 4, 5, 14, 23, 24, 15, // First group
        72, 73, 64, 55, 56, 57, 79, 80, 0, 9, 18, 19, 10, 1, 2, 11, 31, 30, 29, 28, 27, 68, 69, 27, 36, 45, 54, 63, 72, 15, 16, 17, 26, 35, 34, 43, 42, 69, 78, 79    // Second group
           };

        var directions = new string[]
        {
        "left", "left", "up", "left", "down", "down", "left", "up", // Directions for the first group
        "left", "left", "up", "up", "left", "left", "left", "left"                 // Directions for the second group
        };

        // Get all the squares
        var squares = squareIndices.Select(index => grid_squares_[index].GetComponent<GridSquare>()).ToArray();

        for (int i = 32; i < 48; i++)
        {
            if (i == 37 || i == 45) continue;
            int rowDiff = (squareIndices[i] / 9) - (squareIndices[i + 1] / 9);
            int colDiff = (squareIndices[i] % 9) - (squareIndices[i + 1] % 9);
            directionLine = "";

            if (rowDiff == -1 && colDiff == 0) directionLine = "down";

            else if (rowDiff == 0 && colDiff == 1) directionLine = "right";

            else if (rowDiff == 1 && colDiff == 0) directionLine = "up";

            else if (rowDiff == 0 && colDiff == -1) directionLine = "left";

            else if (rowDiff == 1 && colDiff == 1) directionLine = "right-up";

            else if (rowDiff == -1 && colDiff == 1) directionLine = "right-down";

            else if (rowDiff == 1 && colDiff == -1) directionLine = "left-up";

            else if (rowDiff == -1 && colDiff == -1) directionLine = "left-down";

            DrawRenbanLineBetweenSquares(squares[i], squares[i + 1]);
        }

    }

    public void DrawKillers()
    {
        var square = grid_squares_[31].GetComponent<GridSquare>();
        square.SetTexture(12);
        var textComponents = square.GetComponentsInChildren<TextMeshProUGUI>();

        // Find the specific TextMeshPro component with the GameObject name "killerSum"
        TextMeshProUGUI killerText = textComponents.FirstOrDefault(
            tmp => tmp.gameObject.name == "killerSum"
        );

        if (ifFirstCage == true)
        {
            killerText.text = "";
        }
        else killerText.text = "7";
        square = grid_squares_[40].GetComponent<GridSquare>();
        square.SetTexture(1);
        square = grid_squares_[41].GetComponent<GridSquare>();
        square.SetTexture(9);

        square = grid_squares_[67].GetComponent<GridSquare>();
        square.SetTexture(11);

        textComponents = square.GetComponentsInChildren<TextMeshProUGUI>();

        // Find the specific TextMeshPro component with the GameObject name "killerSum"
        killerText = textComponents.FirstOrDefault(
            tmp => tmp.gameObject.name == "killerSum"
        );
        if (ifFirstCage == true)
        {
            killerText.text = "";
        }
        else killerText.text = "14";

        square = grid_squares_[68].GetComponent<GridSquare>();
        square.SetTexture(9);

        square = grid_squares_[20].GetComponent<GridSquare>();
        square.SetTexture(15);

        textComponents = square.GetComponentsInChildren<TextMeshProUGUI>();

        // Find the specific TextMeshPro component with the GameObject name "killerSum"
        killerText = textComponents.FirstOrDefault(
            tmp => tmp.gameObject.name == "killerSum"
        );
        if (ifFirstCage == true)
        {
            killerText.text = "";
        }
        else killerText.text = "8";

        ifFirstCage = false;
    }

    public void DrawKropki()
    {
        var square3 = grid_squares_[33].GetComponent<GridSquare>();
        var square4 = grid_squares_[34].GetComponent<GridSquare>();
        ifDot = true;
        directionLine = "left";

        DrawBlackDotBetweenSquares(square3, square4);

        var square1 = grid_squares_[45].GetComponent<GridSquare>();
        var square2 = grid_squares_[46].GetComponent<GridSquare>();
        ifDot = false;
        directionLine = "left";

        DrawBlackDotBetweenSquares(square1, square2);
    }

    void Start()
    {

        
        bool ifContinue = TouchToChangeScene.ifContinue;
        UnityEngine.Debug.Log("ifContinue: " + ifContinue);
        customNumber = PlayerPrefs.GetInt("number");
        whichSet = PlayerPrefs.GetString("whichSet");
        PlayerPrefs.SetInt("GameReady", 0);
        PlayerPrefs.Save();

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
        

        currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName == "tutorial") sudokuLog = "007028000050073846300000107084000002700042390602005071871904265020780010406000700";
        ResolveLog();
        if (grid_square.GetComponent<GridSquare>() == null)
            UnityEngine.Debug.LogError(
                "grid_square object needs to have GridSquare script attached"
            );
        CreateGrid();
        if (currentSceneName != "set" && currentSceneName != "Custom" && currentSceneName != "tutorial" && ifContinue == false)
        {



            do
            {
                GenerateSudoku();
                SetGridNumbers();
                DeleteSquaresFromEachSubgrid(squaresToDelete);
                if (
                    currentSceneName == "whispers"
                    || currentSceneName == "kropki"
                    || currentSceneName == "renban"
                    || currentSceneName == "killer"
                    || currentSceneName == "thermo"
                    || currentSceneName == "tutorial"
                    || currentSceneName == "killerEasy"
                    || currentSceneName == "killerMedium"
                    || currentSceneName == "thermoEasy"
                    || currentSceneName == "thermoMedium"
                    || currentSceneName == "whispersEasy"
                    || currentSceneName == "whispersMedium"
                    || currentSceneName == "kropkiEasy"
                    || currentSceneName == "kropkiMedium"
                    || currentSceneName == "renbanEasy"
                    || currentSceneName == "renbanMedium"

                )
                    ifOk = true;

            } while (ifOk == false);
            if (
                currentSceneName == "whispers"
                || currentSceneName == "kropki"
                || currentSceneName == "renban"
                || currentSceneName == "killer"
                || currentSceneName == "thermo"
                || currentSceneName == "tutorial"
                || currentSceneName == "killerEasy"
                || currentSceneName == "killerMedium"
                || currentSceneName == "thermoEasy"
                || currentSceneName == "thermoMedium"
                || currentSceneName == "whispersEasy"
                || currentSceneName == "whispersMedium"
                || currentSceneName == "kropkiEasy"
                || currentSceneName == "kropkiMedium"
                || currentSceneName == "renbanEasy"
                || currentSceneName == "renbanMedium"
            )
                GetCurrentGridState();
            UnclickableDigits();
        }
        if (ifContinue == true)
        {
            SudokuSaveSystem save = FindAnyObjectByType<SudokuSaveSystem>();
            save.LoadGame();
            save.ApplyGridToBoard(grid_squares_);
            
            for(int a = 0; a<9; a++)
            {
                for (int b = 0; b < 9; b++)
                {
                    currentGrid[a, b] = save.currentGrid[a, b];
                }

            }
        }
        if (currentSceneName == "Custom" || currentSceneName == "tutorial" || ifContinue == true)
        {
            if(!ifContinue)ConvertTables();
            if(ifContinue)ConvertTables2();
            // PrintGrid2(grid);

            if (ifContinue != true)
            {
                SetGridNumbers();
                UnclickableDigits();
            }
            if (ifContinue == true)
            {
                Timer timer = FindObjectOfType<Timer>();
                timer.currentTime = PlayerPrefs.GetFloat("SudokuSave_Time");
            }
            // UnclickableDigits();
        }

        GetCurrentGridState();
        //ConvertTables();

        int counter = 0;

        while (counter < 81)
        {

            var firstSquare = grid_squares_[counter].GetComponent<GridSquare>();

            Image spriteRenderer = firstSquare.GetComponentInChildren<Image>();


            spriteRenderer.enabled = false;
            counter++;
        }
        void SaveGermanConnections()
        {
            string connectionData = "";
            foreach (var connection in GermanConnections)
            {
                connectionData += connection.fromIndex + "," + connection.toIndex + "," + connection.direction + ";";
            }
            PlayerPrefs.SetString("GermanConnections", connectionData);
            PlayerPrefs.Save();

            UnityEngine.Debug.Log("=== LINE CONNECTIONS SAVED ===");
            UnityEngine.Debug.Log("Total connections: " + GermanConnections.Count);
            UnityEngine.Debug.Log("Saved data: " + connectionData);

            for (int i = 0; i < GermanConnections.Count; i++)
            {
                var connection = GermanConnections[i];
                UnityEngine.Debug.Log($"Connection {i + 1}: From index {connection.fromIndex} to index {connection.toIndex}, Direction: {connection.direction}");
            }
            UnityEngine.Debug.Log("=== END OF SAVED CONNECTIONS ===");
        }

        void LoadGermanConnections()
        {
            string connectionData = PlayerPrefs.GetString("GermanConnections", "");
            if (connectionData != "")
            {
                GermanConnections.Clear();
                string[] connections = connectionData.Split(';');

                UnityEngine.Debug.Log("=== LINE CONNECTIONS LOADED ===");
                UnityEngine.Debug.Log("Raw data: " + connectionData);
                UnityEngine.Debug.Log("Total connections to load: " + (connections.Length - 1)); // -1 because last element is empty

                foreach (string conn in connections)
                {
                    if (conn != "")
                    {
                        string[] parts = conn.Split(',');
                        if (parts.Length == 3)
                        {
                            GermanConnection connection = new GermanConnection
                            {
                                fromIndex = int.Parse(parts[0]),
                                toIndex = int.Parse(parts[1]),
                                direction = parts[2]
                            };
                            GermanConnections.Add(connection);

                            UnityEngine.Debug.Log($"Loaded connection: From index {connection.fromIndex} to index {connection.toIndex}, Direction: {connection.direction}");

                            // Reconstruct the line
                            var square1 = grid_squares_[connection.fromIndex].GetComponent<GridSquare>();
                            var square2 = grid_squares_[connection.toIndex].GetComponent<GridSquare>();
                            directionLine = connection.direction;
                            DrawLineBetweenSquares(square1, square2);
                        }
                    }
                }
                UnityEngine.Debug.Log("=== END OF LOADED CONNECTIONS ===");
            }
            else
            {
                    UnityEngine.Debug.Log("No saved line connections found in PlayerPrefs");
            }
        }

        void SaveDotConnections()
        {
            string connectionData = "";
            foreach (var connection in DotConnections)
            {
                connectionData += connection.fromIndex + "," + connection.toIndex + "," + connection.direction + "," + connection.isDot + ";";
            }
            PlayerPrefs.SetString("DotConnections", connectionData);
            PlayerPrefs.Save();

            UnityEngine.Debug.Log("=== LINE CONNECTIONS SAVED ===");
            UnityEngine.Debug.Log("Total connections: " + DotConnections.Count);

            for (int i = 0; i < DotConnections.Count; i++)
            {
                var connection = DotConnections[i];
                string dotType = connection.isDot ? "White Dot" : "Black Dot";
                UnityEngine.Debug.Log($"Connection {i + 1}: From index {connection.fromIndex} to index {connection.toIndex}, Direction: {connection.direction}, Type: {dotType}");
            }
            UnityEngine.Debug.Log("=== END OF SAVED CONNECTIONS ===");
        }

        void LoadDotConnections()
        {
            string connectionData = PlayerPrefs.GetString("DotConnections", "");
            if (connectionData != "")
            {
                DotConnections.Clear();
                string[] connections = connectionData.Split(';');

                UnityEngine.Debug.Log("=== LINE CONNECTIONS LOADED ===");
                UnityEngine.Debug.Log("Total connections to load: " + (connections.Length - 1));

                foreach (string conn in connections)
                {
                    if (conn != "")
                    {
                        string[] parts = conn.Split(',');
                        if (parts.Length == 4) // Now we have 4 parts: fromIndex, toIndex, direction, isDot
                        {
                            DotConnection connection = new DotConnection
                            {
                                fromIndex = int.Parse(parts[0]),
                                toIndex = int.Parse(parts[1]),
                                direction = parts[2],
                                isDot = bool.Parse(parts[3])
                            };
                            DotConnections.Add(connection);

                            string dotType = connection.isDot ? "White Dot" : "Black Dot";
                            UnityEngine.Debug.Log($"Loaded connection: From index {connection.fromIndex} to index {connection.toIndex}, Direction: {connection.direction}, Type: {dotType}");

                            // Reconstruct the dot
                            var square1 = grid_squares_[connection.fromIndex].GetComponent<GridSquare>();
                            var square2 = grid_squares_[connection.toIndex].GetComponent<GridSquare>();
                            directionLine = connection.direction;
                            ifDot = connection.isDot;
                            DrawBlackDotBetweenSquares(square1, square2);
                        }
                    }
                }
                UnityEngine.Debug.Log("=== END OF LOADED CONNECTIONS ===");
            }
            else
            {
                UnityEngine.Debug.Log("No saved line connections found in PlayerPrefs");
            }
        }

        void SaveThermoConnections()
        {
            string connectionData = "";
            foreach (var connection in ThermoConnections)
            {
                connectionData += connection.fromIndex + "," + connection.toIndex + "," + connection.direction + "," + connection.isDot + "," + connection.isThermo + "," + connection.isThermoStart + ";";
            }
            PlayerPrefs.SetString("ThermoConnections", connectionData);
            PlayerPrefs.Save();

            Debug.Log("=== Thermo CONNECTIONS SAVED ===");
            Debug.Log("Total connections: " + ThermoConnections.Count);

            for (int i = 0; i < ThermoConnections.Count; i++)
            {
                var connection = ThermoConnections[i];
                string connectionType = "";
                if (connection.isThermo && connection.isThermoStart) connectionType = "Thermo Bulb";
                else if (connection.isThermo) connectionType = "Thermo Thermo";
                else if (connection.isDot) connectionType = "White Dot";
                else connectionType = "Black Dot/Thermo";

                Debug.Log($"Connection {i + 1}: From index {connection.fromIndex} to index {connection.toIndex}, Direction: {connection.direction}, Type: {connectionType}");
            }
            Debug.Log("=== END OF SAVED CONNECTIONS ===");
        }

        void LoadThermoConnections()
        {
            string connectionData = PlayerPrefs.GetString("ThermoConnections", "");
            if (connectionData != "")
            {
                ThermoConnections.Clear();
                string[] connections = connectionData.Split(';');

                Debug.Log("=== Thermo CONNECTIONS LOADED ===");
                Debug.Log("Total connections to load: " + (connections.Length - 1));

                foreach (string conn in connections)
                {
                    if (conn != "")
                    {
                        string[] parts = conn.Split(',');
                        if (parts.Length == 6) // Now we have 6 parts: fromIndex, toIndex, direction, isDot, isThermo, isThermoStart
                        {
                            ThermoConnection connection = new ThermoConnection
                            {
                                fromIndex = int.Parse(parts[1]),
                                toIndex = int.Parse(parts[0]),
                                direction = parts[2],
                                isDot = bool.Parse(parts[3]),
                                isThermo = bool.Parse(parts[4]),
                                isThermoStart = bool.Parse(parts[5])
                            };
                            ThermoConnections.Add(connection);

                            string connectionType = "";
                            if (connection.isThermo && connection.isThermoStart)
                            {
                                connectionType = "Thermo Bulb";
                                // Reconstruct the thermo bulb
                                var firstSquare = grid_squares_[connection.fromIndex].GetComponent<GridSquare>();
                                Image spriteRenderer = firstSquare.GetComponentInChildren<Image>();
                                if (spriteRenderer != null)
                                {
                                    spriteRenderer.enabled = true;
                                }
                            }
                            else if (connection.isThermo)
                            {
                                connectionType = "Thermo Thermo";
                                // Reconstruct thermo Thermo
                                var square1 = grid_squares_[connection.fromIndex].GetComponent<GridSquare>();
                                var square2 = grid_squares_[connection.toIndex].GetComponent<GridSquare>();
                                directionLine = connection.direction;
                                DrawLineBetweenSquares(square1, square2);
                            }
                            else if (connection.isDot)
                            {
                                connectionType = "White Dot";
                                // Reconstruct dot
                                var square1 = grid_squares_[connection.fromIndex].GetComponent<GridSquare>();
                                var square2 = grid_squares_[connection.toIndex].GetComponent<GridSquare>();
                                directionLine = connection.direction;
                                ifDot = connection.isDot;
                                DrawBlackDotBetweenSquares(square1, square2);
                            }
                            else
                            {
                                connectionType = "Line";
                                // Reconstruct line
                                var square1 = grid_squares_[connection.fromIndex].GetComponent<GridSquare>();
                                var square2 = grid_squares_[connection.toIndex].GetComponent<GridSquare>();
                                directionLine = connection.direction;
                                DrawLineBetweenSquares(square1, square2);
                            }

                            Debug.Log($"Loaded connection: From index {connection.fromIndex} to index {connection.toIndex}, Direction: {connection.direction}, Type: {connectionType}");
                        }
                    }
                }
                Debug.Log("=== END OF LOADED CONNECTIONS ===");
            }
            else
            {
                Debug.Log("No saved line connections found in PlayerPrefs");
            }
        }

        void SaveKillerConnections()
        {
            string connectionData = "";
            foreach (var connection in KillerConnections)
            {
                connectionData += connection.fromIndex + "," + connection.toIndex + "," + connection.direction + "," +
                                connection.isDot + "," + connection.isThermo + "," + connection.isThermoStart + "," +
                                connection.isKiller + "," + connection.textureID + "," + connection.killerSum + ";";
            }
            PlayerPrefs.SetString("KillerConnections", connectionData);
            PlayerPrefs.Save();

            Debug.Log("=== Killer CONNECTIONS SAVED ===");
            Debug.Log("Total connections: " + KillerConnections.Count);

            for (int i = 0; i < KillerConnections.Count; i++)
            {
                var connection = KillerConnections[i];
                string connectionType = "";
                if (connection.isKiller && connection.direction == "sum") connectionType = "Killer Sum";
                else if (connection.isKiller && connection.direction == "texture") connectionType = "Killer Texture";
                else if (connection.isThermo && connection.isThermoStart) connectionType = "Thermo Bulb";
                else if (connection.isThermo) connectionType = "Thermo Killer";
                else if (connection.isDot) connectionType = "White Dot";
                else connectionType = "Black Dot/Killer";

                Debug.Log($"Connection {i + 1}: From index {connection.fromIndex} to index {connection.toIndex}, Direction: {connection.direction}, Type: {connectionType}");
            }
            Debug.Log("=== END OF SAVED CONNECTIONS ===");
        }

        // Updated Load Method
        void LoadKillerConnections()
        {
            string connectionData = PlayerPrefs.GetString("KillerConnections", "");
            if (connectionData != "")
            {
                KillerConnections.Clear();
                string[] connections = connectionData.Split(';');

                Debug.Log("=== Killer CONNECTIONS LOADED ===");
                Debug.Log("Total connections to load: " + (connections.Length - 1));

                foreach (string conn in connections)
                {
                    if (conn != "")
                    {
                        string[] parts = conn.Split(',');
                        if (parts.Length == 9) // Now we have 9 parts: fromIndex, toIndex, direction, isDot, isThermo, isThermoStart, isKiller, textureID, killerSum
                        {
                            KillerConnection connection = new KillerConnection
                            {
                                fromIndex = int.Parse(parts[0]),
                                toIndex = int.Parse(parts[1]),
                                direction = parts[2],
                                isDot = bool.Parse(parts[3]),
                                isThermo = bool.Parse(parts[4]),
                                isThermoStart = bool.Parse(parts[5]),
                                isKiller = bool.Parse(parts[6]),
                                textureID = int.Parse(parts[7]),
                                killerSum = parts[8]
                            };
                            KillerConnections.Add(connection);

                            string connectionType = "";
                            if (connection.isKiller && connection.direction == "sum")
                            {
                                connectionType = "Killer Sum";
                                // Reconstruct the killer sum text
                                var killerSquare = grid_squares_[connection.fromIndex].GetComponent<GridSquare>();
                                var textComponents = killerSquare.GetComponentsInChildren<TextMeshProUGUI>();
                                TextMeshProUGUI killerText = textComponents.FirstOrDefault(
                                    tmp => tmp.gameObject.name == "killerSum"
                                );
                                if (killerText != null)
                                {
                                    killerText.text = connection.killerSum;
                                }
                            }
                            else if (connection.isKiller && connection.direction == "texture")
                            {
                                connectionType = "Killer Texture";
                                // Reconstruct killer cage texture
                                var square = grid_squares_[connection.fromIndex].GetComponent<GridSquare>();
                                square.SetTexture(connection.textureID);
                            }
                            else if (connection.isThermo && connection.isThermoStart)
                            {
                                connectionType = "Thermo Bulb";
                                // Reconstruct the thermo bulb
                                var firstSquare = grid_squares_[connection.fromIndex].GetComponent<GridSquare>();
                                Image spriteRenderer = firstSquare.GetComponentInChildren<Image>();
                                if (spriteRenderer != null)
                                {
                                    spriteRenderer.enabled = true;
                                }
                            }
                            else if (connection.isThermo)
                            {
                                connectionType = "Thermo Killer";
                                // Reconstruct thermo Killer
                                var square1 = grid_squares_[connection.fromIndex].GetComponent<GridSquare>();
                                var square2 = grid_squares_[connection.toIndex].GetComponent<GridSquare>();
                                directionLine = connection.direction;
                                DrawLineBetweenSquares(square1, square2);
                            }
                            else if (connection.isDot)
                            {
                                connectionType = "White Dot";
                                // Reconstruct dot
                                var square1 = grid_squares_[connection.fromIndex].GetComponent<GridSquare>();
                                var square2 = grid_squares_[connection.toIndex].GetComponent<GridSquare>();
                                directionLine = connection.direction;
                                ifDot = connection.isDot;
                                DrawBlackDotBetweenSquares(square1, square2);
                            }
                            else
                            {
                                connectionType = "Line";
                                // Reconstruct line
                                var square1 = grid_squares_[connection.fromIndex].GetComponent<GridSquare>();
                                var square2 = grid_squares_[connection.toIndex].GetComponent<GridSquare>();
                                directionLine = connection.direction;
                                DrawLineBetweenSquares(square1, square2);
                            }

                            Debug.Log($"Loaded connection: From index {connection.fromIndex} to index {connection.toIndex}, Direction: {connection.direction}, Type: {connectionType}");
                        }
                    }
                }
                Debug.Log("=== END OF LOADED CONNECTIONS ===");
            }
            else
            {
                Debug.Log("No saved line connections found in PlayerPrefs");
            }
        }

        void SaveRenbanConnections()
        {
            string connectionData = "";
            foreach (var connection in RenbanConnections)
            {
                connectionData += connection.fromIndex + "," + connection.toIndex + "," + connection.direction + "," +
                                connection.isDot + "," + connection.isThermo + "," + connection.isThermoStart + "," +
                                connection.isKiller + "," + connection.textureID + "," + connection.killerSum + "," +
                                connection.isRenban + ";";
            }
            PlayerPrefs.SetString("RenbanConnections", connectionData);
            PlayerPrefs.Save();

            Debug.Log("=== Renban CONNECTIONS SAVED ===");
            Debug.Log("Total connections: " + RenbanConnections.Count);

            for (int i = 0; i < RenbanConnections.Count; i++)
            {
                var connection = RenbanConnections[i];
                string connectionType = "";
                if (connection.isRenban) connectionType = "Renban Renban";
                else if (connection.isKiller && connection.direction == "sum") connectionType = "Killer Sum";
                else if (connection.isKiller && connection.direction == "texture") connectionType = "Killer Texture";
                else if (connection.isThermo && connection.isThermoStart) connectionType = "Thermo Bulb";
                else if (connection.isThermo) connectionType = "Thermo Renban";
                else if (connection.isDot) connectionType = "White Dot";
                else connectionType = "Black Dot/Renban";

                Debug.Log($"Connection {i + 1}: From index {connection.fromIndex} to index {connection.toIndex}, Direction: {connection.direction}, Type: {connectionType}");
            }
            Debug.Log("=== END OF SAVED CONNECTIONS ===");
        }

        // Updated Load Method
        void LoadRenbanConnections()
        {
            string connectionData = PlayerPrefs.GetString("RenbanConnections", "");
            if (connectionData != "")
            {
                RenbanConnections.Clear();
                string[] connections = connectionData.Split(';');

                Debug.Log("=== Renban CONNECTIONS LOADED ===");
                Debug.Log("Total connections to load: " + (connections.Length - 1));

                foreach (string conn in connections)
                {
                    if (conn != "")
                    {
                        string[] parts = conn.Split(',');
                        if (parts.Length == 10) // Now we have 10 parts: fromIndex, toIndex, direction, isDot, isThermo, isThermoStart, isKiller, textureID, killerSum, isRenban
                        {
                            RenbanConnection connection = new RenbanConnection
                            {
                                fromIndex = int.Parse(parts[1]),
                                toIndex = int.Parse(parts[0]),
                                direction = parts[2],
                                isDot = bool.Parse(parts[3]),
                                isThermo = bool.Parse(parts[4]),
                                isThermoStart = bool.Parse(parts[5]),
                                isKiller = bool.Parse(parts[6]),
                                textureID = int.Parse(parts[7]),
                                killerSum = parts[8],
                                isRenban = bool.Parse(parts[9])
                            };
                            RenbanConnections.Add(connection);

                            string connectionType = "";
                            if (connection.isRenban)
                            {
                                connectionType = "Renban Line";
                                // Reconstruct renban line
                                var square1 = grid_squares_[connection.fromIndex].GetComponent<GridSquare>();
                                var square2 = grid_squares_[connection.toIndex].GetComponent<GridSquare>();
                                directionLine = connection.direction;
                                DrawLineBetweenSquares(square1, square2);
                            }
                            else if (connection.isKiller && connection.direction == "sum")
                            {
                                connectionType = "Killer Sum";
                                // Reconstruct the killer sum text
                                var killerSquare = grid_squares_[connection.fromIndex].GetComponent<GridSquare>();
                                var textComponents = killerSquare.GetComponentsInChildren<TextMeshProUGUI>();
                                TextMeshProUGUI killerText = textComponents.FirstOrDefault(
                                    tmp => tmp.gameObject.name == "killerSum"
                                );
                                if (killerText != null)
                                {
                                    killerText.text = connection.killerSum;
                                }
                            }
                            else if (connection.isKiller && connection.direction == "texture")
                            {
                                connectionType = "Killer Texture";
                                // Reconstruct killer cage texture
                                var square = grid_squares_[connection.fromIndex].GetComponent<GridSquare>();
                                square.SetTexture(connection.textureID);
                            }
                            else if (connection.isThermo && connection.isThermoStart)
                            {
                                connectionType = "Thermo Bulb";
                                // Reconstruct the thermo bulb
                                var firstSquare = grid_squares_[connection.fromIndex].GetComponent<GridSquare>();
                                Image spriteRenderer = firstSquare.GetComponentInChildren<Image>();
                                if (spriteRenderer != null)
                                {
                                    spriteRenderer.enabled = true;
                                }
                            }
                            else if (connection.isThermo)
                            {
                                connectionType = "Thermo Line";
                                // Reconstruct thermo line
                                var square1 = grid_squares_[connection.fromIndex].GetComponent<GridSquare>();
                                var square2 = grid_squares_[connection.toIndex].GetComponent<GridSquare>();
                                directionLine = connection.direction;
                                DrawLineBetweenSquares(square1, square2);
                            }
                            else if (connection.isDot)
                            {
                                connectionType = "White Dot";
                                // Reconstruct dot
                                var square1 = grid_squares_[connection.fromIndex].GetComponent<GridSquare>();
                                var square2 = grid_squares_[connection.toIndex].GetComponent<GridSquare>();
                                directionLine = connection.direction;
                                ifDot = connection.isDot;
                                DrawBlackDotBetweenSquares(square1, square2);
                            }
                            else
                            {
                                connectionType = "Line";
                                // Reconstruct line
                                var square1 = grid_squares_[connection.fromIndex].GetComponent<GridSquare>();
                                var square2 = grid_squares_[connection.toIndex].GetComponent<GridSquare>();
                                directionLine = connection.direction;
                                DrawLineBetweenSquares(square1, square2);
                            }

                            Debug.Log($"Loaded connection: From index {connection.fromIndex} to index {connection.toIndex}, Direction: {connection.direction}, Type: {connectionType}");
                        }
                    }
                }
                Debug.Log("=== END OF LOADED CONNECTIONS ===");
            }
            else
            {
                Debug.Log("No saved line connections found in PlayerPrefs");
            }
        }


        if ((currentSceneName == "whispers" || currentSceneName == "whispersMedium" || currentSceneName == "whispersEasy"))
        {
            if (ifContinue == false)
            {
                GermanConnections.Clear(); // Clear existing connections

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
                                    directionLine = "down";
                                    DrawLineBetweenSquares(square1, square2);

                                    // Save connection
                                    GermanConnections.Add(new GermanConnection
                                    {
                                        fromIndex = (i * 9) + j,
                                        toIndex = (i * 9) + j + 9,
                                        direction = "down"
                                    });
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
                                    directionLine = "up";
                                    DrawLineBetweenSquares(square1, square2);

                                    // Save connection
                                    GermanConnections.Add(new GermanConnection
                                    {
                                        fromIndex = (i * 9) + j,
                                        toIndex = (i * 9) + j - 9,
                                        direction = "up"
                                    });
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
                                    directionLine = "left";
                                    DrawLineBetweenSquares(square1, square2);

                                    // Save connection
                                    GermanConnections.Add(new GermanConnection
                                    {
                                        fromIndex = (i * 9) + j,
                                        toIndex = (i * 9) + j + 1,
                                        direction = "left"
                                    });
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
                                    directionLine = "right";
                                    DrawLineBetweenSquares(square1, square2);

                                    // Save connection
                                    GermanConnections.Add(new GermanConnection
                                    {
                                        fromIndex = (i * 9) + j,
                                        toIndex = (i * 9) + j - 1,
                                        direction = "right"
                                    });
                                }
                            }
                        }
                    }
                }

                // Save to PlayerPrefs
                SaveGermanConnections();
            }
            else LoadGermanConnections();
        }




        else if ((currentSceneName == "renban" || currentSceneName == "renbanEasy" || currentSceneName == "renbanMedium"))
        {
            if (ifContinue == false)
            {
                RenbanConnections.Clear(); // Clear existing connections

                // Random generator for renban line creation
                System.Random rand = new System.Random();

                bool[] visited = new bool[81];

                // Possible directions: [left, right, up, down] in 1D grid terms
                int[] directions = { -1, -10, -9, -8, 1, 10, 9, 8 };

                // Function to check if a move is valid (inside grid boundaries)
                bool IsValidMove(int index, int direction)
                {
                    // Ensure index stays in the grid
                    if (index + direction < 0 || index + direction >= 81)
                        return false;

                    // Ensure left-right wrapping isn't violated
                    if ((direction == -1 || direction == -10 || direction == 8) && index % 9 == 0) // Going left from the leftmost column
                        return false;
                    if ((direction == 1 || direction == 10 || direction == -8) && (index + 1) % 9 == 0) // Going right from the rightmost column
                        return false;

                    return true;
                }

                int numberOfLines = 20;
                for (int lineCount = 0; lineCount < numberOfLines; lineCount++)
                {
                    // Retry generating a cage if there is an overlap
                    bool lineGenerated = false;
                    while (!lineGenerated)
                    {
                        // Generate random root cell index (0 to 80 for 9x9 grid)
                        int rootCell = rand.Next(0, 81);

                        //UnityEngine.Debug.Log("root cell " + rootCell);
                        // If the root cell is already visited, retry
                        if (visited[rootCell])
                            continue;

                        // Mark root cell as visited and add to the cage
                        List<int> lineCells = new List<int> { rootCell };
                        visited[rootCell] = true;
                        int lineSize = rand.Next(3, 6);
                        HashSet<int> visitedThisLine = new HashSet<int>(lineCells);

                        // Populate the cage
                        loopCounter = 0;
                        while (lineCells.Count - 1 < lineSize)
                        {
                            int currentCell = lineCells[lineCells.Count - 1];

                            int newCell = -1;
                            int size = lineCells.Count;

                            for (int z = 0; z < 8; z++)
                            {
                                int direction = directions[z];

                                if (IsValidMove(currentCell, direction))
                                {
                                    newCell = currentCell + direction;

                                    if (!visited[newCell] && !lineCells.Contains(newCell))
                                    {
                                        bool ifUnique = true;
                                        for (int i = 0; i < lineCells.Count; i++)
                                        {
                                            if (
                                                grid[newCell / 9, newCell % 9]
                                                == grid[(lineCells[i] / 9), (lineCells[i] % 9)]
                                            )
                                                ifUnique = false;
                                        }
                                        if (ifUnique == true)
                                        {
                                            for (int i = 0; i < lineCells.Count; i++)
                                            {
                                                var renbanLine = grid_squares_[
                                                    lineCells[i]
                                                ].GetComponent<GridSquare>();

                                                if (
                                                    grid[((lineCells[i]) / 9), ((lineCells[i]) % 9)]
                                                        == grid[(newCell / 9), (newCell % 9)] + 1
                                                    || grid[(lineCells[i] / 9), (lineCells[i] % 9)]
                                                        == grid[(newCell / 9), (newCell % 9)] - 1
                                                )
                                                {
                                                    lineCells.Add(newCell);
                                                    var square = grid_squares_[newCell].GetComponent<GridSquare>();
                                                    var square2 = grid_squares_[currentCell].GetComponent<GridSquare>();

                                                    int rowDiff = (newCell / 9) - (currentCell / 9);
                                                    int colDiff = (newCell % 9) - (currentCell % 9);
                                                    directionLine = "";

                                                    if (rowDiff == -1 && colDiff == 0) directionLine = "down";
                                                    else if (rowDiff == 0 && colDiff == 1) directionLine = "right";
                                                    else if (rowDiff == 1 && colDiff == 0) directionLine = "up";
                                                    else if (rowDiff == 0 && colDiff == -1) directionLine = "left";
                                                    else if (rowDiff == 1 && colDiff == 1) directionLine = "right-up";
                                                    else if (rowDiff == -1 && colDiff == 1) directionLine = "right-down";
                                                    else if (rowDiff == 1 && colDiff == -1) directionLine = "left-up";
                                                    else if (rowDiff == -1 && colDiff == -1) directionLine = "left-down";

                                                    DrawLineBetweenSquares(square, square2);

                                                    // Save renban line connection
                                                    RenbanConnections.Add(new RenbanConnection
                                                    {
                                                        fromIndex = currentCell,
                                                        toIndex = newCell,
                                                        direction = directionLine,
                                                        isDot = false,
                                                        isThermo = false,
                                                        isThermoStart = false,
                                                        isKiller = false,
                                                        textureID = 0,
                                                        killerSum = "",
                                                        isRenban = true
                                                    });

                                                    visited[newCell] = true;
                                                    visitedThisLine.Add(newCell); // Add to visited this cage
                                                    directionLine = "";
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            loopCounter++;
                            if (loopCounter == 10)
                            {
                                break;
                            }
                        }

                        // Cage successfully generated, print the cells
                        // Determine the correct texture based on neighbors within this cage only
                        lineGenerated = true;
                    }
                }

                // Save to PlayerPrefs
                SaveRenbanConnections();
            }
            else LoadRenbanConnections();
        }
        else if ((currentSceneName == "kropki" || currentSceneName == "kropkiEasy" || currentSceneName == "kropkiMedium"))
        {
            if(ifContinue == false)
            {
               
            DotConnections.Clear(); // Clear existing connections

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

                                directionLine = "down";
                                ifDot = false;
                                DrawBlackDotBetweenSquares(square1, square2);

                                    // Save connection
                                    if (ifAddedDot == true)
                                    {
                                        DotConnections.Add(new DotConnection
                                        {
                                            fromIndex = (i * 9) + j,
                                            toIndex = (i * 9) + j + 9,
                                            direction = "down",
                                            isDot = false // black dot
                                        });
                                    }
                            }
                            else if (((grid[i, j] - grid[i + 1, j]) == 1))
                            {
                                var square1 = grid_squares_[(i * 9) + j].GetComponent<GridSquare>();
                                var square2 = grid_squares_[(i * 9) + j + 9].GetComponent<GridSquare>();

                                directionLine = "down";
                                ifDot = true;
                                DrawBlackDotBetweenSquares(square1, square2);

                                    // Save connection
                                    if (ifAddedDot == true)
                                    {
                                        DotConnections.Add(new DotConnection
                                        {
                                            fromIndex = (i * 9) + j,
                                            toIndex = (i * 9) + j + 9,
                                            direction = "down",
                                            isDot = true // white dot
                                        });
                                    }
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
                                directionLine = "up";
                                DrawBlackDotBetweenSquares(square1, square2);

                                    // Save connection
                                    if (ifAddedDot == true)
                                    {
                                        DotConnections.Add(new DotConnection
                                        {
                                            fromIndex = (i * 9) + j,
                                            toIndex = (i * 9) + j - 9,
                                            direction = "up",
                                            isDot = false // black dot
                                        });
                                    }
                            }
                            else if (((grid[i, j] - grid[i - 1, j]) == 1))
                            {
                                var square1 = grid_squares_[(i * 9) + j].GetComponent<GridSquare>();
                                var square2 = grid_squares_[(i * 9) + j - 9].GetComponent<GridSquare>();
                                ifDot = true;
                                directionLine = "up";
                                DrawBlackDotBetweenSquares(square1, square2);

                                    // Save connection
                                    if (ifAddedDot == true)
                                    {
                                        DotConnections.Add(new DotConnection
                                        {
                                            fromIndex = (i * 9) + j,
                                            toIndex = (i * 9) + j - 9,
                                            direction = "up",
                                            isDot = true // white dot
                                        });
                                    }
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
                                directionLine = "left";
                                DrawBlackDotBetweenSquares(square1, square2);

                                    // Save connection
                                    if (ifAddedDot == true)
                                    {
                                        DotConnections.Add(new DotConnection
                                        {
                                            fromIndex = (i * 9) + j,
                                            toIndex = (i * 9) + j + 1,
                                            direction = "left",
                                            isDot = false // black dot
                                        });
                                    }
                            }
                            else if (((grid[i, j] - grid[i, j + 1]) == 1))
                            {
                                var square1 = grid_squares_[(i * 9) + j].GetComponent<GridSquare>();
                                var square2 = grid_squares_[(i * 9) + j + 1].GetComponent<GridSquare>();
                                ifDot = true;
                                directionLine = "left";
                                DrawBlackDotBetweenSquares(square1, square2);

                                    // Save connection
                                    if (ifAddedDot == true)
                                    {
                                        DotConnections.Add(new DotConnection
                                        {
                                            fromIndex = (i * 9) + j,
                                            toIndex = (i * 9) + j + 1,
                                            direction = "left",
                                            isDot = true // white dot
                                        });
                                    }
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
                                directionLine = "right";
                                DrawBlackDotBetweenSquares(square1, square2);

                                    // Save connection
                                    if (ifAddedDot == true)
                                    {
                                        DotConnections.Add(new DotConnection
                                        {
                                            fromIndex = (i * 9) + j,
                                            toIndex = (i * 9) + j - 1,
                                            direction = "right",
                                            isDot = false // black dot
                                        });
                                    }
                            }
                            else if (((grid[i, j] - grid[i, j - 1]) == 1))
                            {
                                var square1 = grid_squares_[(i * 9) + j].GetComponent<GridSquare>();
                                var square2 = grid_squares_[(i * 9) + j - 1].GetComponent<GridSquare>();
                                ifDot = true;
                                directionLine = "right";
                                DrawBlackDotBetweenSquares(square1, square2);

                                    // Save connection
                                    if (ifAddedDot == true)
                                    {
                                        DotConnections.Add(new DotConnection
                                        {
                                            fromIndex = (i * 9) + j,
                                            toIndex = (i * 9) + j - 1,
                                            direction = "right",
                                            isDot = true // white dot
                                        });
                                    }
                            }
                        }
                    }
                }
            }

            // Save to PlayerPrefs
            SaveDotConnections();

            }
            else LoadDotConnections();
        }


        else if ((currentSceneName == "killer" || currentSceneName == "killerEasy" || currentSceneName == "killerMedium"))
        {
            if (ifContinue == false)
            {
                KillerConnections.Clear(); // Clear existing connections

                // Number of killer cages to generate
                int numberOfCages = 35;
                int cageSum = 0;
                // Random object to generate numbers
                System.Random rand = new System.Random();

                // Define a 9x9 grid as a 1D array (tracking visited cells for all cages)
                bool[] visited = new bool[81];

                // Possible directions: [left, right, up, down] in 1D grid terms
                int[] directions = { -1, 1, -9, 9 }; // left (-1), right (+1), up (-9), down (+9)

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

                int cageGeneratedCounter = 0;

                // Generate multiple killer cages
                for (int cageCount = 0; cageCount < numberOfCages; cageCount++)
                {
                    cageSum = 0;
                    // Retry generating a cage if there is an overlap
                    bool cageGenerated = false;
                    while (!cageGenerated)
                    {
                        cageGeneratedCounter++;
                        if (cageGeneratedCounter > 81)
                        {
                            cageGeneratedCounter = 0;
                            cageCount++;
                            break;
                        }
                        // Generate random root cell index (0 to 80 for 9x9 grid)
                        int rootCell = rand.Next(0, 81);

                        // If the root cell is already visited, retry
                        if (visited[rootCell])
                            continue;

                        // Mark root cell as visited and add to the cage
                        cageSum += grid[rootCell / 9, rootCell % 9];
                        List<int> cageCells = new List<int> { rootCell };
                        visited[rootCell] = true;
                        int cageSize = rand.Next(2, 5); // Random cage size between 2 and 5 cells

                        // Create a HashSet to track visited cells in the current cage
                        HashSet<int> visitedThisCage = new HashSet<int>(cageCells);

                        // Populate the cage
                        loopCounter = 0;
                        while (cageCells.Count < cageSize)
                        {
                            int currentCell = cageCells[rand.Next(cageCells.Count)];
                            int newCell = -1;
                            int direction = directions[rand.Next(4)];

                            // Check if moving in this direction is valid
                            if (IsValidMove(currentCell, direction))
                            {
                                newCell = currentCell + direction;

                                // If the new cell is unvisited, add it to the cage
                                if (!visited[newCell])
                                {
                                    cageSum += grid[newCell / 9, newCell % 9];
                                    cageCells.Add(newCell);
                                    visited[newCell] = true;
                                    visitedThisCage.Add(newCell); // Add to visited this cage
                                }
                            }
                            loopCounter++;
                            if (loopCounter > 10)
                            {
                                cageSize--;
                                loopCounter = 0;
                            }
                            if (cageSize == 0) break;
                        }

                        // Cage successfully generated, print the cells
                        var killerSquare = grid_squares_[cageCells.Min()].GetComponent<GridSquare>();

                        // Determine the correct texture based on neighbors within this cage only
                        foreach (int cell in visitedThisCage) // Check only cells in this cage
                        {
                            bool hasUp = IsValidMove(cell, -9) && visitedThisCage.Contains(cell - 9);
                            bool hasDown = IsValidMove(cell, 9) && visitedThisCage.Contains(cell + 9);
                            bool hasLeft = IsValidMove(cell, -1) && visitedThisCage.Contains(cell - 1);
                            bool hasRight = IsValidMove(cell, 1) && visitedThisCage.Contains(cell + 1);

                            // Assign texture based on neighbor configuration
                            int textureID = 0;

                            if (hasUp && hasRight && !hasDown && !hasLeft)
                                textureID = 1;
                            else if (hasUp && hasLeft && !hasDown && !hasRight)
                                textureID = 2;
                            else if (hasDown && hasLeft && !hasUp && !hasRight)
                                textureID = 3;
                            else if (hasDown && hasRight && !hasUp && !hasLeft)
                                textureID = 4;
                            else if (hasDown && hasLeft && hasRight && !hasUp)
                                textureID = 5;
                            else if (hasUp && hasLeft && hasRight && !hasDown)
                                textureID = 6;
                            else if (hasLeft && hasUp && hasDown && !hasRight)
                                textureID = 7;
                            else if (hasRight && hasUp && hasDown && !hasLeft)
                                textureID = 8;
                            else if (hasLeft && !hasRight && !hasUp && !hasDown)
                                textureID = 9;
                            else if (hasUp && !hasDown && !hasLeft && !hasRight)
                                textureID = 10;
                            else if (hasRight && !hasLeft && !hasUp && !hasDown)
                                textureID = 11;
                            else if (hasDown && !hasUp && !hasLeft && !hasRight)
                                textureID = 12;
                            else if (hasUp && hasDown && !hasLeft && !hasRight)
                                textureID = 13;
                            else if (hasLeft && hasRight && !hasUp && !hasDown)
                                textureID = 14;
                            else if (!hasLeft && !hasRight && !hasUp && !hasDown)
                                ifSingleCage = true;

                            // Set texture on this cell based on textureID
                            var square = grid_squares_[cell].GetComponent<GridSquare>();
                            square.SetTexture(textureID);

                            // Save killer cage texture connection
                            KillerConnections.Add(new KillerConnection
                            {
                                fromIndex = cell,
                                toIndex = cell, // same cell for texture
                                direction = "texture",
                                isDot = false,
                                isThermo = false,
                                isThermoStart = false,
                                isKiller = true,
                                textureID = textureID,
                                killerSum = ""
                            });
                        }

                        if (ifSingleCage == false)
                        {
                            var textComponents = killerSquare.GetComponentsInChildren<TextMeshProUGUI>();

                            // Find the specific TextMeshPro component with the GameObject name "killerSum"
                            TextMeshProUGUI killerText = textComponents.FirstOrDefault(
                                tmp => tmp.gameObject.name == "killerSum"
                            );
                            string killerSum = cageSum.ToString();
                            killerText.text = killerSum;

                            // Save killer cage sum connection
                            int minCellIndex = cageCells.Min();
                            KillerConnections.Add(new KillerConnection
                            {
                                fromIndex = minCellIndex,
                                toIndex = minCellIndex, // same cell for sum text
                                direction = "sum",
                                isDot = false,
                                isThermo = false,
                                isThermoStart = false,
                                isKiller = true,
                                textureID = 0,
                                killerSum = killerSum
                            });
                        }

                        cageSum = 0;
                        cageGenerated = true;
                        ifSingleCage = false;
                    }
                }

                // Save to PlayerPrefs
                SaveKillerConnections();
            }
            else LoadKillerConnections();
        }

        else if ((currentSceneName == "thermo" || currentSceneName == "thermoEasy" || currentSceneName == "thermoMedium"))
        {
            if (ifContinue == false)
            {
                ThermoConnections.Clear(); // Clear existing connections

                // Number of killer cages to generate
                int numberOfCages = 25;

                // Random object to generate numbers
                System.Random rand = new System.Random();

                // Define a 9x9 grid as a 1D array (tracking visited cells for all cages)
                bool[] visited = new bool[81];

                // Possible directions: [left, right, up, down] in 1D grid terms
                int[] directions = { -1, 1, -9, 9 }; // left (-1), right (+1), up (-9), down (+9)

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

                        // Mark root cell as visited and add to the cage
                        List<int> cageCells = new List<int> { rootCell };
                        visited[rootCell] = true;
                        int cageSize = 6; // Random cage size between 2 and 5 cells

                        // Create a HashSet to track visited cells in the current cage
                        HashSet<int> visitedThisCage = new HashSet<int>(cageCells);

                        // Populate the cage
                        loopCounter = 0;
                        while (cageCells.Count < cageSize)
                        {
                            cageCells = cageCells.OrderBy(cell =>
                            {
                                // Convert the 1D index to 2D coordinates (x, y)
                                int x = cell / 9; // Row index
                                int y = cell % 9; // Column index

                                // Return the value at grid[x, y] to sort by
                                return grid[x, y];
                            }).ToList();
                            int currentCell = cageCells[cageCells.Count - 1];

                            int newCell = -1;
                            int newFirstCell = -1;
                            if (loopCounter == 0)
                            {
                                randomDigit = rand.Next(4);
                            }

                            int direction = directions[(randomDigit + loopCounter) % 4];
                            int firstDirection = directions[rand.Next(4)];

                            // Check if moving in this direction is valid
                            if (IsValidMove(currentCell, direction))
                            {
                                newCell = currentCell + direction;

                                // If the new cell is unvisited, add it to the cage
                                if (!visited[newCell] && (grid[newCell / 9, newCell % 9] > grid[currentCell / 9, currentCell % 9]))
                                {
                                    cageCells.Add(newCell);
                                    var square = grid_squares_[newCell].GetComponent<GridSquare>();
                                    var square2 = grid_squares_[currentCell].GetComponent<GridSquare>();

                                    int rowDiff = (newCell / 9) - (currentCell / 9);
                                    int colDiff = (newCell % 9) - (currentCell % 9);
                                    directionLine = "";

                                    if (rowDiff == -1 && colDiff == 0) directionLine = "down";
                                    else if (rowDiff == 0 && colDiff == 1) directionLine = "right";
                                    else if (rowDiff == 1 && colDiff == 0) directionLine = "up";
                                    else if (rowDiff == 0 && colDiff == -1) directionLine = "left";

                                    DrawLineBetweenSquares(square, square2);

                                    // Save thermo connection
                                    ThermoConnections.Add(new ThermoConnection
                                    {
                                        fromIndex = currentCell,
                                        toIndex = newCell,
                                        direction = directionLine,
                                        isDot = false,
                                        isThermo = true,
                                        isThermoStart = false
                                    });

                                    visited[newCell] = true;
                                    visitedThisCage.Add(newCell); // Add to visited this cage
                                    directionLine = "";
                                }
                            }

                            int firstCell = cageCells[0];
                            if (IsValidMove(firstCell, firstDirection))
                            {
                                newFirstCell = firstCell + firstDirection;

                                // If the new cell is unvisited, add it to the cage
                                if (!visited[newFirstCell] && (grid[newFirstCell / 9, newFirstCell % 9] < grid[firstCell / 9, firstCell % 9]))
                                {
                                    cageCells.Add(newFirstCell);
                                    var square = grid_squares_[newFirstCell].GetComponent<GridSquare>();
                                    var square2 = grid_squares_[firstCell].GetComponent<GridSquare>();

                                    int rowDiff = (newFirstCell / 9) - (firstCell / 9);
                                    int colDiff = (newFirstCell % 9) - (firstCell % 9);
                                    directionLine = "";

                                    if (rowDiff == -1 && colDiff == 0) directionLine = "down";
                                    else if (rowDiff == 0 && colDiff == 1) directionLine = "right";
                                    else if (rowDiff == 1 && colDiff == 0) directionLine = "up";
                                    else if (rowDiff == 0 && colDiff == -1) directionLine = "left";

                                    DrawLineBetweenSquares(square, square2);

                                    // Save thermo connection
                                    ThermoConnections.Add(new ThermoConnection
                                    {
                                        fromIndex = firstCell,
                                        toIndex = newFirstCell,
                                        direction = directionLine,
                                        isDot = false,
                                        isThermo = true,
                                        isThermoStart = false
                                    });

                                    visited[newFirstCell] = true;
                                    visitedThisCage.Add(newFirstCell); // Add to visited this cage
                                    directionLine = "";
                                }
                            }

                            loopCounter++;
                            if (loopCounter >= 4)
                            {
                                cageSize--;
                                loopCounter = 0;
                            }
                        }

                        // Cage successfully generated, print the cells
                        cageGenerated = true;

                        cageCells = cageCells.OrderBy(cell =>
                        {
                            // Convert the 1D index to 2D coordinates (x, y)
                            int x = cell / 9; // Row index
                            int y = cell % 9; // Column index

                            // Return the value at grid[x, y] to sort by
                            return grid[x, y];
                        }).ToList();

                        if (cageCells.Count != 1)
                        {
                            int firstCellIndex = cageCells[0];
                            var firstSquare = grid_squares_[firstCellIndex].GetComponent<GridSquare>();

                            Image spriteRenderer = firstSquare.GetComponentInChildren<Image>();

                            // Deactivate the SpriteRenderer
                            if (spriteRenderer != null)
                            {
                                spriteRenderer.enabled = true;
                            }

                            // Save the thermo start (bulb/circle)
                            ThermoConnections.Add(new ThermoConnection
                            {
                                fromIndex = firstCellIndex,
                                toIndex = firstCellIndex, // same cell for the bulb
                                direction = "bulb",
                                isDot = false,
                                isThermo = true,
                                isThermoStart = true
                            });
                        }
                        else
                        {
                            visited[cageCells[0]] = false;
                        }
                    }
                }

                // Save to PlayerPrefs
                SaveThermoConnections();
            }
            else LoadThermoConnections();
        }

        isFinished = true;
        PlayerPrefs.SetInt("GameReady", 1);
        PlayerPrefs.Save();
    }

    void Update()
    {
        GetCurrentGridState();

    

        ChangeColor();
        ChangeKillerColor();
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
        if (currentSceneName != "Custom" || whichSet != "set")
            EndCheck();

        SudokuSaveSystem sudokusave = FindObjectOfType<SudokuSaveSystem>();
        
            bool hasNonZeroValues = false;

            for (int i = 0; i < 9 && !hasNonZeroValues; i++)
            {
                for (int j = 0; j < 9 && !hasNonZeroValues; j++)
                {
                    if (currentGrid[i, j] != "0")
                    {
                        hasNonZeroValues = true;
                    }
                }
            }

            if (hasNonZeroValues)
            {
            
                sudokusave.UpdateGridFromBoard(grid_squares_);
                sudokusave.SaveGame(sudokusave.currentGrid, sudokusave.isNote);
            }
        
    }

    public void UpdateSelectedCell(int number)
    {
        keyboard = FindObjectOfType<DigitKeyboard>();
        x = 0;
        if (keyboard.selectedCount > 1 && number == 0)
        {
            foreach (var square in selectedCells)
            {
                if (x == 0)
                    square.SetNumber(number);
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
        // Get the RawImage and TextMeshProUGUI components from the children of square1 and square2
        RawImage image1 = square1.GetComponentInChildren<RawImage>();
        RawImage image2 = square2.GetComponentInChildren<RawImage>();

        TextMeshProUGUI text1 = square1.GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI text2 = square2.GetComponentInChildren<TextMeshProUGUI>();

        // Calculate the average Z positions
        float z1 = (image1.transform.position.z + text1.transform.position.z) / 2;
        float z2 = (image2.transform.position.z + text2.transform.position.z) / 2;


        // Get the start and end positions
        Vector3 startPosition = new Vector3(square1.transform.position.x, square1.transform.position.y, z1);
        Vector3 endPosition = new Vector3(square2.transform.position.x, square2.transform.position.y, z2);


        float offset = 0.03f;

        startPosition.x -= offset;
        endPosition.x -= offset;
        
        if (directionLine == "up")
        {
            startPosition.y -= 0.1f;
            endPosition.y += 0.1f;
        }
        else if (directionLine == "down")
        {
            startPosition.y += 0.1f;
            endPosition.y -= 0.1f;
        }
        else if (directionLine == "left")
        {
            startPosition.x -= 0.1f;
            endPosition.x += 0.1f;
        }
        else if (directionLine == "right")
        {
            startPosition.x += 0.1f;
            endPosition.x -= 0.1f;
        }
        else if (directionLine == "left-up")
        {
            startPosition.y -= 0.1f;
            endPosition.y += 0.1f;
            startPosition.x -= 0.1f;
            endPosition.x += 0.1f;
        }
        else if (directionLine == "left-down")
        {
            startPosition.y += 0.1f;
            endPosition.y -= 0.1f;
            startPosition.x -= 0.1f;
            endPosition.x += 0.1f;
        }
        else if (directionLine == "right-up")
        {
            startPosition.x += 0.1f;
            endPosition.x -= 0.1f;
            startPosition.y -= 0.1f;
            endPosition.y += 0.1f;
        }
        else if (directionLine == "right-down")
        {
            startPosition.x += 0.1f;
            endPosition.x -= 0.1f;
            startPosition.y += 0.1f;
            endPosition.y -= 0.1f;
        }
        // Move both ends of the line to the left (along the x-axis)


        // Set the start and end positions of the line
        lineRenderer.SetPosition(0, startPosition);
        lineRenderer.SetPosition(1, endPosition);
        
    }

    private void DrawRenbanLineBetweenSquares(GridSquare square1, GridSquare square2)
    {
        // Instantiate a new line
        GameObject lineObject = Instantiate(linePrefabRenban);
        LineRenderer lineRenderer = lineObject.GetComponent<LineRenderer>();

        // Define the pixel offset (in world units)
        // Get the RawImage and TextMeshProUGUI components from the children of square1 and square2
        RawImage image1 = square1.GetComponentInChildren<RawImage>();
        RawImage image2 = square2.GetComponentInChildren<RawImage>();

        TextMeshProUGUI text1 = square1.GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI text2 = square2.GetComponentInChildren<TextMeshProUGUI>();

        // Calculate the average Z positions
        float z1 = (image1.transform.position.z + text1.transform.position.z) / 2;
        float z2 = (image2.transform.position.z + text2.transform.position.z) / 2;


        // Get the start and end positions
        Vector3 startPosition = new Vector3(square1.transform.position.x, square1.transform.position.y, z1);
        Vector3 endPosition = new Vector3(square2.transform.position.x, square2.transform.position.y, z2);


        float offset = 0.03f;

        startPosition.x -= offset;
        endPosition.x -= offset;

        if (directionLine == "up")
        {
            startPosition.y -= 0.1f;
            endPosition.y += 0.1f;
        }
        else if (directionLine == "down")
        {
            startPosition.y += 0.1f;
            endPosition.y -= 0.1f;
        }
        else if (directionLine == "left")
        {
            startPosition.x -= 0.1f;
            endPosition.x += 0.1f;
        }
        else if (directionLine == "right")
        {
            startPosition.x += 0.1f;
            endPosition.x -= 0.1f;
        }
        else if (directionLine == "left-up")
        {
            startPosition.y -= 0.1f;
            endPosition.y += 0.1f;
            startPosition.x -= 0.1f;
            endPosition.x += 0.1f;
        }
        else if (directionLine == "left-down")
        {
            startPosition.y += 0.1f;
            endPosition.y -= 0.1f;
            startPosition.x -= 0.1f;
            endPosition.x += 0.1f;
        }
        else if (directionLine == "right-up")
        {
            startPosition.x += 0.1f;
            endPosition.x -= 0.1f;
            startPosition.y -= 0.1f;
            endPosition.y += 0.1f;
        }
        else if (directionLine == "right-down")
        {
            startPosition.x += 0.1f;
            endPosition.x -= 0.1f;
            startPosition.y += 0.1f;
            endPosition.y -= 0.1f;
        }
        // Move both ends of the line to the left (along the x-axis)


        // Set the start and end positions of the line
        lineRenderer.SetPosition(0, startPosition);
        lineRenderer.SetPosition(1, endPosition);

    }
    private void DrawThermoLineBetweenSquares(GridSquare square1, GridSquare square2)
    {
        // Instantiate a new line
        GameObject lineObject = Instantiate(linePrefabThermo);
        LineRenderer lineRenderer = lineObject.GetComponent<LineRenderer>();

        // Define the pixel offset (in world units)
        // Get the RawImage and TextMeshProUGUI components from the children of square1 and square2
        RawImage image1 = square1.GetComponentInChildren<RawImage>();
        RawImage image2 = square2.GetComponentInChildren<RawImage>();

        TextMeshProUGUI text1 = square1.GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI text2 = square2.GetComponentInChildren<TextMeshProUGUI>();

        // Calculate the average Z positions
        float z1 = (image1.transform.position.z + text1.transform.position.z) / 2;
        float z2 = (image2.transform.position.z + text2.transform.position.z) / 2;


        // Get the start and end positions
        Vector3 startPosition = new Vector3(square1.transform.position.x, square1.transform.position.y, z1);
        Vector3 endPosition = new Vector3(square2.transform.position.x, square2.transform.position.y, z2);


        float offset = 0.03f;

        startPosition.x -= offset;
        endPosition.x -= offset;

        if (directionLine == "up")
        {
            startPosition.y -= 0.1f;
            endPosition.y += 0.1f;
        }
        else if (directionLine == "down")
        {
            startPosition.y += 0.1f;
            endPosition.y -= 0.1f;
        }
        else if (directionLine == "left")
        {
            startPosition.x -= 0.1f;
            endPosition.x += 0.1f;
        }
        else if (directionLine == "right")
        {
            startPosition.x += 0.1f;
            endPosition.x -= 0.1f;
        }
        else if (directionLine == "left-up")
        {
            startPosition.y -= 0.1f;
            endPosition.y += 0.1f;
            startPosition.x -= 0.1f;
            endPosition.x += 0.1f;
        }
        else if (directionLine == "left-down")
        {
            startPosition.y += 0.1f;
            endPosition.y -= 0.1f;
            startPosition.x -= 0.1f;
            endPosition.x += 0.1f;
        }
        else if (directionLine == "right-up")
        {
            startPosition.x += 0.1f;
            endPosition.x -= 0.1f;
            startPosition.y -= 0.1f;
            endPosition.y += 0.1f;
        }
        else if (directionLine == "right-down")
        {
            startPosition.x += 0.1f;
            endPosition.x -= 0.1f;
            startPosition.y += 0.1f;
            endPosition.y -= 0.1f;
        }
        // Move both ends of the line to the left (along the x-axis)


        // Set the start and end positions of the line
        lineRenderer.SetPosition(0, startPosition);
        lineRenderer.SetPosition(1, endPosition);

    }

    private void DrawBlackDotBetweenSquares(GridSquare square1, GridSquare square2)
    {
        // Instantiate a new line
        if (currentSceneName == "tutorial") linePrefab = linePrefabDot;
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
        if (directionLine == "up")
        {
            startPosition.y += 0.2f;
            endPosition.y -= 0.12f;
        }
        else if (directionLine == "down")
        {
            startPosition.y -= 0.12f;
            endPosition.y += 0.2f;
        }
        else if (directionLine == "left")
        {
            startPosition.x += 0.16f;
            endPosition.x -= 0.16f;
        }
        else if (directionLine == "right")
        {
            startPosition.x -= 0.16f;
            endPosition.x += 0.16f;
        }
        // Move both ends of the line to the left (along the x-axis)


        // Set the start and end positions of the line
        Random random = new Random();
        double randomNumber = random.NextDouble();
        if (currentSceneName != "tutorial")
        {
            bool ifContinue = TouchToChangeScene.ifContinue;
            if (randomNumber > 0.3 || ifContinue)
            {
                if (ifDot == false)
                {
                    lineRenderer.SetPosition(0, startPosition);
                    lineRenderer.SetPosition(1, endPosition);
                    ifAddedDot = true;
                }
                else
                {
                    lineRendererDot.SetPosition(0, startPosition);
                    lineRendererDot.SetPosition(1, endPosition);
                    ifAddedDot = true;
                }
            }
            else ifAddedDot = false;
        }
        else
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
                if (x == 0)
                    square.SetNumberNote(number);
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

        StretchImageOnCanvas(blackSquare);
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

    private void StretchImageOnCanvas(Image image)
    {
        // Get the RectTransform component of the image
        RectTransform rectTransform = image.GetComponent<RectTransform>();

        // Set the image's position by defining offsets for the corners
        rectTransform.offsetMin = new Vector2(-400, -310); // Bottom-left corner
        rectTransform.offsetMax = new Vector2(510, 600); // Top-right corner
        image.transform.position = new Vector3(0, 1, 90);
        // Ensure the image stretches fully
        //rectTransform.anchorMin = new Vector2(0, 0);
        //rectTransform.anchorMax = new Vector2(1, 1);

        //// Optional: Zero out the pivot and position to make it easier to stretch
        //rectTransform.pivot = new Vector2(0, 0);
        //rectTransform.anchoredPosition = Vector2.zero;
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
        if (endChecker && currentSceneName == "tutorial") tutorialDone = true;
        if (endChecker && currentSceneName != "tutorial")
        {
            if (done == false)
            {
                
                int countEasy = PlayerPrefs.GetInt("medalEasy", 0);
                int countMed = PlayerPrefs.GetInt("medalMed", 0);
                int countHard = PlayerPrefs.GetInt("medalHard", 0);
                int countWhisper = PlayerPrefs.GetInt("medalWhispers", 0);
                int countRenban = PlayerPrefs.GetInt("medalRenban", 0);
                int countThermo = PlayerPrefs.GetInt("medalThermo", 0);
                int countKiller = PlayerPrefs.GetInt("medalKiller", 0);
                int countKropki = PlayerPrefs.GetInt("medalKropki", 0);
                int exp = PlayerPrefs.GetInt("Exp", 0);

                if (currentSceneName == "easy")
                {
                    exp = exp + 10;
                    countEasy++;
                    PlayerPrefs.SetInt("medalEasy", countEasy);
                    PlayerPrefs.SetInt("Exp", exp);
                }
                else if (currentSceneName == "medium")
                {
                    countMed++;
                    exp = exp + 20;
                    PlayerPrefs.SetInt("medalMed", countMed);
                    PlayerPrefs.SetInt("Exp", exp);
                }
                else if (currentSceneName == "hard")
                {
                    countHard++;
                    exp = exp + 30;
                    PlayerPrefs.SetInt("medalHard", countHard);
                    PlayerPrefs.SetInt("Exp", exp);
                }
                else if (currentSceneName == "thermo")
                {
                    exp = exp + 50;
                    countThermo = countThermo + 3;
                    PlayerPrefs.SetInt("medalThermo", countThermo);
                    PlayerPrefs.SetInt("Exp", exp);
                }
                else if (currentSceneName == "thermoMedium")
                {
                    exp = exp + 35;
                    countThermo = countThermo + 2;
                    PlayerPrefs.SetInt("medalThermo", countThermo);
                    PlayerPrefs.SetInt("Exp", exp);
                }
                else if (currentSceneName == "thermoEasy")
                {
                    exp = exp + 20;
                    countThermo++;
                    PlayerPrefs.SetInt("medalThermo", countThermo);
                    PlayerPrefs.SetInt("Exp", exp);
                }
                else if (currentSceneName == "kropki")
                {
                    exp = exp + 50;
                    countKropki = countKropki + 3;
                    PlayerPrefs.SetInt("medalKropki", countKropki);
                    PlayerPrefs.SetInt("Exp", exp);
                }
                else if (currentSceneName == "kropkiEasy")
                {
                    exp = exp + 20;
                    countKropki++;
                    PlayerPrefs.SetInt("medalKropki", countKropki);
                    PlayerPrefs.SetInt("Exp", exp);
                }
                else if (currentSceneName == "kropkiMedium")
                {
                    exp = exp + 35;
                    countKropki = countKropki + 2;
                    PlayerPrefs.SetInt("medalKropki", countKropki);
                    PlayerPrefs.SetInt("Exp", exp);
                }
                else if (currentSceneName == "killer")
                {
                    exp = exp + 50;
                    countKiller = countKiller + 3;
                    PlayerPrefs.SetInt("medalKiller", countKiller);
                    PlayerPrefs.SetInt("Exp", exp);
                }
                else if (currentSceneName == "killerEasy")
                {
                    exp = exp + 20;
                    countKiller++;
                    PlayerPrefs.SetInt("medalKiller", countKiller);
                    PlayerPrefs.SetInt("Exp", exp);
                }
                else if (currentSceneName == "killerMedium")
                {
                    exp = exp + 35;
                    countKiller = countKiller + 2;
                    PlayerPrefs.SetInt("medalKiller", countKiller);
                    PlayerPrefs.SetInt("Exp", exp);
                }
                else if (currentSceneName == "renbanEasy")
                {
                    exp = exp + 20;
                    countRenban++;
                    PlayerPrefs.SetInt("medalRenban", countRenban);
                    PlayerPrefs.SetInt("Exp", exp);
                }
                else if (currentSceneName == "renbanMedium")
                {
                    exp = exp + 35;
                    countRenban = countRenban + 2;
                    PlayerPrefs.SetInt("medalRenban", countRenban);
                    PlayerPrefs.SetInt("Exp", exp);
                }
                else if (currentSceneName == "renban")
                {
                    exp = exp + 50;
                    countRenban = countRenban + 3;
                    PlayerPrefs.SetInt("medalRenban", countRenban);
                    PlayerPrefs.SetInt("Exp", exp);
                }
                else if (currentSceneName == "whispersEasy")
                {
                    exp = exp + 20;
                    countWhisper++;
                    PlayerPrefs.SetInt("medalWhispers", countWhisper);
                    PlayerPrefs.SetInt("Exp", exp);
                }
                else if (currentSceneName == "whispersMedium")
                {
                    exp = exp + 35;
                    countWhisper = countWhisper + 2;
                    PlayerPrefs.SetInt("medalWhispers", countWhisper);
                    PlayerPrefs.SetInt("Exp", exp);
                }
                else if (currentSceneName == "whispers")
                {
                    exp = exp + 50;
                    countWhisper = countWhisper + 3;
                    PlayerPrefs.SetInt("medalWhispers", countWhisper);
                    PlayerPrefs.SetInt("Exp", exp);
                }

                done = true;
            }
            GameObject panel = new("DimPanel");
            panel.transform.SetParent(canvas.transform);
            RectTransform panelRect = panel.AddComponent<RectTransform>();
            panelRect.anchorMin = Vector2.zero;
            panelRect.anchorMax = Vector2.one;
            panelRect.sizeDelta = Vector2.zero;

            LineRenderer[] allLines = FindObjectsOfType<LineRenderer>();
            foreach (LineRenderer line in allLines)
            {
                line.enabled = false;  // Disable the line rendering
            }

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
        SudokuSaveSystem sudokusave = FindObjectOfType<SudokuSaveSystem>();
        sudokusave.DeleteSave();
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
                if (!IsValidPlacement(row, col, currentGridInt[row, col]) && gridSquare.number_text.GetComponent<TextMeshProUGUI>().text != "")
                {
                    Color redHexColor = new Color32(180, 44, 15, 255);
                    gridSquare.ChangeTextColor(redHexColor);
                }
                gridSquare.SetNumber(grid[row, col]);
                gridSquare.SetGrid(row, col);

                
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

        
            HashSet<int> generatedNumbers = new();
        int z = totalSquaresToDelete;
            for (int b = 0; b < totalSquaresToDelete; b++)
            {
                do
                {
                    Random rnd = new();
                    int c = rnd.Next(0, 3);
                    int d = rnd.Next(0, 3);
                    switch (z % 9)
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
            z--;
            }
        
        RunSolver();
        if (g == 1)
            ifOk = true;
    }




    private bool IsValidPlacement2(int row, int col)
    {
        int index = row * 9 + col;
        var square = grid_squares_[index];
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
                    GameObject square = grid_squares_[i * 9 + j];
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
                    GameObject square = grid_squares_[i * 9 + j];
                    TextMeshProUGUI[] textComponents =
                        square.GetComponentsInChildren<TextMeshProUGUI>();
                    
                    // Find the specific TextMeshPro component with the GameObject name "killerSum"
                    TextMeshProUGUI textMeshPro = textComponents.FirstOrDefault(
                        tmp => tmp.gameObject.name != "killerSum"
                    );
                    if(square.GetComponent<GridSquare>().number_text.GetComponent<TextMeshProUGUI>().text != "")
                    {
                        Color redHexColor = new Color32(180, 44, 15, 255);
                        textMeshPro.color = redHexColor;
                    }
                  
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
                var killerSquare = grid_squares_[i * 9 + j].GetComponent<GridSquare>();
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

    private int[,] ConvertTables2()
    {
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
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
                if (result > 1)
                    return result; // Early exit if more than one solution is found

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
            stackContents +=
                $"(Row: {move.row}, Column: {move.column}, Previous Number: {move.previousNumber}, If Note: {move.ifNote})\n";
        }

        // Logging the stack contents to the console
        UnityEngine.Debug.Log(stackContents);
        UnityEngine.Debug.Log(counter);
    }
}
