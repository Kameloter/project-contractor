using UnityEngine;
using System.Collections;

public class FreezeStreamScript : BaseActivatable {

    public ParticleSystem particle;

    public override void Activate() {
        particle.Play();
    }

    public override void DeActivate() {
        particle.Stop();
    }
}
