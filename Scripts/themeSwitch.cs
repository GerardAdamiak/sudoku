using UnityEngine;
using UnityEngine.UI;



public class ThemeSwitch : MonoBehaviour
{
    public Sprite sprite1; // Original sprite
    public Sprite sprite2; // Sprite to change to
    public Image panel; // Reference to the panel Image component
    public Color lightColor; // Background color for light theme
    public Color darkColor;

    private SpriteRenderer spriteRenderer;
    private Image image;
   
    
    private bool isLight = true; // Flag to track which sprite is currently active

    void Update()
    {
        // Get the SpriteRenderer component attached to the GameObject
        spriteRenderer = GetComponent<SpriteRenderer>();
        image = GetComponent<Image>();

        // Set the initial sprite based on the saved PlayerPrefs value
        isLight = PlayerPrefs.GetInt("IsLight", 1) == 1;
        if (spriteRenderer != null)
        {
            if (isLight)
            {
                spriteRenderer.sprite = sprite1;
               
                
            }
            else
            {
                spriteRenderer.sprite = sprite2;
                
            }
        }
        else
        {
            if (isLight)
            {
               
                image.sprite = sprite1;
            }
            else
            {
                
                image.sprite = sprite2;
            }
        }
        if (panel != null)
        {
            panel.color = isLight ? lightColor : darkColor;
        }

        // Get the main camera

        // Set the initial background color based on the sprite

    }

    void OnMouseDown()
    {
        // Check which sprite is currently active and toggle it
        if (spriteRenderer != null)
        {
            if (isLight)
            {
                spriteRenderer.sprite = sprite2;
          
            }
            else
            {
                spriteRenderer.sprite = sprite1;
            
            }
        }
        else{
            if (isLight)
            {
           
                image.sprite = sprite2;
            }
            else
            {
             
                image.sprite = sprite1;
            }
        }

        // Update the flag


        // Save the current state to PlayerPrefs
        PlayerPrefs.SetInt("IsLight", isLight ? 1 : 0);
        PlayerPrefs.Save();

        // Update the background color based on the new sprite
        
    }

    
}
