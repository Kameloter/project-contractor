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

    NavMeshObstacle navMeshObs;

	void Start () {
        player = GameManager.Instance.Player;
        playerScript = GameManager.Instance.PlayerScript;
        rigidBody = this.GetComponent<Rigidbody>();
        startPos = this.transform.position;
        navMeshObs = this.GetComponent<NavMeshObstacle>();
	}

    public void ResetPos() {
        this.transform.position = startPos;
    }

    public void PickUp() {
        if (playerScript.carriedValve == null) {
            navMeshObs.enabled = false;
            
            this.transform.position = player.transform.position /*- player.transform.forward/10.0f*/ +  player.transform.up/2.2f;
            this.transform.SetParent(player.transform);
            this.transform.localRotation = Quaternion.Euler(0, 270, 90);

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

                //used for TapHere icons in level1
                GameManager.Instance.ClickedObject = this.gameObject; 
                Invoke("RemoveClickedObject", 0.1f);
            }
            else {
                if (IsCarried) Drop();
                else player.GetComponent<PlayerMovement>().SendAgent(transform);
            }
        }
    }

    void Drop() {
        Invoke("EnableNavMeshObstacke", 0.2f);
        this.transform.parent = null;
        rigidBody.useGravity = true;
        rigidBody.isKinematic = false;
        IsCarried = false;
        playerScript.carriedValve = null;
    }

    void EnableNavMeshObstacke() {
        if (navMeshObs) navMeshObs.enabled = true;
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

        if (this.CompareTag(Tags.valve)) playerScript.carriedValve = null;
    }

    public override void actionOnTriggerEnter(Collider player) {
        if (GameManager.Instance.ClickedObject == this.gameObject) {
            //Debug.Log("PICKING CLICKED OBJECT WHEN PLAYHER WAS OUT OF RANGE !  ");
            PickUp();
            RemoveClickedObject();
        }
        base.actionOnTriggerEnter(player);
    }
}
