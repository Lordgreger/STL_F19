using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    const float delayAddRandom = 1;

    State state;
    int currentTarget;
    GameGrid gg;

    float timerAddRandom;
    bool runTimerAddRandom;

    private void Start() {
        gg = GetComponent<GameGrid>();
        setupGame();
    }

    private void Update() {
        addNewRandomRoutine();  
    }

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
        for (int i = 0; i < gg.GetColumnCount(); i++) {
            for (int j = 0; j < 2; j++) { 
                int randomValue = Random.Range(1, 9); 
                gg.AddToColumn(randomValue, i);
            }    
        }

        timerAddRandom = delayAddRandom;
        runTimerAddRandom = true;
    }

    void addRandomToRandomColumn() {
        gg.AddToColumn(Random.Range(1, 10), Random.Range(0, gg.GetColumnCount()));
    }

    enum State {
        start,
        playing,
        score
    }
}
