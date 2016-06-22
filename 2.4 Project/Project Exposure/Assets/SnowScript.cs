using UnityEngine;
using System.Collections;

public class SnowScript : MonoBehaviour {
    bool melting = false;
    bool triggered = false;
    bool _reusable = false;

    float timer = 0;

    RotatableScript rotScript;

    //camera variables
    public GameObject optionalPath;
    public bool StartAtPlayer = true;
    [SerializeField]
    GameObject[] destroy;

   // [SerializeField]
   // ParticleSystem particle;
    /// <summary>
    /// set the object melting and doing the actions for it
    /// </summary>
    /// <param name="pRotScript"></param>
    /// <param name="reuseable"></param>
	// Use this for initialization
	

    public void SetMelting(RotatableScript pRotScript, bool reuseable = false) {
        if (!triggered) { // prevent calling this multiple times
            _reusable = reuseable; // store the reusable bool

            rotScript = pRotScript;
            rotScript.pause = true;

            if (optionalPath != null) {
                Camera.main.GetComponent<CameraControl>().StartCutscene(optionalPath, StartAtPlayer);
            }

            melting = true;
            triggered = true;
        }
    }

    /// <summary>
    /// if it is metling we increase the timer and destroy it after 3 seconds
    /// </summary>
    void Update() {
        if (melting) {
            timer += Time.deltaTime;

            for (int i = 0; i < transform.childCount; i++) {
                transform.GetChild(i).transform.localScale = Vector3.Lerp(transform.GetChild(i).transform.localScale, new Vector3(0.01f, 0.01f, 0.01f), 1 * Time.deltaTime);
            }
            //after 1.9 seconds destroy this gameobject
            if (timer >= 1.9f) {
                if (_reusable) rotScript.pause = false; //unpause rotatable
                foreach (GameObject obj in destroy) {
                    Destroy(obj);
                }
                Destroy(gameObject);
            }
        }
    }
}
