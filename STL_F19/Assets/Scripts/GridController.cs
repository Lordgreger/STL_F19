using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GridController : MonoBehaviour {
    public GameObject selectorPrefab;
    public GameObject elementPrefab;
    public Transform elementParent;
    List<GameObject>[] grid = new List<GameObject>[Constants.gridElementsX];

    float elementSize;
    GameObject selectorFollowing = null;
    GameObject selector;
    bool updateSelectorEnabled = false;

    public void setup() {
        selector = Instantiate(selectorPrefab, transform);
        for (int i = 0; i < grid.Length; i++) {
            grid[i] = new List<GameObject>();
        }
        elementSize = elementPrefab.GetComponent<RectTransform>().sizeDelta.x;
        updateSelectorEnabled = true;
    }

    private void Update() {
        if (updateSelectorEnabled) {
            updateSelector();
        }
    }

    public void spawnNewInColumn(int c, int val) {
        GameObject go = Instantiate(elementPrefab, elementParent);
        go.transform.localPosition = new Vector3((c - (Constants.gridElementsX / 2)) * elementSize + (0.5f * c), (elementSize * (Constants.gridElementsY + 2)));
        go.GetComponentInChildren<TextMeshProUGUI>().text = val.ToString();
        grid[c].Add(go);
        print("Added new to " + c);
    }

    public void spawnMulNewInCol(int c, List<int> vals) {
        for (int i = 0; i < vals.Count; i++) {
            GameObject go = Instantiate(elementPrefab, elementParent);
            go.transform.localPosition = new Vector3((c - (Constants.gridElementsX / 2)) * elementSize + (0.5f * c), (elementSize * (Constants.gridElementsY + i + 1)));
            go.GetComponentInChildren<TextMeshProUGUI>().text = vals[i].ToString();
            grid[c].Add(go);
        }
    }

    public void destroyElement(int c, int y) {
        GameObject go = grid[c][y];
        grid[c].RemoveAt(y);
    }

    public void destroyElement(GameGrid.Element e) {
        GameObject go = grid[e.x][e.y];
        grid[e.x].RemoveAt(e.y);
        Destroy(go);
    }

    public void setFollowSelector(int[] selectorPos) {
        selectorFollowing = grid[selectorPos[0]][selectorPos[1]];
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
        grid[e.x][e.y].GetComponent<Image>().color = Color.red;
    }

    public void clearSelected(GameGrid.Element e) {
        grid[e.x][e.y].GetComponent<Image>().color = Color.white;
    }

    public void setLocked(GameGrid.Element e) {
        grid[e.x][e.y].GetComponent<Image>().color = Color.black;
    }

    public void clearLocked(GameGrid.Element e) {
        grid[e.x][e.y].GetComponent<Image>().color = Color.white;
    }

    public void destroyAll() {
        for (int i = 0; i < grid.Length; i++) {
            foreach (var e in grid[i]) {
                Destroy(e);
            }
            grid[i].Clear();
        }
        Destroy(selector);
        updateSelectorEnabled = false;
    }
}
