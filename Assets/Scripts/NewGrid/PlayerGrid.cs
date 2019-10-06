using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerGrid : MonoBehaviour {
    #region Variables
    // Public
    public int gridWidth;
    public int gridHeight;
    public int gridValMax;
    public int gridValMin;
    public int targetValMax;
    public int targetValMin;
    public float gridElementDistance;
    public GameObject gridElementPrefab;
    public TextMeshPro targetText;
    public GameObject[,] elements = new GameObject[3,3];

    // Private
    //Element[,] elementGrid;
    int currentTarget;
    //GameObject[,] elements;
    bool selectorActive;
    int selectorX;
    int selectorY;
    List<GridElementButton> selectedElements;

    // Definitions
    struct Element {
        public int value;
    }

    #endregion

    #region Unity Scheduling
    private void Start() {
        GenerateGrid();
        SetupSelector();
        SetupSelected();
        NewRandomTarget();
    }

    private void Update() {
        HandleMouseRelease();
    }
    #endregion

    #region Setup
    void GenerateGrid() {
        elements = new GameObject[gridWidth, gridHeight];

        float halfTotalSizeX = halfTotalSizeX = (((float)elements.GetLength(0) * (float)gridElementDistance) / 2f) - (gridElementDistance / 2f);
        float halfTotalSizeY = halfTotalSizeY = (((float)elements.GetLength(1) * (float)gridElementDistance) / 2f) - (gridElementDistance / 2f);

        for (int i = 0; i < elements.GetLength(0); i++) {
            for (int j = 0; j < elements.GetLength(1); j++) {
                GameObject go = Instantiate<GameObject>(gridElementPrefab, transform);
                elements[i, j] = go;
                go.transform.position = new Vector3((i * gridElementDistance) - halfTotalSizeX, (j * gridElementDistance) - halfTotalSizeY, 0);

                GridElementButton ge = go.GetComponent<GridElementButton>();
                ge.gridController = this;
                ge.pos = new GridPos(i, j);
                ReRollGridElement(ge);
            }
        }
    }

    void SetupSelector() {
        selectorActive = false;
        selectorX = 0;
        selectorY = 0;
    }

    void SetupSelected() {
        selectedElements = new List<GridElementButton>();
    }

    #endregion

    #region Grid
    /* OLD
    void initElementGrid(Element[,] grid) {
        for (int i = 0; i < grid.GetLength(0); i++) {
            for (int j = 0; j < grid.GetLength(1); j++) {
                grid[i, j] = new Element();
                grid[i, j].value = Random.Range(gridValMin, gridValMax + 1);
            }
        }
    }
    */

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

    #region Selected
    public void AddToSelected(GridElementButton ge) {
        if (ge.activated == false) {
            if (ValidateSelectedCandidate(ge)) {
                selectedElements.Add(ge);
                ge.activated = true;
                ge.setSelected();
            }
        }
    }

    bool ValidateSelectedCandidate(GridElementButton candidate) {
        if (selectedElements.Count == 0) {
            return true;
        }

        foreach (var ge in selectedElements) {
            if (ge.pos.x == candidate.pos.x) { // check y diff
                int diff = ge.pos.y - candidate.pos.y;
                if (diff == 1 || diff == -1) {
                    return true;
                }
            }
            else if (ge.pos.y == candidate.pos.y) { // check x diff
                int diff = ge.pos.x - candidate.pos.x;
                if (diff == 1 || diff == -1) {
                    return true;
                }
            }
        }
        return false;
    }

    void ResetSelected() {
        foreach (var ge in selectedElements) {
            ge.activated = false;
            ge.resetSelected();
        }
        selectedElements.Clear();
    }

    bool CheckSelected() {
        int res = 0;
        foreach (var ge in selectedElements) {
            res += ge.val;
        }

        if (res == currentTarget) {
            return true;
        }
        return false;
    }

    void ReRollSelected() {
        foreach (var ge in selectedElements) {
            ReRollGridElement(ge);
        }
    }

    void ReRollSelectedNewGuaranteed() {
        foreach (var ge in selectedElements) {
            ReRollGridElementNewGuaranteed(ge);
        }
    }

    void ReRollGridElement(GridElementButton ge) {
        ge.setValAndReset(Random.Range(gridValMin, gridValMax + 1));
    }

    void ReRollGridElementNewGuaranteed(GridElementButton ge) {
        int newVal = Random.Range(gridValMin, gridValMax + 1);
        if (ge.val == newVal) {
            ReRollGridElementNewGuaranteed(ge);
        }
        else {
            ge.setValAndReset(newVal);
        }
    }

    #endregion

    #region Target 
    void NewRandomTarget() {
        int newTarget = Random.Range(targetValMin, targetValMax + 1);
        if (newTarget == currentTarget) {
            NewRandomTarget();
        }
        else {
            currentTarget = newTarget;
            targetText.text = currentTarget.ToString();
        }
        
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

    void HandleMouseRelease() {
        if (Input.GetMouseButtonUp(0)) {
            printSelected();
            if (selectedElements.Count > 0) {
                if (CheckSelected()) {
                    ReRollSelectedNewGuaranteed();
                    NewRandomTarget();
                }
                else {

                }
            }
            ResetSelected();
        }
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

    void printSelected() {
        string output = "";
        foreach (var ge in selectedElements) {
            output += " " + ge.val.ToString();
        }
        Debug.Log(output);
    }
    #endregion

}

[System.Serializable]
public struct GridPos {
    public int x, y;
    public GridPos(int x, int y) {
        this.x = x;
        this.y = y;
    }
}
