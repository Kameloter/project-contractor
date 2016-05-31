using UnityEngine;
using System.Collections;

public class PickableScript : BaseInteractable {

    bool IsCarried = false;
    public bool clickable = true;

    GameObject player;
    PlayerScript playerScript;

    Rigidbody rigidBody;

	// Use this for initialization
	void Start () {
        player = GameManager.Instance.Player;
        playerScript = GameManager.Instance.PlayerScript;
        rigidBody = this.GetComponent<Rigidbody>();
	}

    public void PickUp() {
        if (playerScript.carriedValve == null) {
            this.transform.position = player.transform.position - player.transform.forward;
            this.transform.SetParent(player.transform);
            rigidBody.useGravity = false;
            rigidBody.isKinematic = true;
            IsCarried = true;
            rigidBody.constraints = RigidbodyConstraints.None;

            if (this.CompareTag("Valve")) {
                playerScript.carriedValve = this.gameObject;
            }
        }
    }

    public override void OnInteractableClicked() {
        if (clickable) {
            if (IsCarried) { Drop(); }
            else {
                if (playerInRange) { PickUp(); }
                else base.OnInteractableClicked();
            }
        }
    }

    void Drop() {
        this.transform.parent = null;
        rigidBody.useGravity = true;
        rigidBody.isKinematic = false;
        IsCarried = false;

        if (this.CompareTag("Valve")) {
            playerScript.carriedValve = null;
        }
    }

    void RemoveClickedObject() {
        GameManager.Instance.ClickedObject = null;
    }

    public void Place(Vector3 position, GameObject parent) {
        this.transform.position = position;
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
            PickUp();
            RemoveClickedObject();
        }
        base.actionOnTriggerEnter(player);
    }
}
