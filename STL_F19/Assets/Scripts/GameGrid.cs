using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameGrid : MonoBehaviour {
    const float gridDistance = 50.0f;

    GameElement[,] grid = new GameElement[5,10];

    [HideInInspector]
    public UnityEvent loseEvent = new UnityEvent();
    public UnityEvent<List<GameElement>> selectionEvent = new SelectedEvent();
    public GameObject elementPrefab;
    public GameObject selector;
    public Color normalColor;
    public Color selectedColor;
    public int[] selectorPos = new int[2] { 0, 0 };

    public KeyCode upKey;
    public KeyCode downKey;
    public KeyCode leftKey;
    public KeyCode rightKey;
    public KeyCode selectKey;

    [System.Serializable]
    public class SelectedEvent : UnityEvent<List<GameElement>> {

    }

    List<GameElement> currentSelection = new List<GameElement>();

    private void Start() {
        InitGrid();
        setSelectorPos(selectorPos);
        //enableSelection = false;
    }

    private void Update() {
        handleInput();
    }

    #region Selector
    private void handleInput() {

        // Catch for ending selection
        if (Input.GetKeyUp(selectKey)) {
            if (currentSelection.Count != 0) {
                selectionEvent.Invoke(currentSelection);
                clearSelected();
            }
        }

        // Catch for starting selection
        if (Input.GetKeyDown(selectKey)) {
            //print(atSelector().gameObject.activeSelf);
            if (atSelector().gameObject.activeSelf) {
                addToSelected(selectorPos);
            }
        }

        // Handle movement
        if (Input.GetKeyDown(leftKey)) {
            moveSelector(new int[] {-1, 0});
            doSelectionInput();
        }
        else if (Input.GetKeyDown(rightKey)) {
            moveSelector(new int[] {1, 0});
            doSelectionInput();
        }
        else if (Input.GetKeyDown(upKey)) {
            moveSelector(new int[] {0, 1});
            doSelectionInput();
        }
        else if (Input.GetKeyDown(downKey)) {
            moveSelector(new int[] {0, -1});
            doSelectionInput();
        }
        
    }

    private void doSelectionInput() {
        if (Input.GetKey(selectKey)) {
            if (atSelector().gameObject.activeSelf) {
                addToSelected(selectorPos);
            }
        }
        else {
            setSelected(selectorPos);
        }
    }

    private void moveSelector(int[] dir) {
        int[] selectorPrevPos = new int[2];
        selectorPos.CopyTo(selectorPrevPos, 0);

        selectorPos[0] += dir[0];
        selectorPos[1] += dir[1];

        if (selectorPos[0] >= grid.GetLength(0)) {
            selectorPos[0] = grid.GetLength(0) - 1;
        }

        if (selectorPos[0] < 0) {
            selectorPos[0] = 0;
        }

        if (selectorPos[1] >= grid.GetLength(1)) {
            selectorPos[1] = grid.GetLength(1) - 1;
        }

        if (selectorPos[1] < 0) {
            selectorPos[1] = 0;
        }

        if (!grid[selectorPos[0], selectorPos[1]].gameObject.activeSelf && Input.GetKey(selectKey)) {
            selectorPos = selectorPrevPos;
            return;
        }

        setSelectorPos(selectorPos);
    }

    private void setSelectorPos(int[] pos) {
        selector.transform.localPosition = new Vector3((pos[0] - (grid.GetLength(0) / 2)) * gridDistance, pos[1] * gridDistance, 0);
    }

    private void addToSelected(int[] pos) {

        GameElement ge = grid[selectorPos[0], selectorPos[1]];

        if (ge == null) {
            return;
        }

        if (!currentSelection.Contains(ge)) {
            currentSelection.Add(ge);

            if (ge.image == null) {
                return;
            }

            ge.image.color = selectedColor;
        }
    }

    private void setSelected(int[] pos) {
        clearSelected();
        //addToSelected(pos);
    }

    private void clearSelected() {
        //print(currentSelection.Count);
        foreach (var ge in currentSelection) {
            ge.image.color = normalColor;
        }
        currentSelection.Clear();
    }

    private GameElement atSelector() {
        return grid[selectorPos[0], selectorPos[1]];
    }
    #endregion

    #region Init
    void InitGrid() {
        for (int i = 0; i < grid.GetLength(0); i++) {
            for (int j = 0; j < grid.GetLength(1); j++) {
                GameElement ge = Instantiate(elementPrefab, transform).GetComponent<GameElement>();
                grid[i, j] = ge;
                RectTransform rt = ge.transform as RectTransform;
                rt.localPosition = new Vector3((i - (grid.GetLength(0) / 2)) * gridDistance, j * gridDistance, 0);
                ge.gameObject.SetActive(false);
                ge.column = i;
                ge.row = j;
                ge.GetComponent<RawImage>().color = normalColor;
            }
        }
    }
    #endregion

    #region Misc
    public int GetColumnCount() {
        return grid.GetLength(0);
    }

    public int GetRowCount() {
        return grid.GetLength(1);
    }

    public void AddToColumn(int value, int column) {
        for (int i = 0; i < grid.GetLength(1); i++) {
            if (!grid[column, i].gameObject.activeSelf) {
                grid[column, i].gameObject.SetActive(true);
                grid[column, i].SetElement(value);
                return;
            }
        }
        loseEvent.Invoke();
    }

    public void fillGrid() {
        for (int i = 0; i < grid.GetLength(0); i++) {
            for (int j = 0; j < grid.GetLength(1); j++) {
                grid[i, j].gameObject.SetActive(true);
                grid[i, j].SetElement();
            }
        }
    }

    public void removeElements(List<GameElement> elements) {
        List<int> columnsToFix = new List<int>();

        foreach (var e in elements) {
            columnsToFix.Add(e.column);
            e.gameObject.SetActive(false);
        }

        foreach (var c in columnsToFix) {
            fixColumn(c);
            fillColumn(c);
        }
    }

    void fixColumn(int column) {
        for (int i = 1; i < grid.GetLength(1); i++) {
            if (grid[column, i].gameObject.activeSelf && !grid[column, i - 1].gameObject.activeSelf) {
                moveElement(grid[column, i - 1], grid[column, i]);
                fixColumn(column);
                return;
            }
        } 
    }

    void fillColumn(int column) {
        for (int i = 0; i < grid.GetLength(1); i++) {
            if (!grid[column, i].gameObject.activeSelf) {
                grid[column, i].gameObject.SetActive(true);
                grid[column, i].SetElement();
            }
        }
    }

    void moveElement(GameElement l, GameElement r) {
        l.gameObject.SetActive(true);
        l.SetElement(r.value);
        r.gameObject.SetActive(false);
    }

    public void clearGrid() {
        foreach (var ge in grid) {
            ge.gameObject.SetActive(false);
        }
        selectorPos = new int[] { 0, 0 };
        setSelectorPos(selectorPos);
    }

    public void disableColumn(int c, float time) {
        for (int i = 0; i < grid.GetLength(1); i++) {
            grid[c, i].gameObject.SetActive(false);
        }
        StartCoroutine(reenableColumn(c, time));
    }

    IEnumerator reenableColumn(int c, float time) {
        yield return new WaitForSeconds(time);
        fillColumn(c);
    }

    public void disableNumbers(float time)
    {
        int randomNumber = Random.Range(1, 9);
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                if (grid[i,j].value == randomNumber)
                {
                    Debug.Log("The remove number: " + randomNumber);
                    grid[i, j].gameObject.SetActive(false);
                }
            }
            fixColumn(i);
            StartCoroutine(reenableNumbers(time));
        }
    }

    IEnumerator reenableNumbers(float time)
    {
        yield return new WaitForSeconds(time);
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            fillColumn(i);
        }
        
    }

    #endregion
}