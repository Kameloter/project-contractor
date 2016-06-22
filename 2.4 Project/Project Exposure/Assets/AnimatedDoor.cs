using UnityEngine;
using System.Collections;

public class AnimatedDoor : MonoBehaviour
{
   
	// Use this for initialization
	public void Start () {
        GetComponent<Animator>().SetTrigger("Open");
        GetComponentInChildren<BlinkRedLightControl>().StartBlinking();

	}
    
}
