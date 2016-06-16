using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialSelectorScript : MonoBehaviour {
    [Header("Reference (optional)")]
    [SerializeField] Animator tutorialAnimator;

    GameObject repeatButton;

    void Start() {
        if (tutorialAnimator == null) tutorialAnimator = GameObject.Find("Tutorial").GetComponent<Animator>();
        repeatButton = GameObject.Find("Repeat_Button");
        Invoke("CloseSelector", 0.1f); //disable tutorialselector (invoke to prevent early call errors)
    }

    /// <summary>
    /// Closes the TutorialSelector and shows the selected tutorial.
    /// Also fixes the repeat button to show the correct tutorial.
    /// </summary>
    /// <param name="tutorialName">Fill in the name of the tutorial at the button's onClick in the inspector.</param>
    public void ShowTutorial(string tutorialName) {
        tutorialAnimator.SetTrigger(tutorialName);
        repeatButton.GetComponent<Button>().onClick.RemoveAllListeners();
        repeatButton.GetComponent<Button>().onClick.AddListener(() => { tutorialAnimator.SetTrigger(tutorialName); });
        GameManager.Instance.UiMonitor.ShowMonitor(); //show monitor if its not shown yet.
        DisableSelector(); //disable TutorialSelector (for if its on).
    }

    /// <summary>
    /// Opens the TutorialSelector (including the Monitor).
    /// Stops playerMovement.
    /// </summary>
    public void OpenSelector() {
        GameManager.Instance.UiMonitor.ShowMonitor();

        //if any tutorial is showing right now, disable it.
        if (tutorialAnimator.GetComponent<Canvas>().enabled) tutorialAnimator.SetTrigger("Exit");
        EnableSelector();
    }

    /// <summary>
    /// Closes the TutorialSelector (including the Monitor).
    /// Resumes playerMovement.
    /// </summary>
    public void CloseSelector() {
        GameManager.Instance.UiMonitor.HideMonitor();
        DisableSelector();
    }

    /// <summary>
    /// Switches between active and inactive GameObject state.
    /// Used when the HelpButton is pressed.
    /// </summary>
    public void Toggle() {  //changes state
        if (gameObject.activeSelf) CloseSelector();
        else OpenSelector();
    }

    /// <summary>
    /// Enables the TutorialSelector's gameObject.
    /// Used @ CloseSelector().
    /// </summary>
    void EnableSelector() {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Disables the TutorialSelector's gameObject.
    /// Used in an invoke call (@ Start().
    /// Used @ CloseSelector;
    /// </summary>
    void DisableSelector() {
        gameObject.SetActive(false);
    }
}
