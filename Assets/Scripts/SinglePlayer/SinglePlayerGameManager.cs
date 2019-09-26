using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SinglePlayerGameManager : MonoBehaviour {
    public PlayerManager player;
    public GameObject newGameButton;
    public GameObject playAgainButton;
    public GameObject endBackground;

    public Image countDownBar;
    public Image cdBackground;
    public Image cdOutline;
    public Image gridBackgroundPlayer;

    public TextMeshProUGUI resultTextPlayer;
    public GameObject result;

    public float gameTime;

    float timeLeft;

    private void Start() {
        result.SetActive(false);
        countDownBar.enabled = false;
        cdBackground.enabled = false;
        cdOutline.enabled = false;
        gridBackgroundPlayer.enabled = false;
        playAgainButton.SetActive(false);
        endBackground.SetActive(false);
        //countDownBar.GetComponent<Image>().enabled = false;
    }

    public void startNewGame() {
        Debug.Log("Started new game");
        player.startNewGame();
        newGameButton.SetActive(false);
        playAgainButton.SetActive(false);
        result.SetActive(false);
        countDownBar.enabled = true;
        cdBackground.enabled = true;
        cdOutline.enabled = true;
        gridBackgroundPlayer.enabled = true;
        endBackground.SetActive(false);
        timeLeft = gameTime;

        StartCoroutine(gameCountdown());
    }

    IEnumerator gameCountdown() {
        while (timeLeft > 0f) {
            yield return new WaitForSeconds(0.1f);
            timeLeft -= 0.1f;
            countDownBar.fillAmount = timeLeft / gameTime;
        }
        endGame();
    }

    void endGame() {
        player.endGame();

        Image resultImg = result.GetComponent<Image>();
        // newGameButton.SetActive(true);
        result.SetActive(true);
        playAgainButton.SetActive(true);
        endBackground.SetActive(true);

        resultTextPlayer.text = player.score.getScore().ToString();

        countDownBar.enabled = false;
        cdBackground.enabled = false;
        cdOutline.enabled = false;
    }

    public void ExitGame() {
        Application.Quit();
    }
}
