using UnityEngine;
using System.Collections;

public class AnimatedDoor : BaseActivatable
{
    bool activated = false;
    Animator myAnimator;
	// Use this for initialization
	public override void Start () {
        base.Start();
        myAnimator = GetComponent<Animator>();

	}

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            Activate();

        }
        if (Input.GetKeyDown(KeyCode.H))
        {

            Deactivate();
        }
    }
    public override void Activate()
    {
        myAnimator.speed = 1;
        myAnimator.SetTrigger("Open");
        base.Activate();
    }

    public override void Deactivate()
    {
        myAnimator.speed = -1;
        myAnimator.Play("Close");
        base.Deactivate();
    }
}
