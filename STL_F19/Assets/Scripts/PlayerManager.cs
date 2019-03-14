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
        Four
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
            target.setNewTarget();
            gg.removeElements(elements);
            ComboType combo = checkCombo(elements);
            if (combo != ComboType.None) {
                sendCombo.Invoke(combo);
            }
        }
    }

    public void applyCombo(ComboType type) {
        switch (type) {

            case ComboType.None:
                break;

            case ComboType.Four:
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
}
