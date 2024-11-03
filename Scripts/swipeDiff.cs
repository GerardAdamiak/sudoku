using UnityEngine;

public class SwipeDiff : MonoBehaviour
{
    private Vector2 startTouchPosition;
    private Vector2 currentTouchPosition;
    private Vector2 endTouchPosition;

    public bool isDragging = false;
    public bool canClickDiff = true;
    private float swipeThreshold = 50f; // Minimum distance for a swipe to be recognized
    public GameObject[] elements; // Array of elements
    private int currentIndex = 0;

    private Vector3[] initialPositions; // To store the initial positions of elements

    private void Start()
    {
        initialPositions = new Vector3[elements.Length];
        for (int i = 0; i < elements.Length; i++)
        {
            initialPositions[i] = elements[i].transform.localPosition; // Store initial positions of elements
        }
        ShowElement(currentIndex);
    }

    void Update()
    {
        DetectSwipe();
    }

    void DetectSwipe()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startTouchPosition = touch.position;
                    isDragging = true;
                    canClickDiff = true;
                    break;

                case TouchPhase.Moved:
                    if (isDragging)
                    {
                        currentTouchPosition = touch.position;
                        float deltaX = currentTouchPosition.x - startTouchPosition.x;
                        
                        if(Mathf.Abs(deltaX) > swipeThreshold) canClickDiff = false;
                        DragElements(deltaX);
                    }
                    break;

                case TouchPhase.Ended:
                    endTouchPosition = touch.position;
                    HandleSwipe();
                    isDragging = false;
                    SnapToPosition();
                    break;
            }
        }
    }

    void DragElements(float deltaX)
    {
        // Move each element horizontally along with the swipe
        
            elements[currentIndex].transform.localPosition = initialPositions[currentIndex] + new Vector3((deltaX / 150), 0, 0);
        elements[currentIndex + 5].transform.localPosition = initialPositions[currentIndex + 5] + new Vector3((deltaX / 200), 0, 0);

    }

    void HandleSwipe()
    {
        float deltaX = endTouchPosition.x - startTouchPosition.x;

        // Check if swipe is horizontal and long enough
        if (Mathf.Abs(deltaX) > swipeThreshold)
        {
            if (deltaX > 0)
            {
                // Swipe Right
                OnSwipeRight();
            }
            else
            {
                // Swipe Left
                OnSwipeLeft();
            }
        }
    }

    void SnapToPosition()
    {
        // Move the elements back to their original or final positions after the swipe ends
        for (int i = 0; i < elements.Length; i++)
        {
            elements[i].transform.localPosition = initialPositions[i];
        }
    }

    void OnSwipeLeft()
    {
        currentIndex++;
        if (currentIndex >= elements.Length - 5)
        {
            currentIndex = 0; // Loop back to the first element
        }
        ShowElement(currentIndex);
    }

    void OnSwipeRight()
    {
        currentIndex--;
        if (currentIndex < 0)
        {
            currentIndex = 4; // Loop back to the last element
        }
        ShowElement(currentIndex);
    }

    void ShowElement(int index)
    {
        // Disable all elements
        foreach (GameObject element in elements)
        {
            element.SetActive(false);
        }

        // Enable the current element and the next ones in the list (e.g. showing 5 elements)
        elements[index].SetActive(true);
        elements[index + 5].SetActive(true);
    }
}
