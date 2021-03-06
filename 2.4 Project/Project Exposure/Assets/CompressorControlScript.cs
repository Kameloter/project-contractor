﻿using UnityEngine;
using System.Collections;

/// <summary>
/// This script controls the compressor(a component of the geothermal process)
/// </summary>
public class CompressorControlScript : BaseActivatable {
	
    [Tooltip("The object that holds all the pistons.")]
    [SerializeField]
    private GameObject pistonsObject;

    [Tooltip("The object that holds the compressor model.")]
    [SerializeField]
    private GameObject compressorModel;

    [Tooltip("The object holding the steam particles")]
    [SerializeField]
    private ParticleSystem sphereSteams;
    [SerializeField]
    private ParticleSystem startSteam;

    //all different pistons animator
    Animator[] myPistonsAnimator;
    Animator myAnimator;


	// Use this for initialization
	public override void Start ()
    {
        
        if (pistonsObject == null) { Debug.LogError("Pistons object missing from ->  " + gameObject.name + " .", transform); }
        else { myPistonsAnimator = pistonsObject.GetComponentsInChildren<Animator>(); }

        if (compressorModel == null) { Debug.LogError("Piston model object missing from ->  " + gameObject.name + " .", transform); }
        else { myAnimator = compressorModel.GetComponent<Animator>(); if (myAnimator == null) Debug.LogError("Unable to find animator in compressor model IN + " + gameObject.name); }


		//stop particle on start.
        sphereSteams.Stop();
    }

    /// <summary>
    /// Starts compressing steam(Animations and particles start running).
    /// </summary>
    public void RunSteamCompressor()
    {
        foreach (Animator anim in myPistonsAnimator) //Start animation
        {
            if (anim != null)
                anim.SetTrigger("Work");
        }
        myAnimator.SetTrigger("Work");
        sphereSteams.Play();
    }

    /// <summary>
    /// Releases steam to the other rooms, as in the tutorial.
    /// </summary>
    public void ReleaseSteam()
    {
      
        startSteam.Play();
    }

    public override void Activate()
    {
        
        base.Activate();
        ReleaseSteam();
    }
    public override void Deactivate()
    {
        base.Deactivate();
    }
 
}
