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
    /// <summary>
    /// The navmesh agent component.
    /// </summary>
    NavMeshAgent agent;
    /// <summary>
    /// Prefab of the movement click pointer
    /// </summary>
    public Object movePointer;
    /// <summary>
    /// A ref to navmeshpath so we can calculate a valid path on navmesh click and store it here, so it can be used everywhere in the class.
    /// </summary>
    private NavMeshPath path;
    /// <summary>
    /// The layer with all interactable objects (clickable objects)
    /// </summary>
    public LayerMask interactablesLayer;

    public Vector3 playerVelocity { get { return agent.velocity; }}

    EventSystem eventSystem = EventSystem.current;

    void Start()
    { 
        agent = GetComponent<NavMeshAgent>();
        if (agent == null) Debug.LogError("Nav mesh not found on player where PlayerMovement component is attached", transform);
        path = new NavMeshPath();
    }
 
    void Update()
    {
        //Interraction raycast
        PlayerInterraction();
        //player touch movement
        PlayerTouchMovement();
    }
  
    /// <summary>
    /// One time ray cast per click, used to detect if we clicked any interactable object.
    /// </summary>
    void PlayerInterraction()
    {
        //Ref to store back what we hit
        RaycastHit hit;
        //The ray it self from screen to world space.
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButtonDown(0)) //if we clicked the left mouse button
        {
            if (Physics.Raycast(ray, out hit,interactablesLayer)) //check if we hit something
            {
                //Debug.DrawLine(ray.origin, hit.point);
                Transform objectHit = hit.transform;

                if (hit.transform.GetComponent<BaseInteractable>() != null) //if the object we hit has the interactable script
                {
                    //call the base function for ANY interactable, then derrived classes override that and
                    // do their own behaviour.
                    BaseInteractable interactable = hit.transform.GetComponent<BaseInteractable>();
                    interactable.OnInteractableClicked();
                }
            }
        }
    }

    /// <summary>
    /// Nav-mesh raycast movement
    /// </summary>
    void PlayerTouchMovement() {    // This section prepares a list for all objects hit with the raycast
        //If we are pressing left mouse button
        if (Input.GetMouseButton(0))
        {

       
            //Basicly we make a pointer thats used for UI raycast
            PointerEventData cursor = new PointerEventData(EventSystem.current);                        
            cursor.position = Input.mousePosition;     //set its position to our cursor.
            List<RaycastResult> objectsHit = new List<RaycastResult>(); //make a list of possible hit objects
            EventSystem.current.RaycastAll(cursor, objectsHit); //raycast from the created pointer and store back anything we hit
            if (objectsHit.Count > 0) return; //if we hit an ui -> return dont navigate there.

            //If so , proceed to normal raycast (Screen space to world space)
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            NavMeshHit navHit;
            //If we hit something
            if (Physics.Raycast(ray, out hit, 100)) {
                //Debug.DrawLine(ray.origin, hit.point, Color.red);
                //Sample the pos on the nav mesh
                if (NavMesh.SamplePosition(hit.point, out navHit, 1.0f, NavMesh.AllAreas)) {
                    //Calculate a valid path and store it back in "path" variable
                    NavMesh.CalculatePath(transform.position, hit.point, NavMesh.AllAreas, path);
                    //If its a legit path 
                    if (path.status == NavMeshPathStatus.PathComplete) {
                    
                        //if (!work)
                        //{
                        //    GameObject pointer = (GameObject)Instantiate(movePointer, hit.point + hit.normal * 0.1f , Quaternion.FromToRotation(Vector3.forward, hit.normal));
                        //    Destroy(pointer, 1f);
                        //    work = true;
                        //}

                        //Instruct agent to go there.
                        agent.destination = hit.point;
                    }
                }
            }
        }
        else
        {
            //work = false;
        }
        
    }
    /// <summary>
    /// Sends the agent to a destination (interactable) and then stores the last clicked object of that interactable.
    /// </summary>
    /// <param name="interactable"></param>
    public void SendAgent(Transform interactable)
    {
        agent.SetDestination(interactable.position);
        GameManager.Instance.ClickedObject = interactable.gameObject;
    }

    /// <summary>
    /// Stops the agent from moving. For example when cut scenes are played
    /// or at the end of the level when the finish screen is showed.
    /// </summary>
    public void StopAgent() {
        agent.Stop();
    }
    /// <summary>
    /// When the agent was stopped , this function can be used to resume his behaviour.
    /// The path the agent had when was stopped will be reset.
    /// </summary>
    public void ResumeAgent() {
        agent.ResetPath();
        agent.Resume();
    }


    /// <summary>
    /// Subscribe to camera events
    /// </summary>
    void OnEnable() {
        CameraControl.OnCameraPathEnd.AddListener(ResumeAgent);
        CameraControl.OnCameraPathStart.AddListener(StopAgent);
    }
    /// <summary>
    /// Unsubscribe from camera events
    /// </summary>
    void OnDisable() {
        CameraControl.OnCameraPathEnd.RemoveListener(ResumeAgent);
        CameraControl.OnCameraPathStart.RemoveListener(StopAgent);
    }
}
