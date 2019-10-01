using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridElementButton : MonoBehaviour {

    public PlayerGrid gridController;
    public GridPos pos;
    public int val;
    public bool activated;

    private void Start() {
        activated = false;
    }

    private void OnMouseEnter() {
        Debug.Log("Mouse entered");
        if (Input.GetMouseButton(0)) {
            gridController.AddToSelected(this); 
        }
    }

    private void OnMouseDown() {
        Debug.Log("Mouse down object");
        gridController.AddToSelected(this);
    }
}
