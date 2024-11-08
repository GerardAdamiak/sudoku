using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ImageAnimator : MonoBehaviour
{
    public Image imageComponent;        // Reference to the UI Image component
    public Sprite[] frames;             // Array of sprites for animation
    public float frameRate = 0.05f;      // Time between frames in seconds

    void Start()
    {
       
    }

    public IEnumerator AnimateImage()
    {
        int index = 0;
        while (true)
        {
            imageComponent.sprite = frames[index];
            index = (index + 1) % frames.Length;
            yield return new WaitForSeconds(frameRate);
        }
    }
}
