using UnityEngine;
using System.Collections;

public class testline : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Debug.DrawLine(Vector3.zero, new Vector3(0, 0, 2.25f),Color.blue);
        Debug.DrawLine(Vector3.zero, new Vector3(2.25f, 0, 0),Color.red);
    }
}
