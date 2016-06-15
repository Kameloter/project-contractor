using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreScreenScript : MonoBehaviour {
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
    public void EnableScoreScreen() {
        GameManager.Instance.PlayerMovement.StopAgent();
        scoreScreen.SetActive(true);
    }
    
    /// <summary>
    /// Resumes player movement and removes the scorescreen from view.
    /// </summary>
    public void DisableScoreScreen() {
        GameManager.Instance.PlayerMovement.ResumeAgent();
        scoreScreen.SetActive(false);
    }

    public void UpdateScoreScreen(float timeSpent, float timeLeft, int collected) {
        timeSpentText.text = Mathf.Round(timeSpent).ToString() + " seconden";
        timeLeftText.text = Mathf.Round(timeLeft).ToString() + " seconden";
        collectedText.text = collected.ToString();
    }

    void Continue() { //Called by button on scorescreen.
        //SwitchLevel(level);
    }

    void Stay() { //Called by button on scorescreen.
        DisableScoreScreen();
    }

}
