using UnityEngine;
using System.Collections;

public class AlignRotationScript : MonoBehaviour {
    [SerializeField] bool useMainCamera;

    [Tooltip("Automatically gets the maincam assigned if boolean is true.")]
    public Transform alignTo;

    [Header("Align Axes")]
    [SerializeField] bool x = true; 
    [SerializeField] bool y = true;
    [SerializeField] bool z = true;


    Vector3 alignRot;
    Vector3 ownRot;

    void Start() {
        Align();
    }

    public void Align() {
        if (useMainCamera) alignTo = Camera.main.transform;         

        Quaternion targetRotation = Quaternion.identity;
        if (alignTo != null) {
            alignRot = new Vector3(alignTo.eulerAngles.x, alignTo.eulerAngles.y, alignTo.eulerAngles.z);
            ownRot = transform.rotation.eulerAngles;

            if      (x && y && z)   targetRotation = Quaternion.Euler(alignRot);                        //Rotate on all     //most likely
            else if (!x && y && !z) targetRotation = Quaternion.Euler(ownRot.x, alignRot.y, ownRot.z);  //Rotate just Y     //next most likely
            else if (x && !y && !z) targetRotation = Quaternion.Euler(alignRot.x, ownRot.y, ownRot.z);
            else if (!x && !y && z) targetRotation = Quaternion.Euler(ownRot.x, ownRot.y, alignRot.z);
        
            this.transform.rotation = targetRotation;
        }
         
        else Debug.LogError("Assign something to the 'AlignTo' variable on '" + gameObject.name + "' from '" + gameObject.transform.parent.name + "'.");
    }
}
