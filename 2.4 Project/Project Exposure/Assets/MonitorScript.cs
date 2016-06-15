using UnityEngine;
using System.Collections;

/// <summary>
/// This script is used to automatically set the Main Camera as Render Camera for the 
/// canvas the monitor is on, if there is none specified.
/// </summary>
public class MonitorScript : MonoBehaviour {
     Canvas canvas;

    void Awake() {
        canvas = GetComponent<Canvas>();
        if (canvas.worldCamera == null) canvas.worldCamera = Camera.main;
    }
}
