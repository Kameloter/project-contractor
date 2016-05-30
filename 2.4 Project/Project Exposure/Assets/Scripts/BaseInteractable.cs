using UnityEngine;
using System.Collections;

public class BaseInteractable : MonoBehaviour {

    public bool playerInRange = false;

    public virtual void OnInteract() {
    
    }

    public virtual void OnInteractableClicked() {
     
        if (!playerInRange) {
            GameManager.Instance.Player.GetComponent<NavMeshAgent>().SetDestination(this.transform.position);
            GameManager.Instance.ClickedObject = this.gameObject;
            print(GameManager.Instance.ClickedObject.name);
        }
    }

    public virtual void actionOnTriggerEnter(Collider other) { }
    public virtual void actionOnTriggerExit(Collider other) { }


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
