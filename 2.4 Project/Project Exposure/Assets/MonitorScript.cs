using UnityEngine;
using System.Collections;

/// <summary>
/// This script is used to automatically set the Main Camera as Render Camera for the 
/// canvas the monitor is on, if there is none specified.
/// It also takes care of popping the monitor up / down.
/// </summary>

[ExecuteInEditMode]
public class MonitorScript : MonoBehaviour {
    [ReadOnly][SerializeField] Canvas canvas;
    [ReadOnly][SerializeField] Animator uiMonitorAnimator;

    void Awake() {
        canvas = GetComponent<Canvas>();
        if (canvas.worldCamera == null) canvas.worldCamera = Camera.main;

        uiMonitorAnimator = GetComponentInChildren<Animator>(); //needs to be in Awake
        if (uiMonitorAnimator == null) Debug.LogError("Couldn't find MonitorAnimator (UI)");
    }

    /// <summary>
    /// Triggers the showMonitor animation if its not in 'Show'-state already.
    /// </summary>
    public void ShowMonitor() {
        if (uiMonitorAnimator.GetCurrentAnimatorStateInfo(0).IsName("Down")) uiMonitorAnimator.SetTrigger("Up");
    }

    /// <summary>
    /// Triggers the hideMonitor animation if its not in 'Hide'-state already.
    /// </summary>
    public void HideMonitor() {
        if (uiMonitorAnimator.GetCurrentAnimatorStateInfo(0).IsName("Up")) uiMonitorAnimator.SetTrigger("Down");
    }

}
