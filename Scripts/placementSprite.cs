using UnityEngine;

public class SpriteUpperCorner : MonoBehaviour
{
    public enum Corner { UpperLeft, UpperRight }

    public Corner corner = Corner.UpperRight; // Default to UpperRight
    private SpriteRenderer spriteRenderer;
    private Camera mainCamera;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;

        UpdatePosition();
    }

    void Update()
    {
        UpdatePosition();
    }

    void UpdatePosition()
    {
        if (spriteRenderer == null || mainCamera == null)
            return;

        // Get the world coordinates of the specified corner of the screen
        Vector3 screenCorner = Vector3.zero;
        switch (corner)
        {
            case Corner.UpperLeft:
                screenCorner = mainCamera.ScreenToWorldPoint(new Vector3(0, Screen.height, 0));
                break;
            case Corner.UpperRight:
                screenCorner = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
                break;
        }

        // Adjust the position based on the sprite's size and camera's orthographic size
        float offsetX = spriteRenderer.bounds.size.x / 2;
        float offsetY = spriteRenderer.bounds.size.y / 2;
        Vector3 newPosition = Vector3.zero;

        if (corner == Corner.UpperLeft)
        {
            newPosition = new Vector3(screenCorner.x + offsetX, screenCorner.y - offsetY - 0.1f, 0); // Subtract 0.1f from the y-coordinate
        }
        else if (corner == Corner.UpperRight)
        {
            newPosition = new Vector3(screenCorner.x - offsetX, screenCorner.y - offsetY, 0);
        }

        // Set the position of the GameObject
        transform.position = newPosition;
    }
}
