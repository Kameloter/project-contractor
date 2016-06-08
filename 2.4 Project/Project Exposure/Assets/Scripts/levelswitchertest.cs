using UnityEngine;
using System.Collections;

public class levelswitchertest : MonoBehaviour {

    public int level = 0;

    void Update() {
        if (Input.GetKey(KeyCode.Alpha7)) {
            FindObjectOfType<SceneManager>().SwitchToLevel(2);
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if(other.name == "Player")
        {
            FindObjectOfType<SceneManager>().SwitchToLevel(level);
        }
    }
}
