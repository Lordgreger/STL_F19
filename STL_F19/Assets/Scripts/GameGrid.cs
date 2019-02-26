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
    public GameObject elementPrefab;
    static float gridDistance = 1.1f;

    private void Start()
    {
        InitGrid();
    }

    void InitGrid()
    {
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                GameElement ge = Instantiate(elementPrefab, transform).GetComponent<GameElement>();
                grid[i, j] = ge;
                ge.transform.position = new Vector3(i * gridDistance, j * gridDistance, 0);
                ge.gameObject.SetActive(false); 
            }
        }
    }

    enum State {
        looking,
        selecting
    }
}
