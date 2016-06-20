using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class ScoreScreenScript : MonoBehaviour {
    //scorescreen
    GameObject scoreScreen;
    [SerializeField] Button continueBtn, stayBtn;
    [SerializeField] Text timeSpentText, timeLeftText, collectedText;
    [SerializeField] Animator star_1, star_2, star_3;

    void Start() {
        scoreScreen = transform.gameObject;
        
        SetupScoreScreenElements();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.U)) AddStar(1);
        if (Input.GetKeyDown(KeyCode.I)) AddStar(2);
        if (Input.GetKeyDown(KeyCode.O)) AddStar(3);
        if (Input.GetKeyDown(KeyCode.P)) AddStar(4);
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
        if (!continueBtn || !stayBtn || !timeSpentText || !timeLeftText || !collectedText || !star_1 || !star_2 || !star_3) {
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
}
