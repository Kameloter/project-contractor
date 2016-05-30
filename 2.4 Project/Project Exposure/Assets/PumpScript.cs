using UnityEngine;
using System.Collections;

public class PumpScript : MonoBehaviour {
    float timer = 0.0f;
    bool activated = false;

    public BaseActivatable interactable;
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
            print("yooo2");
            interactable.Activate();
        }
        else {
            print("yooo4");
            interactable.DeActivate();
        }
	}

    void OnParticleCollision(GameObject go) {
        print(go.name);
        if (go.CompareTag(Tags.particleSteam)) {
            print("yooo");
            activated = true;
            timer = 0.0f;
        }
    }
}
