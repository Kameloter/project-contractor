using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialSelectorScript : MonoBehaviour {
    [Header("Reference (optional)")]
    [SerializeField] Animator tutorialAnimator;
    [SerializeField] Animator monitorAnimator;

    GameObject repeatButton;

    void Awake () {
        if (tutorialAnimator == null) tutorialAnimator = GameObject.Find("Tutorial").GetComponent<Animator>();
        if (monitorAnimator == null) monitorAnimator = GameObject.Find("MonitorObj").GetComponent<Animator>();
        repeatButton = GameObject.Find("Repeat_Button");
        CloseSelector();
    }
    
    public void ShowTutorial(string tutorialName) {
        tutorialAnimator.SetTrigger(tutorialName);
        repeatButton.GetComponent<Button>().onClick.RemoveAllListeners();
        repeatButton.GetComponent<Button>().onClick.AddListener(() => { tutorialAnimator.SetTrigger(tutorialName); });
        CloseSelector();
    }	

    public void CloseSelector() {
        if (monitorAnimator.GetCurrentAnimatorStateInfo(0).IsName("Up")) monitorAnimator.SetTrigger("Down");
        DisableSelector();
    }

    public void OpenSelector() {
        if (monitorAnimator.GetCurrentAnimatorStateInfo(0).IsName("Down")) monitorAnimator.SetTrigger("Up");
        
        //if any tutorial is showing right now, disable it.
        if (tutorialAnimator.GetComponent<Canvas>().enabled) tutorialAnimator.SetTrigger("Exit");
        Invoke("EnableSelector", 0.3f); //magic value
    }

    void MonitorUp() {
        monitorAnimator.SetTrigger("Up");
    }

    void MonitorDown() {
        monitorAnimator.SetTrigger("Down");
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
