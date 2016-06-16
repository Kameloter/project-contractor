using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
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

    public int CollectablesAvailable {
        get { return _collectablesAvailable; }
        set { _collectablesAvailable = value; }
    }
}
