using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class SwitchHandler : MonoBehaviour
{
    private bool isMusicOn = true; // Initially music is on
    public GameObject switchBtn;
    public AudioSource audioSource; // Reference to your AudioSource component
    private int switchState = 1;

    public void OnSwitchButtonClicked()
    {
        // Toggle the music state
        isMusicOn = !isMusicOn;

        // Move the switch button
        switchBtn.transform.DOLocalMoveX(-switchBtn.transform.localPosition.x, 0.2f);
        switchState = Math.Sign(-switchBtn.transform.localPosition.x);

        // Toggle the audio
        audioSource.mute = !isMusicOn;
    }
}
