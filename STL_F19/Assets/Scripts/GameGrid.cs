using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameGrid : MonoBehaviour {

    GameElement[,] grid = new GameElement[6,8];
    public UnityEvent loseEvent = new UnityEvent();
    public UnityEvent selectionEvent = new UnityEvent();
    State state;
    List<GameElement> currentSelection = new List<GameElement>();

    enum State {
        looking,
        selecting
    }
}
