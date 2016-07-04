using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

/// <summary>
/// This script is controlling the end score screen. 
/// </summary>
public class EndScreenScript : MonoBehaviour {
    //endscreen
    GameObject endScreen;
    [SerializeField] Text playerName, starsEarned, gameScore;
    [SerializeField] Animator star_1;

    bool completedUnderMinute, foundAllCollectables;
    int _levelScore;

    void Start() {
        endScreen = transform.gameObject;
        SetupEndScreenElements();
    }

    /// <summary>
    /// Setups EndScreen elements and disables the scorescreen.
    /// </summary>
    void SetupEndScreenElements() {
        if (NullCheck()) return;
        endScreen.SetActive(false);
    }

    /// <summary>
    /// Checks if any reference is null. Returns true if any is.
    /// </summary>
    /// <returns></returns>
    bool NullCheck() {
        if (!star_1) {
            Debug.LogError("At least one of the EndScreen references is missing. Please check the EndScreen gameObject.", transform);
            Debug.Break();
            return true;
        } else return false;
    }

    /// <summary>
    /// Stops player movement and shows the endScreen.
    /// </summary>
    public void EnableEndScreen() {
        GameManager.Instance.UiMonitor.ShowMonitor();
        GameManager.Instance.TutorialSelector.helpButton.interactable = false;
        UpdateEndScreen();
        endScreen.SetActive(true);
        star_1.SetTrigger("GiveStar");
    }

    /// <summary>
    /// Resumes player movement and removes the endscreen from view.
    /// Added to be complete.
    /// </summary>
    public void DisableEndScreen() {
        GameManager.Instance.UiMonitor.HideMonitor();
        GameManager.Instance.TutorialSelector.helpButton.interactable = true;
        endScreen.SetActive(false);
    }

    public void UpdateEndScreen() {
        if (Environment.GetCommandLineArgs().Length > 3) playerName.text = Environment.GetCommandLineArgs()[4];
        starsEarned.text = (Game.Score / 10).ToString();
        gameScore.text = Game.Score.ToString();
    }
}
