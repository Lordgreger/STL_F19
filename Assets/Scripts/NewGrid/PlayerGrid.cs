﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrid : MonoBehaviour {
    #region Variables
    // Public
    public int gridWidth;
    public int gridHeight;
    public int gridValMax;
    public int gridValMin;
    public int targetValMax;
    public int targetValMin;

    // Private
    Element[,] elementGrid;
    bool selectorActive;
    int selectorX;
    int selectorY;
    List<Element> selectedElements;

    // Definitions
    struct Element {
        public int value;
    }

    #endregion

    #region Unity Scheduling
    private void Start() {
        SetupGrid();
        SetupSelector();
        SetupSelected();
    }

    private void Update() {
        
    }
    #endregion

    #region Setup
    void SetupGrid() {
        elementGrid = new Element[gridWidth, gridHeight];
        initElementGrid(elementGrid);
        printElementGrid(elementGrid);
    }

    void SetupSelector() {
        selectorActive = false;
        selectorX = 0;
        selectorY = 0;
    }

    void SetupSelected() {
        selectedElements = new List<Element>();
    }

    #endregion

    #region Internal Grid
    void initElementGrid(Element[,] grid) {
        for (int i = 0; i < grid.GetLength(0); i++) {
            for (int j = 0; j < grid.GetLength(1); j++) {
                grid[i, j] = new Element();
                grid[i, j].value = Random.Range(gridValMin, gridValMax + 1);
            }
        }
    }

    #endregion

    #region Selector
    void MoveSelector(string dir) {
        switch (dir) {

            case "Up":
                if (selectorY < gridHeight - 1) {
                    selectorY += 1;
                }
                else {
                    selectorY = gridHeight;
                }
                break;

            case "Down":
                if (selectorY > 1) {
                    selectorY -= 1;
                }
                else {
                    selectorY = 0;
                }
                break;

            case "Right":
                if (selectorX < gridWidth - 1) {
                    selectorX += 1;
                }
                else {
                    selectorX = gridWidth;
                }
                break;

            case "Left":
                if (selectorX > 1) {
                    selectorX -= 1;
                }
                else {
                    selectorX = 0;
                }
                break;

            default:
                break;
        }
    }

    void SetSelectorActive(bool active) {
        selectorActive = active;
    }

    #endregion

    #region Input
    void HandleKeyboardInput() {
        if (Input.GetKeyDown(KeyCode.W)) {
            MoveSelector("Up");
        }
        else if (Input.GetKeyDown(KeyCode.S)) {
            MoveSelector("Down");
        }
        else if (Input.GetKeyDown(KeyCode.A)) {
            MoveSelector("Left");
        }
        else if (Input.GetKeyDown(KeyCode.D)) {
            MoveSelector("Right");
        }

        if (Input.GetKeyDown(KeyCode.L)) {
            SetSelectorActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.L)) {
            SetSelectorActive(false);
        }
    }

    public void gridElementInput(int x, int y) {

    }

    #endregion

    #region Print
    void printElementGrid(Element[,] grid) {
        string output = "";
        for (int i = grid.GetLength(1) - 1; i >= 0; i--) {
            for (int j = 0; j < grid.GetLength(0); j++) {
                output += " " + grid[i, j].value;
            }
            output += "\n";
        }

        Debug.Log(output);
    }
    #endregion

}

public struct GridPos {
    int x, y;
    public GridPos(int x, int y) {
        this.x = x;
        this.y = y;
    }
}
