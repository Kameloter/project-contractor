using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// This script can be attached to any GameObject (as a CHILD!) that has a script inheriting from BaseInteractable. 
/// It subscribes listeners to the BaseInteractable events.
/// 
/// The script contains a WORLD space canvas and its used to detect TOUCH interactions with objects.
/// </summary>
[RequireComponent(typeof (AlignRotationScript))]
public class InteractCanvasScript : MonoBehaviour
{

    /// <summary>
    /// A refference to the owner so that we have access to his public events and we can 
    /// subscribe THIS scripts listeners.
    /// </summary>
    BaseInteractable owner;

    /// <summary>
    /// Comes as a package with InteractCanvasScript.
    /// See AlignRotationScript summary for detailed info.
    /// </summary>
    private AlignRotationScript alignScript;

    /// <summary>
    /// World space canvas used to contain a clickable Image to detect touch input.
    /// </summary>
    [Header("References (auto)")]
    public Canvas objectCanvas;

    /// <summary>
    /// An image that is on the canvas used as a button
    /// Optionally image can be left empty(transparent) and can be placed over the whole object(so it gives illusion that you click on the object)
    /// </summary>
    public Image imageObject;
    
    void Start()
    {
        FindOwner();
        FindObjectCanvas();
        MakeSpriteInvisible();

        alignScript = GetComponent<AlignRotationScript>();

        owner.onTriggerEnterEvent.AddListener(ActivateCanvas);
        owner.onTriggerExitEvent.AddListener(DeactivateCanvas);

    }

    void FindOwner()
    {
        //Try to get the owner from the parent
        owner = transform.parent.GetComponent<BaseInteractable>();
        if (owner == null) { Debug.LogError(gameObject.name + " <- This canvas could not find its owner. Make sure it is a child of a GameObjct with a Component of type BaseInteractable(BigValve,SmallValve,etc",transform); }
    }

    void FindObjectCanvas()
    {
        objectCanvas = GetComponent<Canvas>();
        if (objectCanvas != null) objectCanvas.enabled = false;
        else Debug.LogError("Something wrong with 'objectCanvas' variable on '" + gameObject.name + "'. Make sure canvas exists.",transform);
    }

    void MakeSpriteInvisible()
    {
        imageObject = GetComponentInChildren<Image>();
        if(imageObject != null)
            imageObject.color = new Color(1, 1, 1, 0);
        else
            Debug.LogError("Something wrong with 'Image Object' variable on '" + gameObject.name + "'. Make sure image exists AS a child.",transform);

    }
    
    void ActivateCanvas()
    {
        objectCanvas.enabled = true;
        alignScript.Align();
    }
    void DeactivateCanvas()
    {
        objectCanvas.enabled = false;
    }

    void OnDestroy()
    {
        owner.onTriggerEnterEvent.RemoveListener(ActivateCanvas);
        owner.onTriggerExitEvent.RemoveListener(DeactivateCanvas);
    }
}
