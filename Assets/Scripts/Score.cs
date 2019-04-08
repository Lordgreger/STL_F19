using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour {

    public TextMeshProUGUI text;

    int score = 0;

    public void addScore(int s) {
        score += s;
        text.text = score.ToString();
    }

    public void setScore(int s) {
        score = s;
        text.text = score.ToString();
    }

    public int getScore() {
        return score;
    }
}
