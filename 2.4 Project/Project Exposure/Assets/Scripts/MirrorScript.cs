using UnityEngine;
using System.Collections;

public class MirrorScript : MonoBehaviour {

    [HideInInspector]
    public int state = 0;

    Vector3 rotation = Vector3.zero;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
       
	}

    void OnMouseDown() {
        if (state == 7) {
            state = 0;
        }
        else {
            state++;
        }
        print(state);

        Vector3 rotation = new Vector3(0,state * 45,0);
        this.transform.eulerAngles = rotation;

    }
}
