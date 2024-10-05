using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class HintClickDimming : MonoBehaviour
{
    public GameObject dimPanel; // The UI Panel that will be used to dim the screen.
    public GraphicRaycaster raycaster; // Assign the Canvas GraphicRaycaster for UI detection.
    public EventSystem eventSystem;    // Assign the EventSystem component.

    void Start()
    {
        // Ensure the dim panel is not visible at the start.
        dimPanel.SetActive(false);
    }

    void Update()
    {
        // Detect mouse click
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("click");

            // First, check if the click is over a UI element.
            if (IsPointerOverUIElement())
            {
                // If the "info" UI element is clicked, undim the screen.
                Debug.Log("UI element clicked.");
                UndimScreen();
            }
            else
            {
                // If not over UI, check for 2D sprite clicks.
                RaycastHit2D hit = Physics2D.Raycast(
                    Camera.main.ScreenToWorldPoint(Input.mousePosition),
                    Vector2.zero
                );

                if (hit.collider != null)
                {
                    Debug.Log("World object clicked.");

                    if (hit.collider.CompareTag("hint"))
                    {
                        // Dim the screen when "hint" is clicked.
                        DimScreen();
                    }
                    else if (hit.collider.CompareTag("info"))
                    {
                        // Undim the screen when "info" is clicked.
                        Debug.Log("Sprite Info clicked.");
                        UndimScreen();
                    }
                }
            }
        }
    }

    // Function to check if the mouse is over a UI element
    bool IsPointerOverUIElement()
    {
        PointerEventData pointerData = new PointerEventData(eventSystem)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(pointerData, results);

        // Loop through all UI elements hit by the raycast
        foreach (RaycastResult result in results)
        {
            if (result.gameObject.CompareTag("info"))
            {
                return true; // UI "info" element was clicked.
            }
        }

        return false;
    }

    void DimScreen()
    {
        // Show the dimming panel
        dimPanel.SetActive(true);
    }

    void UndimScreen()
    {
        // Hide the dimming panel
        dimPanel.SetActive(false);
    }
}
