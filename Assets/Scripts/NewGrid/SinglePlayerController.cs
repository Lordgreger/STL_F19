using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class SinglePlayerController : MonoBehaviour {

    #region Variables
    public int score;
    public int countdownTime;
    public float gameTime;
    public float gameTimerUpdateDelay;
    public float levelUpTime;

    public PlayerGrid gridController;
    public TextMeshProUGUI countdownRef;
    public RectTransform timerRef;
    public float timerTotalWidth;

    public TextMeshProUGUI pointsText;

    public GameObject scoreScreenRef;
    public TextMeshProUGUI endScoreText;

    float currentTime;
    #endregion

    #region Unity Scheduling
    private void Start() {
        gridController.scored.AddListener(scoredEvent);
        gridController.levelUpEvent.AddListener(onLevelUp);
        StartCoroutine(CountdownStart(countdownTime));
    }

    private void Update() {
        
    }
    #endregion

    #region Score
    void scoredEvent(int amount) {
        score += amount;
        pointsText.text = score.ToString();
        //Debug.Log("Scored: " + amount);
    }
    #endregion

    #region level
    void onLevelUp(int i) {
        currentTime += levelUpTime;
        if (currentTime > gameTime) {
            currentTime = gameTime;
        }
    }
    #endregion

    #region Timer
    void UpdateTimer(float time) {
        //Debug.Log("gt: " + gameTime + " tl: " + time);
        float relation = (time / gameTime);
        //Debug.Log(relation);
        timerRef.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, relation * timerTotalWidth);
        //Debug.Log("Time left: " + time);
    }
    #endregion

    #region End Game
    void EndGame() {
        gridController.EndGame();
        scoreScreenRef.SetActive(true);
        endScoreText.text = score.ToString();
    }

    #endregion

    #region NewGame
    public void StartNewGame() {
        score = 0;
        pointsText.text = score.ToString();
        scoreScreenRef.SetActive(false);
        StartCoroutine(CountdownStart(countdownTime));
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
        currentTime = gameTime;

        // Countdown
        while (currentTime > 0f) {
            yield return new WaitForSeconds(gameTimerUpdateDelay);
            currentTime -= gameTimerUpdateDelay;
            UpdateTimer(currentTime);
        }

        // End Game
        EndGame();
    }

    #endregion
}
