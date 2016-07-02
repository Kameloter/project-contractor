using UnityEngine;
using System.Collections;


/// <summary>
/// This script is used on the final door of any level. It opens the door and starts blinking a green light. 
/// </summary>
public class AnimatedDoor : MonoBehaviour
{
   
	// Use this for initialization
	public void Start () {
        GetComponent<Animator>().SetTrigger("Open");
        GetComponentInChildren<BlinkRedLightControl>().StartBlinking();
	}
    
}
