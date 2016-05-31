using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class BaseInteractable : MonoBehaviour {

    [ReadOnly] public bool playerInRange = false;
    [HideInInspector] public UnityEvent onTriggerEnterEvent;
    [HideInInspector] public UnityEvent onTriggerExitEvent;

    public void Awake() {
        onTriggerEnterEvent = new UnityEvent();
        onTriggerExitEvent = new UnityEvent();
    }

    public virtual void OnInteract() {
    
    }

    public virtual void OnInteractableClicked() {
        if (!playerInRange) {
            GameManager.Instance.Player.GetComponent<NavMeshAgent>().SetDestination(this.transform.position);
            GameManager.Instance.ClickedObject = this.gameObject;
            print(GameManager.Instance.ClickedObject.name);
        }
    }

    /// <summary>
    /// Assuming the PLAYER enters.
    /// </summary>
    /// <param name="other"></param>
    public virtual void actionOnTriggerEnter(Collider player) { if (onTriggerEnterEvent != null) onTriggerEnterEvent.Invoke(); }
    /// <summary>
    /// Assuming the PLAYER enters.
    /// </summary>
    /// <param name="other"></param>
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
