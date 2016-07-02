using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
/// <summary>
/// This script holds important scene stats.
/// </summary>
public class SceneStats : MonoBehaviour {

    [Header("Level Stats")]
    [ReadOnly] public int _collectablesAvailable = 0;   //keeps track of the amount of collectables to be found
    public int TimeNeededForLevel = 10;

    void Awake() { 
        //Count collectables available this scene -- needs to be in awake to prevent the hud getting updated before counting.
        _collectablesAvailable = GameObject.FindGameObjectsWithTag(Tags.collectable).Length;
    }

    void Update() {
        if (!Application.isPlaying) CollectablesAvailable = GameObject.FindGameObjectsWithTag(Tags.collectable).Length;
    }

	/// <summary>
	/// The collectables in this level. get/set.
	/// </summary>
    public int CollectablesAvailable {
        get { return _collectablesAvailable; }
        set { _collectablesAvailable = value; }
    }
}
