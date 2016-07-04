using UnityEngine;
using System.Collections;

public class GameLogicLevel4 : MonoBehaviour {

    public GameObject path1;
    public GameObject path2;

    bool played1 = false;
    bool played2 = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (GameObject.Find("BigValve").GetComponent<BigValve>().currentState == 1 && !played1) {
            Game.PlayCameraPath(path2,true);
            played1 = true;
        }
	}
}
