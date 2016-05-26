using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SteamPipeJoint : MonoBehaviour {

    public SteamPipeJoint connectTo;
    public List<SmallValveSocket> poweredSockets;

    [HideInInspector]
    public float steamParticleSpeed { get { return smoke.startSpeed; } }

    ParticleSystem smoke;
    bool activated = false;
    
	// Use this for initialization
    void Awake()
    {
        smoke = GetComponentInChildren<ParticleSystem>();
        smoke.Pause();
    }

    void deactivatePoweredSockets()
    {
        foreach (SmallValveSocket poweredsocket in poweredSockets)
        {
            poweredsocket.DeactivateSocket();
        }
    }
    void activatePoweredSockets()
    {
        foreach (SmallValveSocket poweredsocket in poweredSockets)
        {
            if (poweredsocket.socketed != null)
                poweredsocket.ActivateInteractables();
        }
    }
    public void StopSteamConnection()
    {
      
        if (connectTo != null)
        {
            if (poweredSockets.Count > 0) deactivatePoweredSockets();
            smoke.Stop();
            activated = false;
            float distance = Vector3.Distance(transform.position, connectTo.transform.position);
            float waitTime = distance / connectTo.steamParticleSpeed;
          //  print("stoppping smoke" + waitTime);
            Invoke("StopSmoke", waitTime);
         
        }
    }
    void StopSmoke()
    {
        connectTo.StopSteamConnection();
       // print("stop");
    }

    void OnParticleCollision(GameObject go)
    {
        if (connectTo != null)
        {
            if (!activated)
            {
               
                //   print(" this -> " + go.name);
                if (poweredSockets.Count > 0) activatePoweredSockets();
                activated = true;
                smoke.Play();
            }
        }
    }
}
