using UnityEngine;
using System.Collections;

/// <summary>
/// This script should be attached to actual collectables.
/// It handles collision with the player and the destruction after pickup.
/// It notifies the CollectableHudScript that it was picked up and tells him its value.
/// </summary>
public class CollectableScript : MonoBehaviour {
    [Tooltip("Amount of points awarded")] public int value = 1;

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            Collect();
        }
    }

    void Collect() {
        GameObject.FindObjectOfType<CollectableHudScript>().OnCollectCollectable(value);
        Destroy(gameObject);
    }
}