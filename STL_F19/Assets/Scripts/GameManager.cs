using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    State state;
    int currentTarget;
    GameGrid gg;

    private void Start()
    {
        gg = GetComponent<GameGrid>();
        SetUpGame();
    }
    private void Update() {
       
    }

    void SetUpGame()
    {
        for (int i = 0; i < gg.GetColumnCount(); i++)
        {
            for (int j = 0; j < 2; j++)
            {
                
                int randomValue = Random.Range(1, 9);
                
                gg.AddToColumn(randomValue, i);
            }
            
        }
        
    }

    enum State {
        start,
        playing,
        score
    }
}
