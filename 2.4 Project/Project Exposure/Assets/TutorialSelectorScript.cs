using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialSelectorScript : MonoBehaviour {
    [Header("Reference (optional)")]
    [SerializeField] Animator tutorialAnimator;

    GameObject repeatButton;

    void Awake () {
        if (tutorialAnimator == null) tutorialAnimator = GameObject.Find("Tutorial").GetComponent<Animator>();
        repeatButton = GameObject.Find("Repeat_Button");
        Exit();
    }
    
    public void ShowTutorial(string tutorialName) {
        tutorialAnimator.SetTrigger(tutorialName);
        repeatButton.GetComponent<Button>().onClick.RemoveAllListeners();
        repeatButton.GetComponent<Button>().onClick.AddListener(() => { tutorialAnimator.SetTrigger(tutorialName); });
        Exit();
    }	

    public void Exit() {
        gameObject.SetActive(false);
    }

    public void Enable() {
        if (tutorialAnimator.GetComponent<Canvas>().enabled) tutorialAnimator.SetTrigger("Exit");
        gameObject.SetActive(true);
    }
}
