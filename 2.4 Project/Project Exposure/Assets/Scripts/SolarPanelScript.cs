using UnityEngine;
using System.Collections;

public class SolarPanelScript : MonoBehaviour {

    [System.Serializable]
    public struct MirrorInfo {
        public MirrorScript mirror;
        public int correctIndex;
    }

    [SerializeField]
    MirrorInfo[] mirrors;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        print(CheckMirrors());
	}

    bool CheckMirrors() {
        int correct = 0;
        for (int i = 0; i < mirrors.Length; i++) {
            if (mirrors[i].mirror.state == mirrors[i].correctIndex) {
                correct++;
            }
        }
        if (correct == mirrors.Length){
            return true;
        }

        return false;
    }
}
