using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameLogicLevel1 : MonoBehaviour {
    public GameObject tapHereValveHead, tapHereValveSocket;
    bool tapValveHeadActivated = true, tapValveSocketActivated = true;

    void Start() {
        tapHereValveSocket.SetActive(false);
    }

    void Update() {
        if (tapValveHeadActivated && GameManager.Instance.ClickedObject) {
            tapHereValveHead.SetActive(false);
            tapHereValveSocket.SetActive(true);

            tapValveHeadActivated = false;
        } 
    }

    public void OnInteract() {
        if (tapValveSocketActivated) {
            tapHereValveSocket.SetActive(false);
            tapValveSocketActivated = false;
        }
    }
}
