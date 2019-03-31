using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GridController : MonoBehaviour {
    public GameObject selectorPrefab;
    public GameObject elementPrefab;
    public Transform elementParent;
    public Sprite[] sprites = new Sprite[9];
    GameObject[,] grid = new GameObject[Constants.gridElementsX, Constants.gridElementsY];

    float elementSize;
    GameObject selectorFollowing = null;
    GameObject selector;
    bool updateSelectorEnabled = false;

    public void setup() {
        selector = Instantiate(selectorPrefab, transform);
        elementSize = elementPrefab.GetComponent<RectTransform>().sizeDelta.x;
        updateSelectorEnabled = true;
        setGridNull();
    }

    private void Update() {
        if (updateSelectorEnabled) {
            updateSelector();
        }
    }

    void setGridNull() {
        for (int i = 0; i < grid.GetLength(0); i++) {
            for (int j = 0; j < grid.GetLength(0); j++) {
                grid[i, j] = null;
            }
        }
    }

    public void spawnMulNewInCol(int c, List<int> vals) {
        for (int i = 0; i < vals.Count; i++) {
            GameObject go = Instantiate(elementPrefab, elementParent);
            go.transform.localPosition = new Vector3((c - (Constants.gridElementsX / 2)) * elementSize + (1f * c), 200f + (elementSize * 1.2f * (Constants.gridElementsY + i + 1)));
            go.GetComponent<Image>().sprite = sprites[vals[i] - 1];
            if (!addToGrid(c, go)) {
                Destroy(go);
                Debug.Log("ERROR CREATING NEW BLOCK IN " + c);
            }
        }
    }

    bool addToGrid(int c, GameObject go) {
        for (int i = 0; i < grid.GetLength(1); i++) {
            if (grid[c, i] == null) {
                grid[c, i] = go;
                return true;
            }
        }
        return false;
    }

    void removeFromGrid(int x, int y) {
        for (int i = y + 1; i < grid.GetLength(1); i++) {
            if (grid[x, i] == null) {
                grid[x, i - 1] = null;
            }
            else {
                grid[x, i - 1] = grid[x, i];
            } 
        }

        grid[x, grid.GetLength(1) - 1] = null;
    }

    public void fixCols() {
        for (int i = 0; i < grid.GetLength(0); i++) {
            for (int j = 1; j < grid.GetLength(1); j++) {
                if (grid[i, j] != null) {
                    if (grid[i, j - 1] == null) {
                        grid[i, j - 1] = grid[i, j];
                        grid[i, j] = null;
                        fixCols();
                        return;
                    }
                }
            }
        }
    }

    public void destroyElement(GameGrid.Element e) {
        GameObject go = grid[e.x, e.y];
        grid[e.x, e.y] = null;
        go.GetComponent<Image>().enabled = false;
        GridElementVisuals ev = go.GetComponent<GridElementVisuals>();
        ev.StartDestructionAnimation();
        Destroy(go, ev.getDestructionTime());
    }

    public void destroyElement(int x, int y) {
        GameObject go = grid[x, y];
        grid[x, y] = null;
        go.GetComponent<Image>().enabled = false;
        GridElementVisuals ev = go.GetComponent<GridElementVisuals>();
        ev.StartDestructionAnimation();
        Destroy(go, ev.getDestructionTime());
    }

    public void setFollowSelector(int[] selectorPos) {
        selectorFollowing = grid[selectorPos[0], selectorPos[1]];
    }

    public void updateSelector() {
        if (selectorFollowing != null) {
            selector.transform.localPosition = selectorFollowing.transform.localPosition;
        }
        else {
            selector.transform.position = new Vector3(-1000, -1000);
        }
    }

    public void setSelected(GameGrid.Element e) {
        grid[e.x, e.y].GetComponent<GridElementVisuals>().setSelected(true);
    }

    public void clearSelected(GameGrid.Element e) {
        grid[e.x, e.y].GetComponent<GridElementVisuals>().setSelected(false);
    }

    public void setLocked(GameGrid.Element e) {
        grid[e.x, e.y].GetComponent<GridElementVisuals>().setLocked(true);
    }

    public void clearLocked(GameGrid.Element e) {
        grid[e.x, e.y].GetComponent<GridElementVisuals>().setLocked(false);
    }

    public void destroyAll() {
        for (int i = 0; i < grid.GetLength(0); i++) {
            for (int j = 0; j < grid.GetLength(1); j++) {
                destroyElement(i, j);
                grid[i, j] = null;
            }
        }
        Destroy(selector);
        updateSelectorEnabled = false;
    }
}
