using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour {

    public PlayerManager p1;
    public PlayerManager p2;
    public GameObject newGameButton;

    public TextMeshProUGUI countdownText;
    public GameObject countdown;

    public TextMeshProUGUI resultText;
    public GameObject result;

    public float gameTime;

    float timeLeft;

    private void Start() {
        countdown.SetActive(false);
        result.SetActive(false);
    }

    public void startNewGame() {
        p1.startNewGame();
        p2.startNewGame();
        newGameButton.SetActive(false);
        result.SetActive(false);
        timeLeft = gameTime;
        countdown.SetActive(true);
        countdownText.text = timeLeft.ToString();
        StartCoroutine(gameCountdown());
    }

    IEnumerator gameCountdown() {
        while (timeLeft > 0f) {
            yield return new WaitForSeconds(0.1f);
            timeLeft -= 0.1f;
            countdownText.text = ((int)timeLeft).ToString();
        }
        endGame();
    }

    void endGame() {
        p1.endGame();
        p2.endGame();
        newGameButton.SetActive(true);
        countdown.gameObject.SetActive(false);
        result.SetActive(true);

        if (p1.score.getScore() == p2.score.getScore()) {
            resultText.text = "It is a tie!";
        }
        else if (p1.score.getScore() > p2.score.getScore()) {
            resultText.text = "<-- Left win!";
        }
        else {
            resultText.text = "Right win! -->";
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}