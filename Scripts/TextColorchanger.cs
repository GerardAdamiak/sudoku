using UnityEngine;
using TMPro;

public class TextColorChanger : MonoBehaviour
{
    public TMP_Text textMeshPro;

    void Start()
    {
        if (textMeshPro == null)
        {
            Debug.LogError("TMP_Text reference is missing on " + gameObject.name);
            return;
        }

        UpdateTextColor();
    }

    void UpdateTextColor()
    {
        int isLight = PlayerPrefs.GetInt("isLight", 1); // Default to 1 if not set
        Color newColor;

        if (ColorUtility.TryParseHtmlString(isLight == 1 ? "#EFEFD0" : "#2E3138", out newColor))
        {
            textMeshPro.color = newColor;
        }
        else
        {
            Debug.LogError("Failed to parse color string");
        }
    }
}
