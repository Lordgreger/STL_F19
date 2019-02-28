using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameGrid : MonoBehaviour {
    const float gridDistance = 50.0f;

    GameElement[,] grid = new GameElement[6,8];

    public UnityEvent loseEvent = new UnityEvent();
    public UnityEvent<List<GameElement>> selectionEvent = new SelectedEvent();
    public GameObject elementPrefab;

    [System.Serializable]
    public class SelectedEvent : UnityEvent<List<GameElement>> {

    }

    public bool enableSelection;

    State state;
    List<GameElement> currentSelection = new List<GameElement>();
    
    public void onElementSelect(GameElement ge) {
        if (!enableSelection)
            return;

        if (state == State.selecting || Input.GetMouseButtonDown(0)) {
            currentSelection.Add(ge);
            //Debug.Log("Added element with value: " + ge.value);
        }
    }

    private void Awake() {
        InitGrid();
        enableSelection = false;
    }

    private void Update() {
        updateState();
    }

    #region State
    void updateState() {
        if (Input.GetMouseButtonDown(0)) {
            state = State.selecting;
        }
        else if (Input.GetMouseButtonUp(0)) {
            if (currentSelection.Count == 0)
                return;

            selectionEvent.Invoke(currentSelection);
            //print(currentSelection.Count);
            currentSelection.Clear();
            state = State.looking;
        }
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
                ge.mouseEnter.AddListener(onElementSelect);
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

    enum State {
        looking,
        selecting
    }
    #endregion
}
