using UnityEngine;
using System.Collections;

public class levelswitchertest : MonoBehaviour {

    public int level = 0;

    float timeSpent;
    int collected;
    float timeLeft;


    void Update() {
        if (Input.GetKey(KeyCode.Alpha7)) {
            FindObjectOfType<SceneManager>().SwitchToLevel(2);
        }
    }


    void OnTriggerEnter(Collider other) {
        if (other.name == "Player") {
            GetStats(out timeSpent, out collected, out timeLeft);
            //show ui
        }
    }

    void GetStats(out float pTimeSpent, out int pCollected, out float pTimeLeft) {
        pTimeSpent = GameManager.Instance.TimeSpentOnLevel;
        pCollected = GameManager.Instance.PlayerScript.collectables;
        pTimeLeft = GameManager.Instance.TimeLeft;
    }

    public void SwitchLevel (int level){
        Application.LoadLevel(level);
    }
}
