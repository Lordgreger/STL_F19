using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridElement : MonoBehaviour {
    public int value;
    public int column;
    public int row;

    public void setNewRandom() {
        value = Random.Range(Constants.elementValueMin, Constants.elementValueMax + 1);
    }
}
