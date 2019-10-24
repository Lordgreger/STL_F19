using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

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
    public float gridElementSize;
    public GameObject gridElementPrefab;
    public TextMeshProUGUI targetText;
    public GridElementButton[,] elements = new GridElementButton[3,3];
    public IntEvent scored = new IntEvent();

    // Private
    int currentTarget;
    List<GridElementButton> selectedElements;
    

    // Definitions
    struct Element {
        public int value;
    }

    [System.Serializable]
    public class IntEvent : UnityEvent<int> {}

    #endregion

    #region Unity Scheduling
    private void Start() {
        GenerateGrid();
        //SetupSelected();
        //SetTargetIdle();
        //SetGridIdle();

        // Test grid
        //StartNewGame();
    }

    private void Update() {
        HandleMouseRelease();
    }
    #endregion

    #region Control
    public void StartNewGame() {
        SetupNewStartGrid();
        NewRandomTarget();
    }

    public void EndGame() {
        SetGridIdle();
        SetTargetIdle();
    }
    #endregion

    #region Setup
    void GenerateGrid() {
        elements = new GridElementButton[gridWidth, gridHeight];

        float halfTotalSizeX = halfTotalSizeX = (((float)elements.GetLength(0) * (float)gridElementDistance) / 2f) - (gridElementDistance / 2f);
        float halfTotalSizeY = halfTotalSizeY = (((float)elements.GetLength(1) * (float)gridElementDistance) / 2f) - (gridElementDistance / 2f);

        for (int i = 0; i < elements.GetLength(0); i++) {
            for (int j = 0; j < elements.GetLength(1); j++) {
                GameObject go = Instantiate<GameObject>(gridElementPrefab, transform);
                go.transform.localPosition = new Vector3(((i * gridElementDistance) - halfTotalSizeX) * gridElementSize, ((j * gridElementDistance) - halfTotalSizeY) * gridElementSize, 0);

                //GridElementButton ge = go.GetComponent<GridElementButton>();
                //elements[i, j] = ge;
                //ge.gridController = this;
                //ge.pos = new GridPos(i, j);
                //ReRollGridElement(ge);
            }
        }
    }

    void SetupSelected() {
        selectedElements = new List<GridElementButton>();
    }

    #endregion

    #region Grid
    void SetGridIdle() {
        foreach (var ge in elements) {
            ge.setIdle();
        }
    }

    void SetGridActive() {
        foreach (var ge in elements) {
            ge.setActive();
        }
    }

    void SetupNewStartGrid() {
        foreach (var ge in elements) {
            ge.setActive();
            ReRollGridElement(ge);
        }
    }

    #endregion

    #region Selected
    public bool AddToSelected(GridElementButton ge) {
        if (ge.selected == false) {
            if (ValidateSelectedCandidate(ge)) {
                selectedElements.Add(ge);
                ge.selected = true;
                ge.setSelected();
                return true;
            }
        }
        return false;
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
            ge.selected = false;
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
        ge.RollEffect();
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

    void SetTargetIdle() {
        targetText.text = "";
    }
    #endregion

    #region Input
    void HandleMouseRelease() {
        if (Input.GetMouseButtonUp(0)) {
            printSelected();
            if (selectedElements.Count > 0) {
                if (CheckSelected()) {
                    CorrectSelected();
                }
                else {

                }
            }
            ResetSelected();
        }
    }

    class CorrectInfo {
        public int elementCount;
        public int scoreModifier;
    }

    void CorrectSelected() {
        CorrectInfo info = new CorrectInfo();
        info.elementCount = selectedElements.Count;
        info.scoreModifier = 1;

        int i = 0;
        while (selectedElements.Count > i) {
            GridElementButton ge = selectedElements[i];
            ApplyElementEffect(ge, info);
            i++;
        }
        scored.Invoke(info.elementCount * info.scoreModifier);

        ReRollSelectedNewGuaranteed();
        NewRandomTarget();
        scored.Invoke(selectedElements.Count);
    }

    #endregion

    #region Element Effect
    void ApplyElementEffect(GridElementButton ge, CorrectInfo info) {
        switch (ge.effect) {
            case "Bomb":
                ApplyBombEffect(ge, info);
                break;

            case "None":
                break;

            default:
                break;
        }

    }

    void ApplyBombEffect(GridElementButton ge, CorrectInfo info) {
        Debug.Log("Applied bomb effect");
        int addedBlocks = 0;

        if (ge.pos.x > 0) {
            if (ge.pos.y > 0) { if (AddToSelected(elements[ge.pos.x - 1, ge.pos.y - 1])) { addedBlocks++; } }
            if (AddToSelected(elements[ge.pos.x - 1, ge.pos.y])) { addedBlocks++; }
            if (ge.pos.y < gridWidth - 1) { if (AddToSelected(elements[ge.pos.x - 1, ge.pos.y + 1])) { addedBlocks++; } }
        }

        if (ge.pos.y > 0) { if (AddToSelected(elements[ge.pos.x, ge.pos.y - 1])) { addedBlocks++; } }
        if (ge.pos.y < gridWidth - 1) { if (AddToSelected(elements[ge.pos.x, ge.pos.y + 1])) { addedBlocks++; } }

        if (ge.pos.x < gridHeight - 1) {
            if (ge.pos.y > 0) { if (AddToSelected(elements[ge.pos.x + 1, ge.pos.y - 1])) { addedBlocks++; } }
            if (AddToSelected(elements[ge.pos.x + 1, ge.pos.y])) { addedBlocks++; }
            if (ge.pos.y < gridWidth - 1) { if (AddToSelected(elements[ge.pos.x + 1, ge.pos.y + 1])) { addedBlocks++; } }
        }

        info.elementCount += addedBlocks;
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
