using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraTrigger : MonoBehaviour {

    bool activated = false;
    public GameObject path;
    public bool HideOnExecute = true;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other) {
        if (!activated) {
            if (other.CompareTag(Tags.player)) {
              //  Camera.main.GetComponent<CameraControl>().StartCutscene(path);
                activated = true;
            }
        }
    }

}
