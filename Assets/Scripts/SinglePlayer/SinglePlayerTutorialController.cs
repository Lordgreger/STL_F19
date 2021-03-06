﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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

    public Image overlayImage;
    public Image swipey;
    public Image TargetTile;

    public GameObject endScreen;

    public Sprite target0;
    public Sprite target1;
    public Sprite target2;
    public Sprite target3;
    public Sprite target4;
    public Sprite target5;

    public Sprite overlaySwipe;
    public Sprite overlaySwipe2;
    public Sprite overlayStone;
    public Sprite overlayTimer;
    public Sprite overlayLevel;
    public Sprite overlayEnd;

    public Animator swipeyAmim;

    public RectTransform timerRef;
    public Animator timerAnimator;

    int currentTarget;
    public GridElementGUI[,] elements;
    List<GridElementGUI> selectedElements;

    TutorialState currentState;

    #region Unity
    private void Start() {
        GenerateGrid();
        SetupSelected();
        currentState = new TutorialState1Swipe(this);
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
                    explodeReroll();
                }
                else {
                    shakeSelected();
                }
            }
            ResetSelected();
        }
    }
    #endregion

    #region Misc
    public void goToGame() {
        SceneManager.LoadScene("SinglePlayerMobile");
    }

    void setTarget(int val) {
        currentTarget = val;
        targetText.text = val.ToString();
        //targetAnimator.SetTrigger("Pop");
    }

    void ReRollGridElementNoEffect(GridElementGUI ge) {
        ge.setValAndReset(Random.Range(gridValMin, gridValMax + 1));
    }

    public void shakeSelected() {
        foreach (var element in selectedElements) {
            element.shake();
        }
    }

    public void explodeReroll() {
        foreach (var ge in selectedElements) {
            ge.explode();
            ReRollGridElementNoEffect(ge);
        }
    }

    class CorrectInfo {
        public int elementCount;
        public int scoreModifier;
    }

    public void bombEffectCheck() {
        CorrectInfo info = new CorrectInfo();
        info.elementCount = selectedElements.Count;
        info.scoreModifier = 1;

        int i = 0;
        while (selectedElements.Count > i) {
            GridElementGUI ge = selectedElements[i];
            ApplyElementEffect(ge, info);
            i++;
        }
    }

    void ApplyElementEffect(GridElementGUI ge, CorrectInfo info) {
        switch (ge.effect) {
            case "Bomb":
                ApplyBombEffect(ge, info);
                break;

            case "None":
                break;

            default:
                break;
        }

    }

    void ApplyBombEffect(GridElementGUI ge, CorrectInfo info) {
        //Debug.Log("Applied bomb effect");
        int addedBlocks = 0;

        if (ge.pos.x > 0) {
            if (ge.pos.y > 0) { if (AddToSelected(elements[ge.pos.x - 1, ge.pos.y - 1])) { addedBlocks++; } }
            if (AddToSelected(elements[ge.pos.x - 1, ge.pos.y])) { addedBlocks++; }
            if (ge.pos.y < gridHeight - 1) { if (AddToSelected(elements[ge.pos.x - 1, ge.pos.y + 1])) { addedBlocks++; } }
        }

        if (ge.pos.y > 0) { if (AddToSelected(elements[ge.pos.x, ge.pos.y - 1])) { addedBlocks++; } }
        if (ge.pos.y < gridHeight - 1) { if (AddToSelected(elements[ge.pos.x, ge.pos.y + 1])) { addedBlocks++; } }

        if (ge.pos.x < gridWidth - 1) {
            //Debug.Log("Size: " + elements.GetLength(0) + ", " + elements.GetLength(1));
            if (ge.pos.y > 0) {
                //Debug.Log("x: " + (ge.pos.x + 1) + " y: " + (ge.pos.y - 1));
                if (AddToSelected(elements[ge.pos.x + 1, ge.pos.y - 1])) { addedBlocks++; }
            }

            //Debug.Log("x: " + (ge.pos.x + 1) + " y: " + (ge.pos.y));
            if (AddToSelected(elements[ge.pos.x + 1, ge.pos.y])) {
                addedBlocks++;
            }

            if (ge.pos.y < gridHeight - 1) {
                //Debug.Log("x: " + (ge.pos.x + 1) + " y: " + (ge.pos.y));
                if (AddToSelected(elements[ge.pos.x + 1, ge.pos.y + 1])) { addedBlocks++; }
            }
        }

        info.elementCount += addedBlocks;
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

    public class TutorialState1Swipe : TutorialState {
        public TutorialState1Swipe(SinglePlayerTutorialController sptc) : base(sptc) {}

        void HandleMouseRelease() {
            if (Input.GetMouseButtonUp(0)) {
                sptc.printSelected();
                if (sptc.selectedElements.Count > 0) {
                    if (sptc.CheckSelected()) {
                        sptc.explodeReroll();
                        Exit();
                    }
                    else {
                        sptc.shakeSelected();
                    }
                }
                sptc.ResetSelected();
            }
        }

        public override void Enter() {
            sptc.setTarget(13);
            sptc.elements[1, 3].setVal(7);
            sptc.elements[2, 3].setVal(6);
            sptc.overlayImage.sprite = sptc.overlaySwipe;
            sptc.swipeyAmim.Play("SwipeyAnimation");
            sptc.targetAnimator.Play("TargetPopTut");
        }

        public override void Update() {
            HandleMouseRelease();
        }

        public override void Exit() {
            sptc.currentState = new TutorialState2Multiple(sptc);
            sptc.currentState.Enter();
        }
    }

    public class TutorialState2Multiple : TutorialState {
        public TutorialState2Multiple(SinglePlayerTutorialController sptc) : base(sptc) { }

        void HandleMouseRelease() {
            if (Input.GetMouseButtonUp(0)) {
                sptc.printSelected();
                if (sptc.selectedElements.Count > 0) {
                    if (sptc.CheckSelected()) {
                        sptc.explodeReroll();
                        Exit();
                    }
                    else {
                        sptc.shakeSelected();
                    }
                }
                sptc.ResetSelected();
            }
        }

        public override void Enter() {
            sptc.setTarget(15);
            sptc.elements[1, 3].setVal(2);
            sptc.elements[1, 2].setVal(3);
            sptc.elements[2, 3].setVal(5);
            sptc.elements[1, 4].setVal(5);
            sptc.overlayImage.sprite = sptc.overlaySwipe2;
            sptc.TargetTile.sprite = sptc.target1;
            sptc.targetAnimator.Play("TargetPopTut");
            sptc.swipeyAmim.Play("SwipeyAnimationT");
            
        }

        public override void Update() {
            HandleMouseRelease();
        }

        public override void Exit() {
            sptc.currentState = new TutorialState3Stone(sptc);
            sptc.currentState.Enter();
        }
    }

    public class TutorialState3Stone : TutorialState {
        public TutorialState3Stone(SinglePlayerTutorialController sptc) : base(sptc) { }

        void HandleMouseRelease() {
            if (Input.GetMouseButtonUp(0)) {
                sptc.printSelected();
                if (sptc.selectedElements.Count > 0) {
                    if (sptc.CheckSelected()) {
                        sptc.explodeReroll();
                        sptc.elements[0, 2].checkStoneEffect(sptc.selectedElements);
                        sptc.elements[0, 2].checkStoneEffect(sptc.selectedElements);
                        sptc.elements[0, 3].checkStoneEffect(sptc.selectedElements);
                        sptc.elements[0, 4].checkStoneEffect(sptc.selectedElements);
                        sptc.elements[1, 2].checkStoneEffect(sptc.selectedElements);
                        sptc.elements[1, 4].checkStoneEffect(sptc.selectedElements);
                        sptc.elements[2, 2].checkStoneEffect(sptc.selectedElements);
                        sptc.elements[2, 4].checkStoneEffect(sptc.selectedElements);
                        sptc.elements[3, 2].checkStoneEffect(sptc.selectedElements);
                        sptc.elements[3, 3].checkStoneEffect(sptc.selectedElements);
                        sptc.elements[3, 4].checkStoneEffect(sptc.selectedElements);
                        Exit();
                    }
                    else {
                        sptc.shakeSelected();
                    }
                }
                sptc.ResetSelected();
            }
        }

        public override void Enter() {
            sptc.setTarget(17);
            sptc.TargetTile.sprite = sptc.target2;
            sptc.elements[1, 3].setVal(8);
            sptc.elements[2, 3].setVal(9);
            sptc.elements[0, 2].setEffect("Stone");
            sptc.elements[0, 3].setEffect("Stone");
            sptc.elements[0, 4].setEffect("Stone");
            sptc.elements[1, 2].setEffect("Stone");
            sptc.elements[1, 4].setEffect("Stone");
            sptc.elements[2, 2].setEffect("Stone");
            sptc.elements[2, 4].setEffect("Stone");
            sptc.elements[3, 2].setEffect("Stone");
            sptc.elements[3, 3].setEffect("Stone");
            sptc.elements[3, 4].setEffect("Stone");
            sptc.overlayImage.sprite = sptc.overlayStone;
            sptc.targetAnimator.Play("TargetPop");
            sptc.swipeyAmim.Play("SwipeyAnimation");

        }

        public override void Update() {
            HandleMouseRelease();
        }

        public override void Exit() {
            sptc.currentState = new TutorialState4Bomb(sptc);
            sptc.currentState.Enter();
        }
    }

    public class TutorialState4Bomb : TutorialState {
        public TutorialState4Bomb(SinglePlayerTutorialController sptc) : base(sptc) { }

        void HandleMouseRelease() {
            if (Input.GetMouseButtonUp(0)) {
                sptc.printSelected();
                if (sptc.selectedElements.Count > 0) {
                    if (sptc.CheckSelected()) {
                        sptc.bombEffectCheck();
                        sptc.explodeReroll();
                        sptc.elements[0, 2].checkStoneEffect(sptc.selectedElements);
                        sptc.elements[0, 4].checkStoneEffect(sptc.selectedElements);
                        sptc.elements[3, 2].checkStoneEffect(sptc.selectedElements);
                        sptc.elements[3, 4].checkStoneEffect(sptc.selectedElements);
                        
                        Exit();
                    }
                    else {
                        sptc.shakeSelected();
                    }
                }
                sptc.ResetSelected();
            }
        }

        public override void Enter() {
            sptc.TargetTile.sprite = sptc.target3;
            sptc.setTarget(11);
            sptc.elements[1, 3].setVal(6);
            sptc.elements[2, 3].setVal(5);
            sptc.elements[1, 3].setEffect("Bomb");

            sptc.elements[0, 3].setVal(5);
            sptc.elements[1, 2].setVal(5);
            sptc.elements[1, 4].setVal(5);
            sptc.elements[2, 2].setVal(5);
            sptc.elements[2, 4].setVal(5);
            sptc.elements[3, 3].setVal(5);

            sptc.overlayImage.sprite = sptc.overlayStone;
            sptc.targetAnimator.Play("TargetPop");
            sptc.swipeyAmim.Play("SwipeyAnimation");

        }

        public override void Update() {
            HandleMouseRelease();
        }

        public override void Exit() {
            sptc.elements[1, 3].setEffect("None");
            sptc.currentState = new TutorialState5FreeSelect(sptc);
            sptc.currentState.Enter();
        }
    }

    public class TutorialState5FreeSelect : TutorialState {
        public TutorialState5FreeSelect(SinglePlayerTutorialController sptc) : base(sptc) { }

        void HandleMouseRelease() {
            if (Input.GetMouseButtonUp(0)) {
                sptc.printSelected();
                if (sptc.selectedElements.Count > 0) {
                    if (sptc.CheckSelected()) {
                        sptc.bombEffectCheck();
                        sptc.explodeReroll();
                        Exit();
                    }
                    else {
                        sptc.shakeSelected();
                    }
                }
                sptc.ResetSelected();
            }
        }

        public override void Enter() {
            sptc.TargetTile.sprite = sptc.target4;
            sptc.setTarget(14);
            sptc.overlayImage.gameObject.SetActive(false);
            sptc.targetAnimator.Play("TargetPop");
            sptc.swipeyAmim.gameObject.SetActive(false);

        }

        public override void Update() {
            HandleMouseRelease();
        }

        public override void Exit() {
            sptc.currentState = new TutorialState6LvlUp(sptc);
            sptc.currentState.Enter();
        }
    }

    public class TutorialState6LvlUp : TutorialState {
        public TutorialState6LvlUp(SinglePlayerTutorialController sptc) : base(sptc) { }

        void HandleMouseRelease() {
            if (Input.GetMouseButtonUp(0)) {
                sptc.printSelected();
                if (sptc.selectedElements.Count > 0) {
                    if (sptc.CheckSelected()) {
                        sptc.explodeReroll();
                        Exit();
                    }
                    else {
                        sptc.shakeSelected();
                    }
                }
                sptc.ResetSelected();
            }
        }

        public override void Enter() {
            sptc.TargetTile.sprite = sptc.target5;
            sptc.setTarget(16);
            sptc.targetAnimator.Play("TargetPop");
            sptc.overlayImage.gameObject.SetActive(true);
            sptc.overlayImage.sprite = sptc.overlayLevel;
            //når lvl up screen kører sættet targettile.sprite til 0;
        }

        public override void Update() {
            HandleMouseRelease();
        }

        public override void Exit() {
            sptc.currentState = new TutorialState7Timer(sptc);
            sptc.currentState.Enter();
        }
    }

    public class TutorialState7Timer : TutorialState {
        public TutorialState7Timer(SinglePlayerTutorialController sptc) : base(sptc) { }
        float time;

        void UpdateTimer(float time) {
            //Debug.Log("gt: " + gameTime + " tl: " + time);
            float relation = (time / 60f);
            //Debug.Log(relation);
            sptc.timerRef.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, relation * 445);
            //Debug.Log("Time left: " + time);

            if (relation < 0.5) {
                sptc.timerAnimator.SetBool("Blink", true);
            }
            else {
                sptc.timerAnimator.SetBool("Blink", false);
            }
        }

        public override void Enter() {
            sptc.setTarget(12);
            sptc.overlayImage.raycastTarget = true;
            sptc.targetAnimator.Play("TargetPop");
            sptc.overlayImage.sprite = sptc.overlayTimer;
            time = 10f;
            UpdateTimer(time);
            //set timer til at være 10 sekunder og lad dem se tiden rinde ud mens de intet kan gøre for at stoppe det (muahahahah >:D)
            //exit når tid er slut

        }

        public override void Update() {
            time -= Time.deltaTime;
            UpdateTimer(time);
            if (time <= 0f) {
                Exit();
            }
        }

        public override void Exit() {
            sptc.currentState = new TutorialState8EndScreen(sptc);
            sptc.currentState.Enter();
        }
    }

    public class TutorialState8EndScreen : TutorialState {
        public TutorialState8EndScreen(SinglePlayerTutorialController sptc) : base(sptc) { }

        public override void Enter() {
            sptc.endScreen.SetActive(true);
            sptc.overlayImage.sprite = sptc.overlayEnd;

        }

        public override void Update() {

        }

        public override void Exit() {
            //load scene
            sptc.currentState = new TutorialState8EndScreen(sptc);
            sptc.currentState.Enter();
        }
    }
    #endregion
}