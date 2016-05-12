using UnityEngine;
using System.Collections;

/// <summary>
/// Attached to the player. 
/// Handles basic  movement of the player.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    NavMeshAgent agent;
    
    private Camera cam;
    private Rigidbody rigibody;
    public float speed = 10.0f;
    private float gravity = 10.0f;
    private float maxVelocityChange = 10.0f;

    private NavMeshPath path;
    public LayerMask clickableLayer;
    bool eventCalled = false;
  //  Animator anim;
    void Start()
    {
        print(clickableLayer.value);
       // anim = GetComponentInChildren<Animator>();
        cam = Camera.main;
        rigibody = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();

        path = new NavMeshPath();
    }

    void FixedUpdate()
    {
        MouseMovement();
        //Movement();
        //rigibody.AddForce(new Vector3(0, -0.1f, 0));
    }

    void MouseMovement() {
        //agent.areaMask = 4;
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            NavMeshHit navHit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
            {

                if (hit.transform.GetComponent<CustomEvent>() != null && !eventCalled)
                {
                    print("ha");
                    hit.transform.GetComponent<CustomEvent>().OnCustomEvent();
                    eventCalled = true;
                }


                //Navigation
                if (NavMesh.SamplePosition(hit.point, out navHit, 1.0f, NavMesh.AllAreas))
                {
                    NavMesh.CalculatePath(transform.position, hit.point, NavMesh.AllAreas, path);
                    if (path.status == NavMeshPathStatus.PathComplete)
                    {
                        agent.destination = hit.point;
                    }
                }
            }
            //for (int i = 0; i < path.corners.Length - 1; i++)
            //    Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
        }
        else if (Input.GetMouseButtonUp(0)) { print("up"); eventCalled = false; }


    }

    void Movement()
    {
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        //if (input.x == 0 && input.z == 0) anim.SetBool("Moving", false); else anim.SetBool("Moving", true);

        Vector3 lookdir = cam.transform.forward;
        lookdir.y = 0;
        lookdir.Normalize();

        Vector3 targetVelocity = Vector3.zero;
        targetVelocity += lookdir * input.z;
        targetVelocity -= cam.transform.right * -input.x;

        targetVelocity *= (Mathf.Abs(input.x) == 1 && Mathf.Abs(input.z) == 1) ? 0.7f : 1;
        targetVelocity *= speed;

        if (targetVelocity.magnitude > 0)
        {
            transform.rotation = Quaternion.LookRotation(new Vector3(targetVelocity.x, 0, targetVelocity.z));
        }

        // Apply a force that attempts to reach our target velocity
        Vector3 velocity = rigibody.velocity;
        Vector3 velocityChange = (targetVelocity - velocity);
        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
        velocityChange.y = 0;

        rigibody.AddForce(velocityChange, ForceMode.VelocityChange);

        //Add gravity
        rigibody.AddForce(new Vector3(0, -gravity * rigibody.mass, 0));
    }
}
