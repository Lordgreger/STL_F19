using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SinglePlayerTutorialController : MonoBehaviour {

    public int gridWidth;
    public int gridHeight;
    public int gridValMax;
    public int gridValMin;
    public float gridElementDistance;
    public float gridElementSize;
    public GameObject gridElementPrefab;
    public TextMeshProUGUI targetText;
    public Animator targetAnimator;

    int currentTarget;
    public GridElementGUI[,] elements;
    List<GridElementGUI> selectedElements;

    TutorialState currentState;

    #region Unity
    private void Start() {
        GenerateGrid();
        SetupSelected();
        currentState = new TutorialState1(this);
        currentState.Enter();
    }

    private void Update() {
        //HandleMouseRelease();
        currentState.Update();
    }

    #endregion

    #region Setup
    void GenerateGrid() {
        elements = new GridElementGUI[gridWidth, gridHeight];

        float halfTotalSizeX = halfTotalSizeX = (((float)elements.GetLength(0) * (float)gridElementDistance) / 2f) - (gridElementDistance / 2f);
        float halfTotalSizeY = halfTotalSizeY = (((float)elements.GetLength(1) * (float)gridElementDistance) / 2f) - (gridElementDistance / 2f);

        for (int i = 0; i < elements.GetLength(0); i++) {
            for (int j = 0; j < elements.GetLength(1); j++) {
                GameObject go = Instantiate<GameObject>(gridElementPrefab, transform);
                go.transform.localPosition = new Vector3(((i * gridElementDistance) - halfTotalSizeX) * gridElementSize, ((j * gridElementDistance) - halfTotalSizeY) * gridElementSize, 0);

                GridElementGUI ge = go.GetComponent<GridElementGUI>();
                elements[i, j] = ge;
                ge.sptcRef = this;
                ge.pos = new GridPos(i, j);
                ReRollGridElementNoEffect(ge);
            }
        }
    }

    void SetupSelected() {
        selectedElements = new List<GridElementGUI>();
    }
    #endregion

    #region Selection
    public bool AddToSelected(GridElementGUI ge) {
        if (ge.selected == false) {
            if (ValidateSelectedCandidate(ge)) {
                selectedElements.Add(ge);
                ge.selected = true;
                ge.setSelected();
                return true;
            }
        }
        return false;
    }

    bool ValidateSelectedCandidate(GridElementGUI candidate) {
        if (selectedElements.Count == 0) {
            return true;
        }

        foreach (var ge in selectedElements) {
            if (ge.pos.x == candidate.pos.x) { // check y diff
                int diff = ge.pos.y - candidate.pos.y;
                if (diff == 1 || diff == -1) {
                    return true;
                }
            }
            else if (ge.pos.y == candidate.pos.y) { // check x diff
                int diff = ge.pos.x - candidate.pos.x;
                if (diff == 1 || diff == -1) {
                    return true;
                }
            }
        }
        return false;
    }

    void ResetSelected() {
        foreach (var ge in selectedElements) {
            ge.selected = false;
            ge.resetSelected();
        }
        selectedElements.Clear();
    }

    bool CheckSelected() {
        int res = 0;
        foreach (var ge in selectedElements) {
            res += ge.val;
        }

        if (res == currentTarget) {
            return true;
        }
        return false;
    }
    #endregion

    #region Input
    void HandleMouseRelease() {
        if (Input.GetMouseButtonUp(0)) {
            printSelected();
            if (selectedElements.Count > 0) {
                if (CheckSelected()) {
                    
                }
                else {
                    
                }
            }
            ResetSelected();
        }
    }
    #endregion

    #region Misc
    void setTarget(int val) {
        currentTarget = val;
        targetText.text = val.ToString();
        targetAnimator.SetTrigger("Pop");
    }

    void ReRollGridElementNoEffect(GridElementGUI ge) {
        ge.setValAndReset(Random.Range(gridValMin, gridValMax + 1));
    }
    #endregion

    #region Print
    void printSelected() {
        string output = "";
        foreach (var ge in selectedElements) {
            output += " " + ge.val.ToString();
        }
        Debug.Log(output);
    }
    #endregion

    #region Tutorial state
    public class TutorialState {
        public SinglePlayerTutorialController sptc;

        public TutorialState(SinglePlayerTutorialController sptc) {
            this.sptc = sptc;
        }

        public virtual void Enter() {}
        public virtual void Update() {}
        public virtual void Exit() {}
    }

    public class TutorialState1 : TutorialState {
        public TutorialState1(SinglePlayerTutorialController sptc) : base(sptc) {}

        void HandleMouseRelease() {
            if (Input.GetMouseButtonUp(0)) {
                sptc.printSelected();
                if (sptc.selectedElements.Count > 0) {
                    if (sptc.CheckSelected()) {
                        Exit();
                    }
                    else {

                    }
                }
                sptc.ResetSelected();
            }
        }

        public override void Enter() {
            sptc.setTarget(15);
            sptc.elements[1, 1].setVal(3);
            sptc.elements[1, 2].setVal(4);
            sptc.elements[1, 3].setVal(5);
        }

        public override void Update() {
            HandleMouseRelease();
        }

        public override void Exit() {
            sptc.currentState = new TutorialState2(sptc);
        }
    }

    public class TutorialState2 : TutorialState {
        public TutorialState2(SinglePlayerTutorialController sptc) : base(sptc) { }
    }

    #endregion
}