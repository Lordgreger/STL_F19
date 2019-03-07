using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Target : MonoBehaviour {

    public int targetValue;

    TextMeshProUGUI text;

    private void Start() {
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void setNewTarget() {
        targetValue = Random.Range(11, 30);
        text.text = targetValue.ToString();
    }

    public void clearTarget() {
        text.text = "";
    }

}
