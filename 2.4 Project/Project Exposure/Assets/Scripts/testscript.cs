using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class testscript : MonoBehaviour {
    public Animator anim;
    public Image image;

	void Start () {
        //print(transform.lossyScale.z);
	}
	
	void Update () {
	    if (Input.GetKey(KeyCode.H)) {
            image.color = Color.white;
            anim.SetTrigger("Valve");
        }
        if (Input.GetKey(KeyCode.J)) {
            image.color = Color.white;
            anim.SetTrigger("Laser");
        }
    }
}
