using UnityEngine;
using UnityEngine.UI;

public class MultiProgressBar : MonoBehaviour
{
    public Image progressBar1;   // Reference to the first progress bar
    public Image progressBar2;   // Reference to the second progress bar
    public Image progressBar3;   // Reference to the third progress bar
    private int maxValue1;
    private int maxValue2;
    private int maxValue3;// Maximum value for each progress bar

    private int currentValue1;                     // Current value for the first bar
    private int currentValue2;                     // Current value for the second bar
    private int currentValue3;                     // Current value for the third bar

    void Start()
    {
        // Initialize all bars to 0 or any starting value
        
        UpdateAllProgressBars();
    }

    // Methods to update each progress bar
    public void SetProgressBar1(int value, int maxValue)
    {
        maxValue1 = maxValue;
        currentValue1 = Mathf.Clamp(value, 0, maxValue1);
        UpdateProgressBar(progressBar1, currentValue1, maxValue1);
    }

    public void SetProgressBar2(int value, int maxValue)
    {
        maxValue2 = maxValue;
        currentValue2 = Mathf.Clamp(value, 0, maxValue2);
        UpdateProgressBar(progressBar2, currentValue2, maxValue2);
    }

    public void SetProgressBar3(int value, int maxValue)
    {
        maxValue3 = maxValue;
        currentValue3 = Mathf.Clamp(value, 0, maxValue3);
        UpdateProgressBar(progressBar3, currentValue3, maxValue3);
    }

    // Helper function to update a specific bar's fill amount
    private void UpdateProgressBar(Image barImage, int currentValue, int maxValue)
    {
        float fillAmount = (float)currentValue / maxValue;
        barImage.fillAmount = fillAmount;
    }

    // Updates all bars at once, if needed
    public void UpdateAllProgressBars()
    {
        UpdateProgressBar(progressBar1, currentValue1, maxValue1);
        UpdateProgressBar(progressBar2, currentValue2, maxValue2);
        UpdateProgressBar(progressBar3, currentValue3, maxValue3);
    }
}
