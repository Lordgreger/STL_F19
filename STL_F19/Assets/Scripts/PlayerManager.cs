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
        Corner
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

            case ComboType.Corner:
                gg.disableColumn(Random.Range(0, gg.GetColumnCount()), 10);
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

        // Check for corner
        bool[,] c1Mask = new bool[,] { {false, true }, {true, true } };
        bool[,] c2Mask = new bool[,] { {true, false }, {true, true } };
        bool[,] c3Mask = new bool[,] { {true, true }, {false, true } };
        bool[,] c4Mask = new bool[,] { {true, true }, {true, false } };
        if (checkArrayForMask(arr, c1Mask) ||
            checkArrayForMask(arr, c2Mask) ||
            checkArrayForMask(arr, c3Mask) ||
            checkArrayForMask(arr, c4Mask)) {
            return ComboType.Corner;
        }


        if (elements.Count > 2) {
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


        if (elements.Count > 3) {
            return ComboType.Four;
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
