using UnityEngine;
using System.Collections;

public class LookAtScript : MonoBehaviour {

    public Transform lookAt;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.rotation = lookAt.rotation;
        //transform.LookAt(lookAt);
    }
}
