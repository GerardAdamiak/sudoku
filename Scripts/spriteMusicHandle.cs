using UnityEngine;

public class SpriteClick : MonoBehaviour
{
    public Sprite sprite1; // Original sprite
    public Sprite sprite2; // Sprite to change to
    public Sprite sprite3; // Original sprite
    public Sprite sprite4; // Sprite to change to

    private SpriteRenderer spriteRenderer;
    private bool isSprite1 = true; // Flag to track which sprite is currently active
    private bool isLight = true; // Flag to track which sprite is currently active
    private AudioSource audioSource; // Reference to the AudioSource component

    void Start()
    {
        // Get the SpriteRenderer component attached to the GameObject
        
    }

    private void Update()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Set the initial sprite based on the saved PlayerPrefs value
        isSprite1 = PlayerPrefs.GetInt("IsSprite1", 1) == 1;
        isLight = PlayerPrefs.GetInt("IsLight", 1) == 1;
        if (isSprite1)
        {
            if (!isLight) spriteRenderer.sprite = sprite3;
            else spriteRenderer.sprite = sprite1;
        }
        else
        {
            if (!isLight) spriteRenderer.sprite = sprite4;
            else spriteRenderer.sprite = sprite2;
        }

        // Find the AudioSource component by tag
        audioSource = GameObject.FindGameObjectWithTag("musicPlayer").GetComponent<AudioSource>();

        // Set the mute state based on the saved PlayerPrefs value
        audioSource.mute = PlayerPrefs.GetInt("IsAudioMuted", 0) == 1;
    }

    void OnMouseDown()
    {
        // Check if AudioSource is not null
        if (audioSource != null)
        {
            // Check which sprite is currently active and toggle it
            if (isSprite1)
            {
                if (!isLight) spriteRenderer.sprite = sprite3;
                else spriteRenderer.sprite = sprite1;
            }
            else
            {
                if (!isLight) spriteRenderer.sprite = sprite4;
                else spriteRenderer.sprite = sprite2;
            }

            // Update the flag
            isSprite1 = !isSprite1;

            // Toggle the mute state of the AudioSource
            audioSource.mute = !audioSource.mute;

            // Save the current state to PlayerPrefs
            PlayerPrefs.SetInt("IsSprite1", isSprite1 ? 1 : 0);
            PlayerPrefs.SetInt("IsAudioMuted", audioSource.mute ? 1 : 0);
            PlayerPrefs.Save();
        }
        else
        {
            Debug.LogWarning("AudioSource not found.");
        }
    }
}