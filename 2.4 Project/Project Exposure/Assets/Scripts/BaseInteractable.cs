using UnityEngine;
using UnityEngine.Events;
using System.Collections;

/// <summary>
/// Base class for all Interactables that the player can interract with.
/// Every object needs to know if the player is in range so the trigger/collision check is put here and an event is raised
/// so derrived classes can do actions on that triggerEnter/Exit events.
/// Also separate componenet classses (like InteractionCanvas and ShaderOutliner) can subscribe to the events 
/// and enable/disable their actions.
/// </summary>
public class BaseInteractable : MonoBehaviour {

   /// <summary>
   /// Bool to know if the player is in range of hte interactable object.
   /// </summary>
    [ReadOnly] public bool playerInRange = false;
    /// <summary>
    /// Raise the event when the PLAYER enters the trigger. Can be overriden by derrived classes and BASE should be called, otherwise the event wont be invoked.
    /// </summary>
    [HideInInspector] public UnityEvent onTriggerEnterEvent;
    /// <summary>
    /// Raise the event when the PLAYER exits the trigger. Can be overriden by derrived classes and BASE should be called, otherwise the event wont be invoked.
    /// </summary>
    [HideInInspector] public UnityEvent onTriggerExitEvent;


    public virtual void Awake()
    {
        //Make the events
        onTriggerEnterEvent = new UnityEvent();
        onTriggerExitEvent = new UnityEvent();
    }

    /// <summary>
    /// When we tap on an interactable IN RANGE OF PLAYER. We call this function from the InteractCanvas
    /// Store the last interacted object so it can be used from creatives in GameLogic scripts.
    /// </summary>
    public virtual void OnInteract()
    {
        GameManager.Instance.InteractedObject = this.gameObject;
    }

    /// <summary>
    /// When the player clicked outside of range on the object, we use raycast to detect that and that is  calling this function.
    /// When the object was clicked we then complete the action as soon as the player enters the triggerenter of the interactable object.
    /// </summary>
    public virtual void OnInteractableClicked() {}

    /// <summary>
    /// Additional function so derrived classes can have the ability to use the ontriggerenter.
    /// Base must be called so event is invoked.
    /// </summary>
    /// <param name="player"> The player </param>
    public virtual void actionOnTriggerEnter(Collider player) { if (onTriggerEnterEvent != null) onTriggerEnterEvent.Invoke(); }
    /// <summary>
    /// Additional function so derrived classes can have the ability to use the ontriggerexit.
    /// Base must be called so event is invoked.
    /// </summary>
    /// <param name="player">The player</param>
    public virtual void actionOnTriggerExit(Collider player) { if (onTriggerExitEvent != null) onTriggerExitEvent.Invoke(); }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            playerInRange = true;
            actionOnTriggerEnter(other);
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            playerInRange = false;
            actionOnTriggerExit(other);
        }
    }
}
