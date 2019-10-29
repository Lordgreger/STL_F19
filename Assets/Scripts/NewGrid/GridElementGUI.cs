using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridElementGUI : MonoBehaviour {
    public PlayerGrid gridController; // Ref to controller
    public GridPos pos; // Pos in grid
    public int val; // Current value
    public bool selected; // Is selected
    public bool idle; // When not playing the game
    public Sprite[] numberSprites; // Ref to sprites for numbers
    public Sprite idleSprite; // Ref to idle sprite
    public Image imageRef; // Ref to image
    public string effect;

    private void Start() {
        imageRef.sprite = idleSprite;
        effect = "None";
    }

    private void OnMouseEnter() {
        if (idle) { return; } // Catch for idle

        if (Input.GetMouseButton(0)) {
            //gridController.AddToSelected(this);
        }
    }

    private void OnMouseDown() {
        if (idle) { return; } // Catch for idle

        //gridController.AddToSelected(this);
    }

    #region Public Sets
    // Sets new val and updates graphics
    public void setVal(int i) {
        if (i > 0 && i < 8) { // Valid number
            imageRef.sprite = numberSprites[i - 1];
            val = i;
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
    }

    // Sets elelment to be active and updates graphics
    public void setActive() {
        idle = false;
        setVal(val);
    }
    #endregion

    #region Misc
    public void setSelected() {
        //selectedRendererRef.enabled = true;
    }

    public void resetSelected() {
        //selectedRendererRef.enabled = false;
    }

    public void RollEffect() {
        int roll = Random.Range(0, 10);
        if (roll < 3) {
            effect = "Bomb";
        }
        else {
            effect = "None";
        }
    }
    #endregion
}
