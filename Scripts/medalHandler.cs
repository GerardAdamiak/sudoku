using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MedalHandler : MonoBehaviour
{
    public TextMeshProUGUI tmpText;
    public int value;

    private int countEasy;
    private int countMed;
    private int countHard;

    public Color colorForZero = Color.red;
    public Color colorForOne = Color.green;

    public Image easyMedalImage;
    public Image mediumMedalImage;
    public Image hardMedalImage;

    public Sprite bronzeMedal;
    public Sprite silverMedal;
    public Sprite goldMedal;

    public Sprite emptyMedal; // Default empty medal

    void Start()
    {
        countEasy = PlayerPrefs.GetInt("medalEasy", 0);
        countMed = PlayerPrefs.GetInt("medalMed", 0);
        countHard = PlayerPrefs.GetInt("medalHard", 0);
        value = PlayerPrefs.GetInt("IsLight", 0);

        UpdateTextColor();
        UpdateMedalImages();
    }

    void UpdateTextColor()
    {

        MultiProgressBar progressBar = FindObjectOfType<MultiProgressBar>();

        if (tmpText.CompareTag("easyBronze"))
        {
            if (countEasy >= 10)
            {
                tmpText.enabled = false;


            }
            else progressBar.SetProgressBar1(countEasy, 10);
            tmpText.text = countEasy.ToString() + "/10";

        }
        else if (tmpText.CompareTag("mediumBronze"))
        {
            if (countMed >= 10)
            {
                tmpText.enabled = false;

            }
            else progressBar.SetProgressBar2(countMed, 10);
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

            }
            else progressBar.SetProgressBar2(countMed - 10, 30);
            tmpText.text = (countMed - 10).ToString() + "/30";

        }
        else if (tmpText.CompareTag("hardSilver"))
        {
            if (countHard >= 40 || countHard < 10)
            {
                tmpText.enabled = false;

            }
            else progressBar.SetProgressBar3(countHard - 10, 30);
            tmpText.text = (countHard - 10).ToString() + "/30";
        }
        else if (tmpText.CompareTag("easyGold"))
        {
            if (countEasy >= 130 || countEasy < 40)
            {
                tmpText.enabled = false;

            }
            else progressBar.SetProgressBar1(countEasy - 40, 90);
            tmpText.text = (countEasy - 40).ToString() + "/90";
        }
        else if (tmpText.CompareTag("mediumGold"))
        {
            if (countMed >= 130 || countMed < 40)
            {
                tmpText.enabled = false;

            }
            else progressBar.SetProgressBar2(countMed - 40, 90);
            tmpText.text = (countMed - 40).ToString() + "/90";
        }
        else if (tmpText.CompareTag("hardGold"))
        {
            if (countHard >= 130 || countHard < 40)
            {
                tmpText.enabled = false;

            }
            else progressBar.SetProgressBar3(countHard - 40, 90);
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

    void UpdateMedalImages()
    {
        Debug.Log(easyMedalImage.name);
        easyMedalImage.sprite = GetMedalSprite(countEasy);
        mediumMedalImage.sprite = GetMedalSprite(countMed);
        hardMedalImage.sprite = GetMedalSprite(countHard);
    }

    Sprite GetMedalSprite(int count)
    {
        if (count < 10) return bronzeMedal;
        if (count < 40) return silverMedal;
        if (count < 130) return goldMedal;


        else return goldMedal;
    }
}
