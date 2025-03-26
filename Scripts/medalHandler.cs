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
    private int countRenban;
    private int countKiller;
    private int countKropki;
    private int countThermo;
    private int countWhispers;


    public Color colorForZero = Color.red;
    public Color colorForOne = Color.green;

    public Image easyMedalImage;
    public Image mediumMedalImage;
    public Image hardMedalImage;
    public Image renbanMedalImage;
    public Image kropkiMedalImage;
    public Image thermoMedalImage;
    public Image whispersMedalImage;
    public Image killerMedalImage;

    public Sprite bronzeMedal;
    public Sprite silverMedal;
    public Sprite goldMedal;

    public Sprite emptyMedal; // Default empty medal

    void Start()
    {
        countEasy = PlayerPrefs.GetInt("medalEasy", 0);
        countMed = PlayerPrefs.GetInt("medalMed", 0);
        countHard = PlayerPrefs.GetInt("medalHard", 0);
        countKropki = PlayerPrefs.GetInt("medalKropki", 0);
        countWhispers = PlayerPrefs.GetInt("medalWhispers", 0);
        countKiller = PlayerPrefs.GetInt("medalKiller", 0);
        countRenban = PlayerPrefs.GetInt("medalRenban", 0);
        countThermo = PlayerPrefs.GetInt("medalThermo", 0);


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
        else if (tmpText.CompareTag("whispersBronze"))
        {
            if (countWhispers >= 10)
            {
                tmpText.enabled = false;

            }
            else progressBar.SetProgressBar4(countWhispers, 10);

            tmpText.text = countWhispers.ToString() + "/10";

        }
        else if (tmpText.CompareTag("kropkiBronze"))
        {
            if (countKropki >= 10)
            {
                tmpText.enabled = false;

            }
            else progressBar.SetProgressBar5(countKropki, 10);

            tmpText.text = countKropki.ToString() + "/10";

        }
        else if (tmpText.CompareTag("renbanBronze"))
        {
            if (countRenban >= 10)
            {
                tmpText.enabled = false;

            }
            else progressBar.SetProgressBar6(countRenban, 10);

            tmpText.text = countRenban.ToString() + "/10";

        }
        else if (tmpText.CompareTag("killerBronze"))
        {
            if (countKiller >= 10)
            {
                tmpText.enabled = false;

            }
            else progressBar.SetProgressBar7(countKiller, 10);

            tmpText.text = countKiller.ToString() + "/10";

        }
        else if (tmpText.CompareTag("thermoBronze"))
        {
            if (countThermo >= 10)
            {
                tmpText.enabled = false;

            }
            else progressBar.SetProgressBar8(countThermo, 10);

            tmpText.text = countThermo.ToString() + "/10";

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
        else if (tmpText.CompareTag("whispersSilver"))
        {
            if (countWhispers >= 40 || countWhispers < 10)
            {
                tmpText.enabled = false;

            }
            else progressBar.SetProgressBar4(countWhispers - 10, 30);
            tmpText.text = (countWhispers - 10).ToString() + "/30";
        }
        else if (tmpText.CompareTag("kropkiSilver"))
        {
            if (countKropki >= 40 || countKropki < 10)
            {
                tmpText.enabled = false;

            }
            else progressBar.SetProgressBar5(countKropki - 10, 30);
            tmpText.text = (countKropki - 10).ToString() + "/30";
        }
        else if (tmpText.CompareTag("renbanSilver"))
        {
            if (countRenban >= 40 || countRenban < 10)
            {
                tmpText.enabled = false;

            }
            else progressBar.SetProgressBar6(countRenban - 10, 30);
            tmpText.text = (countRenban - 10).ToString() + "/30";
        }
        else if (tmpText.CompareTag("killerSilver"))
        {
            if (countKiller >= 40 || countKiller < 10)
            {
                tmpText.enabled = false;

            }
            else progressBar.SetProgressBar7(countKiller - 10, 30);
            tmpText.text = (countKiller - 10).ToString() + "/30";
        }
        else if (tmpText.CompareTag("thermoSilver"))
        {
            if (countThermo >= 40 || countThermo < 10)
            {
                tmpText.enabled = false;

            }
            else progressBar.SetProgressBar8(countThermo - 10, 30);
            tmpText.text = (countThermo - 10).ToString() + "/30";
        }
        else if (tmpText.CompareTag("easyGold"))
        {
            if (countEasy >= 130 || countEasy < 40)
            {
                tmpText.enabled = false;

            }
            else progressBar.SetProgressBar1(countEasy - 40, 90);
            tmpText.text = (countEasy - 40).ToString() + "/90";
            if (countEasy >= 130)
            {
                progressBar.SetProgressBar1(90, 90);

            }
        }
        else if (tmpText.CompareTag("mediumGold"))
        {
            if (countMed >= 130 || countMed < 40)
            {
                tmpText.enabled = false;

            }
            else progressBar.SetProgressBar2(countMed - 40, 90);
            tmpText.text = (countMed - 40).ToString() + "/90";
            if (countMed >= 130)
            {
                progressBar.SetProgressBar2(90, 90);

            }
        }
        else if (tmpText.CompareTag("hardGold"))
        {
            if (countHard >= 130 || countHard < 40)
            {
                tmpText.enabled = false;

            }
            else progressBar.SetProgressBar3(countHard - 40, 90);
            tmpText.text = (countHard - 40).ToString() + "/90";
            if (countHard >= 130 )
            {
                progressBar.SetProgressBar3(90, 90);

            }
        }
        else if (tmpText.CompareTag("whispersGold"))
        {
            if (countWhispers >= 130 || countWhispers < 40)
            {
                tmpText.enabled = false;

            }
            else progressBar.SetProgressBar4(countWhispers - 40, 90);
            tmpText.text = (countWhispers - 40).ToString() + "/90";
            if (countWhispers >= 130)
            {
                progressBar.SetProgressBar4(90, 90);

            }
        }
        else if (tmpText.CompareTag("kropkiGold"))
        {
            if (countKropki >= 130 || countKropki < 40)
            {
                tmpText.enabled = false;

            }
            else progressBar.SetProgressBar5(countKropki - 40, 90);
            tmpText.text = (countKropki - 40).ToString() + "/90";
            if (countKropki >= 130)
            {
                progressBar.SetProgressBar5(90, 90);

            }
        }
        else if (tmpText.CompareTag("renbanGold"))
        {
            if (countRenban >= 130 || countRenban < 40)
            {
                tmpText.enabled = false;

            }
            else progressBar.SetProgressBar6(countRenban - 40, 90);
            tmpText.text = (countRenban - 40).ToString() + "/90";
            if (countRenban >= 130)
            {
                progressBar.SetProgressBar6(90, 90);

            }
        }
        else if (tmpText.CompareTag("killerGold"))
        {
            if (countKiller >= 130 || countKiller < 40)
            {
                tmpText.enabled = false;

            }
            else progressBar.SetProgressBar7(countKiller - 40, 90);
            tmpText.text = (countKiller - 40).ToString() + "/90";
            if (countKiller >= 130)
            {
                progressBar.SetProgressBar7(90, 90);

            }
        }
        else if (tmpText.CompareTag("thermoGold"))
        {
            if (countThermo >= 130 || countThermo < 40)
            {
                tmpText.enabled = false;

            }
            else progressBar.SetProgressBar8(countThermo - 40, 90);
            tmpText.text = (countThermo - 40).ToString() + "/90";
            if (countThermo >= 130)
            {
                progressBar.SetProgressBar8(90, 90);

            }
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
        
        easyMedalImage.sprite = GetMedalSprite(countEasy);
        mediumMedalImage.sprite = GetMedalSprite(countMed);
        hardMedalImage.sprite = GetMedalSprite(countHard);
        kropkiMedalImage.sprite = GetMedalSprite(countKropki);
        thermoMedalImage.sprite = GetMedalSprite(countThermo);
        whispersMedalImage.sprite = GetMedalSprite(countWhispers);
        killerMedalImage.sprite = GetMedalSprite(countKiller);
        renbanMedalImage.sprite = GetMedalSprite(countRenban);
    }

    Sprite GetMedalSprite(int count)
    {
        if (count < 10) return emptyMedal;
        if (count>=10 && count < 40) return bronzeMedal;
        if (count >= 40 && count < 130) return silverMedal;


        else return goldMedal;
    }
}
