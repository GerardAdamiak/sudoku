using UnityEngine;
using UnityEngine.UI;

public class MultiProgressBar : MonoBehaviour
{
    public Image progressBar1;   // Reference to the first progress bar
    public Image progressBar2;   // Reference to the second progress bar
    public Image progressBar3;   // Reference to the third progress bar
    public Image progressBar4;
    public Image progressBar5;   // Reference to the first progress bar
    public Image progressBar6;   // Reference to the second progress bar
    public Image progressBar7;   // Reference to the third progress bar
    public Image progressBar8;
    public Image progressBar9;

    private int maxValue1;
    private int maxValue2;
    private int maxValue3;
    private int maxValue4;// Maximum value for each progress bar
    private int maxValue5;
    private int maxValue6;
    private int maxValue7;
    private int maxValue8;
    private int maxValue9;


    private int currentValue1;                     // Current value for the first bar
    private int currentValue2;                     // Current value for the second bar
    private int currentValue3;
    private int currentValue4;                     // Current value for the first bar
    private int currentValue5;                     // Current value for the second bar
    private int currentValue6;
    private int currentValue7;                     // Current value for the first bar
    private int currentValue8;
    private int currentValue9; // Current value for the second bar
                               // Current value for the third bar

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
    public void SetProgressBar4(int value, int maxValue)
    {
        maxValue4 = maxValue;
        currentValue4 = Mathf.Clamp(value, 0, maxValue4);
        UpdateProgressBar(progressBar4, currentValue4, maxValue4);
    }
    public void SetProgressBar5(int value, int maxValue)
    {
        maxValue5 = maxValue;
        currentValue5 = Mathf.Clamp(value, 0, maxValue5);
        UpdateProgressBar(progressBar5, currentValue5, maxValue5);
    }
    public void SetProgressBar6(int value, int maxValue)
    {
        maxValue6 = maxValue;
        currentValue6 = Mathf.Clamp(value, 0, maxValue6);
        UpdateProgressBar(progressBar6, currentValue6, maxValue6);
    }
    public void SetProgressBar7(int value, int maxValue)
    {
        maxValue7 = maxValue;
        currentValue7 = Mathf.Clamp(value, 0, maxValue7);
        UpdateProgressBar(progressBar7, currentValue7, maxValue7);
    }
    public void SetProgressBar8(int value, int maxValue)
    {
        maxValue8 = maxValue;
        currentValue8 = Mathf.Clamp(value, 0, maxValue8);
        UpdateProgressBar(progressBar8, currentValue8, maxValue8);
    }
    public void SetProgressBar9(int value, int maxValue)
    {
        maxValue9 = maxValue;
        currentValue9 = Mathf.Clamp(value, 0, maxValue9);
        UpdateProgressBar(progressBar9, currentValue9, maxValue9);
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
        UpdateProgressBar(progressBar4, currentValue4, maxValue4);
        UpdateProgressBar(progressBar5, currentValue5, maxValue5);
        UpdateProgressBar(progressBar6, currentValue6, maxValue6);
        UpdateProgressBar(progressBar7, currentValue7, maxValue7);
        UpdateProgressBar(progressBar8, currentValue8, maxValue8);
        UpdateProgressBar(progressBar9, currentValue9, maxValue9);

    }
}
