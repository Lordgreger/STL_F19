using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class GridElementGUI : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler {
    public PlayerGrid gridController; // Ref to controller
    public GridPos pos; // Pos in grid
    public int val; // Current value
    public bool selected; // Is selected
    public bool idle; // When not playing the game
    public Sprite[] numberSprites; // Ref to sprites for numbers
    public Sprite idleSprite; // Ref to idle sprite
    public Image imageRef; // Ref to image
    public TextMeshProUGUI valueText;
    public float selectedDarknessVal; // Value of V in HSV color of image
    public string effect;

    private void Start() {
        //imageRef.sprite = idleSprite;
        effect = "None";
    }

    public void OnPointerEnter(PointerEventData pointerEventData) {
        if (idle) { return; } // Catch for idle
        //Debug.Log("Got enter");

        if (Input.GetMouseButton(0)) {
            gridController.AddToSelected(this);
        }
    }

    public void OnPointerDown(PointerEventData pointerEventData) {
        if (idle) { return; } // Catch for idle
        Debug.Log("Got down");

        gridController.AddToSelected(this);
    }

    #region Public Sets
    // Sets new val and updates graphics
    public void setVal(int i) {
        if (i > 0 && i < 8) { // Valid number
            imageRef.sprite = numberSprites[i - 1];
            val = i;
            valueText.text = val.ToString();
        }
    }

    // Sets new val and resets selected params
    public void setValAndReset(int i) {
        setVal(i);
        resetSelected();
    }

    // Sets element to be idle and updates graphics
    public void setIdle() {
        idle = true;
        selected = false;
        imageRef.sprite = idleSprite;
        resetSelected();
        valueText.enabled = false;
    }

    // Sets elelment to be active and updates graphics
    public void setActive() {
        idle = false;
        setVal(val);
        valueText.enabled = true;
    }
    #endregion

    #region Misc
    public void setSelected() {
        //Debug.Log("Sets selected");
        Color c = Color.HSVToRGB(0, 0, 1);
        //Debug.Log("Color = " + c);
        imageRef.color = Color.HSVToRGB(0, 0, selectedDarknessVal);
    }

    public void resetSelected() {
        //Debug.Log("Resets selected");
        imageRef.color = Color.HSVToRGB(0, 0, 1);
    }

    public void RollEffect() {
        int roll = Random.Range(0, 10);
        if (roll < 0) {
            effect = "Bomb";
        }
        else {
            effect = "None";
        }
    }
    #endregion
}
