using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class SinglePlayerController : MonoBehaviour {

    #region Variables
    public int score;
    public int countdownTime;
    public float gameTime;
    public float gameTimerUpdateDelay;

    public PlayerGrid gridController;
    public TextMeshProUGUI countdownRef;

    #endregion

    #region Unity Scheduling
    private void Start() {
        gridController.scored.AddListener(scoredEvent);
        StartCoroutine(CountdownStart(countdownTime));
    }

    private void Update() {
        
    }
    #endregion

    #region Score
    void scoredEvent(int amount) {
        score += amount;
        Debug.Log("Scored: " + amount);
    }

    #endregion

    #region Timer
    void UpdateTimer(float time) {
        //Debug.Log("Time left: " + time);
    }
    #endregion

    #region End Game
    void EndGame() {
        gridController.EndGame();
    }

    #endregion

    #region Coroutines
    IEnumerator CountdownStart(int seconds) {
        countdownRef.gameObject.SetActive(true);
        for (int i = 0; i < seconds; i++) {
            countdownRef.text = (seconds - i).ToString();
            yield return new WaitForSeconds(1);
        }
        countdownRef.text = "";
        countdownRef.gameObject.SetActive(false);

        gridController.StartNewGame();
        StartCoroutine(GameTimer());
    }

    IEnumerator GameTimer() {
        // Setup countdown
        float iterations = gameTime / gameTimerUpdateDelay;
        float time = gameTime;

        // Countdown
        for (int i = 0; i < iterations; i++) {
            yield return new WaitForSeconds(gameTimerUpdateDelay);
            time -= gameTimerUpdateDelay;
            UpdateTimer(time);
        }

        // End Game
        EndGame();
    }

    #endregion
}
