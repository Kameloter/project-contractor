using UnityEngine;
using System.Collections;

public class GameLogicIntroLevel : MonoBehaviour {

    public GameObject path1;
    bool didpath1= false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Game.LastInteractedObject != null && !didpath1) {
            if (Game.LastInteractedObject.name == "object1")
            Game.PlayCameraPath(path1);
            didpath1 = true;
        }
	}

    void DoSomething()
    {
        print("lalalala");
    }

    void OnEnable()
    {
        CameraControl.OnCameraPathEnd += DoSomething;
    }

    void OnDisable()
    {
        CameraControl.OnCameraPathEnd -= DoSomething;
    }
}
