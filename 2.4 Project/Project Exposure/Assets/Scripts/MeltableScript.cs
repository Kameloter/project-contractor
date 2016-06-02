﻿using UnityEngine;
using System.Collections;

public class MeltableScript : MonoBehaviour {

    public bool melting = false;
    float timer = 0;

    public GameObject optionalPath;
    bool playedCamera = false;
    // Use this for initialization
    void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        if (melting) {

            if (optionalPath != null && !playedCamera)
            {
                Camera.main.GetComponent<CameraControl>().StartCutscene(optionalPath);
                playedCamera = true;
            }
            for (int i = 0; i < transform.childCount; i++) {
                transform.GetChild(i).transform.localScale = Vector3.Lerp(transform.GetChild(i).transform.localScale, new Vector3(0.01f,0.01f,0.01f), 0.3f * Time.deltaTime);
            }
            timer += Time.deltaTime;

            if (timer >= 3) {
                Destroy(gameObject);
            }

        }
	}
}
