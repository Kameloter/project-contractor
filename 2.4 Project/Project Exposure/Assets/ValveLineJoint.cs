using UnityEngine;
using System.Collections;

public class ValveLineJoint : MonoBehaviour {

   public ValveLineJoint connectTo;
    ParticleSystem smoke;
	// Use this for initialization
	void Start () {
       
        smoke.Pause();
        if (smoke == null)
            Debug.Log("ebi sa ");
	}
	public void DrawConnection(Color color)
    {
        smoke.Play();
       
    }
    public void DeleteConnection()
    {
        if (smoke == null)
            smoke = GetComponentInChildren<ParticleSystem>();
        smoke.Stop();
    }
	
}
