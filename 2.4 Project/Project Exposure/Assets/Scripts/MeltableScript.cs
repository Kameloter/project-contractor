using UnityEngine;
using System.Collections;

/// <summary>
/// this script plays an animation and makes objects interactable and walkable etc
/// </summary>
public class MeltableScript : MonoBehaviour {
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

    [SerializeField]
     ParticleSystem particle;
    /// <summary>
    /// set the object melting and doing the actions for it
    /// </summary>
    /// <param name="pRotScript"></param>
    /// <param name="reuseable"></param>
    public void SetMelting(RotatableScript pRotScript, bool reuseable = false) {
        if (!triggered) { // prevent calling this multiple times
            _reusable = reuseable; // store the reusable bool

            rotScript = pRotScript;
            rotScript.pause = true;

            transform.parent.gameObject.GetComponent<Animator>().SetBool("Melting", true);

            if (optionalPath != null) {
                Camera.main.GetComponent<CameraControl>().StartCutscene(optionalPath, StartAtPlayer);
            }

            particle.Play();

            melting = true;
            triggered = true;
        } 
    }
	
    /// <summary>
    /// if it is metling we increase the timer and destroy it after 3 seconds
    /// </summary>
	void Update () {
        if (melting) {
            timer += Time.deltaTime;

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
