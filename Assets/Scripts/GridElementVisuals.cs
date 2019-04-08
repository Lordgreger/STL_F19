using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridElementVisuals : MonoBehaviour {

    public Image image;

    public Image selected;
    public Image locked;
     
    public Image destructionImage;
    public List<Sprite> destructionSpritesOne = new List<Sprite>();
    public List<Sprite> destructionSpritesTwo = new List<Sprite>();
    public List<Sprite> destructionSpritesThree = new List<Sprite>();
    public List<Sprite> destructionSpritesFour = new List<Sprite>();
    public List<Sprite> destructionSpritesFive = new List<Sprite>();
    public List<Sprite> destructionSpritesSix = new List<Sprite>();
    public List<Sprite> destructionSpritesSeven = new List<Sprite>();
    public float spriteChangeDelay;

    public void setSelected(bool i) {
        selected.enabled = i;
    }

    public void setLocked(bool i) {
        locked.enabled = i;
        //image.enabled = !i;
    }

    public void StartDestructionAnimation() {
        switch (image.sprite.name) {

            case "tile_one":
                StartCoroutine(animateDestruction(destructionSpritesOne));
                break;

            case "tile_two":
                StartCoroutine(animateDestruction(destructionSpritesTwo));
                break;

            case "tile_three":
                StartCoroutine(animateDestruction(destructionSpritesThree));
                break;

            case "tile_four":
                StartCoroutine(animateDestruction(destructionSpritesFour));
                break;

            case "tile_five":
                StartCoroutine(animateDestruction(destructionSpritesFive));
                break;

            case "tile_six":
                StartCoroutine(animateDestruction(destructionSpritesSix));
                break;

            case "tile_seven":
                StartCoroutine(animateDestruction(destructionSpritesSeven));
                break;

            default:
                StartCoroutine(animateDestruction(destructionSpritesOne));
                break;
        }
    }

    public float getDestructionTime() {
        return spriteChangeDelay * destructionSpritesOne.Count;
    }

    IEnumerator animateDestruction(List<Sprite> sprites) {
        selected.enabled = false;
        destructionImage.enabled = true;
        for (int i = 0; i < sprites.Count; i++) {
            destructionImage.sprite = sprites[i];
            destructionImage.rectTransform.sizeDelta *= 1.075f;
            destructionImage.color = new Color(256, 256, 256, 1f - ((float)i / (float)sprites.Count));
            yield return new WaitForSeconds(spriteChangeDelay);
        }
    }

}
