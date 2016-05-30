using UnityEngine;
using System.Collections;

public class AlignRotationScript : MonoBehaviour {
    [SerializeField] bool useMainCamera;

    [Tooltip("Automatically gets the maincam assigned if boolean is true.")]
    public Transform alignTo;

    void Start() {
        if (useMainCamera) alignTo = Camera.main.transform;

        if (alignTo != null) this.transform.localRotation = alignTo.rotation;
        else Debug.LogError("Assign something to the 'AlignTo' variable on '" + gameObject.name + "' from '" + gameObject.transform.parent.name + "'.");
    }
    
}
