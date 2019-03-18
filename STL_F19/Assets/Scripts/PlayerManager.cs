using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour
{
    const float delayAddRandom = 3f;

    public Target target;
    public Score score;
    public GameGrid gg;
    public UnityEvent<ComboType> sendCombo = new ComboEvent();

    public enum ComboType {
        None,
        Four,
        Numbers,
        ReverseLBlock,
        LBlock,
        TBlock, 
        SBlock,
        ZBlock,
        IBlock, 
        OBlock
    }

    [System.Serializable]
    public class ComboEvent : UnityEvent<ComboType> {

    }

    State state;
    int currentTarget;

    float timerAddRandom;
    bool runTimerAddRandom;

    private void Start() {
        state = State.notPlaying;
        gg.selectionEvent.AddListener(onSelection);
    }

    private void Update() {
        runState();
    }

    public void onSelection(List<GameElement> elements) {
        if (checkSolution(elements)) {
            score.addScore(elements.Count);
            ComboType combo = checkCombo(elements);
            if (combo != ComboType.None)
            {
                sendCombo.Invoke(combo);
            }

            target.setNewTarget();
            gg.removeElements(elements);
            
        }
    }

    public void applyCombo(ComboType type) {
        switch (type) {

            case ComboType.None:
                break;

            case ComboType.Four:
                gg.disableColumn(Random.Range(0, gg.GetColumnCount()), 10);
                break;

            case ComboType.Numbers:
                gg.disableNumbers(10);
                break;

            case ComboType.LBlock:
                gg.disableLBlocks(10);
                break;

            case ComboType.ReverseLBlock:
                gg.disableRLBlocks(10);
                break;

            case ComboType.TBlock:
                gg.disableTBlock(10);
                break;

            case ComboType.SBlock:
                gg.disableSBlock(10);
                break;

            case ComboType.ZBlock:
                gg.disableZBlock(10);
                break;

            case ComboType.IBlock:
                gg.disableIBlock(10);
                break;

            case ComboType.OBlock:
                gg.disableOBlock(10);
                break;

            default:
                break;
        }
    }
    

    bool checkSolution(List<GameElement> elements) {
        int sum = 0;
        foreach (var e in elements) {
            sum += e.value;
        }

        if (sum == target.targetValue) {
            return true;
        }
        else {
            return false;
        }
    }

    ComboType checkCombo(List<GameElement> elements) {

        // Create position array from elements
        bool[,] arr = elementsToPositionArray(elements);

        // Check for revese L combos
        bool[,] rL1Mask = new bool[,] { {false, true }, {false, true }, { true, true } };
        bool[,] rL2Mask = new bool[,] { {true, true }, {true, false }, {true, false } };
        bool[,] rL3Mask = new bool[,] { {true, false, false }, {true, true, true } };
        bool[,] rL4Mask = new bool[,] { {true, true, true }, {false, false, true } };
        if (checkArrayForMask(arr, rL1Mask) ||
            checkArrayForMask(arr, rL2Mask) ||
            checkArrayForMask(arr, rL3Mask) ||
            checkArrayForMask(arr, rL4Mask)) {
            return ComboType.ReverseLBlock;
        }

        // Check for L combo
        bool[,] l1Mask = new bool[,] { { true, false }, { true, false }, { true, true } };
        bool[,] l2Mask = new bool[,] { { true, true }, { false, true }, { false, true } };
        bool[,] l3Mask = new bool[,] { { false, false, true }, { true, true, true } };
        bool[,] l4Mask = new bool[,] { { true, true, true }, { true, false, false } };
        if (checkArrayForMask(arr, l1Mask) ||
            checkArrayForMask(arr, l2Mask) ||
            checkArrayForMask(arr, l3Mask) ||
            checkArrayForMask(arr, l4Mask)) {
            return ComboType.LBlock;
        }

        // Check for T block
        bool[,] t1Mask = new bool[,] { { false, true, false }, { true, true, true } };
        bool[,] t2Mask = new bool[,] { { true, true, true }, { false, true, false } };
        bool[,] t3Mask = new bool[,] { { true, false }, { true, true }, {true, false } };
        bool[,] t4Mask = new bool[,] { { false, true}, { true, true }, { false, true } };
        if (checkArrayForMask(arr, t1Mask) ||
            checkArrayForMask(arr, t2Mask) ||
            checkArrayForMask(arr, t3Mask) ||
            checkArrayForMask(arr, t4Mask)) {
            return ComboType.TBlock;
        }

        // Check for S block
        bool[,] s1Mask = new bool[,] { { false, true, true }, { true, true, false } };
        bool[,] s2Mask = new bool[,] { { true, false }, { true, true }, { false, true } };
        if (checkArrayForMask(arr, s1Mask) ||
            checkArrayForMask(arr, s2Mask)) {
            return ComboType.SBlock;
        }

        // Check for Z block
        bool[,] z1Mask = new bool[,] { { true, true, false }, { false, true, true } };
        bool[,] z2Mask = new bool[,] { { false, true }, { true, true }, { true, false } };
        if (checkArrayForMask(arr, z1Mask) ||
            checkArrayForMask(arr, z2Mask)) {
            return ComboType.ZBlock;
        }

        // Check for I block
        bool[,] i1Mask = new bool[,] { { true, true, true, true }};
        bool[,] i2Mask = new bool[,] { { true }, { true }, { true } , { true } };
        if (checkArrayForMask(arr, i1Mask) ||
            checkArrayForMask(arr, i2Mask)) {
            return ComboType.IBlock;
        }

        // Check for O block
        bool[,] o1Mask = new bool[,] { { true, true }, { true, true } };
        if (checkArrayForMask(arr, o1Mask)) {
            return ComboType.OBlock;
        }

        // Same Number combo
        if (elements.Count > 3) {
            int sameNumber = 0;
            for (int i = 0; i < elements.Count; i++) {  
                if (elements[i].value == 3) {
                    sameNumber += 1;
                    if (sameNumber > 1) {
                        return ComboType.Numbers;
                    }
                }
            }    
        }

        return ComboType.None;
    }

    public void startNewGame() {
        if (state == State.playing)
            return;

        setupGame();
        target.setNewTarget();
        state = State.playing;
    }

    public void endGame() {
        state = State.notPlaying;
        gg.clearGrid();
        target.clearTarget();
        gg.selector.SetActive(false);
    }

    public void exitGame() {
        Application.Quit();
    }

    #region States
    void runState() {
        if (state == State.notPlaying) {

        }
        else if (state == State.playing) {
            addNewRandomRoutine();
        }
    }
    #endregion

    #region Misc
    void addNewRandomRoutine() {
        if (runTimerAddRandom) {
            if (timerAddRandom <= 0f) {
                addRandomToRandomColumn();
                timerAddRandom = delayAddRandom;
            }
            else {
                timerAddRandom -= Time.deltaTime;
            }
        }
    }

    void setupGame() {
        /*
        for (int i = 0; i < gg.GetColumnCount(); i++) {
            for (int j = 0; j < 2; j++) {
                int randomValue = Random.Range(1, 9);
                gg.AddToColumn(randomValue, i);
            }
        }
        */
        gg.fillGrid();

        gg.selector.SetActive(true);
        //timerAddRandom = delayAddRandom;
        //runTimerAddRandom = true;
        score.setScore(0);
    }

    void addRandomToRandomColumn() {
        gg.AddToColumn(Random.Range(1, 10), Random.Range(0, gg.GetColumnCount()));
    }

    enum State {
        notPlaying,
        playing
    }
    #endregion

    bool[,] elementsToPositionArray(List<GameElement> elements) {
        bool[,] output = new bool[gg.GetColumnCount(), gg.GetRowCount()];

        for (int i = 0; i < output.GetLength(0); i++) {
            for (int j = 0; j < output.GetLength(1); j++) {
                output[i, j] = false;
            }
        }


        foreach (var e in elements) {
            output[e.column, e.row] = true;
        }

        return output;
    }

    bool checkArrayForMask(bool[,] arr, bool[,] mask) {
        for (int i = 0; i < arr.GetLength(0) - mask.GetLength(0); i++) {
            for (int j = 0; j < arr.GetLength(1) - mask.GetLength(1); j++) {
                if (checkMaskInternal(arr, mask, i, j)) {
                    return true;
                }
            }
        }
        return false;
    }

    bool checkMaskInternal(bool[,] arr, bool[,] mask, int i, int j) {
        for (int im = 0; im < mask.GetLength(0); im++) {
            for (int jm = 0; jm < mask.GetLength(1); jm++) {
                if (arr[i + im, j + jm] != mask[im, jm]) {
                    return false;
                }
            }
        }
        return true;
    }
}
