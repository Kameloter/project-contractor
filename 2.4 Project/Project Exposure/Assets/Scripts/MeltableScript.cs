using UnityEngine;
using System.Collections;

public class MeltableScript : MonoBehaviour {

    bool melting = false;
    float timer = 0;

    public GameObject optionalPath;
    public bool StartAtPlayer = true;
    bool playedCamera = false;
    RotatableScript rotScript;

    bool triggered = false;

    bool _reusable = false;

    // Use this for initialization
    void Start () {
	    
	}

    public void SetMelting(RotatableScript pRotScript, bool reuseable = false) {
        if (!triggered) {
            _reusable = reuseable;

            rotScript = pRotScript;
            melting = true;
            rotScript.pause = true;
            triggered = true;
        } 
    }
	
	// Update is called once per frame
	void Update () {
        if (melting) {

            if (optionalPath != null && !playedCamera)
            {
                Camera.main.GetComponent<CameraControl>().StartCutscene(optionalPath, StartAtPlayer);
                playedCamera = true;
            }
            for (int i = 0; i < transform.childCount; i++) {
                transform.GetChild(i).transform.localScale = Vector3.Lerp(transform.GetChild(i).transform.localScale, new Vector3(0.01f,0.01f,0.01f), 0.3f * Time.deltaTime);
            }
            timer += Time.deltaTime;

            if (timer >= 3) {
                if (_reusable) rotScript.pause = false; //unpause rotatable
                Destroy(gameObject);
            }

        }
	}
}
