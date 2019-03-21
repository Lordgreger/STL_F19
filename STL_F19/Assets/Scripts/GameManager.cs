﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour {

    public PlayerManager p1;
    public PlayerManager p2;
    public GameObject newGameButton;
    public GameObject buttonTutorial1;
    public GameObject buttonTutorial2;

    public Image countDownBar;

    public TextMeshProUGUI resultText;
    public GameObject result;

    public float gameTime;

    float timeLeft;

    private void Start() {
        result.SetActive(false);
        p1.sendCombo.AddListener(p2.applyCombo);
        p2.sendCombo.AddListener(p1.applyCombo);
        //countDownBar.GetComponent<Image>().enabled = false;
    }

    public void startNewGame() {
        p1.startNewGame();
        p2.startNewGame();
        buttonTutorial1.SetActive(false);
        buttonTutorial2.SetActive(false);
        newGameButton.SetActive(false);
        result.SetActive(false);
        timeLeft = gameTime;
        
        StartCoroutine(gameCountdown());
    }

    IEnumerator gameCountdown() {
        while (timeLeft > 0f) {
            yield return new WaitForSeconds(0.1f);
            timeLeft -= 0.1f;
            countDownBar.fillAmount = timeLeft / gameTime;
            countDownBar.GetComponent<Image>().enabled = true;
        }
        endGame();
    }

    void endGame() {
        p1.endGame();
        p2.endGame();
        newGameButton.SetActive(true);
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

    public void ExitGame() {
        Application.Quit();
    }

}