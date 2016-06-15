using UnityEngine;
using System.Collections;

/// <summary>
/// script to make any object rotatable
/// either continous or when activated
/// </summary>
public class RotatableScript : BaseInteractable {
    //if moving continous
    [SerializeField] bool continuous = false;
    //axis to rotate around
    [SerializeField] Vector3 axis = Vector3.zero;

    [Tooltip("If continous angle/sec otherwise angle/click")]
    [SerializeField] float angle = 22.5f;

    //for rotating the objectcanvas
    AlignRotationScript somethingToAlign;

    //stop the rotatable to be interactable when for eample in a cutscene
    public bool pause = false;

    void Start() {
        somethingToAlign = transform.root.GetComponentInChildren<AlignRotationScript>();
        if (axis == Vector3.zero) axis.y = 1;
    }

    /// <summary>
    /// rotate if it is continuous
    /// </summary>
	void FixedUpdate () {
        if (continuous) {
            RotateContinuous();
        }
	}

    void RotateContinuous() {
        this.transform.Rotate(axis, angle * Time.deltaTime);
    }

    /// <summary>
    /// rotate around axis with angle
    /// </summary>
    void Rotate() {
        if (!pause) this.transform.Rotate(axis, angle);
    }

    /// <summary>
    /// called when the player clicks the object to rotate
    /// </summary>
    public override void OnInteract() {
        base.OnInteract();
        Rotate();
        if (somethingToAlign != null) { somethingToAlign.Align(); }
    }
}
