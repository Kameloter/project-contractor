using UnityEngine;
using System.Collections;

public class FreezeStreamScript : BaseActivatable {

    public ParticleSystem particle;

    public bool PlayAtStart = false;

    public override void Start()
    {
        if (PlayAtStart)
        {
            particle.Play();
        }
    }
    public override void Activate() {
        particle.Play();
    }

    public override void Deactivate() {
        particle.Stop();
    }
}
