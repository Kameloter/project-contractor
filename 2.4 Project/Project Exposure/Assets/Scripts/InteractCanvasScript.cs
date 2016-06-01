using UnityEngine;
using System.Collections;

[RequireComponent(typeof (AlignRotationScript))]
public class InteractCanvasScript : MonoBehaviour {
    BaseInteractable owner;

    [Header("Object Canvas")]
    public Canvas objectCanvas;
    AlignRotationScript alignScript;

    void Start() {
        owner = transform.root.GetComponent<BaseInteractable>();
        objectCanvas = GetComponentInChildren<Canvas>();
        alignScript = GetComponent<AlignRotationScript>();

        if (objectCanvas != null) objectCanvas.enabled = false;
        else Debug.LogError("Assign something to the 'objectCanvas' variable on '" + gameObject.name + "'.");

        owner.onTriggerEnterEvent.AddListener(ActivateCanvas);
        owner.onTriggerExitEvent.AddListener(DeactivateCanvas);
    }

    void ActivateCanvas() {
        objectCanvas.enabled = true;
        alignScript.Align();
    }
    void DeactivateCanvas() {
        objectCanvas.enabled = false;
    }

    void OnDestroy() {
        owner.onTriggerEnterEvent.RemoveListener(ActivateCanvas);
        owner.onTriggerExitEvent.RemoveListener(DeactivateCanvas);
    }
}
