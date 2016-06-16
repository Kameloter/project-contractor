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
        GameManager.Instance.UiMonitor.ShowMonitor();
        scoreScreen.SetActive(true);
    }
    
    /// <summary>
    /// Resumes player movement and removes the scorescreen from view.
    /// </summary>
    public void DisableScoreScreen() {
        GameManager.Instance.UiMonitor.HideMonitor();
        scoreScreen.SetActive(false);
    }

    /// <summary>
    /// Updates the ScoreScreen.
    /// </summary>
    /// <param name="timeSpent"> will be rounded </param>
    /// <param name="timeLeft"> will be rounded </param>
    /// <param name="collected"></param>
    public void UpdateScoreScreen(float timeSpent, float timeLeft, int collected) {
        timeSpentText.text = Mathf.Round(timeSpent).ToString() + " seconden";
        timeLeftText.text = Mathf.Round(timeLeft).ToString() + " seconden";
        collectedText.text = collected.ToString();
    }

    /// <summary>
    /// Called by ContinueButton on ScoreScreen. It uses a LevelSwitcher to advance to the in the levelSwitcher specified level.
    /// </summary>
    void Continue() { //Called by button on scorescreen.
        GameObject levelSwitcher = GameObject.FindGameObjectWithTag(Tags.levelSwitcher);
        if (levelSwitcher != null) levelSwitcher.GetComponent<LevelSwitcherScript>().SwitchLevel();
        else Debug.LogError("Couldn't find LevelSwitcher.");
    }

    /// <summary>
    /// Called by StayButton on ScoreScreen. It disables the score screen.
    /// </summary>
    void Stay() { //Called by button on scorescreen.
        DisableScoreScreen();
    }
}
