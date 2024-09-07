using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class SolutionsButton : MonoBehaviour
{
    public SudokuGrid grid;
    public Sprite uniqueSolutionSprite;
    public Sprite notUniqueSolutionSprite;
    public Sprite noSolutionSprite;
    public Sprite uniqueSolutionSpriteLight;
    public Sprite notUniqueSolutionSpriteLight;
    public Sprite noSolutionSpriteLight;
    public Sprite startLight;
    public Sprite start;

    private SpriteRenderer solutionCheckSpriteRenderer;
    private bool isLight = true;
    private Sprite originalSprite; // To store the original sprite

    void Start()
    {
        isLight = PlayerPrefs.GetInt("IsLight", 1) == 1;

        // Check PlayerPrefs for whichSet
        string whichSet = PlayerPrefs.GetString("whichSet", "");

        if (whichSet == "custom")
        {
            // If whichSet is "custom", hide the sprite and make it unclickable
            gameObject.SetActive(false); // Hide the GameObject
            return; // Exit Start() early
        }

        if (grid == null)
        {
            grid = FindObjectOfType<SudokuGrid>();
            if (grid == null)
            {
                Debug.LogError("sudokuGrid instance not found!");
                return;
            }
        }

        GameObject solutionCheckObject = GameObject.FindGameObjectWithTag("solutionCheck");
        if (solutionCheckObject != null)
        {
            if (!solutionCheckObject.TryGetComponent<SpriteRenderer>(out solutionCheckSpriteRenderer))
            {
                Debug.LogError("No SpriteRenderer component found on the GameObject with tag 'solutionCheck'!");
                return;
            }

            if (!isLight) solutionCheckSpriteRenderer.sprite = start;
            else solutionCheckSpriteRenderer.sprite = startLight;

            originalSprite = solutionCheckSpriteRenderer.sprite; // Store the original sprite

            BoxCollider2D collider = solutionCheckObject.GetComponent<BoxCollider2D>()
                                     ?? solutionCheckObject.AddComponent<BoxCollider2D>();
            if (!solutionCheckObject.TryGetComponent<EventTrigger>(out var trigger))
            {
                trigger = solutionCheckObject.AddComponent<EventTrigger>();
            }

            EventTrigger.Entry entry = new()
            {
                eventID = EventTriggerType.PointerClick
            };
            entry.callback.AddListener((eventData) => { OnMouseDown(); });
            trigger.triggers.Add(entry);
        }
        else
        {
            Debug.LogError("Image with tag 'solutionCheck' not found!");
        }
    }

    void OnMouseDown()
    {
        if (solutionCheckSpriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer not assigned!");
            return;
        }

        grid.RunSolver();
        if (grid.g == 1)
        {
            if (!isLight) solutionCheckSpriteRenderer.sprite = uniqueSolutionSprite;
            else solutionCheckSpriteRenderer.sprite = uniqueSolutionSpriteLight;
        }
        else if (grid.g > 1)
        {
            if (!isLight) solutionCheckSpriteRenderer.sprite = notUniqueSolutionSprite;
            else solutionCheckSpriteRenderer.sprite = notUniqueSolutionSpriteLight;
        }
        else if (grid.g == 0)
        {
            if (!isLight) solutionCheckSpriteRenderer.sprite = noSolutionSprite;
            else solutionCheckSpriteRenderer.sprite = noSolutionSpriteLight;
        }

        StartCoroutine(RevertSpriteAfterDelay(2f)); // Start the coroutine to revert the sprite after 2 seconds
    }

    private IEnumerator RevertSpriteAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified delay
        solutionCheckSpriteRenderer.sprite = originalSprite; // Revert to the original sprite
    }
}
