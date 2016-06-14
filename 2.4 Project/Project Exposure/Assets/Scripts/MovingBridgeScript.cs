using UnityEngine;
using System.Collections;

/// <summary>
/// moving bridge
/// </summary>
public class MovingBridgeScript : MoveableScript {
    //array of object with navmeshobstacle to block the player
    public GameObject [] obstacle;

	// Use this for initialization
	public override void Start ()
    {
        base.Start();
	}

    /// <summary>
    /// activate or deactivate obstacle based on velocity
    /// </summary>
    void Update() {
        if (transform.GetComponent<Rigidbody>().velocity.magnitude > 0) {
            for (int i = 0; i < obstacle.Length; i++) {
                obstacle[i].SetActive(true);
            }
        }
        else {
            for (int i = 0; i < obstacle.Length; i++) {
                if (Vector3.Distance(this.transform.position, obstacle[i].transform.position) <= 3.0f) {
                    obstacle[i].SetActive(false);
                }
            }
        }
    }
	
	// Update is called once per frame
    public override void Activate() {
        base.Activate();
    }

    public override void Deactivate() {
        base.Deactivate();
    }
}
