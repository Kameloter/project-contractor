using UnityEngine;
using System.Collections;
/// <summary>
/// This script is used to animate the player model.
/// Expects the animator to be a child of the main player object.
/// </summary>
public class PlayerAnimatorScript : MonoBehaviour {

    /// <summary>
    /// The animator holding the PlayerAnimController with the animations, etc.
    /// </summary>
    private Animator myAnimator;
    /// <summary>
    /// A ref to the script that moves our player. We use its velocity to determine whether we move or not.
    /// </summary>
    private PlayerMovement playerMovement;

    // Use this for initialization
    void Start () {
        playerMovement = GetComponent<PlayerMovement>();
        if(playerMovement == null) { Debug.LogError("Player animatorScript could not find the PlayerMovement script. Make sure its attached to player !", transform); }
            
        myAnimator = GetComponentInChildren<Animator>();
        if(myAnimator == null) { Debug.LogError("Animator component missing from a child of the player game object.",transform); }

    }
    
	
	// Update is called once per frame
	void Update ()
    {
        //Just give the velocity of the player to the animator and it handles the tresholds  when to move /stop.
        Vector3 vel = playerMovement.playerVelocity;
        float velMagn = vel.magnitude;
        myAnimator.SetFloat("Speed", velMagn);
    }
}
