using UnityEngine;
using System.Collections;

public class ValveRespawnScript : MonoBehaviour {
  
    void OnTriggerEnter(Collider other) {
        if (other.CompareTag(Tags.valve)) {
            other.GetComponent<PickableScript>().ResetPos();
        }
    }
}
