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
        CloseSelector(); //doesnt work @ awake!
    }

    public void ShowTutorial(string tutorialName) {
        tutorialAnimator.SetTrigger(tutorialName);
        repeatButton.GetComponent<Button>().onClick.RemoveAllListeners();
        repeatButton.GetComponent<Button>().onClick.AddListener(() => { tutorialAnimator.SetTrigger(tutorialName); });
        CloseSelector();
    }

    public void CloseSelector() {
        //if (monitorAnimator.GetCurrentAnimatorStateInfo(0).IsName("Up")) monitorAnimator.SetTrigger("Down");
        GameManager.Instance.UiMonitor.HideMonitor();
        DisableSelector();
    }

    public void OpenSelector() {
        //if (monitorAnimator.GetCurrentAnimatorStateInfo(0).IsName("Down")) monitorAnimator.SetTrigger("Up");
        GameManager.Instance.UiMonitor.ShowMonitor();

        //if any tutorial is showing right now, disable it.
        if (tutorialAnimator.GetComponent<Canvas>().enabled) tutorialAnimator.SetTrigger("Exit");
        Invoke("EnableSelector", 0.3f); //magic value
    }

    public void Toggle() {  //changes state
        if (gameObject.activeSelf) CloseSelector();
        else OpenSelector();
    }

    void EnableSelector() {
        gameObject.SetActive(true);
    }

    void DisableSelector() {
        gameObject.SetActive(false);
    }
}
