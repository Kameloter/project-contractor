using UnityEngine;
using System.Collections;

public class ValveLineJoint : MonoBehaviour {

   public ValveLineJoint connectTo;
    ParticleSystem smoke;
	// Use this for initialization
    void Awake()
    {
        smoke = GetComponentInChildren<ParticleSystem>();
    }
	void Start () {
       
        smoke.Pause();

	}
	public void DrawConnection(Color color)
    {
		if(smoke!=null && connectTo != null)
          smoke.Play();
       
    }
    public void DeleteConnection()
    {
        if (smoke == null)
            smoke = GetComponentInChildren<ParticleSystem>();
        smoke.Stop();
    }
	
}
