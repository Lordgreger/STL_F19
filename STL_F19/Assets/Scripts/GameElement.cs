using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class GameElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public int value;
    public UnityEvent mouseEnter = new UnityEvent();
    public UnityEvent mouseExit = new UnityEvent();

    private void Awake()
    {
        value = Random.Range(1, 10);
    }

    public GameElement() {
        
    }

    public void OnPointerEnter(PointerEventData eventData) {
        mouseEnter.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData) {
        mouseExit.Invoke();
    }


}
