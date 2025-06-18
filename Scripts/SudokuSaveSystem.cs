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
    private const string HAS_SAVE_KEY = "_HasSave";
    private const string TIME = "_Time";

    [Header("Current Game State")]
    public string[,] currentGrid = new string[9, 9];

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
                }
            }
        }
    }

    /// <summary>
    /// Saves the current game state to PlayerPrefs
    /// </summary>
    /// <param name="grid">9x9 string array representing the sudoku board</param>
    public void SaveGame(string[,] grid)
    {
        if (grid == null || grid.GetLength(0) != 9 || grid.GetLength(1) != 9)
        {
            Debug.LogError("Grid must be exactly 9x9!");
            return;
        }

        // Get current scene name
        string currentScene = SceneManager.GetActiveScene().name;
        Timer timer = FindObjectOfType<Timer>();
        float time = timer.currentTime;

        // Convert 2D grid to 1D array, then to string
        string[] flatGrid = new string[81];
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
                index++;
            }
        }

        string gridString = string.Join(",", flatGrid);

        // Save to PlayerPrefs
        PlayerPrefs.SetString(saveKey + SCENE_KEY, currentScene);
        PlayerPrefs.SetString(saveKey + BOARD_KEY, gridString);
        PlayerPrefs.SetInt(saveKey + HAS_SAVE_KEY, 1);
        PlayerPrefs.SetFloat(saveKey + TIME, time);

        // Save immediately
        PlayerPrefs.Save();

        int filledCells = flatGrid.Count(x => x != "0" && !string.IsNullOrEmpty(x));
       // Debug.Log($"Game saved! Scene: {currentScene}, Filled cells: {filledCells}");
    }

    /// <summary>
    /// Loads the saved game state from PlayerPrefs
    /// </summary>
    /// <returns>True if save data was found and loaded</returns>
    public bool LoadGame()
    {
        
        // Load scene name
        string savedScene = PlayerPrefs.GetString(saveKey + SCENE_KEY, "");

        // Load board data
        string gridString = PlayerPrefs.GetString(saveKey + BOARD_KEY, "");

        if (string.IsNullOrEmpty(gridString))
        {
            Debug.LogError("Save data corrupted - no board data");
            return false;
        }

        try
        {
            // Convert string back to array
            string[] flatGrid = gridString.Split(',');
         

            if (flatGrid.Length != 81)
            {
                Debug.LogError("Save data corrupted - incorrect board size");
                return false;
            }

            // Convert flat array back to 2D grid
            int index = 0;
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    currentGrid[row, col] = flatGrid[index];
                    index++;
                }
            }
           

            int filledCells = flatGrid.Count(x => x != "0" && !string.IsNullOrEmpty(x));
           // Debug.Log($"Game loaded! Scene: {savedScene}, Filled cells: {filledCells}");

            // Optionally load the scene if it's different from current
            if (savedScene != SceneManager.GetActiveScene().name)
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

    /// <summary>
    /// Checks if save data exists
    /// </summary>
    /// <returns>True if save data exists</returns>
    public bool HasSaveData()
    {
        return PlayerPrefs.GetInt(saveKey + HAS_SAVE_KEY, 0) == 1;
    }

    /// <summary>
    /// Deletes all save data
    /// </summary>
    public void DeleteSaveData()
    {
        PlayerPrefs.DeleteKey(saveKey + SCENE_KEY);
        PlayerPrefs.DeleteKey(saveKey + BOARD_KEY);
        PlayerPrefs.DeleteKey(saveKey + HAS_SAVE_KEY);
        PlayerPrefs.Save();

        Debug.Log("Save data deleted");
    }

    /// <summary>
    /// Gets the saved scene name without loading
    /// </summary>
    /// <returns>Saved scene name or empty string if no save</returns>
    public string GetSavedSceneName()
    {
        if (!HasSaveData()) return "";
        return PlayerPrefs.GetString(saveKey + SCENE_KEY, "");
    }

    /// <summary>
    /// Gets the saved grid without loading
    /// </summary>
    /// <returns>Saved grid array or null if no save</returns>
    public string[,] GetSavedGrid()
    {
        if (!HasSaveData()) return null;

        string gridString = PlayerPrefs.GetString(saveKey + BOARD_KEY, "");
        if (string.IsNullOrEmpty(gridString)) return null;

        try
        {
            string[] flatGrid = gridString.Split(',');
            if (flatGrid.Length != 81) return null;

            string[,] grid = new string[9, 9];
            int index = 0;

            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    grid[row, col] = flatGrid[index];
                    index++;
                }
            }

            return grid;
        }
        catch
        {
            return null;
        }
    }

    // Example methods for integration with your game

    /// <summary>
    /// Call this when the player makes a move or you want to auto-save
    /// </summary>
    public void AutoSave()
    {
        if (currentGrid != null)
        {
            SaveGame(currentGrid);
        }
    }

    /// <summary>
    /// Update the current grid state (call this when player makes moves)
    /// </summary>
    /// <param name="row">Row index (0-8)</param>
    /// <param name="col">Column index (0-8)</param>
    /// <param name="value">Value to set (string, "0" for empty)</param>
    public void UpdateGrid(int row, int col, string value)
    {
        if (row < 0 || row > 8 || col < 0 || col > 8)
        {
            Debug.LogError("Invalid grid position");
            return;
        }

        // Handle null or empty as "0"
        if (string.IsNullOrEmpty(value))
            value = "0";

        currentGrid[row, col] = value;

        // Auto-save after each move (optional)
        // AutoSave();
    }

    /// <summary>
    /// Set the entire grid at once
    /// </summary>
    /// <param name="grid">9x9 string array representing the grid</param>
    public void SetGrid(string[,] grid)
    {
        if (grid == null || grid.GetLength(0) != 9 || grid.GetLength(1) != 9)
        {
            Debug.LogError("Grid must be exactly 9x9!");
            return;
        }

        // Deep copy the grid
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                string value = grid[row, col];
                if (string.IsNullOrEmpty(value))
                    value = "0";

                currentGrid[row, col] = value;
            }
        }
    }

    /// <summary>
    /// Get value at specific position
    /// </summary>
    /// <param name="row">Row index (0-8)</param>
    /// <param name="col">Column index (0-8)</param>
    /// <returns>Value at position (string)</returns>
    public string GetGridValue(int row, int col)
    {
        if (row < 0 || row > 8 || col < 0 || col > 8)
        {
            Debug.LogError("Invalid grid position");
            return "0";
        }

        string value = currentGrid[row, col];
        return string.IsNullOrEmpty(value) ? "0" : value;
    }

    /// <summary>
    /// Clear the entire grid (set all values to "0")
    /// </summary>
    public void ClearGrid()
    {
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                currentGrid[row, col] = "0";
            }
        }
    }

    /// <summary>
    /// Get a copy of the current grid
    /// </summary>
    /// <returns>Copy of the current 9x9 string grid</returns>
    public string[,] GetGridCopy()
    {
        string[,] copy = new string[9, 9];
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                copy[row, col] = currentGrid[row, col];
            }
        }
        return copy;
    }

    /// <summary>
    /// Print the current grid to console (for debugging)
    /// </summary>
    public void PrintGrid()
    {
        string gridOutput = "Current Grid:\n";
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                gridOutput += currentGrid[row, col] + " ";
            }
            gridOutput += "\n";
        }
        Debug.Log(gridOutput);
    }

    // Unity lifecycle methods for automatic saving/loading

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