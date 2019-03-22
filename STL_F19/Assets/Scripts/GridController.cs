using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour {
    public GameObject elementPrefab;
    List<GameObject>[] grid = new List<GameObject>[Constants.gridElementsX];

    float elementSize;

    private void Start() {
        elementSize = elementPrefab.GetComponent<RectTransform>().sizeDelta.x;
    }

    public void spawnNewInColumn(int c) {
        GameObject go = Instantiate(elementPrefab, transform);
        go.transform.position = new Vector3(c * elementSize, this.transform.position.y + 200);
        grid[c].Add(go);
    }

    public void destroyElement(int c, int y) {
        GameObject go = grid[c][y];
        grid[c].RemoveAt(y);

    }

}
