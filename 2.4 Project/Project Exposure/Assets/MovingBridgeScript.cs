using UnityEngine;
using System.Collections;

public class MovingBridgeScript : MoveableScript {

    public GameObject obstacle;

	// Use this for initialization
	public override void Start () {
        base.Start();
        print("base");
	}

    void Update() {
        if (temperatureScript.temperatureState != TemperatureScript.TemperatureState.Frozen) {
            obstacle.SetActive(false);
        }
        else {
            obstacle.SetActive(true);
        }
    }
	
	// Update is called once per frame
    public override void Activate() {
        base.Activate();
    }

    public override void DeActivate() {
        base.DeActivate();
    }
}
