using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameGrid : MonoBehaviour {
    const float gridDistance = 50.0f;

    GameElement[,] grid = new GameElement[6,8];

    [HideInInspector]
    public UnityEvent loseEvent = new UnityEvent();
    public UnityEvent<List<GameElement>> selectionEvent = new SelectedEvent();
    public GameObject elementPrefab;
    public GameObject selector;
    public Color normalColor;
    public Color selectedColor;
    public int[] selectorPos = new int[2] { 0, 0 };

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

    private void handleInput() {  
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            moveSelector(new int[] {-1, 0});
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            moveSelector(new int[] {1, 0});
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow)) {
            moveSelector(new int[] {0, 1});
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            moveSelector(new int[] {0, -1});
        }

        if(Input.GetKey(KeyCode.A)) {
            
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

        //if (!(selectorPos[0] == selectorPrevPos[0] && selectorPos[1] == selectorPrevPos[1])) {
        //grid[selectorPos[0], selectorPos[1]].GetComponent<RawImage>().color = mouseOverColor;
        //grid[selectorPrevPos[0], selectorPrevPos[1]].GetComponent<RawImage>().color = normalColor;
        //}
        setSelectorPos(selectorPos);
    }

    private void setSelectorPos(int[] pos) {
        selector.transform.localPosition = new Vector3((pos[0] - (grid.GetLength(0) / 2)) * gridDistance, pos[1] * gridDistance, 0);
    }

    private void addToSelected(int[] pos) {
        currentSelection.Add(grid[selectorPos[0], selectorPos[1]]);
    }

    private void setSelected(int[] pos) {
        currentSelection.Clear();
        addToSelected(pos);
    }

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

    public void removeElements(List<GameElement> elements) {
        List<int> columnsToFix = new List<int>();

        foreach (var e in elements) {
            columnsToFix.Add(e.column);
            e.gameObject.SetActive(false);
        }

        foreach (var c in columnsToFix) {
            fixColumn(c);
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

    void moveElement(GameElement l, GameElement r) {
        l.gameObject.SetActive(true);
        l.SetElement(r.value);
        r.gameObject.SetActive(false);
    }

    #endregion
}