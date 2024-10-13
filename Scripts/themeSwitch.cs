using UnityEngine;

public class ThemeSwitch : MonoBehaviour
{
    public Sprite sprite1; // Original sprite
    public Sprite sprite2; // Sprite to change to


    private SpriteRenderer spriteRenderer;
    private bool isLight = true; // Flag to track which sprite is currently active

    void Update()
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

        // Set the initial background color based on the sprite
        
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
        

        // Save the current state to PlayerPrefs
        PlayerPrefs.SetInt("IsLight", isLight ? 1 : 0);
        PlayerPrefs.Save();

        // Update the background color based on the new sprite
        
    }

    
}
