using UnityEngine;
using System.Collections;

public class PlayerAnimatorScript : MonoBehaviour {
    Animator myAnimator;
    PlayerMovement playerMovement;

    // Use this for initialization
    void Start () {
        playerMovement = GetComponent<PlayerMovement>();
        myAnimator = GetComponentInChildren<Animator>();
    }
    
	
	// Update is called once per frame
	void Update () {

        Vector3 vel = playerMovement.playerVelocity;
     //   Debug.Log("Vel magn" + vel.magnitude);
        float velMagn = vel.magnitude;
        myAnimator.SetFloat("Speed", velMagn);

    }
}
