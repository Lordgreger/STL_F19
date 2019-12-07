using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class GridElementGUI : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler {
    public PlayerGrid gridController; // Ref to controller
    public SinglePlayerTutorialController sptcRef; // Ref to controller
    public GridPos pos; // Pos in grid
    public int val; // Current value
    public bool selected; // Is selected
    public bool idle; // When not playing the game
    public Sprite[] numberSprites; // Ref to sprites for numbers
    public Sprite idleSprite; // Ref to idle sprite
    public Image imageRef; // Ref to image
    public TextMeshProUGUI valueText;
    public float selectedDarknessVal; // Value of V in HSV color of image
    public string effect;
    public GameObject[] explosionPrefabs;
    public GameObject stoneExplosion;
    public GameObject bombImageRef;
    public GameObject stoneImageRef;
    public Animator animator;

    private void Start() {
        //imageRef.sprite = idleSprite;
        effect = "None";

        if (gridController != null)
            gridController.scoredListEvent.AddListener(OnScoredList);
    }

    private void Update() {
        setEffectGraphics();
    }

    public void OnPointerEnter(PointerEventData pointerEventData) {
        if (idle) { return; } // Catch for idle
        if (effect == "Stone") { return; } // Catch for stone
        //Debug.Log("Got enter");

        if (Input.GetMouseButton(0)) {
            if (gridController != null)
                gridController.AddToSelected(this);
            if (sptcRef != null)
                sptcRef.AddToSelected(this);
        }
    }

    public void OnPointerDown(PointerEventData pointerEventData) {
        if (idle) { return; } // Catch for idle
        if (effect == "Stone") { return; } // Catch for stone

        if (gridController != null)
            gridController.AddToSelected(this);
        if (sptcRef != null)
            sptcRef.AddToSelected(this);
    }

    void OnScoredList(List<GridElementGUI> elements) {
        //Debug.Log("Got Scored list");
        checkStoneEffect(elements);
    }

    void checkStoneEffect(List<GridElementGUI> elements) {
        if (effect != "Stone")
            return;

        foreach (var element in elements) {
            if (isNextTo4(pos, element.pos)) {
                breakStoneEffect();
            }
        }
    }

    void breakStoneEffect() {
        Debug.Log("Effect: " + effect);
        if (effect != "Stone") {
            return;
        }

        effect = "None"; 
        Instantiate(stoneExplosion, this.transform).GetComponent<GridElementExplosion>().StartExplosion(0);
    }

    bool isNextTo8(GridPos x, GridPos y) {
        int calcX = y.x - x.x;
        int calcY = y.y - x.y;
        if (calcX > -2 && calcX < 2) {
            if (calcY > -2 && calcY < 2) {
                return true;
            }
        }
        return false;
    }

    bool isNextTo4(GridPos x, GridPos y) {
        if (x.x == y.x) {
            int calc = x.y - y.y;
            if (calc == 1 || calc == -1) {
                return true;
            }
        }

        if (x.y == y.y) {
            int calc = x.x - y.x;
            if (calc == 1 || calc == -1) {
                return true;
            }
        }

        return false;
    }


    #region Public Sets
    // Sets new val and updates graphics
    public void setVal(int i) {
        if (i > 0 && i < numberSprites.GetLength(0) + 1) { // Valid number
            imageRef.sprite = numberSprites[i - 1];
            val = i;
            valueText.text = val.ToString();
        }
    }

    // Sets new val and resets selected params
    public void setValAndReset(int i) {
        setVal(i);
        resetSelected();
    }

    // Sets new val and explode effect
    public void setValResetExplode(int i) {
        setVal(i);
        resetSelected();
    }

    // Sets element to be idle and updates graphics
    public void setIdle() {
        idle = true;
        selected = false;
        imageRef.sprite = idleSprite;
        resetSelected();
        valueText.enabled = false;
    }

    // Sets element to be active and updates graphics
    public void setActive() {
        idle = false;
        setVal(val);
        valueText.enabled = true;
    }

    // Plays shake animation
    public void shake() {
        animator.SetTrigger("Shake");
    }
    #endregion

    #region Misc
    public void setSelected() {
        //Debug.Log("Sets selected");
        Color c = Color.HSVToRGB(0, 0, 1);
        //Debug.Log("Color = " + c);
        imageRef.color = Color.HSVToRGB(0, 0, selectedDarknessVal);
        setEffectGraphics();
    }

    public void resetSelected() {
        //Debug.Log("Resets selected");
        imageRef.color = Color.HSVToRGB(0, 0, 1);
        setEffectGraphics();
    }

    public void explode() {
        //Debug.Log("Explode");
        float delay = Random.Range(0f, 0.15f);
        Instantiate(explosionPrefabs[val - 1], this.transform).GetComponent<GridElementExplosion>().StartExplosion(delay);
    }

    void setEffectGraphics() {
        //Debug.Log("Setting graphics...");
        bombImageRef.SetActive(false);
        stoneImageRef.SetActive(false);

        if (idle == true)
            return;

        if (effect == "Bomb") {
            //Debug.Log("Was BOMB!");
            bombImageRef.SetActive(true);
        }

        if (effect == "Stone") {
            stoneImageRef.SetActive(true);
        }
    }

    #endregion

    #region Effects
    public void RollEffect() {

        effect = "None";

        // Roll Stone
        int roll = Random.Range(0, 100);
        roll += 5 * gridController.level;

        if (roll > 100) {
            effect = "Stone";
            Debug.Log("Got stone effect");
            setEffectGraphics();
            return;
        }

        // Roll Bomb
        roll = Random.Range(0, 100);
        roll += 5 * gridController.level;

        if (roll > 100) {
            effect = "Bomb";
            setEffectGraphics();
            return;
        }
    }
    #endregion
}
