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
        targetValue = Random.Range(2, 11);
        text.text = targetValue.ToString();
    }

}
