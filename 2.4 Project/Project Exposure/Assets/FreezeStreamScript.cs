using UnityEngine;
using System.Collections;

public class FreezeStreamScript : BaseActivatable {

    public ParticleSystem particle;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void Activate() {
        particle.Play();
    }

    public override void DeActivate() {
        particle.Stop();
    }
}
