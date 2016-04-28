using UnityEngine;
using System.Collections;

public class BridgeScript : Interactable {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void Activate() {
        print("activated");
    }

    public override void Deactivate() {
        print("deactivated");

    }
}
