using UnityEngine;
using System.Collections;

public class ValveLineJoint : MonoBehaviour {

   public ValveLineJoint connectTo;

	// Use this for initialization
	void Start () {
	    
	}
	public void DrawConnection(Color color)
    {
        if(connectTo != null)
        {
            float distance = Vector3.Distance(transform.position, connectTo.transform.position);
            Debug.DrawRay(transform.position, transform.right * distance, color,1000);
        }
       
    }
	
}
