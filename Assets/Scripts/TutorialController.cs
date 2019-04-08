using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialController : MonoBehaviour {
    public Image image;
    public Sprite[] sprites;
    public GameObject nextButton;
    public GameObject prevButton;

    int i = 0;

    public void next() {
        i += 1;

        if (i == sprites.Length) {
            SceneManager.LoadScene(1);
            return;
        }

        prevButton.SetActive(true);
        image.sprite = sprites[i];
    }

    public void prev() {
        if (i == 0) {
            return;
        }

        i -= 1;

        if (i == 0) {
            prevButton.SetActive(false);
        }

        image.sprite = sprites[i];
    }
}
