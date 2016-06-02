using UnityEngine;
using System.Collections;

public class MeltableScript : MonoBehaviour {

    public bool melting = false;
    float timer = 0;
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        if (melting) {
            for (int i = 0; i < transform.childCount; i++) {
                transform.GetChild(i).transform.localScale = Vector3.Lerp(transform.GetChild(i).transform.localScale, new Vector3(0.01f,0.01f,0.01f), 1 * Time.deltaTime);
            }
            timer += Time.deltaTime;

            if (timer >= 2) {
                Destroy(gameObject);
            }

        }
	}
}
