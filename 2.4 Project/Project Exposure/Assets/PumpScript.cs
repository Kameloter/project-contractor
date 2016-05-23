using UnityEngine;
using System.Collections;

public class PumpScript : MonoBehaviour {
    float timer = 0.0f;
    bool activated = false;

    public Interactable interactable;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {


        timer += Time.deltaTime;

        if (timer > 0.5f) {
            activated = false;
        }

        if (activated) {
            interactable.currentState = 2;
        }
        else {
            interactable.currentState = 1;
        }
	}

    void OnParticleCollision(GameObject go) {
        print("yooo");
        if (go.CompareTag(Tags.particleSteam)) {
            activated = true;
            timer = 0.0f;
        }
    }
}
