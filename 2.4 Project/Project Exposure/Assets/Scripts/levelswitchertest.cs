using UnityEngine;
using System.Collections;

public class levelswitchertest : MonoBehaviour {

    public int level = 0;
	void OnTriggerEnter(Collider other)
    {
        if(other.name == "Player")
        {
            FindObjectOfType<SceneManager>().SwitchToLevel(level);
        }
    }
}
