using UnityEngine;
using System.Collections;

public class PickableScript : MonoBehaviour {

    bool InRange = false;
    bool IsCarried = false;
    public bool clickable = true;

    GameObject Player;

    Rigidbody rigidBody;

	// Use this for initialization
	void Start () {
        Player = GameObject.FindGameObjectWithTag("Player");
        rigidBody = this.GetComponent<Rigidbody>();
	}

    public void PickUp() {
        if (GameManager.Instance.PlayerScript.carriedValve == null) {
            this.transform.position = Player.transform.position - Player.transform.forward;
            this.transform.SetParent(Player.transform);
            rigidBody.useGravity = false;
            rigidBody.isKinematic = true;
            IsCarried = true;
            rigidBody.constraints = RigidbodyConstraints.None;

            if (this.CompareTag("Valve")) {
                Player.GetComponent<PlayerScript>().carriedValve = this.gameObject;
            }
        }
    }

    public void OnCustomEvent() {
        if (clickable) {
            if (IsCarried) {
                Drop();
            }
            else {
                if (InRange) {
                    PickUp();
                }
                else {
                    GameObject.FindGameObjectWithTag(Tags.player).GetComponent<NavMeshAgent>().SetDestination(this.transform.position);
                    GameManager.Instance.ClickedObject = this.gameObject;
                    print(GameManager.Instance.ClickedObject.name);
                }
            }
        }
    }

    void Drop() {
        this.transform.parent = null;
        rigidBody.useGravity = true;
        rigidBody.isKinematic = false;
        IsCarried = false;

          if (this.CompareTag("Valve")) {
              Player.GetComponent<PlayerScript>().carriedValve = null;
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
            Player.GetComponent<PlayerScript>().carriedValve = null;
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            InRange = true;
            if (GameManager.Instance.ClickedObject == this.gameObject) {
                PickUp();
                RemoveClickedObject();
            }
          //  print("In Range TRUE");
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            InRange = false;
          //  print("In Range FALSE");
        }
    }
}
