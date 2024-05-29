using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GridSquare : MonoBehaviour, IPointerClickHandler
{
    public GameObject number_text;
    private int number_ = 0;
    private sudokuGrid grid;
    private RawImage squareRawImage; // Reference to the RawImage component
    public TextMeshProUGUI textMeshProComponent;
    // Define variables to hold the textures
    public Texture selectedTexture;
    private Texture originalTexture;

    void Start()
    {
        grid = FindObjectOfType<sudokuGrid>();
        squareRawImage = GetComponentInChildren<RawImage>(); // Get the RawImage component from children

        // Store the original texture
        originalTexture = squareRawImage.texture;
    }

    void Update()
    {

    }
    public void ChangeTextColor(Color color)
    {
        textMeshProComponent.color = color; // Assuming textMeshProComponent is the reference to your TextMeshPro component
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        // Handle grid square click
        grid.SelectGridSquare(this);
    }

    public void DisplayText()
    {
        if (number_ <= 0)
            number_text.GetComponent<TextMeshProUGUI>().text = "";
        else
            number_text.GetComponent<TextMeshProUGUI>().text = number_.ToString();
    }

    public void Select()
    {
        // Change the texture of the square when selected
        squareRawImage.texture = selectedTexture;
    }

    public void Deselect()
    {
        // Restore the original texture when deselected
        squareRawImage.texture = originalTexture;
    }

    public void SetNumber(int number)
    {
        if (number_ != number) number_ = number;
        else number_ = 0;
        DisplayText();
    }
    public string GetNumber()
    {
        return number_text.GetComponent<TextMeshProUGUI>().text;
    }
}
