using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MedalHandler : MonoBehaviour
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
    

    
    

    void UpdateTextColor()
    {

       MultiProgressBar progressBar = FindObjectOfType<MultiProgressBar>();

        if (tmpText.CompareTag("easyBronze"))
        {
            if (countEasy >= 10)
            {
                tmpText.enabled = false;
               
                
            }else progressBar.SetProgressBar1(countEasy, 10);
                tmpText.text = countEasy.ToString() + "/10";
            
        }
        else if (tmpText.CompareTag("mediumBronze"))
        {
            if (countMed >= 10)
            {
                tmpText.enabled = false;
              
            }else progressBar.SetProgressBar2(countMed, 10);
            tmpText.text = countMed.ToString() + "/10";
           
        }
        else if (tmpText.CompareTag("hardBronze"))
        {
            if (countHard >= 10)
            {
                tmpText.enabled = false;

            }
            else progressBar.SetProgressBar3(countHard, 10);

            tmpText.text = countHard.ToString() + "/10";
          
        }
        else if (tmpText.CompareTag("easySilver"))
        {
            if (countEasy >= 40 || countEasy < 10)
            {
                tmpText.enabled = false;
               
            }
            else progressBar.SetProgressBar1(countEasy - 10, 30);
            tmpText.text = (countEasy - 10).ToString() + "/30";
           
        }
        else if (tmpText.CompareTag("mediumSilver"))
        {
            if (countMed >= 40 || countMed < 10)
            {
                tmpText.enabled = false;
               
            }else progressBar.SetProgressBar2(countMed - 10, 30);
            tmpText.text = (countMed - 10).ToString() + "/30";
            
        }
        else if (tmpText.CompareTag("hardSilver"))
        {
            if (countHard >= 40 || countHard < 10)
            {
                tmpText.enabled = false;
               
            }else progressBar.SetProgressBar3(countHard-10, 30);
            tmpText.text = (countHard - 10).ToString() + "/30";
        }
        else if (tmpText.CompareTag("easyGold"))
        {
            if (countEasy >= 130 || countEasy < 40)
            {
                tmpText.enabled = false;
               
            } else progressBar.SetProgressBar1(countEasy - 40, 90);
            tmpText.text = (countEasy - 40).ToString() + "/90";
        }
        else if (tmpText.CompareTag("mediumGold"))
        {
            if (countMed >= 130 || countMed < 40)
            {
                tmpText.enabled = false;
                
            } else progressBar.SetProgressBar2(countMed - 40, 90);
            tmpText.text = (countMed - 40).ToString() + "/90";
        }
        else if (tmpText.CompareTag("hardGold"))
        {
            if (countHard >= 130 || countHard < 40)
            {
                tmpText.enabled = false;
              
            }else progressBar.SetProgressBar3(countHard - 40, 90);
            tmpText.text = (countHard - 40).ToString() + "/90";
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
   

}
