using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameGrid : MonoBehaviour {

    GameElement[,] grid = new GameElement[6,8];
    public UnityEvent loseEvent = new UnityEvent();
    public UnityEvent selectionEvent = new UnityEvent();
    State state;
    List<GameElement> currentSelection = new List<GameElement>();
    public GameObject elementPrefab;
    static float gridDistance = 50.0f;

    private void Awake()
    {
        InitGrid();
        // Temperai test addings 
        //AddToColumn(1, 0);
        //AddToColumn(8, 2);
        //AddToColumn(3, 2);
    }

    public int GetColumnCount()
    {
        return grid.GetLength(0);
    }

    void InitGrid()
    {
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                GameElement ge = Instantiate(elementPrefab, transform).GetComponent<GameElement>();
                grid[i, j] = ge;
                RectTransform rt = ge.transform as RectTransform; 
                rt.localPosition = new Vector3((i - (grid.GetLength(0) / 2)) * gridDistance, j * gridDistance, 0);
                ge.gameObject.SetActive(false); 
            }
        }
    }

    public void AddToColumn(int value, int column)
    {
        for (int i = 0; i < grid.GetLength(1); i++)
        {
            if (!grid[column, i].gameObject.activeSelf)
            {
                grid[column, i].gameObject.SetActive(true);
                grid[column, i].SetElement(value);
                return;
            }
        }

        
    }

    enum State {
        looking,
        selecting
    }
}
