using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

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
    public LayerMask interactablesLayer;
    public LayerMask navigationLayer;

    [HideInInspector]
    public bool allowNavigationInput;

    public Vector3 playerVelocity { get { return agent.velocity; }}
    float frameCount = 0;
    //  Animator anim;

    EventSystem eventSystem = EventSystem.current;

    void Start()
    {
        allowNavigationInput = true;
        //    print(clickableLayer.value);
        // anim = GetComponentInChildren<Animator>();
        cam = Camera.main;
        rigibody = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        
        path = new NavMeshPath();
       
    }
 
    void Update()     {
        PickUpRaycast();
        MouseMovement();
    }
  
    void PickUpRaycast()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit,interactablesLayer))
            {
                Debug.DrawLine(ray.origin, hit.point);
                Transform objectHit = hit.transform;
             //   print(" Object hit ===>>" + objectHit.gameObject.name);

                if (hit.transform.GetComponent<BaseInteractable>() != null)
                {
                    BaseInteractable interactable = hit.transform.GetComponent<BaseInteractable>();
                    interactable.OnInteractableClicked();

                    //StartCoroutine(dontNavigateWhenClickedOnInteractable());//method name explains.

                }
            }
        }
    }

    IEnumerator dontNavigateWhenClickedOnInteractable() {
        Debug.Log("set to false");
        allowNavigationInput = false; //disable navigation input.
        yield return  new WaitForSeconds(0.3f);
        allowNavigationInput = true; //alows navigation input again.
    }

    void MouseMovement() {
        if (Input.GetMouseButton(0)) {
            PointerEventData cursor = new PointerEventData(EventSystem.current);                            // This section prepares a list for all objects hit with the raycast
            cursor.position = Input.mousePosition;
            List<RaycastResult> objectsHit = new List<RaycastResult>();
            EventSystem.current.RaycastAll(cursor, objectsHit);
            if (objectsHit.Count > 0) return;

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            NavMeshHit navHit;
            if (Physics.Raycast(ray, out hit, 100)) {
                Debug.DrawLine(ray.origin, hit.point, Color.red);


                if (NavMesh.SamplePosition(hit.point, out navHit, 1.0f, NavMesh.AllAreas)) {
                    NavMesh.CalculatePath(transform.position, hit.point, NavMesh.AllAreas, path);
                    if (path.status == NavMeshPathStatus.PathComplete) {
                        //  Debug.Log("SET PATH WHEN SHOULD NOT !");
                        agent.destination = hit.point;
                    }
                }
            }
        }
    }

    public void SendAgent(Transform interactable) {
        agent.SetDestination(interactable.position);
        GameManager.Instance.ClickedObject = interactable.gameObject;
      //  print("set destination to " + interactable.gameObject.name);
    }

    public void StopAgent() {
        agent.Stop();
      //  print("Stopped agent");
    }

    public void ResumeAgent() {
        agent.ResetPath();
        agent.Resume();
    }

    void Movement() {
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
    
        Vector3 lookdir = cam.transform.forward;
        lookdir.y = 0;
        lookdir.Normalize();

        Vector3 targetVelocity = Vector3.zero;
        targetVelocity += lookdir * input.z;
        targetVelocity -= cam.transform.right * -input.x;

        targetVelocity *= (Mathf.Abs(input.x) == 1 && Mathf.Abs(input.z) == 1) ? 0.7f : 1;
        targetVelocity *= speed;

        if (targetVelocity.magnitude > 0) {
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
