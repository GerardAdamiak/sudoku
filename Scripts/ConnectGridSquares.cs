using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConnectGridSquares : MonoBehaviour
{
    private GridSquare gridSquareA;
    private GridSquare gridSquareB;
    private LineRenderer lineRenderer;
    private SudokuGrid grid;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        if (gridSquareA == null || gridSquareB == null)
        {
            Debug.LogError("Grid Squares are not assigned.");
            return;
        }

        // Set the initial position
        UpdateLinePosition();
    }

    void Update()
    {
        // Continuously update the line's position in case of movement or resizing
        UpdateLinePosition();
    }

    void UpdateLinePosition()
    {   
        grid = FindObjectOfType<SudokuGrid>();
        gridSquareA = grid.grid_squares_[0].GetComponent<GridSquare>();
        gridSquareB = grid.grid_squares_[2].GetComponent<GridSquare>();
        // Access RectTransforms
        RectTransform rawImageA = gridSquareA.GetComponentInChildren<RawImage>().rectTransform;
        RectTransform textA = gridSquareA.GetComponentInChildren<TextMeshProUGUI>().rectTransform;
        RectTransform rawImageB = gridSquareB.GetComponentInChildren<RawImage>().rectTransform;
        RectTransform textB = gridSquareB.GetComponentInChildren<TextMeshProUGUI>().rectTransform;

        // Get world positions
        Vector3 rawImageAPos = GetWorldPosition(rawImageA);
        Vector3 textAPos = GetWorldPosition(textA);
        Vector3 rawImageBPos = GetWorldPosition(rawImageB);
        Vector3 textBPos = GetWorldPosition(textB);

        // Midpoints between RawImage and Text for both grid squares
        Vector3 startPos = (rawImageAPos + textAPos) / 2;
        Vector3 endPos = (rawImageBPos + textBPos) / 2;

        // Set positions in the LineRenderer
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);
    }

    Vector3 GetWorldPosition(RectTransform rectTransform)
    {
        // Convert RectTransform position to world position
        Vector3 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, rectTransform.position);
        return Camera.main.ScreenToWorldPoint(new Vector3(screenPoint.x, screenPoint.y, screenPoint.z));
    }
}
