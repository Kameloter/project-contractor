using UnityEngine;
using System.Collections;

public class GameLogicIntroLevel : MonoBehaviour {

    public GameObject path1;
    bool didpath1= false;
    public BaseActivatable activatable;
    public GameObject trigger;
	// Use this for initialization
	void Start () {
        GameObject.Find("BigValve").GetComponent<BigValve>().isPowered = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (Game.LastInteractedObject != null && !didpath1) {
			if (Game.LastInteractedObject.name == "object1")
            Game.PlayCameraPath(path1);
            activatable.Activate();
            GameObject.Find("BigValve").GetComponent<BigValve>().isPowered = true;
            didpath1 = true;
        }

        if (GameObject.Find("BigValve").GetComponent<BigValve>().currentState == 1) {
            trigger.SetActive(true);
        }
        else {
            trigger.SetActive(false);
        }
	}

    void DoSomething()
    {
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
