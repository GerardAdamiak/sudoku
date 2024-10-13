using UnityEngine;
using TMPro;

public class CenterTextOnScreen : MonoBehaviour
{
    public TMP_Text textMeshProText;

    void Start()
    {
        // Call the method to center the text on the screen
        CenterText();
    }

    void CenterText()
    {
        // Get the RectTransform of the text
        RectTransform rectTransform = textMeshProText.GetComponent<RectTransform>();

        // Set the anchored position to the center of the screen
        rectTransform.anchoredPosition = new Vector2(Screen.width / 2f, Screen.height);
    }
}
