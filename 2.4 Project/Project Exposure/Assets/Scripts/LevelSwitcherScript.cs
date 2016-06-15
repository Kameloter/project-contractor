using UnityEngine;
using System.Collections;

public class LevelSwitcherScript : MonoBehaviour {

    public int level = 0;

    float timeSpent;
    int collected;
    float timeLeft;
            
    void OnTriggerEnter(Collider other) {
        if (other.name == "Player") {
            GetStats(out timeSpent, out collected, out timeLeft);

            //show UI
            GameManager.Instance.ScoreScreen.UpdateScoreScreen(timeSpent, timeLeft, collected);
            GameManager.Instance.ScoreScreen.EnableScoreScreen();
        }
    }

    void GetStats(out float pTimeSpent, out int pCollected, out float pTimeLeft) {
        pTimeSpent = GameManager.Instance.TimeSpentOnLevel;
        pCollected = GameManager.Instance.PlayerScript.collectables;
        pTimeLeft = GameManager.Instance.TimeLeft;
    }

    public void SwitchLevel (int level){
        GameManager.Instance.SceneManager.SwitchToLevel(level);
    }
}
