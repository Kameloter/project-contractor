using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameLogicLevel1 : MonoBehaviour {

    bool triggered = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void ShowHint()
    {
        if (!triggered)
        {
            GameObject.Find("Repeat_Button").GetComponent<Button>().onClick.RemoveAllListeners();
            GameObject.Find("Repeat_Button").GetComponent<Button>().onClick.AddListener(() =>{ GameObject.Find("Tutorial").GetComponent<Animator>().SetTrigger("Valve");});
            GameObject.Find("Tutorial").GetComponent<Animator>().SetTrigger("Valve");
            triggered = true;
        }
    }

    void OnEnable()
    {
       // CameraControl.OnCameraPathEnd += ShowHint;
    }

    void OnDisable()
    {
      //  CameraControl.OnCameraPathEnd -= ShowHint;
    }
}
