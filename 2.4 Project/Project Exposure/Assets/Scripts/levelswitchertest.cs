using UnityEngine;
using System.Collections;

public class levelswitchertest : MonoBehaviour {

	void OnTriggerEnter(Collider other)
    {
        if(other.name == "Player")
        {
            FindObjectOfType<SceneManager>().SwitchToLevel(1);
        }
    }
}
