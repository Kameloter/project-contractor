using UnityEngine;
using System.Collections;

public class PipeScript : MonoBehaviour {

    bool InRange = false;
    GameObject Player;
    PlayerScript playerScript;

    [SerializeField]
    Interactable[] interactables;

	// Use this for initialization
	void Start () {
        Player = GameObject.FindGameObjectWithTag("Player");
        playerScript = Player.GetComponent<PlayerScript>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseDown() {
        if (playerScript.carriedValve != null && InRange) {
            PlaceValve(playerScript.carriedValve);
        }
    }

    void PlaceValve(GameObject valve) {
        valve.GetComponent<PickableScript>().Place(this.transform.position + this.transform.up,this.gameObject);
        //foreach (Interactable interactable in interactables) {
        //    interactable.Activate();
        //}
    }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            InRange = true;
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            InRange = false;
        }
    }
}
