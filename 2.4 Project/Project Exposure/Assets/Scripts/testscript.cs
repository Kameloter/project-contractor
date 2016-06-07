using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Used to acces the tutorial images for now.
/// </summary>

public class testscript : MonoBehaviour {
    public Animator anim;
    public Image image;

	void Start () {
        //print(transform.lossyScale.z);
	}
	
	void Update () {
	    if (Input.GetKey(KeyCode.Alpha1)) {
            image.color = Color.white;
            anim.SetTrigger("Valve");
        }
        if (Input.GetKey(KeyCode.Alpha2)) {
            image.color = Color.white;
            anim.SetTrigger("Laser");
        }
        if (Input.GetKey(KeyCode.Alpha3)) {
            image.color = Color.white;
            anim.SetTrigger("Bridge");
        }
        if (Input.GetKey(KeyCode.Alpha4)) {
            image.color = Color.white;
            anim.SetTrigger("Door");
        }
        if (Input.GetKey(KeyCode.Alpha5)) {
            image.color = Color.white;
            anim.SetTrigger("Ice");
        }
    }
}
