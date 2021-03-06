﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;

public class GameElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler {
    public int value;
    public int column;
    public int row;
    public UnityEvent<GameElement> mouseEnter = new ElementEvent();
    public UnityEvent<GameElement> mouseExit = new ElementEvent();
    public TextMeshProUGUI text;

    [HideInInspector]
    public RawImage image;

    [System.Serializable]
    public class ElementEvent : UnityEvent<GameElement> {

    }


    private void Start()
    {
        image = GetComponent<RawImage>();
    }

    public GameElement() {
        
    }

    public void OnPointerEnter(PointerEventData eventData) {
        mouseEnter.Invoke(this);
        //Debug.Log("pointer entered");
    }

    public void OnPointerExit(PointerEventData eventData) {
        mouseExit.Invoke(this);
        //Debug.Log("pointer exited");
    }

    public void SetElement(int value)
    {
        text.text = value.ToString();
        this.value = value;
    }

    public void SetElement() {
        int value = Random.Range(Constants.elementValueMin, Constants.elementValueMax + 1);
        text.text = value.ToString();
        this.value = value;
    }

    public void OnPointerDown(PointerEventData eventData) {
        mouseEnter.Invoke(this);
        //print("Adding first!");
    }
}
