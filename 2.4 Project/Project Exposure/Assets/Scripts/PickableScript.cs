using UnityEngine;
using System.Collections;
using UnityEngine.Events;


public class PickableScript : BaseInteractable {

    bool IsCarried = false;
    public bool clickable = true;

    GameObject player;
    PlayerScript playerScript;
    
    Rigidbody rigidBody;
    [HideInInspector]

    Vector3 startPos;

	void Start () {
        player = GameManager.Instance.Player;
        playerScript = GameManager.Instance.PlayerScript;
        rigidBody = this.GetComponent<Rigidbody>();
        startPos = this.transform.position;
	}

    public void ResetPos() {
        this.transform.position = startPos;
    }

    public void PickUp() {
        if (playerScript.carriedValve == null) {
            this.transform.position = player.transform.position - player.transform.forward + Vector3.up;
            this.transform.SetParent(player.transform);
            rigidBody.useGravity = false;
            rigidBody.isKinematic = true;
            IsCarried = true;
            rigidBody.constraints = RigidbodyConstraints.None;
        
            playerScript.carriedValve = this.gameObject;
        }
    }

    public override void OnInteractableClicked() {
        if (clickable) {
            if (playerInRange) {
                if (IsCarried) Drop();
                else PickUp();
            }
            else {
                if (IsCarried) Drop();
                else player.GetComponent<PlayerMovement>().SendAgent(transform);
            }
        }
    }

    void Drop() {
        this.transform.parent = null;
        rigidBody.useGravity = true;
        rigidBody.isKinematic = false;
        IsCarried = false;
        playerScript.carriedValve = null;
    }

    void RemoveClickedObject() {
        GameManager.Instance.ClickedObject = null;
    }

    public void Place(Vector3 position, GameObject parent) {
        this.transform.position = position;
        this.transform.rotation = Quaternion.Euler(0, 0, 0); ;
        this.transform.SetParent(parent.transform);

        rigidBody.constraints = RigidbodyConstraints.FreezeAll;
        rigidBody.useGravity = true;
        rigidBody.isKinematic = false;
        IsCarried = false;

        if (this.CompareTag("Valve")) {
            playerScript.carriedValve = null;
        }
    }

    public override void actionOnTriggerEnter(Collider player) {
        if (GameManager.Instance.ClickedObject == this.gameObject) {
            Debug.Log("PICKING CLICKED OBJECT WHEN PLAYHER WAS OUT OF RANGE !  ");
            PickUp();
            RemoveClickedObject();
        }
        base.actionOnTriggerEnter(player);
    }
}
