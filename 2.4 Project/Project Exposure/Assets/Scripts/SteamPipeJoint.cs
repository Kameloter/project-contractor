using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This script is attached to a pipe-join which is part of the pipeline of a bigvalve. 
/// It activates and deactivates closest smallvavesockets to it so that when
///  steam reaches a joint it makes visual SENSE to the player.
/// </summary>
public class SteamPipeJoint : MonoBehaviour {

    public SteamPipeJoint connectTo;
    public List<SmallValveSocket> poweredSockets;

    [HideInInspector]
	/// <summary>
	/// used to calculate approx time for the particle to reach its point.
	/// </summary>
	/// <value>The steam particle speed.</value>
    public float steamParticleSpeed { get { return smoke.startSpeed; } }

    ParticleSystem smoke;
    bool activated = false;
    
    void Awake() {
        smoke = GetComponentInChildren<ParticleSystem>();
        smoke.Pause();
    }

    void deactivatePoweredSockets() {
        foreach (SmallValveSocket poweredsocket in poweredSockets) {
            poweredsocket.particle.Stop();
            poweredsocket.DeactivateSocket();
        }
    }

    void activatePoweredSockets() {
        foreach (SmallValveSocket poweredsocket in poweredSockets) {
            if (poweredsocket.socketed != null) poweredsocket.ActivateInteractables();
            else poweredsocket.particle.Play();
        }
    }

    public void AddToListItem(SmallValveSocket socket) {
        if (!poweredSockets.Contains(socket)) poweredSockets.Add(socket);
    }

	/// <summary>
	/// Stops the steam connection based on time required for the particle to reach the joint.
	/// </summary>
    public void StopSteamConnection() {
        if (connectTo != null) { //if we are connected to the joint
            if (poweredSockets.Count > 0) deactivatePoweredSockets(); //deactivate any powered sockets
            smoke.Stop(); //stop our smoke 
            activated = false;
            float distance = Vector3.Distance(transform.position, connectTo.transform.position); //calculate distance to our next joint
            float waitTime = distance / connectTo.steamParticleSpeed; //calculate the time it will need  for the last particle to get to the next jooint
            Invoke("StopSmoke", waitTime);//tell the next joint to stop steam after that time.
        }
    }

    void StopSmoke() {
        connectTo.StopSteamConnection();
    }

    void OnParticleCollision(GameObject go) {
        if (connectTo != null) {
            if (!activated) {
                if (poweredSockets.Count > 0) activatePoweredSockets();
                activated = true;
                smoke.Play();
            }
        }
    }
}
