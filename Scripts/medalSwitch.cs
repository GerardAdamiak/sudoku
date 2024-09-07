using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;

public class MedalSwitch : MonoBehaviour
{

    public int value; // The int value to check (0 or 1)
    private int countEasy;
    private int countMed;
    private int countHard;
    // Define the colors

    public Sprite doneMedalDark;
    public Sprite doneMedalLight;
    public Sprite MedalDark;
    public Sprite MedalLight;
    private SpriteRenderer spriteRenderer;
    public GameObject medalImage;

    // Start is called before the first frame update
    void Start()
    {
        countEasy = PlayerPrefs.GetInt("medalEasy");
        countMed = PlayerPrefs.GetInt("medalMed");
        countHard = PlayerPrefs.GetInt("medalHard");
        value = PlayerPrefs.GetInt("IsLight");
        UpdateMedal();

    }

    // Update is called once per frame
   




    
    void UpdateMedal()
    {

        spriteRenderer = GetComponent<SpriteRenderer>();

        // Set the initial sprite based on the saved PlayerPrefs value
        
        

        if (medalImage.CompareTag("easyBronze"))
        {
            
            if (countEasy > 10)
            {
                
                if (value == 1)
                {
                    spriteRenderer.sprite = doneMedalLight;
                }
                else spriteRenderer.sprite = doneMedalDark;

            }
            else
            {
                if (value == 1)
                {
                    spriteRenderer.sprite = MedalLight;
                }
                else spriteRenderer.sprite = MedalDark;
            }


        }
        else if (medalImage.CompareTag("mediumBronze"))
        {
            if (countMed > 10)
            {

                if (value == 1)
                {
                    spriteRenderer.sprite = doneMedalLight;
                }
                else spriteRenderer.sprite = doneMedalDark;

            }
            else
            {
                if (value == 1)
                {
                    spriteRenderer.sprite = MedalLight;
                }
                else spriteRenderer.sprite = MedalDark;
            }

        }
        else if (medalImage.CompareTag("hardBronze"))
        {
            if (countHard > 10)
            {

                if (value == 1)
                {
                    spriteRenderer.sprite = doneMedalLight;
                }
                else spriteRenderer.sprite = doneMedalDark;

            }
            else
            {
                if (value == 1)
                {
                    spriteRenderer.sprite = MedalLight;
                }
                else spriteRenderer.sprite = MedalDark;
            }


        }
        else if (medalImage.CompareTag("easySilver"))
        {
            if (countEasy > 30)
            {

                if (value == 1)
                {
                    spriteRenderer.sprite = doneMedalLight;
                }
                else spriteRenderer.sprite = doneMedalDark;

            }
            else
            {
                if (value == 1)
                {
                    spriteRenderer.sprite = MedalLight;
                }
                else spriteRenderer.sprite = MedalDark;
            }
        }
        else if (medalImage.CompareTag("mediumSilver"))
        {
            if (countMed > 30)
            {

                if (value == 1)
                {
                    spriteRenderer.sprite = doneMedalLight;
                }
                else spriteRenderer.sprite = doneMedalDark;

            }
            else
            {
                if (value == 1)
                {
                    spriteRenderer.sprite = MedalLight;
                }
                else spriteRenderer.sprite = MedalDark;
            }

        }
        else if (medalImage.CompareTag("hardSilver"))
        {
            if (countHard > 30)
            {

                if (value == 1)
                {
                    spriteRenderer.sprite = doneMedalLight;
                }
                else spriteRenderer.sprite = doneMedalDark;

            }
            else
            {
                if (value == 1)
                {
                    spriteRenderer.sprite = MedalLight;
                }
                else spriteRenderer.sprite = MedalDark;
            }

        }
        else if (medalImage.CompareTag("easyGold"))
        {
            if (countEasy > 90)
            {

                if (value == 1)
                {
                    spriteRenderer.sprite = doneMedalLight;
                }
                else spriteRenderer.sprite = doneMedalDark;

            }
            else
            {
                if (value == 1)
                {
                    spriteRenderer.sprite = MedalLight;
                }
                else spriteRenderer.sprite = MedalDark;
            }

        }
        else if (medalImage.CompareTag("mediumGold"))
        {
            if (countMed > 90)
            {

                if (value == 1)
                {
                    spriteRenderer.sprite = doneMedalLight;
                }
                else spriteRenderer.sprite = doneMedalDark;

            }
            else
            {
                if (value == 1)
                {
                    spriteRenderer.sprite = MedalLight;
                }
                else spriteRenderer.sprite = MedalDark;
            }

        }
        else if (medalImage.CompareTag("hardGold"))
        {
            if (countHard > 90)
            {

                if (value == 1)
                {
                    spriteRenderer.sprite = doneMedalLight;
                }
                else spriteRenderer.sprite = doneMedalDark;

            }
            else
            {
                if (value == 1)
                {
                    spriteRenderer.sprite = MedalLight;
                }
                else spriteRenderer.sprite = MedalDark;
            }

        }

        // Check the value and update the TMP text color
        
    }

    // Optional: You can update the color dynamically through this method
    

}
