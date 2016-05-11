using UnityEngine;
using System.Collections;

public class CollectableScript : MonoBehaviour {
    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            Collect();
        }
    }

    void Collect() {
        Destroy(gameObject);
    }
}