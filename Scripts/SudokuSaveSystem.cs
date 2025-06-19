using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class SudokuSaveSystem : MonoBehaviour
{
    [Header("Save Settings")]
    public string saveKey = "SudokuSave";

    // Keys for PlayerPrefs
    private const string SCENE_KEY = "_Scene";
    private const string BOARD_KEY = "_Board";
    private const string NOTES_KEY = "_Notes";
    private const string HAS_SAVE_KEY = "_HasSave";
    private const string TIME = "_Time";

    [Header("Current Game State")]
    public string[,] currentGrid = new string[9, 9];
    public bool[,] isNote = new bool[9, 9]; // Track which cells contain notes

    [System.Serializable]
    public class CellData
    {
        public string value;
        public bool isNote;

        public CellData(string val, bool note)
        {
            value = val;
            isNote = note;
        }
    }

    void Start()
    {
        // Initialize grid with empty values
        InitializeGrid();

        // Auto-load on start if you want
        // LoadGame();
    }

    /// <summary>
    /// Initialize the grid with empty values ("0")
    /// </summary>
    private void InitializeGrid()
    {
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                if (string.IsNullOrEmpty(currentGrid[row, col]))
                {
                    currentGrid[row, col] = "0";
                    isNote[row, col] = false;
                }
            }
        }
    }

    /// <summary>
    /// Updates the current grid state from the actual game board
    /// </summary>
    /// <param name="squares">List of GridSquare GameObjects representing the board</param>
    public void UpdateGridFromBoard(System.Collections.Generic.List<GameObject> squares)
    {
        if (squares == null || squares.Count != 81)
        {
            Debug.LogError("Squares list must contain exactly 81 elements!");
            return;
        }

        int index = 0;
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                var square = squares[index];
                var gridSquare = square.GetComponent<GridSquare>();

                if (gridSquare != null && gridSquare.number_text != null)
                {
                    var textComponent = gridSquare.number_text.GetComponent<TMPro.TextMeshProUGUI>();
                    if (textComponent != null)
                    {
                        string cellValue = textComponent.text;
                        bool cellIsNote = textComponent.fontSize == 35;

                        // Store empty cells as "0"
                        currentGrid[row, col] = string.IsNullOrEmpty(cellValue) ? "0" : cellValue;
                        isNote[row, col] = cellIsNote && !string.IsNullOrEmpty(cellValue) && cellValue != "0";
                    }
                }
                index++;
            }
        }
    }
    /// <summary>
    /// Saves the current game state to PlayerPrefs
    /// </summary>
    /// <param name="grid">9x9 string array representing the sudoku board</param>
    /// <param name="noteGrid">9x9 bool array indicating which cells are notes</param>
    public void SaveGame(string[,] grid, bool[,] noteGrid = null)
    {
        if (grid == null || grid.GetLength(0) != 9 || grid.GetLength(1) != 9)
        {
            Debug.LogError("Grid must be exactly 9x9!");
            return;
        }

        // Use the class noteGrid if no parameter is provided
        if (noteGrid == null)
            noteGrid = isNote;

        // Get current scene name
        string currentScene = SceneManager.GetActiveScene().name;
        Timer timer = FindObjectOfType<Timer>();
        float time = timer != null ? timer.currentTime : 0f;

        // Convert 2D grids to 1D arrays, then to strings
        string[] flatGrid = new string[81];
        string[] flatNotes = new string[81];
        int index = 0;

        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                // Handle null or empty values as "0"
                string value = grid[row, col];
                if (string.IsNullOrEmpty(value))
                    value = "0";

                flatGrid[index] = value;
                flatNotes[index] = noteGrid[row, col] ? "1" : "0";
                index++;
            }
        }

        string gridString = string.Join(",", flatGrid);
        string notesString = string.Join(",", flatNotes);



        // Save to PlayerPrefs
        PlayerPrefs.SetString(saveKey + SCENE_KEY, currentScene);
        PlayerPrefs.SetString(saveKey + BOARD_KEY, gridString);
        PlayerPrefs.SetString(saveKey + NOTES_KEY, notesString);
        PlayerPrefs.SetInt(saveKey + HAS_SAVE_KEY, 1);
        PlayerPrefs.SetFloat(saveKey + TIME, time);

        // Save immediately
        PlayerPrefs.Save();

        int filledCells = flatGrid.Count(x => x != "0" && !string.IsNullOrEmpty(x));
        int noteCells = flatNotes.Count(x => x == "1");

    }
    /// <summary>
    /// Loads the saved game state from PlayerPrefs
    /// </summary>
    /// <returns>True if save data was found and loaded</returns>
    public bool LoadGame()
    {
        // Check if save exists
        if (PlayerPrefs.GetInt(saveKey + HAS_SAVE_KEY, 0) == 0)
        {
            Debug.Log("No save data found");
            return false;
        }

        // Load scene name
        string savedScene = PlayerPrefs.GetString(saveKey + SCENE_KEY, "");

        // Load board data
        string gridString = PlayerPrefs.GetString(saveKey + BOARD_KEY, "");
        string notesString = PlayerPrefs.GetString(saveKey + NOTES_KEY, "");

        if (string.IsNullOrEmpty(gridString))
        {
            Debug.LogError("Save data corrupted - no board data");
            return false;
        }

        try
        {
            // Convert strings back to arrays
            string[] flatGrid = gridString.Split(',');
            string[] flatNotes = string.IsNullOrEmpty(notesString) ? new string[81] : notesString.Split(',');

            if (flatGrid.Length != 81)
            {
                Debug.LogError("Save data corrupted - incorrect board size");
                return false;
            }

            // Ensure notes array is correct size (backward compatibility)
            if (flatNotes.Length != 81)
            {
                flatNotes = new string[81];
                for (int i = 0; i < 81; i++)
                    flatNotes[i] = "0";
            }

            // Convert flat arrays back to 2D grids
            int index = 0;
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    currentGrid[row, col] = flatGrid[index];
                    isNote[row, col] = flatNotes[index] == "1";
                    index++;
                }
            }

            int filledCells = flatGrid.Count(x => x != "0" && !string.IsNullOrEmpty(x));
            int noteCells = flatNotes.Count(x => x == "1");
            Debug.Log($"Game loaded! Scene: {savedScene}, Filled cells: {filledCells}, Note cells: {noteCells}");

            // Optionally load the scene if it's different from current
            if (!string.IsNullOrEmpty(savedScene) && savedScene != SceneManager.GetActiveScene().name)
            {
                Debug.Log($"Loading scene: {savedScene}");
                SceneManager.LoadScene(savedScene);
            }

            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error loading save data: {e.Message}");
            return false;
        }
    }
    public void ApplyGridToBoard(System.Collections.Generic.List<GameObject> squares)
    {
        if (squares == null || squares.Count != 81)
        {
            Debug.LogError("Squares list must contain exactly 81 elements!");
            return;
        }

        int index = 0;
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                var square = squares[index];
                var gridSquare = square.GetComponent<GridSquare>();

                if (gridSquare != null && gridSquare.number_text != null)
                {
                    var textComponent = gridSquare.number_text.GetComponent<TMPro.TextMeshProUGUI>();
                    if (textComponent != null)
                    {
                        string cellValue = currentGrid[row, col];
                        bool cellIsNote = isNote[row, col];

                        // Set text (empty cells stored as "0" become empty text)
                        textComponent.text = cellValue == "0" ? "" : cellValue;

                        // Set font size based on whether it's a note
                        textComponent.fontSize = cellIsNote ? 35 : 60;
                    }
                }
                index++;
            }
        }
    }
    /// <summary>
    /// Call this when the player makes a move or you want to auto-save
    /// Requires the squares array to read current board state
    /// </summary>
    /// <param name="squares">Array of GridSquare GameObjects representing the board</param>
    public void AutoSave(System.Collections.Generic.List<GameObject> squares = null)
    {
        if (squares != null)
        {
            UpdateGridFromBoard(squares);
        }

        if (currentGrid != null)
        {
            SaveGame(currentGrid, isNote);
        }
    }

    /// <summary>
    /// Checks if a save file exists
    /// </summary>
    /// <returns>True if save data exists</returns>
    public bool HasSaveData()
    {
        return PlayerPrefs.GetInt(saveKey + HAS_SAVE_KEY, 0) == 1;
    }

    /// <summary>
    /// Gets the saved time from the save file
    /// </summary>
    /// <returns>Saved time or 0 if no save exists</returns>
    public float GetSavedTime()
    {
        return PlayerPrefs.GetFloat(saveKey + TIME, 0f);
    }

    /// <summary>
    /// Deletes the current save data
    /// </summary>
    public void DeleteSave()
    {
        PlayerPrefs.DeleteKey("SudokuSave_Scene");
        PlayerPrefs.DeleteKey("SudokuSave_Board");
        PlayerPrefs.DeleteKey("SudokuSave_Notes");
        PlayerPrefs.DeleteKey("SudokuSave_HasSave");
        PlayerPrefs.DeleteKey("SudokuSave_Time");
        PlayerPrefs.Save();

        Debug.Log("Save data deleted");
    }

    // Unity lifecycle methods for automatic saving

    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus) // App is being paused
        {
            AutoSave();
        }
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus) // App lost focus
        {
            AutoSave();
        }
    }
}