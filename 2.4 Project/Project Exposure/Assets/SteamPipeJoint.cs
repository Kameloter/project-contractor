using UnityEngine;
using System.Collections;

public class SteamPipeJoint : MonoBehaviour {

   public SteamPipeJoint connectTo;

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


    public void StopSteamConnection()
    {
        //if(smoke != null)
        //{
       
        //}

        activated = false;
        if (connectTo != null)
        {
            smoke.Stop();
            float distance = Vector3.Distance(transform.position, connectTo.transform.position);
            float waitTime = distance / connectTo.steamParticleSpeed;
            Invoke("StopSmoke", waitTime);
        }
    }
    void StopSmoke()
    {
        connectTo.StopSteamConnection();
        print("stop");
    }

    void OnParticleCollision(GameObject go)
    {
        if (connectTo != null)
        {
            if (!activated)
            {
                print(" this -> " + go.name);
                activated = true;
                smoke.Play();
            }
        }

    }
}
