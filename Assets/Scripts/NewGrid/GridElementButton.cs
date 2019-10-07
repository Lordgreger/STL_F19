using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridElementButton : MonoBehaviour {

    public PlayerGrid gridController;
    public GridPos pos;
    public int val;
    public bool activated; // When selected
    public bool idle; // When not playing the game
    public Material[] numberMaterials;
    public Material idleMaterial;
    public MeshRenderer rendererRef;
    public MeshRenderer selectedRendererRef;

    private void Start() {
        setIdle();
        selectedRendererRef.enabled = false;
    }

    private void OnMouseEnter() {
        if (idle) { return; } // Catch for idle

        Debug.Log("Mouse entered");
        if (Input.GetMouseButton(0)) {
            //setSelected();
            gridController.AddToSelected(this); 
        }
    }

    private void OnMouseDown() {
        if (idle) { return; } // Catch for idle

        Debug.Log("Mouse down object");
        //setSelected();
        gridController.AddToSelected(this);
    }

    #region Public Sets
    public void setVal(int i) {
        if (i > 0 && i < 8) { // Valid number
            rendererRef.material = numberMaterials[i - 1];
            val = i;
        }
    }

    public void setValAndReset(int i) {
        setVal(i);
        resetSelected();
    }

    public void setIdle() {
        idle = true;
        activated = false;
        rendererRef.material = idleMaterial;
        resetSelected();
    }

    public void setActive() {
        idle = false;
        setVal(val);
    }

    #endregion

    #region Misc
    public void setSelected() {
        selectedRendererRef.enabled = true;
    }

    public void resetSelected() {
        selectedRendererRef.enabled = false;
    }
    #endregion
}