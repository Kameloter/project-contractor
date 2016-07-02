using UnityEngine;
using System.Collections;

/// <summary>
/// This script instructs any valve to respawn itself if knocked off the playground.
/// </summary>
public class ValveRespawnScript : MonoBehaviour {
  
    void OnTriggerEnter(Collider other) {
        if (other.CompareTag(Tags.valve)) {
            other.GetComponent<PickableScript>().ResetPos();
        }
    }
}
