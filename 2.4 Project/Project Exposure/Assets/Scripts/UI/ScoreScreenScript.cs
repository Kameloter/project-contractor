using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

/// <summary>
/// This script is controlling the final score screen. 
/// </summary>
public class ScoreScreenScript : MonoBehaviour {
    //scorescreen
    GameObject scoreScreen;
    [SerializeField] Button continueBtn, stayBtn;
    [SerializeField] Text levelTimeSpent, gameTimeLeft, collectedText;
    [SerializeField] Animator star_1, star_2, star_3;

    bool completedUnderMinute, foundAllCollectables;
    int _levelScore;


    void Start() {
        scoreScreen = transform.gameObject;
        
        SetupScoreScreenElements();
    }

    /// <summary>
    /// Setups ScoreScreen elements and disables the scorescreen.
    /// </summary>
    void SetupScoreScreenElements() {
        if (NullCheck()) return; 

        continueBtn.onClick.RemoveAllListeners();
        continueBtn.onClick.AddListener(() => { Continue(); });

        stayBtn.onClick.RemoveAllListeners();
        stayBtn.onClick.AddListener(() => { Stay(); });

        scoreScreen.SetActive(false);
    }

    /// <summary>
    /// Checks if any reference is null. Returns true if any is.
    /// </summary>
    /// <returns></returns>
    bool NullCheck() {
        if (!continueBtn || !stayBtn || !levelTimeSpent || !gameTimeLeft || !collectedText || !star_1 || !star_2 || !star_3) {
            Debug.LogError("At least one of the ScoreScreen references is missing. Please check the ScoreScreen gameObject.", transform);
            Debug.Break();
            return true;
        } else return false;
    }

    /// <summary>
    /// Stops player movement and shows the ScoreScreen.
    /// </summary>
    public void EnableScoreScreen() {
        GameManager.Instance.UiMonitor.ShowMonitor();
        GameManager.Instance.TutorialSelector.helpButton.interactable = false;

        scoreScreen.SetActive(true);
        ScoreLogic(); //needs to be called after activating the scorescreen
        InvokeRepeating("UpdateGameTimeLeft", 0, 0.01f); //sync GameTimeLeft
    }
    
    /// <summary>
    /// Resumes player movement and removes the scorescreen from view.
    /// </summary>
    public void DisableScoreScreen() {
        GameManager.Instance.UiMonitor.HideMonitor();
        GameManager.Instance.TutorialSelector.helpButton.interactable = true;
        scoreScreen.SetActive(false);
    }

    /// <summary>
    /// Updates the ScoreScreen. Called by LevelSwitcher.
    /// </summary>
    /// <param name="timeSpent"> will be rounded </param>
    /// <param name="timeLeft"> will be rounded </param>
    /// <param name="collected"></param>
    public void UpdateScoreScreen(float timeSpent, float timeLeft, int collected) {
        levelTimeSpent.text = Mathf.Round(timeSpent).ToString() + " s";
        gameTimeLeft.text = Mathf.Round(timeLeft).ToString() + " s";
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

    void AddStar(int starNumber) {
        string trigger = "GiveStar";
        switch (starNumber) {
            case 3:
                star_3.SetTrigger(trigger);
                goto case 2;
            case 2:
                star_2.SetTrigger(trigger);
                goto case 1;
            case 1:
                star_1.SetTrigger(trigger);
                break;
            default:
                Debug.LogError("AddStar: Insert a valid number (1-3).", transform);
                break;
        }
    }

    void ScoreLogic() {
        completedUnderMinute = false;
        foundAllCollectables = false;
        if (GameManager.Instance.timeSpentLevel <= 60) completedUnderMinute = true;
        if (GameManager.Instance.PlayerScript.Collectables >= GameManager.Instance.SceneStats.CollectablesAvailable) foundAllCollectables = true;

        if      (completedUnderMinute && foundAllCollectables)  { AddStar(3); LevelScore = 30; } 
        else if (completedUnderMinute || foundAllCollectables)  { AddStar(2); LevelScore = 20; } 
        else                                                    { AddStar(1); LevelScore = 10; }
    }

    public int LevelScore {
        get { return _levelScore; }
        set { _levelScore = value; }
    }

    /// <summary>
    /// Updates the GameTimeLeft shown on the scorescreen.
    /// </summary>
    void UpdateGameTimeLeft() {
        gameTimeLeft.text = GameManager.Instance.UpdateGameTimerText();
    }
}
