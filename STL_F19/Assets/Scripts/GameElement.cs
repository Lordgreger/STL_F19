using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using TMPro;

public class GameElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public int value;
    public UnityEvent mouseEnter = new UnityEvent();
    public UnityEvent mouseExit = new UnityEvent();
    public TextMeshProUGUI text;

    //private void Start()
    //{
    //    text = GetComponentInChildren<TextMeshProUGUI>();
    //}

    public GameElement() {
        
    }

    public void OnPointerEnter(PointerEventData eventData) {
        mouseEnter.Invoke();
        Debug.Log("pointer entered");
    }

    public void OnPointerExit(PointerEventData eventData) {
        mouseExit.Invoke();
        Debug.Log("pointer exited");
    }

    public void SetElement(int value)
    {
        text.text = value.ToString();
        this.value = value;
    }

}
