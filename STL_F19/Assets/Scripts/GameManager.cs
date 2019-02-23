using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    State state;
    int currentTarget;

    private void Update() {
       
    }



    enum State {
        start,
        playing,
        score
    }
}
