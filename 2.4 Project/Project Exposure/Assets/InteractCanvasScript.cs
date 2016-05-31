using UnityEngine;
using System.Collections;

public class InteractCanvasScript : MonoBehaviour {
    BaseInteractable owner;

    [Header("Object Canvas")]
    public Canvas objectCanvas;


    void Start() {
        owner = transform.root.GetComponent<BaseInteractable>();

        objectCanvas = GetComponentInChildren<Canvas>();
        if (objectCanvas != null) objectCanvas.enabled = false;
        else Debug.LogError("Assign something to the 'objectCanvas' variable on '" + gameObject.name + "'.");

        owner.onTriggerEnterEvent.AddListener(ActivateCanvas);
        owner.onTriggerExitEvent.AddListener(DeactivateCanvas);
    }

    void ActivateCanvas() {
        objectCanvas.enabled = true;
    }
    void DeactivateCanvas() {
        objectCanvas.enabled = false;
    }

    void OnDestroy() {
        owner.onTriggerEnterEvent.RemoveListener(ActivateCanvas);
        owner.onTriggerExitEvent.RemoveListener(DeactivateCanvas);
    }

}
