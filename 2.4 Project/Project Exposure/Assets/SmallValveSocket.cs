using UnityEngine;
using System.Collections;

public class SmallValveSocket : MonoBehaviour {

    bool InRange = false;
    GameObject Player;
    PlayerScript playerScript;

    [SerializeField]
    Interactable[] interactables;

    GameObject socketed = null;
    [HideInInspector]
    public BigValve controlValve;

    [Header("Connects to : ")]
    public int valveID;
    public int valveLine;

	// Use this for initialization
	void Start () {
        Player = GameObject.FindGameObjectWithTag("Player");
        playerScript = Player.GetComponent<PlayerScript>();
	}
	
	// Update is called once per frame
	void Update () {
	}

    void OnMouseDown() {

        if (playerScript.carriedValve != null && InRange && !socketed) {
            PlaceValve(playerScript.carriedValve);
        }
        else if (socketed != null && InRange) {
            RemoveValve(socketed);
        }
    }

    void PlaceValve(GameObject valve) {
        valve.GetComponent<PickableScript>().Place(this.transform.position + this.transform.up,this.gameObject);
        valve.GetComponent<PickableScript>().clickable = false;
        socketed = valve;
        if(controlValve.currentState == valveLine)
        {
            ActivateInteractables();
        }
        else
        {
            Debug.Log("Line not active! Not doing action.");
        }
     
    }
    public void ActivateInteractables()
    {
        if (socketed == null) return;
        foreach (Interactable interactable in interactables)
        {
            interactable.Activate();
        }

    }
    void RemoveValve(GameObject valve) {
        valve.GetComponent<PickableScript>().PickUp();
        valve.GetComponent<PickableScript>().clickable = true;
        socketed = null;
        foreach (Interactable interactable in interactables) {
            interactable.Deactivate();
        }
    }
    public void DeactivateSocket()
    {
        foreach (Interactable interactable in interactables)
        {
            interactable.Deactivate();
        }
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
