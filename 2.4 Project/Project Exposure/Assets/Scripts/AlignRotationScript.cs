using UnityEngine;
using System.Collections;

public class AlignRotationScript : MonoBehaviour {

    public Transform alignTo;  //camera?
    void Start() {
        if (alignTo != null) this.transform.rotation = alignTo.rotation;
        else Debug.LogError("Assign something to the 'AlignTo' variable on '" + gameObject.name + "' from '" + gameObject.transform.parent.name + "'.");
    }
    
    //When do we actually change te camera?
    //void Update () {
    //   this.transform.rotation = alignTo.rotation;
    //}
}
