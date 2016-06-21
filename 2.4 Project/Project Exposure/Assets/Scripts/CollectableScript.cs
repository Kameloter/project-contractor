using UnityEngine;
using System.Collections;

/// <summary>
/// This script should be attached to actual collectables.
/// It handles collision with the player and the destruction after pickup.
/// It notifies the CollectableHudScript that it was picked up and tells him its value.
/// </summary>
public class CollectableScript : MonoBehaviour {
    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            Collect();
        }
    }

    void Collect() {
        //notify GameManager that a collectable has been picked up.
        GameManager.Instance.CollectCollectable();
        Destroy(gameObject);
    }
}