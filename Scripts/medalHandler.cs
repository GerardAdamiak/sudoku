using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class medalHandler : MonoBehaviour
{

    public TextMeshProUGUI tmpText; // Reference to the TextMeshPro component (for UI)
    public int value; // The int value to check (0 or 1)
    private int countEasy;
    private int countMed;
    private int countHard;
    // Define the colors
    public Color colorForZero = Color.red;
    public Color colorForOne = Color.green;

    // Start is called before the first frame update
    void Start()
    {
        countEasy = PlayerPrefs.GetInt("medalEasy");
        countMed = PlayerPrefs.GetInt("medalMed");
        countHard = PlayerPrefs.GetInt("medalHard");
        value = PlayerPrefs.GetInt("IsLight");
        UpdateTextColor(); 

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    

    void UpdateTextColor()
    {
        if (tmpText.CompareTag("easyBronze"))
        {
            countEasy = countEasy % 10;
            tmpText.text = countEasy.ToString() + "/10";
        }
        else if (tmpText.CompareTag("mediumBronze"))
        {
            countMed = countMed % 10;
            tmpText.text = countMed.ToString() + "/10";
        }
        else if (tmpText.CompareTag("hardBronze"))
        {
            countHard = countHard % 10;
            tmpText.text = countHard.ToString() + "/10";
        }
        else if (tmpText.CompareTag("easySilver"))
        {
            countEasy = countEasy % 30;
            tmpText.text = countEasy.ToString() + "/30";
        }
        else if (tmpText.CompareTag("mediumSilver"))
        {
            countMed = countMed % 30;
            tmpText.text = countMed.ToString() + "/30";
        }
        else if (tmpText.CompareTag("hardSilver"))
        {
            countHard = countHard % 30;
            tmpText.text = countHard.ToString() + "/30";
        }
        else if (tmpText.CompareTag("easyGold"))
        {
            countEasy = countEasy % 90;
            tmpText.text = countEasy.ToString() + "/90";
        }
        else if (tmpText.CompareTag("mediumGold"))
        {
            countMed = countMed % 90;
            tmpText.text = countMed.ToString() + "/90";
        }
        else if (tmpText.CompareTag("hardGold"))
        {
            countHard = countHard % 90;
            tmpText.text = countHard.ToString() + "/90";
        }

        // Check the value and update the TMP text color
        if (value == 0)
        {
            tmpText.color = colorForZero; // Set color to red if value is 0
        }
        else if (value == 1)
        {
            tmpText.color = colorForOne; // Set color to green if value is 1
        }
    }

    // Optional: You can update the color dynamically through this method
    public void SetValue(int newValue)
    {
        value = newValue;
        UpdateTextColor();
    }

}
