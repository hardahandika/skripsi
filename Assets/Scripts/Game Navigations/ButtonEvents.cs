using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ButtonEvents : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    public bool isButtonDown = false;

    void Update() {
        // Is the mouse still being held down?
        if (isButtonDown) {
            // Keep blocking
        }
    }

    public void OnPointerDown(PointerEventData data) {
        isButtonDown = true;
        // Start blocking
    }

    public void OnPointerUp(PointerEventData data) {
        isButtonDown = false;
        // Stop blocking
    }

}