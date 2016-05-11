using UnityEngine;
using System.Collections;

public class CollectableScript : MonoBehaviour {
    [Tooltip("Amount of points awarded")] public int value = 1;

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            Collect();
        }
    }

    void Collect() {
        GameManager.Instance.IncreaseCollectables(value);
        Destroy(gameObject);
    }
}