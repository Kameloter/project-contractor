using UnityEngine;
using System.Collections;

public class RotatableScript : BaseInteractable {

    public bool continuous = false;

    public Vector3 axis = Vector3.zero;

     [Tooltip("If continous angle/sec otherwise angle/click")]
    public float angle = 22.5f;

    AlignRotationScript somethingToAlign; 

    void Start() {
        somethingToAlign = transform.root.GetComponentInChildren<AlignRotationScript>();
    }


	void FixedUpdate () {
        if (continuous) {
            RotateContinuous();
        }
	}

    void RotateContinuous() {
        this.transform.Rotate(axis, angle * Time.deltaTime);
    }

    void Rotate() {

        this.transform.Rotate(axis, angle);
    }

    public override void OnInteract() {
        Rotate();
        if (somethingToAlign != null) { somethingToAlign.Align(); print("Yes..."); }
        
    }
}
