using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameLogicLevel2 : MonoBehaviour
{
    bool triggered = false;
    // Use this for initialization
    void Start()
    {
        GameObject.Find("Repeat_Button").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("Repeat_Button").GetComponent<Button>().onClick.AddListener(() => { GameObject.Find("Tutorial").GetComponent<Animator>().SetTrigger("Ice"); });
        GameObject.Find("Tutorial").GetComponent<Animator>().SetTrigger("Ice");
    }

    //void ShowHint()
    //{
    //    if (!triggered)
    //    {
    //        GameObject.Find("Repeat_Button").GetComponent<Button>().onClick.AddListener(() => { GameObject.Find("Tutorial").GetComponent<Animator>().SetTrigger("Laser"); });
    //        GameObject.Find("Tutorial").GetComponent<Animator>().SetTrigger("Laser");
    //        triggered = true;
    //    }
    //}

    //void OnEnable()
    //{
    //    CameraControl.OnCameraPathEnd += ShowHint;
    //}

    //void OnDisable()
    //{
    //    CameraControl.OnCameraPathEnd -= ShowHint;
    //}
}
