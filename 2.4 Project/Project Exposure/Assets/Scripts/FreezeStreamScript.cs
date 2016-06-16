using UnityEngine;
using System.Collections;

/// <summary>
/// class to make a particle start to make object freeze
/// </summary>
public class FreezeStreamScript : BaseActivatable {
    //particlesystem to use
    [SerializeField] ParticleSystem particle;

    //start playing a start
    [SerializeField] bool PlayAtStart = false;

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
