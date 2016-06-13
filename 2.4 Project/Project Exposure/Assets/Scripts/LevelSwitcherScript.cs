using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelSwitcherScript : MonoBehaviour {

    public int level = 0;

    float timeSpent;
    int collected;
    float timeLeft;

    //scorescreen
    GameObject scoreScreen;
    Button continueBtn, stayBtn;
    Text timeSpentText, timeLeftText, collectedText;

    void Start() {
        SetupScoreScreenElements();
    }

    /// <summary>
    /// Setups ScoreScreen elements and disables the scorescreen.
    /// </summary>
    void SetupScoreScreenElements() {
        scoreScreen = GameObject.Find("ScoreScreen");
        timeSpentText = GameObject.Find("TimeSpentAmount").GetComponent<Text>();
        collectedText = GameObject.Find("CollectablesAmount").GetComponent<Text>();
        timeLeftText = GameObject.Find("RemainingTimeAmount").GetComponent<Text>();

        continueBtn = scoreScreen.transform.FindChild("ContinueButton").GetComponent<Button>();
        stayBtn = scoreScreen.transform.FindChild("StayButton").GetComponent<Button>();

        continueBtn.onClick.RemoveAllListeners();
        continueBtn.onClick.AddListener(() => { Continue(); });

        stayBtn.onClick.RemoveAllListeners();
        stayBtn.onClick.AddListener(() => { Stay(); });

        scoreScreen.SetActive(false);
    }

    /// <summary>
    /// Stops player movement and shows the ScoreScreen.
    /// </summary>
    void EnableScoreScreen() {
        GameManager.Instance.PlayerMovement.StopAgent();
        scoreScreen.SetActive(true);
    }

    void DisableScoreScreen() {
        GameManager.Instance.PlayerMovement.ResumeAgent();
        scoreScreen.SetActive(false);
    }

    public void Continue() { //Called by button on scorescreen.
        SwitchLevel(level);
    }

    public void Stay() { //Called by button on scorescreen.
        DisableScoreScreen();
    }
    
    void OnTriggerEnter(Collider other) {
        if (other.name == "Player") {
            GetStats(out timeSpent, out collected, out timeLeft);

            //show UI
            EnableScoreScreen();
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
