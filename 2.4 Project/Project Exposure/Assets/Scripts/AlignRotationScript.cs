using UnityEngine;
using System.Collections;

public class AlignRotationScript : MonoBehaviour {
    [SerializeField] bool useMainCamera;

    [Tooltip("Automatically gets the maincam assigned if boolean is true.")]
    public Transform alignTo;

    void Start() {
        Align();
    }

    public void Align() {
        if (useMainCamera) alignTo = Camera.main.transform;
        Quaternion targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, alignTo.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

        if (alignTo != null) {
            this.transform.rotation = targetRotation;
        } 
        else Debug.LogError("Assign something to the 'AlignTo' variable on '" + gameObject.name + "' from '" + gameObject.transform.parent.name + "'.");
    }
}
