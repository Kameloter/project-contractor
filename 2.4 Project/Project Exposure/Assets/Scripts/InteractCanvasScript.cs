using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Adds listeners to enable / disable the ObjectCanvas.
/// </summary>
[RequireComponent(typeof (AlignRotationScript))]
public class InteractCanvasScript : MonoBehaviour {
    BaseInteractable owner;
    AlignRotationScript alignScript;

    [Header("References (auto)")]
    public Canvas objectCanvas;
    public Image imageObject;
    
    void Start() {
        FindOwner();
        FindObjectCanvas();
        MakeSpriteInvisible();

        alignScript = GetComponent<AlignRotationScript>();

        owner.onTriggerEnterEvent.AddListener(ActivateCanvas);
        owner.onTriggerExitEvent.AddListener(DeactivateCanvas);
    }

    void FindOwner() {
        owner = transform.parent.GetComponent<BaseInteractable>(); //Try to get the owner from the parent
        if (owner == null) owner = transform.root.GetComponentInChildren<BaseInteractable>(); //if previous failed, try to get owner from any of the parents children
        if (owner == null) Debug.LogError("Cannot find owner.");
    }

    void FindObjectCanvas() {
        objectCanvas = GetComponent<Canvas>();
        if (objectCanvas != null) objectCanvas.enabled = false;
        else Debug.LogError("Assign something to the 'objectCanvas' variable on '" + gameObject.name + "'.");
    }

    void MakeSpriteInvisible() {
        imageObject = GetComponentInChildren<Image>();
        imageObject.color = new Color(1, 1, 1, 0); 
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
