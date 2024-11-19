using UnityEngine;

public class SpriteMover : MonoBehaviour
{
    // Reference to the SudokuGrid object
    private TouchToChangeScene sudokuGrid;

    // Animation settings
    public float moveDistance = 0.5f; // Distance to move left
    public float moveSpeed = 1.0f;    // Speed of the movement

    // Original position of the sprite
    private Vector3 originalPosition;

    // SpriteRenderer component
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        // Cache the original position
        originalPosition = transform.position;

        // Get the SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Validate dependencies
        sudokuGrid = FindObjectOfType<TouchToChangeScene>();

        if (sudokuGrid == null)
        {
            Debug.LogError("SudokuGrid reference is missing!");
        }
    }

    void Update()
    {
        if (sudokuGrid != null)
        {
            // Check the ifFirst boolean
            if (!sudokuGrid.ifFirst)
            {
                // Disable the sprite and return
                spriteRenderer.enabled = false;
                return;
            }
            else
            {
                // Enable the sprite
                spriteRenderer.enabled = true;

                // Perform the animation
                AnimateSprite();
            }
        }
    }

    private void AnimateSprite()
    {
        // Calculate the offset based on time
        float offset = Mathf.PingPong(Time.time * moveSpeed, 1.0f) * moveDistance;

        // Apply the animation (move left and snap back)
        transform.position = originalPosition - new Vector3(offset, 0, 0);
    }
}
