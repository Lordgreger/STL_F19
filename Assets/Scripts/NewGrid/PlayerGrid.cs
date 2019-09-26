using System.Collections;
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

    // Definitions
    struct Element {
        public int value;
    }

    #endregion

    #region Unity Scheduling
    private void Start() {
        elementGrid = new Element[gridWidth, gridHeight];
        initElementGrid(elementGrid);
        printElementGrid(elementGrid);
    }

    private void Update() {
        
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
