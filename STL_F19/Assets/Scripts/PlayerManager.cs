using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour {
    public Target target;
    public Score score;
    public GameGrid gg;
    public EffectsCreator ec;
    public UnityEvent<Combo> sendCombo = new ComboEvent();
    public AudioClip[] soundsList;

    AudioSource soundEFX;

    public enum ComboType {
        None,
        MoreThanOne,
        ReverseLBlock,
        LBlock,
        TBlock, 
        SBlock,
        ZBlock,
        IBlock, 
        OBlock
    }

    [System.Serializable] public class ComboEvent : UnityEvent<Combo> {}

    State state;
    int currentTarget;

    private void Start() {
        state = State.notPlaying;
        gg.selectionEvent.AddListener(onSelection);
        soundEFX = GetComponent<AudioSource>();
    }

    public void onSelection(List<GameGrid.Element> elements) {
        print(Constants.elementListToString(elements));
        if (checkSolution(elements)) {
            score.addScore(elements.Count);
            soundEFX.PlayOneShot(soundsList[1], 0.8f);
            ComboType comboType = checkCombo(elements);
            if (comboType != ComboType.None) {
                Combo combo = new Combo(this, comboType, elements);
                sendCombo.Invoke(combo);
            }

            Vector2 pos = gg.GetElementRealPos(elements[elements.Count - 1]);
            pos.x = -pos.x;
            ec.createPopUp(pos, elements.Count);

            target.setNewTarget();
            gg.removeElements(elements);

        } else {
            soundEFX.PlayOneShot(soundsList[0], 0.5f);
        }
    }

    public void applyCombo(Combo combo) {
        switch (combo.type) {

            case ComboType.MoreThanOne:
                ec.createBeams(combo.parameters, gg);
                for (int i = 0; i < combo.parameters.Length; i += 4) {
                    gg.disableElement((int)combo.parameters[i + 2], (int)combo.parameters[i + 3], 5f);
                }
                break;

            
            case ComboType.None:
                break;

            /*
            case ComboType.LBlock:
                gg.disableLBlocks(10);
                break;

            case ComboType.ReverseLBlock:
                gg.disableRLBlocks(10);
                break;

            case ComboType.TBlock:
                gg.disableTBlock(10);
                break;

            case ComboType.SBlock:
                gg.disableSBlock(10);
                break;

            case ComboType.ZBlock:
                gg.disableZBlock(10);
                break;

            case ComboType.IBlock:
                gg.disableIBlock(10);
                break;

            case ComboType.OBlock:
                gg.disableOBlock(10);
                break;

            */

            default:
                break;
        }
    }

    bool checkSolution(List<GameGrid.Element> elements) {
        int sum = 0;
        foreach (var e in elements) {
            sum += e.value;
        }

        if (sum == target.targetValue) {
            return true;
        }
        else {
            return false;
        }
    }

    public class Combo {
        public ComboType type;
        public float[] parameters;

        public Combo(PlayerManager pm, ComboType type, List<GameGrid.Element> elements) {
            this.type = type;

            if (type == ComboType.MoreThanOne) {
                parameters = new float[elements.Count * 4];
                for (int i = 0; i < elements.Count * 4; i += 4) {
                    // From
                    Vector2 realPos = pm.gg.GetElementRealPos(elements[i / 4]);
                    parameters[i] = realPos.x;
                    parameters[i + 1] = realPos.y;

                    // To
                    parameters[i + 2] = (int)Random.Range(0, Constants.gridElementsX);
                    parameters[i + 3] = (int)Random.Range(0, Constants.gridElementsY);
                }
            }
        }
    }

    ComboType checkCombo(List<GameGrid.Element> elements) {

        /*
        // Create position array from elements
        bool[,] arr = elementsToPositionArray(elements);

        // Check for revese L combos
        bool[,] rL1Mask = new bool[,] { {false, true }, {false, true }, { true, true } };
        bool[,] rL2Mask = new bool[,] { {true, true }, {true, false }, {true, false } };
        bool[,] rL3Mask = new bool[,] { {true, false, false }, {true, true, true } };
        bool[,] rL4Mask = new bool[,] { {true, true, true }, {false, false, true } };
        if (checkArrayForMask(arr, rL1Mask) ||
            checkArrayForMask(arr, rL2Mask) ||
            checkArrayForMask(arr, rL3Mask) ||
            checkArrayForMask(arr, rL4Mask)) {
            return ComboType.ReverseLBlock;
        }

        // Check for L combo
        bool[,] l1Mask = new bool[,] { { true, false }, { true, false }, { true, true } };
        bool[,] l2Mask = new bool[,] { { true, true }, { false, true }, { false, true } };
        bool[,] l3Mask = new bool[,] { { false, false, true }, { true, true, true } };
        bool[,] l4Mask = new bool[,] { { true, true, true }, { true, false, false } };
        if (checkArrayForMask(arr, l1Mask) ||
            checkArrayForMask(arr, l2Mask) ||
            checkArrayForMask(arr, l3Mask) ||
            checkArrayForMask(arr, l4Mask)) {
            return ComboType.LBlock;
        }

        // Check for T block
        bool[,] t1Mask = new bool[,] { { false, true, false }, { true, true, true } };
        bool[,] t2Mask = new bool[,] { { true, true, true }, { false, true, false } };
        bool[,] t3Mask = new bool[,] { { true, false }, { true, true }, {true, false } };
        bool[,] t4Mask = new bool[,] { { false, true}, { true, true }, { false, true } };
        if (checkArrayForMask(arr, t1Mask) ||
            checkArrayForMask(arr, t2Mask) ||
            checkArrayForMask(arr, t3Mask) ||
            checkArrayForMask(arr, t4Mask)) {
            return ComboType.TBlock;
        }

        // Check for S block
        bool[,] s1Mask = new bool[,] { { false, true, true }, { true, true, false } };
        bool[,] s2Mask = new bool[,] { { true, false }, { true, true }, { false, true } };
        if (checkArrayForMask(arr, s1Mask) ||
            checkArrayForMask(arr, s2Mask)) {
            return ComboType.SBlock;
        }

        // Check for Z block
        bool[,] z1Mask = new bool[,] { { true, true, false }, { false, true, true } };
        bool[,] z2Mask = new bool[,] { { false, true }, { true, true }, { true, false } };
        if (checkArrayForMask(arr, z1Mask) ||
            checkArrayForMask(arr, z2Mask)) {
            return ComboType.ZBlock;
        }

        // Check for I block
        bool[,] i1Mask = new bool[,] { { true, true, true, true }};
        bool[,] i2Mask = new bool[,] { { true }, { true }, { true } , { true } };
        if (checkArrayForMask(arr, i1Mask) ||
            checkArrayForMask(arr, i2Mask)) {
            return ComboType.IBlock;
        }

        // Check for O block
        bool[,] o1Mask = new bool[,] { { true, true }, { true, true } };
        if (checkArrayForMask(arr, o1Mask)) {
            return ComboType.OBlock;
        }
        */

        if (elements.Count > 1) {
            return ComboType.MoreThanOne;
        }

        return ComboType.None;
    }

    public void startNewGame() {
        if (state == State.playing)
            return;

        setupGame();
        target.setNewTarget();
        state = State.playing;
    }

    public void endGame() {
        state = State.notPlaying;
        target.clearTarget();
        gg.clearGrid();
    }

    public void exitGame() {
        Application.Quit();
    }

    #region Misc
    void setupGame() {
        gg.initGrid();
        score.setScore(0);
    }


    enum State {
        notPlaying,
        playing
    }
    #endregion

    bool[,] elementsToPositionArray(List<GameGrid.Element> elements) {
        bool[,] output = new bool[gg.GetColumnCount(), gg.GetRowCount()];

        for (int i = 0; i < output.GetLength(0); i++) {
            for (int j = 0; j < output.GetLength(1); j++) {
                output[i, j] = false;
            }
        }


        foreach (var e in elements) {
            output[e.x, e.y] = true;
        }

        return output;
    }

    bool checkArrayForMask(bool[,] arr, bool[,] mask) {
        for (int i = 0; i < arr.GetLength(0) - mask.GetLength(0); i++) {
            for (int j = 0; j < arr.GetLength(1) - mask.GetLength(1); j++) {
                if (checkMaskInternal(arr, mask, i, j)) {
                    return true;
                }
            }
        }
        return false;
    }

    bool checkMaskInternal(bool[,] arr, bool[,] mask, int i, int j) {
        for (int im = 0; im < mask.GetLength(0); im++) {
            for (int jm = 0; jm < mask.GetLength(1); jm++) {
                if (arr[i + im, j + jm] != mask[im, jm]) {
                    return false;
                }
            }
        }
        return true;
    }
}
