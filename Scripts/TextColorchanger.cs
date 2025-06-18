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
        int isLight = PlayerPrefs.GetInt("IsLight"); // Default to 1 if not set
        

        if (isLight == 1)
        {
            textMeshPro.color = new Color32(46, 49, 56, 255); ;
        }
        else
        {
            textMeshPro.color = new Color32(239, 239, 208, 255); ;
        }
    }
}
