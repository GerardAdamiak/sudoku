using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer instance;

    public AudioSource musicSource;

    void Awake()
    {
        // Ensure only one instance of MusicPlayer exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        PlayerPrefs.SetInt("number", 1);
        // Check if the AudioSource is assigned
        if (musicSource != null)
        {
            // Play the music
            musicSource.Play();
        }
        else
        {
            Debug.LogError("Music source is not assigned!");
        }
    }
}
