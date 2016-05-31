using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FrozenBridgeScript : MonoBehaviour {

    public GameObject obstacle;

    public List<GameObject> iceCubes;

    bool unfrozen = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (GetComponent<TemperatureScript>().temperatureState != TemperatureScript.TemperatureState.Frozen && !unfrozen) {
            foreach (GameObject iceCube in iceCubes) {
                Destroy(iceCube);
            }
            obstacle.SetActive(false);
            unfrozen = true;
        }
	}
}
