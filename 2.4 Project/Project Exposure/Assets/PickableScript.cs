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
	
	// Update is called once per frame
	void Update () {
	    
	}

    void OnMouseDown() {
        if (clickable) {
            if (IsCarried) {
                Drop();
            }
            else {
                if (InRange) {
                    PickUp();
                }
            }
        }
    }

    public void PickUp() {
        this.transform.position = Player.transform.position - Player.transform.forward;
        this.transform.SetParent(Player.transform);
        rigidBody.useGravity = false;
        rigidBody.isKinematic = true;
        IsCarried = true;

        if (this.CompareTag("Valve")) {
            Player.GetComponent<PlayerScript>().carriedValve = this.gameObject;
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

    public void Place(Vector3 position, GameObject parent) {
        this.transform.position = position;
        this.transform.SetParent(parent.transform);

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
            print("In Range TRUE");
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            InRange = false;
            print("In Range FALSE");
        }
    }
}
