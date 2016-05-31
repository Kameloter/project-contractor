using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CustomEvent))]
public class RotatableScript : MonoBehaviour {

    public bool continuous = false;

    public Vector3 axis = Vector3.zero;

     [Tooltip("If continous angle/sec otherwise angle/click")]
    public float angle = 22.5f;

     bool InRange = false;

     [HideInInspector]
     public int state = 0;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (continuous) {
            RotateContinuous();
        }
	}

    void RotateContinuous() {
        this.transform.Rotate(axis, angle * Time.deltaTime);
    }

    void Rotate() {
        if (state == 7) {
            state = 0;
        }
        else {
            state++;
        }

        this.transform.Rotate(axis, angle);
    }

    public void OnCustomEvent() {

        print("GETTING PRINT");
        if (InRange) {
            print("IN RANGE PRINT");
            Rotate();
        }
        else {
            print("ELSE PRINT");
            GameManager.Instance.Player.GetComponent<NavMeshAgent>().SetDestination(this.transform.position);
            GameManager.Instance.ClickedObject = this.gameObject;
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            InRange = true;
            if (GameManager.Instance.ClickedObject == this.gameObject) {
                Rotate();
            }
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            InRange = false;
        }
    }
}
