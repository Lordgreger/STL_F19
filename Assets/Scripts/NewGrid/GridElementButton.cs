using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridElementButton : MonoBehaviour {

    public PlayerGrid gridController;
    public GridPos pos;
    public int val;
    public bool activated;
    public Material[] numberMaterials;
    public MeshRenderer rendererRef;
    public MeshRenderer selectedRendererRef;

    private void Start() {
        activated = false;
        selectedRendererRef.enabled = false;
    }

    private void OnMouseEnter() {
        Debug.Log("Mouse entered");
        if (Input.GetMouseButton(0)) {
            //setSelected();
            gridController.AddToSelected(this); 
        }
    }

    private void OnMouseDown() {
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
