using UnityEngine;

public class ThemeHandler : MonoBehaviour
{
    public Sprite sprite1; // Original sprite
    public Sprite sprite2; // Sprite to change to
    public Color color1; // Background color for sprite1
    public Color color2; // Background color for sprite2

    private SpriteRenderer spriteRenderer;
    private bool isLight = true; // Flag to track which sprite is currently active
    private Camera mainCamera; // Reference to the main camera

    void Start()
    {
        // Get the SpriteRenderer component attached to the GameObject
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Set the initial sprite based on the saved PlayerPrefs value
        isLight = PlayerPrefs.GetInt("IsLight", 1) == 1;
        if (isLight)
        {
            spriteRenderer.sprite = sprite1;
        }
        else
        {
            spriteRenderer.sprite = sprite2;
        }

        // Get the main camera
        mainCamera = Camera.main;

        // Set the initial background color based on the sprite
        UpdateBackgroundColor();
    }

    void OnMouseDown()
    {
        // Check which sprite is currently active and toggle it
        if (isLight)
        {
            spriteRenderer.sprite = sprite2;
        }
        else
        {
            spriteRenderer.sprite = sprite1;
        }

        // Update the flag
        isLight = !isLight;

        // Save the current state to PlayerPrefs
        PlayerPrefs.SetInt("IsLight", isLight ? 1 : 0);
        PlayerPrefs.Save();

        // Update the background color based on the new sprite
        UpdateBackgroundColor();
    }

    void UpdateBackgroundColor()
    {
        if (isLight)
        {
            mainCamera.backgroundColor = color1;
        }
        else
        {
            mainCamera.backgroundColor = color2;
        }
    }
}
