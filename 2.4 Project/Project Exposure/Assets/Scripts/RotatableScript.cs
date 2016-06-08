using UnityEngine;
using System.Collections;

public class RotatableScript : BaseInteractable {

    public bool continuous = false;

    public Vector3 axis = Vector3.zero;

     [Tooltip("If continous angle/sec otherwise angle/click")]
    public float angle = 22.5f;

    AlignRotationScript somethingToAlign;

    public bool pause = false;

    void Start() {
        somethingToAlign = transform.root.GetComponentInChildren<AlignRotationScript>();
        if (axis == Vector3.zero) axis.y = 1;
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
        print(pause);
        if (!pause) this.transform.Rotate(axis, angle);
    }

    public override void OnInteract() {
        base.OnInteract();
        Rotate();
        if (somethingToAlign != null) { somethingToAlign.Align(); }
        

    }
}
