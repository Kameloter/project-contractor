using UnityEngine;
using System.Collections;

/// <summary>
/// gamelogic script for simple scripting
/// Game class hold functions and variables you can easely use to make some simple actions.
/// </summary>
public class GameLogicIntroLevel : MonoBehaviour {
    public GameObject path1;
    bool didpath1= false;
    public BaseActivatable activatable;
    public GameObject trigger;
    private BigValve bigValve;
	// Use this for initialization
	void Start () {
        bigValve = GameObject.FindWithTag(Tags.bigValve).GetComponent<BigValve>();
        bigValve.isPowered = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (Game.LastInteractedObject != null && !didpath1) {
			if (Game.LastInteractedObject.name == "object1")
            Game.PlayCameraPath(path1,true);
            activatable.Activate();
            bigValve.isPowered = true;
            didpath1 = true;
        }

        if (bigValve.currentState == 1) trigger.SetActive(true);
        else trigger.SetActive(false);

        if (Game.TimeSpentOnLevel > Game.TimeNeededForLevel){
            //then?
        }
	}

    void DoSomething() { }

    void OnEnable() { CameraControl.OnCameraPathEnd.AddListener( DoSomething); }

    void OnDisable() { CameraControl.OnCameraPathEnd.RemoveListener(DoSomething); }
}
