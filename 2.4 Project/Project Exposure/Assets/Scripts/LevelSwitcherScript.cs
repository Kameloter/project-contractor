using UnityEngine;
using System.Collections;

/// <summary>
/// This script is attached on a trigger that will eventually switch to a new level.
/// 
/// Also it pops up the final score screen. (Has to change to a different script .. a note to the one that added it here)
/// </summary>
public class LevelSwitcherScript : MonoBehaviour {
    public int level = 0;

    float timeSpent;
    int collected;
    float timeLeft;

    void Update() {
        if (Input.GetKeyDown(KeyCode.P)) { GameManager.Instance.ScoreScreen.UpdateScoreScreen(timeSpent, timeLeft, collected); GameManager.Instance.ScoreScreen.EnableScoreScreen(); }
        if (Input.GetKeyDown(KeyCode.O)) { GameManager.Instance.EndScreen.EnableEndScreen(); }
    }

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
        pCollected = GameManager.Instance.PlayerScript.Collectables;
        pTimeLeft = GameManager.Instance.gameTimeLeft;
    }

	/// <summary>
	/// Switches the level to the index "level" provided on the script 
	/// </summary>
    public void SwitchLevel (){
        GameManager.Instance.SceneManager.SwitchToLevel(level);
    }
}
