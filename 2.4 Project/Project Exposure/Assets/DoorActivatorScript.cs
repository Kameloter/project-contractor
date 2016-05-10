using UnityEngine;
using System.Collections;

public class DoorActivatorScript : MonoBehaviour {
    float timer = 0.0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;

        if (timer > 0.5f) {
            transform.parent.GetComponent<Interactable>().currentState = 1;
        }
	}

    void OnParticleCollision(GameObject go) {
        if (go.CompareTag(Tags.particleSteam)) {
            transform.parent.GetComponent<Interactable>().currentState = 2;
            timer = 0.0f;
        }
    }
}
